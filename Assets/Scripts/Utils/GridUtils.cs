using UnityEngine;

namespace Utils {
    public static class GridUtils {
        public static (Vector3 localPosition, int xIndex, int yIndex) SnapPointToGrid(Vector3 point, Vector3 gridStart, float cellSize)
        {
            Vector3 relativePosition = point - gridStart;
            
            int xIndex = Mathf.RoundToInt(relativePosition.x / cellSize);
            int yIndex = Mathf.RoundToInt(-relativePosition.y / cellSize);
            
            Vector3 snappedPosition = new Vector3(
                gridStart.x + xIndex * cellSize,
                gridStart.y - yIndex * cellSize,
                gridStart.z);

            return (snappedPosition, xIndex, yIndex);
        }
        
        public static Vector2 GridIndexToWorldPosition(int xIndex, int yIndex, Vector3 gridStart, float cellSize)
        {
            var offset = new Vector2(xIndex * cellSize, -yIndex * cellSize);
            return (Vector2)gridStart + offset;
        }
    }
}