using System.Collections.Generic;
using Core.Systems;
using PoorMansECS.Systems;

namespace Core.Managers {
    public class GameLoop {
        private readonly HashSet<IUpdateable> _updateables;
        private readonly HashSet<IGizmoDrawable> _gizmoDrawables;

        public GameLoop() {
            _updateables = new HashSet<IUpdateable>();
            _gizmoDrawables = new HashSet<IGizmoDrawable>();
        }

        public void Update(float deltaTime) {
            foreach (var updateable in _updateables) {
                updateable.Update(deltaTime);
            }
        }

        public void DrawGizmos() {
            foreach (var gizmoDrawable in _gizmoDrawables) {
                gizmoDrawable.DrawGizmos();
            }
        }

        public void AddUpdateable(IUpdateable updateable) {
            _updateables.Add(updateable);
        }

        public void AddGizmoDrawable(IGizmoDrawable drawable) {
            _gizmoDrawables.Add(drawable);
        }
    }
}
