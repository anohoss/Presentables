using UnityEditor;
using UnityEngine;
using System;

namespace anoho.Presentables.Editor {
    [CustomEditor(typeof(CanvasSettingsObject))]
    public class CanvasSettingsObjectInspector : UnityEditor.Editor {
        SerializedProperty _nameProp;

        SerializedProperty _tagProp;

        SerializedProperty _layerProp;

        SerializedProperty _renderModeProp;

        SerializedProperty _pixelPerfectProp;

        SerializedProperty _planeDistanceProp;

        SerializedProperty _sortingLayerIdProp;

        SerializedProperty _orderInLayerProp;

        SerializedProperty _shaderChannelsProp;

        SerializedProperty _uiScaleModeProp;

        SerializedProperty _scaleFactorProp;

        SerializedProperty _referenceResolutionProp;

        SerializedProperty _screenMatchModeProp;

        SerializedProperty _matchWidthOrHeightProp;

        SerializedProperty _physicalUnitProp;

        SerializedProperty _fallbackScreenDpiProp;

        SerializedProperty _defaultSpriteDpiProp;

        SerializedProperty _referencePixelsPerUnitProp;

        SerializedProperty _ignoreReversedGraphicsProp;

        SerializedProperty _blockingObjectsProp;

        SerializedProperty _blockingMaskProp;

        private class Styles {
            public readonly GUIContent Canvas;

            public readonly GUIContent SortingLayer;

            public readonly GUIContent ShaderChannel;

            public readonly GUIContent CanvasScaler;

            public readonly GUIContent UiScaleMode;

            public readonly GUIContent ScreenMatchMode;

            public readonly GUIContent Match;

            public readonly GUIContent MatchWidth;

            public readonly GUIContent MatchHeight;

            public readonly GUIContent GraphicsRaycaster;

            public Styles() {
                Canvas = EditorGUIUtility.TrTextContent("Canvas");
                SortingLayer = EditorGUIUtility.TrTextContent("Sorting Layer");
                ShaderChannel = EditorGUIUtility.TrTextContent("Additional Shader Channels");

                CanvasScaler = EditorGUIUtility.TrTextContent("Canvas Scaler");
                UiScaleMode = EditorGUIUtility.TrTextContent("UI Scale Mode");
                ScreenMatchMode = EditorGUIUtility.TrTextContent("Screen Match Mode");
                Match = EditorGUIUtility.TrTextContent("Match");
                MatchWidth = EditorGUIUtility.TrTextContent("Width");
                MatchHeight = EditorGUIUtility.TrTextContent("Height");

                GraphicsRaycaster = EditorGUIUtility.TrTextContent("Graphics Raycaster");
            }
        }

        private Styles _styles;

        private string[] _shaderChannelOptions = { "TexCoord1", "TexCoord2", "TexCoord3", "Normal", "Tangent" };

        private string[] _uiScaleModeOptions = { "Constant Pixel Size", "Scale With Screen Size", "Constant Physical Size" };

        private string[] _screenMatchModeOptions = { "Match Width Or Height", "Expand", "Shrink" };

        public void OnEnable() {
            _nameProp = serializedObject.FindProperty("_name");
            _tagProp = serializedObject.FindProperty("_tag");
            _layerProp = serializedObject.FindProperty("_layer");
            _renderModeProp = serializedObject.FindProperty("_renderMode");
            _pixelPerfectProp = serializedObject.FindProperty("_pixelPerfect");
            _planeDistanceProp = serializedObject.FindProperty("_planeDistance");
            _sortingLayerIdProp = serializedObject.FindProperty("_sortingLayerId");
            _orderInLayerProp = serializedObject.FindProperty("_orderInLayer");
            _shaderChannelsProp = serializedObject.FindProperty("_additionalShaderChannels");
            _uiScaleModeProp = serializedObject.FindProperty("_uiScaleMode");
            _scaleFactorProp = serializedObject.FindProperty("_scaleFactor");
            _referenceResolutionProp = serializedObject.FindProperty("_referenceResolution");
            _screenMatchModeProp = serializedObject.FindProperty("_screenMatchMode");
            _matchWidthOrHeightProp = serializedObject.FindProperty("_matchWidthOrHeight");
            _physicalUnitProp = serializedObject.FindProperty("_physicalUnit");
            _fallbackScreenDpiProp = serializedObject.FindProperty("_fallbackScreenDpi");
            _defaultSpriteDpiProp = serializedObject.FindProperty("_defaultSpriteDpi");
            _referencePixelsPerUnitProp = serializedObject.FindProperty("_referencePixelsPerUnit");
            _ignoreReversedGraphicsProp = serializedObject.FindProperty("_ignoreReversedGraphics");
            _blockingObjectsProp = serializedObject.FindProperty("_blockingObjects");
            _blockingMaskProp = serializedObject.FindProperty("_blockingMask");

            _styles ??= new Styles();
        }



        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawGameObjectField();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            DrawCanvasField();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            DrawCanvasScalerField();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            DrawGraphicsRaycasterField();

            serializedObject.ApplyModifiedProperties();
        }



        private void DrawGameObjectField() {
            EditorGUILayout.LabelField("GameObject", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_nameProp);

            EditorGUI.BeginChangeCheck();
            string tag = EditorGUILayout.TagField(new GUIContent("Tag"), _tagProp.stringValue);
            if (EditorGUI.EndChangeCheck()) {
                _tagProp.stringValue = tag;
            }

            EditorGUI.BeginChangeCheck();
            int layer = EditorGUILayout.LayerField(new GUIContent("Layer"), _layerProp.intValue);
            if (EditorGUI.EndChangeCheck()) {
                _layerProp.intValue = layer;
            }
        }



        private void DrawCanvasField() {
            EditorGUILayout.LabelField(_styles.Canvas, EditorStyles.boldLabel);
            using (new EditorGUI.DisabledScope(true)) {
                EditorGUILayout.PropertyField(_renderModeProp);
            }

            using (new EditorGUI.IndentLevelScope(1)) {
                EditorGUILayout.PropertyField(_pixelPerfectProp);
                EditorGUILayout.PropertyField(_planeDistanceProp);
                SortingLayerField(_styles.SortingLayer, _sortingLayerIdProp, EditorStyles.popup);
                EditorGUILayout.PropertyField(_orderInLayerProp);
            }

            _shaderChannelsProp.intValue = EditorGUILayout.MaskField(_styles.ShaderChannel, _shaderChannelsProp.intValue, _shaderChannelOptions);
        }



        private void DrawCanvasScalerField() {
            EditorGUILayout.LabelField(_styles.CanvasScaler, EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            int uiScaleMode = EditorGUILayout.Popup(_styles.UiScaleMode, _uiScaleModeProp.intValue, _uiScaleModeOptions);
            if (EditorGUI.EndChangeCheck()) {
                _uiScaleModeProp.intValue = uiScaleMode;
                Debug.Log(_uiScaleModeProp.intValue);
            }

            EditorGUILayout.Space();

            switch (_uiScaleModeProp.intValue) {
                case 0:
                    EditorGUILayout.PropertyField(_scaleFactorProp);
                    break;
                case 1:
                    EditorGUILayout.PropertyField(_referenceResolutionProp);

                    EditorGUI.BeginChangeCheck();
                    int screenMatchMode = EditorGUILayout.Popup(_styles.ScreenMatchMode, _screenMatchModeProp.intValue, _screenMatchModeOptions);
                    if (EditorGUI.EndChangeCheck()) {
                        _screenMatchModeProp.intValue = screenMatchMode;
                    }

                    if (_screenMatchModeProp.intValue == 0) {
                        EditorGUI.BeginChangeCheck();
                        float _match = EditorGUILayout.Slider(_styles.Match, _matchWidthOrHeightProp.floatValue, 0f, 1f);
                        if (EditorGUI.EndChangeCheck()) {
                            _matchWidthOrHeightProp.floatValue = _match;
                        }

                        using (new EditorGUILayout.HorizontalScope()) {
                            GUILayout.Space(EditorGUIUtility.labelWidth);
                            EditorGUILayout.LabelField(_styles.MatchWidth);
                            EditorGUILayout.LabelField(_styles.MatchHeight);
                        }
                    }
                    break;
                case 2:
                    EditorGUILayout.PropertyField(_physicalUnitProp);
                    EditorGUILayout.PropertyField(_fallbackScreenDpiProp);
                    EditorGUILayout.PropertyField(_defaultSpriteDpiProp);
                    break;
                default:
                    break;
            }

            EditorGUILayout.PropertyField(_referencePixelsPerUnitProp);
        }



        private void DrawGraphicsRaycasterField() {
            EditorGUILayout.LabelField(_styles.GraphicsRaycaster, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_ignoreReversedGraphicsProp);
            EditorGUILayout.PropertyField(_blockingObjectsProp);
            EditorGUILayout.PropertyField(_blockingMaskProp);
        }



        private delegate void SortingLayerFieldDelegate(Rect position, GUIContent label, SerializedProperty layerID, GUIStyle style, GUIStyle labelStyle);

        SortingLayerFieldDelegate _sortingLayerField = null;

        private void SortingLayerField(GUIContent label, SerializedProperty layerId, GUIStyle style) {
            if (_sortingLayerField == null) {
                Type editorGuiType = typeof(EditorGUI);
                var sortingLayerFieldMethod = editorGuiType.GetMethod("SortingLayerField",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic,
                    null,
                    new Type[] { typeof(Rect), typeof(GUIContent), typeof(SerializedProperty), typeof(GUIStyle), typeof(GUIStyle) },
                    null);

                if (sortingLayerFieldMethod == null) {
                    return;
                }

                _sortingLayerField = Delegate.CreateDelegate(typeof(SortingLayerFieldDelegate), sortingLayerFieldMethod) as SortingLayerFieldDelegate;
            }

            Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, style);
            _sortingLayerField.Invoke(rect, label, layerId, style, EditorStyles.label);
        }
    }
}
