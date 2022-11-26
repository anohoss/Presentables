using System;
using UnityEditor;
using UnityEngine;

namespace anoho.Presentables.Editor {

    [CustomEditor(typeof(CameraSettingsObject))]
    public class CameraSettingsObjectInspector : UnityEditor.Editor {
        private class Styles {
            public readonly GUIContent ClearFlags;
            public readonly GUIContent Projection;
            public readonly GUIContent Orthographic;
            public readonly GUIContent Size;
            public readonly GUIContent ClippingPlanes;
            public readonly GUIContent Near;
            public readonly GUIContent Far;
            public readonly GUIContent ViewportRect;
            public readonly GUIContent RenderingPath;
            public readonly GUIContent OcclusionCulling;
            public readonly GUIContent HDR;
            public readonly GUIContent MSAA;
            public readonly GUIContent TargetDisplay;

            public Styles() {
                ClearFlags = EditorGUIUtility.TrTextContent("Clear Flags");
                Projection = EditorGUIUtility.TrTextContent("Projection");
                Orthographic = EditorGUIUtility.TrTextContent("Orthographic");
                Size = EditorGUIUtility.TrTextContent("Size");
                ClippingPlanes = EditorGUIUtility.TrTextContent("Clipping Planes");
                Near = EditorGUIUtility.TrTextContent("Near");
                Far = EditorGUIUtility.TrTextContent("Far");
                ViewportRect = EditorGUIUtility.TrTextContent("Viewport Rect");
                RenderingPath = EditorGUIUtility.TrTextContent("Rendering Path");
                OcclusionCulling = EditorGUIUtility.TrTextContent("Occlusion Culling");
                HDR = EditorGUIUtility.TrTextContent("HDR");
                MSAA = EditorGUIUtility.TrTextContent("MSAA");
                TargetDisplay = EditorGUIUtility.TrTextContent("Target Display");
            }
        }



        SerializedProperty _clearFlagsProp;
        SerializedProperty _cullingMaskProp;
        SerializedProperty _orthographicSizeProp;
        SerializedProperty _nearClipPlaneProp;
        SerializedProperty _farClipPlaneProp;
        SerializedProperty _rectProp;
        SerializedProperty _depthProp;
        SerializedProperty _renderingPathProp;
        SerializedProperty _targetTextureProp;
        SerializedProperty _useOcclusionCullingProp;
        SerializedProperty _allowHDRProp;
        SerializedProperty _allowMSAAProp;
        SerializedProperty _allowDynamicResolutionProp;
        SerializedProperty _targetDisplayProp;

        Styles _styles;

        string[] _renderingPathOptions = new string[] { "Use Graphics Settings", "Forward", "Deferred", "Legacy Vertex Lit", "Legacy Deffered (light prepass)" };

        string[] _clearFlagsOptions = new string[] { "Depth only", "Don't Clear" };

        string[] _allowHDROptions = new string[] { "Off", "Use Graphics Settings" };

        string[] _allowMSAAOptions = new string[] { "Off", "Use Graphics Settings" };

        string[] _targetDisplayOptions = new string[] { "Display 1", "Display 2", "Display 3", "Display 4", "Display 5", "Display 6", "Display 7", "Display 8" };



        private void OnEnable() {
            _clearFlagsProp = serializedObject.FindProperty("_clearFlags");
            _cullingMaskProp = serializedObject.FindProperty("_cullingMask");
            _orthographicSizeProp = serializedObject.FindProperty("_orthographicSize");
            _nearClipPlaneProp = serializedObject.FindProperty("_nearClipPlane");
            _farClipPlaneProp = serializedObject.FindProperty("_farClipPlane");
            _rectProp = serializedObject.FindProperty("_rect");
            _depthProp = serializedObject.FindProperty("_depth");
            _renderingPathProp = serializedObject.FindProperty("_renderingPath");
            _targetTextureProp = serializedObject.FindProperty("_targetTexture");
            _useOcclusionCullingProp = serializedObject.FindProperty("_useOcclusionCulling");
            _allowHDRProp = serializedObject.FindProperty("_allowHDR");
            _allowMSAAProp = serializedObject.FindProperty("_allowMSAA");
            _allowDynamicResolutionProp = serializedObject.FindProperty("_allowDynamicResolution");
            _targetDisplayProp = serializedObject.FindProperty("_targetDisplay");

            _styles ??= new Styles();
        }



        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            // enum CameraClearFlags
            // - Skybox = 1,
            // - Color = 2,
            // - SolidColor = 2,
            // - Depth = 3,
            // - Nothing = 4
            // Cameras for Presenter doesn't need to cover the background.
            // So, exclude "Skybox", "Color" and "SolidColor" flags.
            const int flagsGap = 3;
            int clearFlags = EditorGUILayout.Popup(_styles.ClearFlags, _clearFlagsProp.intValue - flagsGap, _clearFlagsOptions);
            if (EditorGUI.EndChangeCheck()) {
                _clearFlagsProp.intValue = clearFlags + flagsGap;
            }
            EditorGUILayout.PropertyField(_cullingMaskProp);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField(_styles.Projection, _styles.Orthographic, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_orthographicSizeProp, _styles.Size);
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField(_styles.ClippingPlanes, GUILayout.Width(EditorGUIUtility.labelWidth));
                using (new EditorGUILayout.VerticalScope()) {
                    using (new EditorGUILayout.HorizontalScope()) {
                        EditorGUILayout.LabelField(_styles.Near, GUILayout.Width(32f));
                        EditorGUI.BeginChangeCheck();
                        float near = EditorGUILayout.FloatField(_nearClipPlaneProp.floatValue);
                        if (EditorGUI.EndChangeCheck()) {
                            _nearClipPlaneProp.floatValue = near;
                        }
                    }
                    using (new EditorGUILayout.HorizontalScope()) {
                        EditorGUILayout.LabelField(_styles.Far, GUILayout.Width(32f));
                        EditorGUI.BeginChangeCheck();
                        float far = EditorGUILayout.FloatField(_farClipPlaneProp.floatValue);
                        if (EditorGUI.EndChangeCheck()) {
                            _farClipPlaneProp.floatValue = far;
                        }
                    }
                }
            }
            EditorGUILayout.PropertyField(_rectProp, new GUIContent(_styles.ViewportRect));

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_depthProp);
            EditorGUI.BeginChangeCheck();
            int renderingPath = _renderingPathProp.intValue switch {
                -1 => 0,
                0 => 3,
                1 => 1,
                2 => 4,
                3 => 2,
                _ => 0,
            };
            renderingPath = EditorGUILayout.Popup(_styles.RenderingPath, renderingPath, _renderingPathOptions);
            if (EditorGUI.EndChangeCheck()) {
                _renderingPathProp.intValue = renderingPath switch {
                    0 => -1,
                    1 => 1,
                    2 => 3,
                    3 => 0,
                    4 => 2,
                    _ => -1,
                };
            }
            EditorGUILayout.PropertyField(_targetTextureProp);
            EditorGUILayout.PropertyField(_useOcclusionCullingProp, _styles.OcclusionCulling);
            EditorGUI.BeginChangeCheck();
            int allowHDR = _allowHDRProp.boolValue ? 1: 0;
            allowHDR = EditorGUILayout.Popup(_styles.HDR, allowHDR, _allowHDROptions);
            if (EditorGUI.EndChangeCheck()) {
                _allowHDRProp.boolValue = Convert.ToBoolean(allowHDR);
            }
            EditorGUI.BeginChangeCheck();
            int allowMSAA = _allowMSAAProp.boolValue ? 1 : 0;
            allowMSAA = EditorGUILayout.Popup(_styles.MSAA, allowMSAA, _allowMSAAOptions);
            if (EditorGUI.EndChangeCheck()) {
                _allowMSAAProp.boolValue = Convert.ToBoolean(allowMSAA);
            }
            EditorGUILayout.PropertyField (_allowDynamicResolutionProp);

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            // Target Display 0 ~ 7
            int targetDisplay = EditorGUILayout.Popup(_styles.TargetDisplay, _targetDisplayProp.intValue, _targetDisplayOptions);

            if (EditorGUI.EndChangeCheck()) {
                _targetDisplayProp.intValue = targetDisplay;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
