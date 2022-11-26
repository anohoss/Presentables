using UnityEngine;

namespace anoho.Presentables {

    /// <summary>
    /// Root Canvas for Presenters. <br />
    /// Create a Canvas for each Presenter.
    /// </summary>
    public sealed class PresentableCanvas : MonoBehaviour {

        private Canvas _canvas;

        /// <summary>
        /// Root Canvas for Presenter's Canvas.
        /// </summary>
        public Canvas Canvas {
            get {
                return _canvas;
            }
            internal set {
                _canvas = value;
            }
        }



        internal Camera _camera;

        /// <summary>
        /// Camera for Presenter's Canvas
        /// </summary>
        public Camera Camera {
            get {
                return _camera;
            }
            internal set {
                Canvas.worldCamera = value;
                _camera = value;
            }
        }



        /// <summary>
        /// Open Presenter's view 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public PresentRequest PresentAsync<T>() where T : Presenter {
            return PresentAsync(typeof(T));
        }



        /// <summary>
        /// Open Presenter's view 
        /// </summary>
        /// <param name="type">Type inherited from <see cref="Presenter"/></param>
        /// <returns></returns>
        public PresentRequest PresentAsync(System.Type type) {
            return new PresentRequest(this, type);
        }
    }
}
