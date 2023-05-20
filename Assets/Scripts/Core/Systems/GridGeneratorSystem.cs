using System;
using Core.Components;
using UnityEditor;
using UnityEngine;
using Grid = Core.Entities.Grid;

namespace Core.Systems {
    public class GridGeneratorSystem : SystemBase {
        public event Action<Grid> GridCreated;

        public GridGeneratorSystem(SystemsContext context) : base(context) {
        }

        protected override void OnStart() {
            GenerateGrid();
        }

        protected override void OnDrawGizmos() {
            Handles.color = Color.red;
            Handles.DrawSolidDisc(_startPos, Vector3.forward, .3f);
        
            Handles.color = Color.cyan;
            Handles.DrawSolidDisc(Vector3.zero, Vector3.forward, .3f);

            var centerPosition = Vector3.zero;
            var bottomLeftCorner = new Vector3(
                centerPosition.x - (_gridSizeXStartingFromZero* 0.5f * cellSize),
                centerPosition.y - (_gridSizeYStartingFromZero * 0.5f) * cellSize,
                centerPosition.z);
        
            Handles.color = Color.red;
            Handles.DrawSolidDisc(bottomLeftCorner, Vector3.forward, .3f);
        
            var bottomRightCorner = new Vector3(
                centerPosition.x + (_gridSizeXStartingFromZero * 0.5f* cellSize),
                centerPosition.y - (_gridSizeYStartingFromZero * 0.5f* cellSize),
                centerPosition.z);
            Handles.DrawSolidDisc(bottomRightCorner, Vector3.forward, .3f);
        
            var upperRightCorner = new Vector3(
                centerPosition.x + (_gridSizeXStartingFromZero * 0.5f* cellSize) ,
                centerPosition.y + (_gridSizeYStartingFromZero * 0.5f* cellSize) ,
                centerPosition.z);
            Handles.DrawSolidDisc(upperRightCorner, Vector3.forward, .3f);
        }

        protected override void OnUpdate(float delta) {
        }

        private Vector3 _startPos;
    
        public const int gridSizeX = 3;
        public const int gridSizeY = 3;
        private int _gridSizeXStartingFromZero = gridSizeX - 1;
        private int _gridSizeYStartingFromZero = gridSizeY - 1;
        public float cellSize = 2f;

        private void GenerateGrid()
        {
            var grid = new Grid();
            var cellsComponent = new GridCellsComponent();
            grid.AddComponent(cellsComponent);

            Vector3 centerPosition = Vector3.zero;
            Vector3 startPos = new Vector3(
                centerPosition.x - (_gridSizeXStartingFromZero* 0.5f * cellSize),
                centerPosition.y + (_gridSizeYStartingFromZero * 0.5f * cellSize),
                centerPosition.z);
            _startPos = startPos;
        
            for (int x = 0; x <= _gridSizeXStartingFromZero; x++)
            {
                for (int y = 0; y <= _gridSizeYStartingFromZero; y++) {
                    var cellPosition = new Vector3(startPos.x + x * cellSize, startPos.y - y * cellSize, startPos.z);
                    var cell = new GridCell(cellSize, cellPosition);
                    cellsComponent.Cells.Add(cell);
                }
            }

            _context.Entities.Add(grid);
            GridCreated?.Invoke(grid);
        }
    }
}
