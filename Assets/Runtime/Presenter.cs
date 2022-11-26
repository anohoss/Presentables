using System;
using System.Collections;
using UnityEngine;

namespace anoho.Presentables {
    /// <summary>
    /// Abstract Presenter
    /// </summary>
    public abstract class Presenter : MonoBehaviour {
        internal Canvas _canvas;
        protected Canvas Canvas => _canvas;
        internal abstract IEnumerator Instantiate(Action<Presenter> onCompleted);
    }
}
