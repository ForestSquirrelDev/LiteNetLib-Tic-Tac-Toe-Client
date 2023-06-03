using System.Collections.Generic;
using System.Linq;
using Core.Components;
using Core.Managers;
using DG.Tweening;
using Server.Shared.Network;
using ServerShared.Shared.Network;
using UnityEngine;
using Utils;
using Grid = Core.Entities.Grid;

namespace Core.View {
    public class GameOverWinningLineDrawer : INetMessageListener {
        private readonly GameModel _model;
        private readonly TrailRenderer _trailPrefab;
        private readonly Dictionary<Vector2Extensions.Direction, Vector2> _startOffsets;
        private readonly Color _circleColor;
        private readonly Color _crossColor;

        private const float _offsetLength = 2.5f;
        
        public GameOverWinningLineDrawer(GameModel model, TrailRenderer trailPrefab, Color circleColor, Color crossColor) {
            _model = model;
            _trailPrefab = trailPrefab;
            _circleColor = circleColor;
            _crossColor = crossColor;
            _startOffsets = GetDirectionalStartOffsets(_offsetLength);
        }

        public void Start() {
            _model.IncomingMessagesPipe.Register(MessageType.GameOverMessage, this);
        }

        public void ReceiveMessage(MessageWrapper messageWrapper) {
            if (messageWrapper.Message is GameOverMessage gameOverMessage) {
                DrawWinner(_trailPrefab, gameOverMessage, _startOffsets, _model);
            }
        }

        private void DrawWinner(TrailRenderer trailPrefab, GameOverMessage gameOverMessage, IReadOnlyDictionary<Vector2Extensions.Direction, Vector2> offsets, GameModel model) {
            var winningCombination = gameOverMessage.WinningCombination;
            var grid = model.World.Entities.GetFirst<Grid>();
            var cells = grid.GetComponent<GridCellsComponent>().Cells;
            var gridStart = grid.GetComponent<GridStartComponent>().GridStart;
            var cellSize = grid.GetComponent<CellSizeComponent>().CellSize;
            
            var firstIndices = winningCombination.First();
            var lastIndices = winningCombination.Last();
            var startCell = cells[firstIndices.row, firstIndices.column];
            var endCell = cells[lastIndices.row, lastIndices.column];
            
            var startPosition = GridUtils.GridIndexToWorldPosition((int)startCell.CellIndex.x, (int)startCell.CellIndex.y, gridStart, cellSize);
            var endPosition = GridUtils.GridIndexToWorldPosition((int)endCell.CellIndex.x, (int)endCell.CellIndex.y, gridStart, cellSize);

            var direction = Vector2Extensions.GetDirection(startPosition, endPosition);
            var startOffset = offsets[direction];
            var endOffset = startOffset.Mirror();
            
            var winningColor = (GameSide)gameOverMessage.Winner == GameSide.Cross ? _crossColor : _circleColor;
            SetTrailColor(trailPrefab, winningColor);
            MoveTrailPrefab(trailPrefab.gameObject, startPosition + startOffset, endPosition + endOffset);
        }

        private void SetTrailColor(TrailRenderer trailRenderer, Color winningColor) {
            var color = winningColor;
            trailRenderer.startColor = color;
            trailRenderer.endColor = color;
        }

        private void MoveTrailPrefab(GameObject trailPrefab, Vector2 start, Vector2 end) {
            trailPrefab.gameObject.SetActive(false);
            trailPrefab.transform.position = start;
            trailPrefab.gameObject.SetActive(true);
            trailPrefab.transform.DOMove(end, Random.Range(1f, 1.5f));
        }
        
        private Dictionary<Vector2Extensions.Direction, Vector2> GetDirectionalStartOffsets(float offsetLength) {
            return new Dictionary<Vector2Extensions.Direction, Vector2> {
                [Vector2Extensions.Direction.Horizontal] = new Vector2(-offsetLength, 0f),
                [Vector2Extensions.Direction.Vertical] = new Vector2(0f, offsetLength),
                [Vector2Extensions.Direction.DiagonalLeft] = new Vector2(offsetLength, offsetLength),
                [Vector2Extensions.Direction.DiagonalRight] = new Vector2(-offsetLength, offsetLength),
                [Vector2Extensions.Direction.Undefined] = new Vector2(0f, 0f)
            };
        }
    }
}
