using Core.Components;
using Core.Managers;
using Core.Systems.Events;
using DG.Tweening;
using PoorMansECS.Systems;
using ServerShared.Shared.Network;
using UnityEngine;
using Utils;
using Grid = Core.Entities.Grid;
using Random = UnityEngine.Random;

namespace Core.View {
    public class GridView : MonoBehaviour, ISystemsEventListener, INetMessageListener {
        [SerializeField] private GameModel _model;

        [SerializeField] private GameObject _crossPrefab;
        [SerializeField] private GameObject _circlePrefab;
        [SerializeField] private GameObject _drawingTrail;

        private void Start() {
            _model.World.EventBus.Subscribe<GridGeneratedEvent>(this);
        }

        public void ReceiveEvent<T>(T systemEvent) where T : ISystemEvent {
            if (systemEvent is not GridGeneratedEvent) {
                Debug.LogError("Wrong type of event in GridView");
                return;
            }
            
            var grid = _model.World.Entities.GetFirst<Grid>();
            DrawGrid(grid, _drawingTrail);
        }
        
        public void ReceiveMessage(MessageWrapper messageWrapper) {
            if (messageWrapper.Message is not TurnFinishedMessage turnFinishedMessage) {
                Debug.LogError("Wrong net message");
                return;
            }
            
            var grid = _model.World.Entities.GetFirst<Grid>();
            var cellSize = grid.GetComponent<CellSizeComponent>().CellSize;
            var gridStart = grid.GetComponent<GridStartComponent>().GridStart;
            var worldPosition = GridUtils.GridIndexToWorldPosition(turnFinishedMessage.X, turnFinishedMessage.Y, gridStart, cellSize);
            var gameSide = (GameSide)turnFinishedMessage.GameSide;
            var gameObj = gameSide == GameSide.Cross ? _crossPrefab : _circlePrefab;
            Instantiate(gameObj, worldPosition, Quaternion.identity);
        }

        private void DrawGrid(Grid grid, GameObject trailPrefab) {
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

            var verticalFirstTrail = Instantiate(trailPrefab, verticalFirstStart, Quaternion.identity);
            var verticalSecondTrail = Instantiate(trailPrefab, verticalSecondStart, Quaternion.identity);
            var horizontalFirstTrail = Instantiate(trailPrefab, horizontalFirstStart, Quaternion.identity);
            var horizontalSecondTrail = Instantiate(trailPrefab, horizontalSecondStart, Quaternion.identity);
            
            DrawTo(verticalFirstEnd, verticalFirstTrail.transform);
            DrawTo(verticalSecondEnd, verticalSecondTrail.transform);
            DrawTo(horizontalFirstEnd, horizontalFirstTrail.transform);
            DrawTo(horizontalSecondEnd, horizontalSecondTrail.transform);
        }

        private void DrawTo(Vector2 to, Transform t) {
            var randomDuration = Random.Range(1f, 1.5f);
            t.DOMove(to, randomDuration);
        }
    }
}
