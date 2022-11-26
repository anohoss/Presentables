using System;
using UnityEngine;

namespace anoho.Presentables {
    /// <summary>
    /// Setting GameObject to be used as Camera
    /// </summary>
    public interface ICameraSettings {
        /// <summary>
        /// Types of component to add GameObject.   <br />
        /// Even if component: <see cref="Camera"/> is not included, it will be added.
        /// </summary>
        Type[] ComponentTypes { get; }

        /// <summary>
        /// Setup components that is added to GameObject
        /// </summary>
        void Setup(Component[] components);
    }
}
