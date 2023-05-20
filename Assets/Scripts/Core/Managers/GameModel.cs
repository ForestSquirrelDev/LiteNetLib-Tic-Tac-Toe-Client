using System;
using Core.Systems;
using LiteNetLib;
using UnityEngine;

namespace Core.Managers {
    public class GameModel : MonoBehaviour {
        public event Action Started;
    
        public ConnectionManager ConnectionManager { get; private set; }
        public MessagesPipe MessagesPipe { get; private set; }
        public PoorMansECS.Entities.Entities Entities { get; private set; }
        public SystemsManager SystemsManager { get; private set; }
        public GameLoop GameLoop { get; private set; }
    
        private void Awake() {
            ConnectionManager = new ConnectionManager();
            Entities = new PoorMansECS.Entities.Entities();
            GameLoop = new GameLoop();
            MessagesPipe = new MessagesPipe(ConnectionManager);
            SystemsManager = new SystemsManager(Entities, MessagesPipe);
        }

        private void Start() {
            GameLoop.AddUpdateable(SystemsManager);
            GameLoop.AddUpdateable(ConnectionManager);
            GameLoop.AddGizmoDrawable(SystemsManager);
        
            SystemsManager.Start();
            Started?.Invoke();
        }

        private void Update() {
            GameLoop.Update(Time.unscaledDeltaTime);
            if (Input.GetKeyDown(KeyCode.H)) {
                ConnectionManager.Connect();
            }
        }

        private void OnDestroy() {
        }
    
#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (!Application.isPlaying) return;
            GameLoop.DrawGizmos();
        }
#endif
    }
}
