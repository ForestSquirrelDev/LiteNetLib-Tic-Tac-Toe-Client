using UnityEngine;

namespace Core.Components {
    public readonly struct GridCell {
        public float Size { get; }
        public Vector3 LocalPosition { get; }
        public Vector2 CellIndex { get; }

        public GridCell(float size, Vector3 localPosition, Vector2 cellIndex) {
            Size = size;
            LocalPosition = localPosition;
            CellIndex = cellIndex;
        }
    }
}
