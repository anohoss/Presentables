using System;
using UnityEngine;
using UnityEngine.UI;

namespace anoho.Presentables {
    [CreateAssetMenu(fileName = "NewCanvasSettings", menuName = "Presentables/Create Canvas Settings")]
    public class CanvasSettingsObject : ScriptableObject, ICanvasSettings {

        // GameObject Settings
        [SerializeField]
        private string _name = "Presentable Canvas";

        /// <summary>
        /// <see cref="GameObject.name"/>
        /// </summary>
        public string Name => _name;



        [SerializeField]
        private string _tag = "Untagged";

        /// <summary>
        /// <see cref="GameObject.tag"/>
        /// </summary>
        public string Tag => _tag;



        [SerializeField]
        private int _layer = 0;

        /// <summary>
        /// <see cref="GameObject.layer"/>
        /// </summary>
        public int Layer => _layer;



        // Canvas Settings
        [SerializeField]
        private RenderMode _renderMode = RenderMode.ScreenSpaceCamera;

        /// <summary>
        /// <see cref="Canvas.renderMode"/>
        /// </summary>
        public RenderMode RenderMode => _renderMode;



        [SerializeField]
        private bool _pixelPerfect = false;

        /// <summary>
        /// <see cref="Canvas.pixelPerfect"/>
        /// </summary>
        public bool PixelPerfect => _pixelPerfect;



        [SerializeField]
        private float _planeDistance = 100f;

        /// <summary>
        /// <see cref="Canvas.planeDistance"/>
        /// </summary>
        public float PlaneDistance => _planeDistance;



        [SerializeField]
        private int _sortingLayerId = 0;

        /// <summary>
        /// <see cref="Canvas.sortingLayerID"/>
        /// </summary>
        public int SortingLayerId => _sortingLayerId;



        [SerializeField]
        private int _orderInLayer = 0;

        /// <summary>
        /// <see cref="Canvas.sortingOrder"/>
        /// </summary>
        public int OrderInLayer => _orderInLayer;



        [SerializeField]
        private AdditionalCanvasShaderChannels _additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;

        /// <summary>
        /// <see cref="Canvas.additionalShaderChannels"/>
        /// </summary>
        public AdditionalCanvasShaderChannels AdditionalShaderChannels => _additionalShaderChannels;



        // Canvas Scaler Settings
        [SerializeField]
        private CanvasScaler.ScaleMode _uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

        /// <summary>
        /// <see cref="CanvasScaler.uiScaleMode"/>
        /// </summary>
        public CanvasScaler.ScaleMode UiScaleMode => _uiScaleMode;



        // _uiScaleMode == CanvasScale.ScaleMode.ConstantPixelSize
        [SerializeField]
        private float _scaleFactor = 1f;

        /// <summary>
        /// <see cref="CanvasScaler.scaleFactor"/>
        /// </summary>
        public float ScaleFactor => _scaleFactor;



        // _uiScaleMode == CanvasScale.ScaleMode.ScaleWithScreenSize

        [SerializeField]
        private Vector2 _referenceResolution = new Vector2(800f, 600f);

        /// <summary>
        /// <see cref="CanvasScaler.referenceResolution"/>
        /// </summary>
        public Vector2 ReferenceResolution => _referenceResolution;



        [SerializeField]
        private CanvasScaler.ScreenMatchMode _screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        /// <summary>
        /// <see cref="CanvasScaler.screenMatchMode"/>
        /// </summary>
        public CanvasScaler.ScreenMatchMode ScreenMatchMode => _screenMatchMode;



        [SerializeField]
        private float _matchWidthOrHeight = 0f;

        /// <summary>
        /// <see cref="CanvasScaler.matchWidthOrHeight"/>
        /// </summary>
        public float Match => _matchWidthOrHeight;



        // _uiScaleMode == CanvasScale.ScaleMode.ConstantPhysicalSize
        [SerializeField]
        private CanvasScaler.Unit _physicalUnit = CanvasScaler.Unit.Points;

        /// <summary>
        /// <see cref="CanvasScaler.physicalUnit"/>
        /// </summary>
        public CanvasScaler.Unit PhysicalUnit => _physicalUnit;



        [SerializeField]
        private float _fallbackScreenDpi = 96f;

        /// <summary>
        /// <see cref="CanvasScaler.fallbackScreenDPI"/>
        /// </summary>
        public float FallbackScreenDpi => _fallbackScreenDpi;



        [SerializeField]
        private float _defaultSpriteDpi = 96f;

        /// <summary>
        /// <see cref="CanvasScaler.defaultSpriteDPI"/>
        /// </summary>
        public float DefaultSpriteDpi => _defaultSpriteDpi;



        // common
        [SerializeField]
        private float _referencePixelsPerUnit = 100f;

        /// <summary>
        /// <see cref="CanvasScaler.referencePixelsPerUnit"/>
        /// </summary>
        public float ReferencePixelsPerUnit => _referencePixelsPerUnit;



        // GraphicRaycaster Settings
        [SerializeField]
        private bool _ignoreReversedGraphics = true;

        /// <summary>
        /// <see cref="GraphicRaycaster.ignoreReversedGraphics"/>
        /// </summary>
        public bool IgnoreReversedGraphics => _ignoreReversedGraphics;



        [SerializeField]
        private GraphicRaycaster.BlockingObjects _blockingObjects = GraphicRaycaster.BlockingObjects.None;

        /// <summary>
        /// <see cref="GraphicRaycaster.blockingObjects"
        /// </summary>
        public GraphicRaycaster.BlockingObjects BlockingObjects => _blockingObjects;



        [SerializeField]
        private LayerMask _blockingMask;

        /// <summary>
        /// <see cref="GraphicRaycaster.blockingMask"/>
        /// </summary>
        public LayerMask BlockingMask => _blockingMask;



        public Type[] ComponentTypes { get; } = new Type[] { typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster) };



        private void Reset() {
            int blockingMask = 0;
            for (int layer = 0; layer < 32; layer++) {
                if (!string.IsNullOrEmpty(LayerMask.LayerToName(layer))) {
                    blockingMask |= 1 << layer;
                }
            }

            _blockingMask = blockingMask;
        }



        public void Setup(Component[] components) {
            GameObject gameObject = components[0].gameObject;
            gameObject.name = _name;
            gameObject.tag = _tag;
            gameObject.layer = _layer;

            Canvas canvas = components[0] as Canvas;

            canvas.renderMode = _renderMode;
            canvas.pixelPerfect = _pixelPerfect;
            canvas.planeDistance = _planeDistance;
            canvas.sortingLayerID = _sortingLayerId;
            canvas.sortingOrder = _orderInLayer;
            canvas.additionalShaderChannels = _additionalShaderChannels;

            CanvasScaler scaler = components[1] as CanvasScaler;

            scaler.uiScaleMode = _uiScaleMode;
            scaler.scaleFactor = _scaleFactor;

            scaler.referenceResolution = _referenceResolution;
            scaler.screenMatchMode = _screenMatchMode;
            scaler.matchWidthOrHeight = _matchWidthOrHeight;
            scaler.physicalUnit = _physicalUnit;
            scaler.fallbackScreenDPI = _fallbackScreenDpi;
            scaler.defaultSpriteDPI = _defaultSpriteDpi;

            GraphicRaycaster raycaster = components[2] as GraphicRaycaster;
            raycaster.ignoreReversedGraphics = _ignoreReversedGraphics;
            raycaster.blockingObjects = _blockingObjects;
            raycaster.blockingMask = _blockingMask;
        }
    }
}
