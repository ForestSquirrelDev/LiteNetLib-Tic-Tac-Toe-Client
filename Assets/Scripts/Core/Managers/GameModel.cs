using System;
using System.Linq;
using Core.Entities;
using Core.Systems;
using PoorMansECS;
using Server.Shared.Network;
using ServerShared.Shared.Network;
using UnityEngine;

namespace Core.Managers {
    public class GameModel : MonoBehaviour {
        [SerializeField] private Camera _mainCamera;
        
        public World World { get; private set; }
        public ConnectionManager ConnectionManager { get; private set; }
        public GameLoop GameLoop { get; private set; }
        public OutgoingMessagesPipe OutgoingMessagesPipe { get; private set; }
        public IncomingMessagesPipe IncomingMessagesPipe { get; private set; }
    
        private void Awake() {
            World = new World();
            GameLoop = new GameLoop();
            IncomingMessagesPipe = new IncomingMessagesPipe();
            OutgoingMessagesPipe = new OutgoingMessagesPipe(IncomingMessagesPipe);
            ConnectionManager = new ConnectionManager(IncomingMessagesPipe);
            
            var systemsBuilder = new SystemsBuilder(World, OutgoingMessagesPipe, IncomingMessagesPipe, _mainCamera);
            var entitiesBuilder = new EntitiesBuilder(World);
            systemsBuilder.Build();
            entitiesBuilder.Build();

            GameLoop.AddUpdateable(ConnectionManager);
            GameLoop.AddUpdateable(World);

#if UNITY_EDITOR
            GameLoop.AddGizmoDrawables(World.Systems.GetAll().OfType<IGizmoDrawable>());
#endif
        }

        private void Start() {
            ConnectionManager.Start();
            World.Start();
            
            ConnectionManager.Connect();
        }

        private void Update() {
            GameLoop.Update(Time.deltaTime);
        }

        private void OnDestroy() {
            ConnectionManager.Dispose();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (!Application.isPlaying) return;
            GameLoop.DrawGizmos();
        }
#endif
    }
}
