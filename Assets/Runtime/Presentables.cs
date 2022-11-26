using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityObject = UnityEngine.Object;

namespace anoho.Presentables {

    public class Presentables {
        static Dictionary<ICanvasSettings, Canvas> s_settingsToCanvas = new Dictionary<ICanvasSettings, Canvas>();



        static Dictionary<ICameraSettings, Camera> s_settingsToCamera = new Dictionary<ICameraSettings, Camera>();



        private static Canvas CreateCanvas(ICanvasSettings settings) {
            GameObject go = new GameObject("Canvas");

            if (!go.TryGetComponent<Canvas>(out var canvas)) {
                canvas = go.AddComponent<Canvas>();
            }

            // Componentを初期化する
            SetupComponent(go, settings);

            return canvas;
        }



        private static Camera CreateCamera(ICameraSettings settings) {
            GameObject go = new GameObject("Camera");

            if (!go.TryGetComponent<Camera>(out var camera)) {
                camera = go.AddComponent<Camera>();
            }

            // Componentを初期化する
            SetupComponent(go, settings);

            return camera;
        }



        /// <summary>
        /// Get Canvas for Presenters.  <br />
        /// ICanvasSettings are cached. If Canvas that created based on it already exist, it will be used. The same goes for ICameraSettings.
        /// </summary>
        /// <param name="canvasSettings">If this is null, <see cref="DefaultCanvasSettings"/> will be used instead.</param>
        /// <param name="cameraSettings">If this is null, <see cref="DefaultCameraSettings"/> will be used instead.</param>
        /// <returns></returns>
        public static PresentableCanvas GetCanvas(ICanvasSettings canvasSettings = null, ICameraSettings cameraSettings = null) {
            if (canvasSettings == null) {
                canvasSettings = DefaultCanvasSettings;
            }

            if (cameraSettings == null) {
                cameraSettings = DefaultCameraSettings;
            }

            Canvas canvas;
            if (!s_settingsToCanvas.TryGetValue(canvasSettings, out canvas) || canvas == null) {
                canvas = CreateCanvas(canvasSettings);
                s_settingsToCanvas[canvasSettings] = canvas;
                UnityObject.DontDestroyOnLoad(canvas.gameObject);
            }

            Camera camera;
            if (!s_settingsToCamera.TryGetValue(cameraSettings, out camera) || camera == null) {
                camera = CreateCamera(cameraSettings);
                s_settingsToCamera[cameraSettings] = camera;
                UnityObject.DontDestroyOnLoad(camera.gameObject);
            }

            if (!canvas.TryGetComponent<PresentableCanvas>(out var presentableCanvas)) {
                presentableCanvas = canvas.gameObject.AddComponent<PresentableCanvas>();
                presentableCanvas.Canvas = canvas;
                presentableCanvas.Camera = camera;
            }

            return presentableCanvas;
        }



        private static void SetupComponent(GameObject go, ICanvasSettings settings) {
            Component[] components = Enumerable.Repeat<Component>(null, settings.ComponentTypes.Length).ToArray();

            for (int i = 0; i < settings.ComponentTypes.Length; i++) {
                if (!go.TryGetComponent(settings.ComponentTypes[i], out Component component)) {
                    component = go.AddComponent(settings.ComponentTypes[i]);

                    if (component == null) {
                        Debug.LogError($"Failed to add component: {settings.ComponentTypes[i]}");
                    }
                }

                components[i] = component;
            }

            settings.Setup(components);
        }



        private static void SetupComponent(GameObject go, ICameraSettings settings) {
            Component[] components = Enumerable.Repeat<Component>(null, settings.ComponentTypes.Length).ToArray();

            for (int i = 0; i < settings.ComponentTypes.Length; i++) {
                if (!go.TryGetComponent(settings.ComponentTypes[i], out Component component)) {
                    component = go.AddComponent(settings.ComponentTypes[i]);

                    if (component == null) {
                        Debug.LogError($"Failed to add component: {settings.ComponentTypes[i]}");
                    }
                }

                components[i] = component;
            }

            settings.Setup(components);
        }




        private class CanvasSettings : ICanvasSettings {

            public Type[] ComponentTypes { get; } = new Type[] { typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster) };

            public void Setup(Component[] components) {
                GameObject gameObject = components[0].gameObject;
                gameObject.name = "UI Canvas";
                gameObject.tag = "Untagged";
                gameObject.layer = 5;   // UI

                Canvas canvas = components[0] as Canvas;

                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.pixelPerfect = false;
                canvas.planeDistance = 100f;
                canvas.sortingLayerID = 0;
                canvas.sortingOrder = 0;
                canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1
                    | AdditionalCanvasShaderChannels.Normal
                    | AdditionalCanvasShaderChannels.Tangent;

                CanvasScaler scaler = components[1] as CanvasScaler;

                scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                scaler.scaleFactor = 1f;

                scaler.referenceResolution = new Vector2(800f, 600f);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = 0f;
                scaler.physicalUnit = CanvasScaler.Unit.Points;
                scaler.fallbackScreenDPI = 96f;
                scaler.defaultSpriteDPI = 96f;

                GraphicRaycaster raycaster = components[2] as GraphicRaycaster;
                raycaster.ignoreReversedGraphics = true;
                raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

                int blockingMask = 0;
                for (int layer = 0; layer < 32; layer++) {
                    if (!string.IsNullOrEmpty(LayerMask.LayerToName(layer))) {
                        blockingMask |= 1 << layer;
                    }
                }
                raycaster.blockingMask = blockingMask;
            }
        }



        private static ICanvasSettings s_defaultCanvasSettings;

        public static ICanvasSettings DefaultCanvasSettings => s_defaultCanvasSettings ??= new CanvasSettings();




        private class CameraSettings : ICameraSettings {

            public Type[] ComponentTypes { get; } = new Type[] { typeof(Camera) };

            public void Setup(Component[] components) {
                GameObject gameObject = components[0].gameObject;
                gameObject.name = "UI Camera";
                gameObject.tag = "Untagged";
                gameObject.layer = 0;

                var camera = components[0] as Camera;
                camera.clearFlags = CameraClearFlags.Depth;
                int cullingMask = 0;
                for (int layer = 0; layer < 32; layer++) {
                    if (!string.IsNullOrEmpty(LayerMask.LayerToName(layer))) {
                        cullingMask |= 1 << layer;
                    }
                }
                camera.cullingMask = cullingMask;
                camera.orthographic = true;
                camera.orthographicSize = 5f;
                camera.nearClipPlane = 0.3f;
                camera.farClipPlane = 1000f;
                camera.rect = new Rect(0f, 0f, 1f, 1f);
                camera.depth = -1f;
                camera.renderingPath = RenderingPath.UsePlayerSettings;
                camera.useOcclusionCulling = true;
                camera.allowHDR = true;
                camera.allowMSAA = true;
                camera.allowDynamicResolution = false;
                camera.targetDisplay = 0;
            }
        }



        private static ICameraSettings s_defaultCameraSettings;

        public static ICameraSettings DefaultCameraSettings => s_defaultCameraSettings ??= new CameraSettings();
    }
}
