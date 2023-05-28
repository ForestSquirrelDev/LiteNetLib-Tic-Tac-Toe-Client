using Core.Components;
using PoorMansECS;
using ServerShared.Shared.Network;

namespace Core.Entities {
    public class EntitiesBuilder {
        private readonly World _world;

        public EntitiesBuilder(World world) {
            _world = world;
        }

        public void Build() {
            BuildGrid(_world);
            BuildRoom(_world);
            BuildPlayer(_world);
        }

        private void BuildGrid(World world) {
            var grid = world.CreateEntity<Grid>();
            grid.SetComponent(new GridCellsComponent());
        }

        private void BuildRoom(World world) {
            world.CreateEntity<Room>().SetComponent(new RoomServerComponent());
        }

        private void BuildPlayer(World world) {
            world.CreateEntity<Player>().SetComponent(new GameSideComponent(GameSide.None));
        }
    }
}
