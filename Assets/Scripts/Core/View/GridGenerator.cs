using Core.Components;
using Core.Managers;
using Core.Systems.Events;
using DG.Tweening;
using PoorMansECS.Entities;
using PoorMansECS.Systems;
using UnityEngine;
using Utils;
using Grid = Core.Entities.Grid;

namespace Core.View {
    public class GridGenerator : ISystemsEventListener {
        private readonly GameModel _model;
        private readonly GameObject _trailPrefab;
        
        public GridGenerator(GameModel model, GameObject trailPrefab) {
            _model = model;
            _trailPrefab = trailPrefab;
        }

        public void Start() {
            _model.World.EventBus.Subscribe<GridGeneratedEvent>(this);
        }
        
        public void ReceiveEvent<T>(T systemEvent) where T : ISystemEvent {
            if (systemEvent is GridGeneratedEvent) {
                var grid = _model.World.Entities.GetFirst<Grid>();
                DrawGrid(grid, _trailPrefab);
            }
        }

        private void DrawGrid(IEntity grid, GameObject trailPrefab) {
            var gridStart = grid.GetComponent<GridStartComponent>().GridStart;
            var cellSize = grid.GetComponent<CellSizeComponent>().CellSize;
            
            var rightTopCellOffset = new Vector2(cellSize * 0.5f, cellSize * 0.5f);
            var leftTopCellOffset = new Vector2(-cellSize * 0.5f, cellSize * 0.5f);
            var leftBottomCellOffset = new Vector2(-cellSize * 0.5f, -cellSize * 0.5f);
            var rightBottomCellOffset = new Vector2(cellSize * 0.5f, -cellSize * 0.5f);

            var verticalFirstStart = GridUtils.GridIndexToWorldPosition(0, 0, gridStart, cellSize) + rightTopCellOffset;
            var verticalFirstEnd = GridUtils.GridIndexToWorldPosition(0, 2, gridStart, cellSize) + rightBottomCellOffset;
            
            var verticalSecondStart = GridUtils.GridIndexToWorldPosition(1, 0, gridStart, cellSize) + rightTopCellOffset;
            var verticalSecondEnd = GridUtils.GridIndexToWorldPosition(1, 2, gridStart, cellSize) + rightBottomCellOffset;
            
            var horizontalFirstStart = GridUtils.GridIndexToWorldPosition(0, 0, gridStart, cellSize) + leftBottomCellOffset;
            var horizontalFirstEnd = GridUtils.GridIndexToWorldPosition(2, 0, gridStart, cellSize) + rightBottomCellOffset;
            
            var horizontalSecondStart = GridUtils.GridIndexToWorldPosition(0, 2, gridStart, cellSize) + leftTopCellOffset;
            var horizontalSecondEnd = GridUtils.GridIndexToWorldPosition(2, 2, gridStart, cellSize) + rightTopCellOffset;

            var verticalFirstTrail = Object.Instantiate(trailPrefab, verticalFirstStart, Quaternion.identity);
            var verticalSecondTrail = Object.Instantiate(trailPrefab, verticalSecondStart, Quaternion.identity);
            var horizontalFirstTrail = Object.Instantiate(trailPrefab, horizontalFirstStart, Quaternion.identity);
            var horizontalSecondTrail = Object.Instantiate(trailPrefab, horizontalSecondStart, Quaternion.identity);
            
            DrawTo(verticalFirstEnd, verticalFirstTrail.transform);
            DrawTo(verticalSecondEnd, verticalSecondTrail.transform);
            DrawTo(horizontalFirstEnd, horizontalFirstTrail.transform);
            DrawTo(horizontalSecondEnd, horizontalSecondTrail.transform);
        }

        private void DrawTo(Vector2 to, Transform t) {
            var randomDuration = RandomizeDuration();
            t.DOMove(to, randomDuration);
        }

        private float RandomizeDuration() {
            return Random.Range(1.0f, 1.5f);
        }
    }
}
