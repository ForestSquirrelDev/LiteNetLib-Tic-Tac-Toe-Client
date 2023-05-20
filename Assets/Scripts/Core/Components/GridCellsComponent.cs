using System.Collections.Generic;
using PoorMansECS.Components;

namespace Core.Components {
    public class GridCellsComponent : IComponentData {
        public List<GridCell> Cells { get; } = new();
    }
}
