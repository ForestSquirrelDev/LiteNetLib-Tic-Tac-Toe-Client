using Core.Components;
using Core.Managers;
using Core.Systems.Events;
using PoorMansECS.Systems;
using UnityEngine;
using Grid = Core.Entities.Grid;

namespace Core.View {
    public class GridView : MonoBehaviour, ISystemsEventListener {
        [SerializeField] private GameObject _squarePrefab; 
        [SerializeField] private GameModel _model;

        private void Start() {
            _model.World.EventBus.Subscribe<GridGeneratedEvent>(this);
        }

        public void ReceiveEvent<T>(T systemEvent) where T : ISystemEvent {
            if (systemEvent is not GridGeneratedEvent) {
                Debug.LogError("Wrong type of event in GridView");
                return;
            }
            
            var grid = _model.World.Entities.GetFirst<Grid>();
            var gridCells = grid.GetComponent<GridCellsComponent>();
            foreach (var cell in gridCells.Cells) {
                Instantiate(_squarePrefab, cell.LocalPosition, Quaternion.identity, transform);
            }
        }
    }
}
