using PoorMansECS.Components;
using UnityEngine;

namespace Core.Components {
    public readonly struct GridStartComponent : IComponentData {
        public Vector3 GridStart { get; }

        public GridStartComponent(Vector3 gridStart) {
            GridStart = gridStart;
        }
    }
}