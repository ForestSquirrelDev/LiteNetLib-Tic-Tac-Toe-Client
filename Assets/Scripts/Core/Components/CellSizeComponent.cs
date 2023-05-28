using PoorMansECS.Components;

namespace Core.Components {
    public readonly struct CellSizeComponent : IComponentData {
        public float CellSize { get; }

        public CellSizeComponent(float cellSize) {
            CellSize = cellSize;
        }
    }
}