using PoorMansECS.Components;
using ServerShared.Shared.Network;

namespace Core.Components {
    public readonly struct GameSideComponent : IComponentData {
        public GameSide GameSide { get; }

        public GameSideComponent(GameSide gameSide) {
            GameSide = gameSide;
        }
    }
}