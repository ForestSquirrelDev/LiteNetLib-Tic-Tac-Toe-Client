using Core.Managers;
using PoorMansECS;
using Server.Shared.Network;
using ServerShared.Shared.Network;
using UnityEngine;

namespace Core.Systems {
    public class SystemsBuilder {
        private readonly World _world;
        private readonly OutgoingPacketsPipe _outgoingPacketsPipe;
        private readonly IncomingPacketsPipe _incomingPacketsPipe;
        private readonly Camera _mainCamera;

        public SystemsBuilder(World world, OutgoingPacketsPipe outgoingPacketsPipe, IncomingPacketsPipe incomingPacketsPipe, Camera mainCamera) {
            _world = world;
            _outgoingPacketsPipe = outgoingPacketsPipe;
            _incomingPacketsPipe = incomingPacketsPipe;
            _mainCamera = mainCamera;
        }

        public void Build() {
            _world.CreateSystem<GridGeneratorSystem>().InjectDependencies(_incomingPacketsPipe);
            _world.CreateSystem<JoinHandlerSystem>().InjectDependencies(_incomingPacketsPipe, _outgoingPacketsPipe);
            _world.CreateSystem<PlayerInputSystem>().InjectDependencies(_mainCamera, _outgoingPacketsPipe);
        }
    }
}
