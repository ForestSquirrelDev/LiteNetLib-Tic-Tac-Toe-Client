using Core.Components;
using Core.Managers;
using Server.Shared.Network;
using ServerShared.Shared.Network;
using UnityEngine;
using Utils;
using Grid = Core.Entities.Grid;

namespace Core.View {
    public class TurnFinishCrossCircleSpawner : INetMessageListener {
        private readonly GameModel _model;
        private readonly GameObject _crossPrefab;
        private readonly GameObject _circlePrefab;
        
        public TurnFinishCrossCircleSpawner(GameModel model, GameObject crossPrefab, GameObject circlePrefab) {
            _model = model;
            _crossPrefab = crossPrefab;
            _circlePrefab = circlePrefab;
        }

        public void Start() {
            _model.IncomingMessagesPipe.Register(MessageType.TurnFinished, this);
        }
        
        public void ReceiveMessage(MessageWrapper messageWrapper) {
            if (messageWrapper.Message is TurnFinishedMessage turnFinishedMessage) {
                ProcessTurnFinished(_model, _crossPrefab, _circlePrefab, turnFinishedMessage);
            }
        }

        private void ProcessTurnFinished(GameModel model, GameObject crossPrefab, GameObject circlePrefab, TurnFinishedMessage turnFinishedMessage) {
            var grid = model.World.Entities.GetFirst<Grid>();
            var cellSize = grid.GetComponent<CellSizeComponent>().CellSize;
            var gridStart = grid.GetComponent<GridStartComponent>().GridStart;
            var worldPosition = GridUtils.GridIndexToWorldPosition(turnFinishedMessage.X, turnFinishedMessage.Y, gridStart, cellSize);
            var gameSide = (GameSide)turnFinishedMessage.GameSide;
            var gameObj = gameSide == GameSide.Cross ? crossPrefab : circlePrefab;
            Object.Instantiate(gameObj, worldPosition, Quaternion.identity);
        }
    }
}
