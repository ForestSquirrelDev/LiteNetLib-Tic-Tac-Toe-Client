using UnityEngine;

namespace Core.Components {
    public readonly struct GridCell {
        public float Size { get; }
        public Vector3 LocalPosition { get; }

        public GridCell(float size, Vector3 localPosition) {
            Size = size;
            LocalPosition = localPosition;
        }
    }
}
