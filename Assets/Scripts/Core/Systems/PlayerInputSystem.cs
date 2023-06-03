using Core.Components;
using Core.Entities;
using PoorMansECS.Systems;
using Server.Shared.Network;
using ServerShared.Shared.Network;
using UnityEngine;
using Utils;
using Grid = Core.Entities.Grid;

namespace Core.Systems {
    public class PlayerInputSystem : SystemBase {
        private Camera _mainCamera;
        private OutgoingMessagesPipe _outgoingMessagesPipe;
        
        public PlayerInputSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(Camera mainCamera, OutgoingMessagesPipe outgoingMessagesPipe) {
            _mainCamera = mainCamera;
            _outgoingMessagesPipe = outgoingMessagesPipe;
        }

        protected override void OnUpdate(float delta) {
            if (!Input.GetMouseButtonDown(0)) return;
            
            var mousePos = Input.mousePosition;
            var ray = _mainCamera.ScreenPointToRay(mousePos);
            var (x, y) = MouseToGrid(ray.origin, _context);
            SendInputMessage(_outgoingMessagesPipe, _context, x, y);
        }

        private (int xIndex, int yIndex) MouseToGrid(Vector3 worldPoint, SystemsContext context) {
            var grid = context.World.Entities.GetFirst<Grid>();
            var gridStart = grid.GetComponent<GridStartComponent>().GridStart;
            var cellSize = grid.GetComponent<CellSizeComponent>().CellSize;
            var gridPosition = GridUtils.SnapPointToGrid(worldPoint, gridStart, cellSize);
            return (gridPosition.xIndex, gridPosition.yIndex);
        }

        private async void SendInputMessage(OutgoingMessagesPipe outgoingMessagesPipe, SystemsContext context, int x, int y) {
            var room = context.World.Entities.GetFirst<Room>();
            var server = room.GetComponent<RoomServerComponent>().Server;
            var inputMessage = new InputMessage(x, y);
            var result = await outgoingMessagesPipe.SendAndWaitForResponse(server, inputMessage, MessageType.InputResponseMessage, 2);
            if (!result.success) {
                Debug.LogError($"Input message failed to reach");
                return;
            }

            var response = result.response;
            if (response.Message is not InputResponseMessage inputResponse) {
                Debug.LogError("Wrong message type");
                return;
            }

            if (!inputResponse.Success) {
                Debug.Log($"Input rejected: {inputResponse.ResponseReason.ToString()}");
            }
        }
        
        protected override void OnStart() { }
        protected override void OnStop() { }
    }
}
