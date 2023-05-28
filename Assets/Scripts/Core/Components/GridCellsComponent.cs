using PoorMansECS.Components;

namespace Core.Components {
    public readonly struct GridCellsComponent : IComponentData {
        public GridCell [,] Cells { get; }

        public GridCellsComponent(GridCell[,] cells) {
            Cells = cells;
        }
    }
}
