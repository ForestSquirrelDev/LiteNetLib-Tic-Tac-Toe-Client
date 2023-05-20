using PoorMansECS.Systems;

namespace Core.Systems {
    public abstract class SystemBase : ISystem {
        protected readonly SystemsContext _context;
    
        protected SystemBase(SystemsContext context) {
            _context = context;
        }
    
        public void Update(float delta) {
            OnUpdate(delta);
        }

        public void Start() {
            OnStart();
        }

        public void DrawGizmos() {
            OnDrawGizmos();
        }

        protected abstract void OnUpdate(float delta);
        protected abstract void OnStart();
        protected abstract void OnDrawGizmos();
    }
}
