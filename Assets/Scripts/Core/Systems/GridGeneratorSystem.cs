﻿using Core.Components;
using Core.Systems.Events;
using PoorMansECS.Systems;
using ServerShared.Shared.Network;
using UnityEditor;
using UnityEngine;
using Grid = Core.Entities.Grid;
using MessageType = ServerShared.Shared.Network.MessageType;

namespace Core.Systems {
    public class GridGeneratorSystem : SystemBase, IGizmoDrawable, INetMessageListener {
        private IncomingMessagesPipe _incomingMessagesPipe;
        
        private const float _cellSize = 3.2f;
        private Vector3 _gridStart;
        private int _gridSizeXStartingFromZero;
        private int _gridSizeYStartingFromZero;
        
        public GridGeneratorSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(IncomingMessagesPipe incomingMessagesPipe) {
            _incomingMessagesPipe = incomingMessagesPipe;
        }

        protected override void OnStart() {
            _incomingMessagesPipe.Register(MessageType.GameStartedMessage, this);
        }
        
        public void ReceiveMessage(MessageWrapper messageWrapper) {
            if (messageWrapper.Message is not GameStartedMessage gameStartedMessage) {
                Debug.LogError("Wrong message in game starter system");
                return;
            }

            _gridSizeXStartingFromZero = gameStartedMessage.GridSizeX - 1;
            _gridSizeYStartingFromZero = gameStartedMessage.GridSizeY - 1;
            var (gridStart, gridCellsComponent) = GenerateGrid(new Vector2(_gridSizeXStartingFromZero, _gridSizeYStartingFromZero));
            var grid = _context.World.Entities.GetFirst<Grid>();
            grid.SetComponent(gridCellsComponent);
            grid.SetComponent(new CellSizeComponent(_cellSize));
            grid.SetComponent(new GridStartComponent(gridStart));
            _gridStart = gridStart;
            _context.EventBus.SendEvent(new GridGeneratedEvent());
        }

        public void DrawGizmos() {
#if UNITY_EDITOR
            Handles.color = Color.red;
            Handles.DrawSolidDisc(_gridStart, Vector3.forward, .3f);
        
            Handles.color = Color.cyan;
            Handles.DrawSolidDisc(Vector3.zero, Vector3.forward, .3f);

            var centerPosition = Vector3.zero;
            var bottomLeftCorner = new Vector3(
                centerPosition.x - (_gridSizeXStartingFromZero* 0.5f * _cellSize),
                centerPosition.y - (_gridSizeYStartingFromZero * 0.5f) * _cellSize,
                centerPosition.z);
        
            Handles.color = Color.red;
            Handles.DrawSolidDisc(bottomLeftCorner, Vector3.forward, .3f);
        
            var bottomRightCorner = new Vector3(
                centerPosition.x + (_gridSizeXStartingFromZero * 0.5f * _cellSize),
                centerPosition.y - (_gridSizeYStartingFromZero * 0.5f * _cellSize),
                centerPosition.z);
            Handles.DrawSolidDisc(bottomRightCorner, Vector3.forward, .3f);
        
            var upperRightCorner = new Vector3(
                centerPosition.x + (_gridSizeXStartingFromZero * 0.5f * _cellSize) ,
                centerPosition.y + (_gridSizeYStartingFromZero * 0.5f * _cellSize) ,
                centerPosition.z);
            Handles.DrawSolidDisc(upperRightCorner, Vector3.forward, .3f);
#endif
        }

        private (Vector3 gridStart, GridCellsComponent gridCellsComponent) GenerateGrid(Vector2 gridSize)
        {
            Vector3 centerPosition = Vector3.zero;
            Vector3 startPos = new Vector3(
                centerPosition.x - (gridSize.x * 0.5f * _cellSize),
                centerPosition.y + (gridSize.y * 0.5f * _cellSize),
                centerPosition.z);

            var gridCells = new GridCell[_gridSizeXStartingFromZero + 1, _gridSizeYStartingFromZero + 1];
            for (int x = 0; x <= _gridSizeXStartingFromZero; x++)
            {
                for (int y = 0; y <= _gridSizeYStartingFromZero; y++) {
                    var cellPosition = new Vector3(startPos.x + x * _cellSize, startPos.y - y * _cellSize, startPos.z);
                    var cellIndex = new Vector2(x, y);
                    var cell = new GridCell(_cellSize, cellPosition, cellIndex);
                    gridCells[x, y] = cell;
                }
            }
            var cellsComponent = new GridCellsComponent(gridCells);
            
            return (startPos, cellsComponent);
        }
        
        protected override void OnUpdate(float delta) { }
        protected override void OnStop() { }
    }
}
