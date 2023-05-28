using PoorMansECS.Systems;
using Server.Shared.Network;
using UnityEngine;

namespace Core.Systems {
    public class PlayerInputSystem : SystemBase {
        private Camera _mainCamera;
        private OutgoingPacketsPipe _outgoingPacketsPipe;
        
        public PlayerInputSystem(SystemsContext context) : base(context) {
        }

        public void InjectDependencies(Camera mainCamera, OutgoingPacketsPipe outgoingPacketsPipe) {
            _mainCamera = mainCamera;
            _outgoingPacketsPipe = outgoingPacketsPipe;
        }

        protected override void OnUpdate(float delta) {
            if (Input.GetMouseButtonDown(0)) {
                var mousePos = Input.mousePosition;
                var coords = _mainCamera.ScreenPointToRay(mousePos);
            }
        }
        
        protected override void OnStart() { }
        protected override void OnStop() { }
    }
}
