using System;
using UnityEngine;

namespace anoho.Presentables {
    /// <summary>
    /// Setting GameObject to be used as Canvas
    /// </summary>
    public interface ICanvasSettings {
        /// <summary>
        /// Types of component to add to GameObject.    <br />
        /// Even if component: <see cref="Canvas"/> is not included, it will be added.
        /// </summary>
        Type[] ComponentTypes { get; }

        /// <summary>
        /// Setup components that is added to GameObject
        /// </summary>
        /// <param name="components"></param>
        void Setup(Component[] components);
    }
}
