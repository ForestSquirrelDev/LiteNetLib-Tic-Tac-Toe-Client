using LiteNetLib;
using PoorMansECS.Components;

namespace Core.Components {
    public readonly struct RoomServerComponent : IComponentData {
        public NetPeer Server { get; }

        public RoomServerComponent(NetPeer server) {
            Server = server;
        }
    }
}