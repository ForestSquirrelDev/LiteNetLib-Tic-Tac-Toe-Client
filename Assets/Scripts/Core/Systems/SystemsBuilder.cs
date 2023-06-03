using Core.Managers;
using PoorMansECS;
using Server.Shared.Network;
using ServerShared.Shared.Network;
using UnityEngine;

namespace Core.Systems {
    public class SystemsBuilder {
        private readonly World _world;
        private readonly OutgoingMessagesPipe _outgoingMessagesPipe;
        private readonly IncomingMessagesPipe _incomingMessagesPipe;
        private readonly Camera _mainCamera;

        public SystemsBuilder(World world, OutgoingMessagesPipe outgoingMessagesPipe, IncomingMessagesPipe incomingMessagesPipe, Camera mainCamera) {
            _world = world;
            _outgoingMessagesPipe = outgoingMessagesPipe;
            _incomingMessagesPipe = incomingMessagesPipe;
            _mainCamera = mainCamera;
        }

        public void Build() {
            _world.CreateSystem<GridGeneratorSystem>().InjectDependencies(_incomingMessagesPipe);
            _world.CreateSystem<JoinHandlerSystem>().InjectDependencies(_incomingMessagesPipe, _outgoingMessagesPipe);
            _world.CreateSystem<PlayerInputSystem>().InjectDependencies(_mainCamera, _outgoingMessagesPipe);
        }
    }
}
