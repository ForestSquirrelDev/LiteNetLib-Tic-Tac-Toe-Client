using UnityEngine;

namespace Utils {
    public static class Vector2Extensions
    {
        public enum Direction
        {
            Undefined = -1, Horizontal = 0, Vertical = 1, DiagonalRight = 2, DiagonalLeft = 3
        }
        
        public static Direction GetDirection(this Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 direction = endPoint - startPoint;

            if (Mathf.Approximately(direction.y, 0f) && !Mathf.Approximately(direction.x, 0f))
                return Direction.Horizontal;
            if (Mathf.Approximately(direction.x, 0f) && !Mathf.Approximately(direction.y, 0f))
                return Direction.Vertical;
            
            var xSign = Mathf.Sign(direction.x);
            return xSign > 0 ? Direction.DiagonalRight : Direction.DiagonalLeft;
        }

        public static Vector2 Mirror(this Vector2 originalVector) {
            return new Vector2(originalVector.x * -1f, originalVector.y * -1f);
        }
    }
}
