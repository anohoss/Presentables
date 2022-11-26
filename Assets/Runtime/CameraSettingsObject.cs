using System;
using UnityEngine;

namespace anoho.Presentables {

    [CreateAssetMenu(fileName = "NewCameraSettings", menuName = "Presentables/Create Camera Settings")]
    public class CameraSettingsObject : ScriptableObject, ICameraSettings {

        [SerializeField]
        private CameraClearFlags _clearFlags = CameraClearFlags.Depth;

        public CameraClearFlags ClearFlags => _clearFlags;



        [SerializeField]
        private LayerMask _cullingMask;

        public LayerMask CullingMask => _cullingMask;



        [SerializeField]
        private float _orthographicSize = 5f;

        public float OrthographicSize => _orthographicSize;



        [SerializeField]
        private float _nearClipPlane = 0.3f;

        public float NearClipPlane => _nearClipPlane;



        [SerializeField]
        private float _farClipPlane = 1000f;

        public float FarClipPlane => _farClipPlane;



        [SerializeField]
        private Rect _rect = new Rect(0f, 0f, 1f, 1f);

        public Rect Rect => _rect;



        [SerializeField]
        private float _depth = -1f;

        public float Depth => _depth;



        [SerializeField]
        private RenderingPath _renderingPath = RenderingPath.UsePlayerSettings;

        public RenderingPath RenderingPath => _renderingPath;



        [SerializeField]
        private RenderTexture _targetTexture = null;

        public RenderTexture TargetTexture => _targetTexture;



        [SerializeField]
        private bool _useOcclusionCulling = true;

        public bool UseOcclusionCulling => _useOcclusionCulling;



        [SerializeField]
        private bool _allowHDR = true;



        public bool AllowHDR => _allowHDR;



        [SerializeField]
        private bool _allowMSAA = true;

        public bool AllowMSAA => _allowMSAA;



        [SerializeField]
        private bool _allowDynamicResolution = false;

        public bool AllowDynamicResolution => _allowDynamicResolution;




        [SerializeField]
        private int _targetDisplay = 1;

        public int TargetDisplay => _targetDisplay;



        private void Reset() {
            int cullingMask = 0;
            for (int layer = 0; layer < 32; layer++) {
                if (!string.IsNullOrEmpty(LayerMask.LayerToName(layer))) {
                    cullingMask |= 1 << layer;
                }
            }
            _cullingMask = cullingMask;
        }


        public Type[] ComponentTypes => new Type[] { typeof(Camera) };

        public void Setup(Component[] components) {
            var camera = components[0] as Camera;
            camera.clearFlags = _clearFlags;
            camera.cullingMask = _cullingMask;
            camera.orthographic = true;
            camera.orthographicSize = _orthographicSize;
            camera.nearClipPlane = _nearClipPlane;
            camera.farClipPlane = _farClipPlane;
            camera.rect = _rect;
            camera.depth = _depth;
            camera.renderingPath = _renderingPath;
            camera.targetTexture = _targetTexture;
            camera.useOcclusionCulling = _useOcclusionCulling;
            camera.allowHDR = _allowHDR;
            camera.allowMSAA = _allowMSAA;
            camera.allowDynamicResolution = _allowDynamicResolution;
            camera.targetDisplay = _targetDisplay;
        }
    }
}
