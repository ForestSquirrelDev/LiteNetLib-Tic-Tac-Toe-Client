using Core.Managers;
using PoorMansECS;
using Server.Shared.Network;
using ServerShared.Shared.Network;

namespace Core.Systems {
    public class SystemsBuilder {
        private readonly World _world;

        public SystemsBuilder(World world) {
            _world = world;
        }

        public void Build(OutgoingPacketsPipe outgoingPacketsPipe, IncomingPacketsPipe incomingPacketsPipe) {
            _world.CreateSystem<GridGeneratorSystem>().InjectDependencies(incomingPacketsPipe);
            _world.CreateSystem<JoinHandlerSystem>().InjectDependencies(incomingPacketsPipe, outgoingPacketsPipe);
        }
    }
}
