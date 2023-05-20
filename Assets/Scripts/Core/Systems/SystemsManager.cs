using System.Collections.Generic;
using Core.Managers;
using PoorMansECS.Systems;

namespace Core.Systems {
    public class SystemsManager : IUpdateable, IGizmoDrawable {
        private readonly PoorMansECS.Systems.Systems _systems;

        public SystemsManager(PoorMansECS.Entities.Entities entities, MessagesPipe messagesPipe) {
            var systemsContext = new SystemsContext(entities, messagesPipe);
            var systemsList = new List<ISystem>() {
                new GridGeneratorSystem(systemsContext)
            };
            _systems = new PoorMansECS.Systems.Systems(systemsList);
        }

        public void Start() {
            _systems.Start();
        }

        public void Update(float delta) {
            _systems.Update(delta);
        }

        public void DrawGizmos() {
            foreach (var system in _systems) {
                if (system is SystemBase systemBase)
                    systemBase.DrawGizmos();
            }
        }
    }
}