using Core.Components;
using Core.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Grid = Core.Entities.Grid;

namespace Core.View {
    public class GridView : MonoBehaviour {
        [SerializeField] private GameObject _squarePrefab; 
        [SerializeField] private GameModel _model;

        private void Awake() {
            _model.Started += OnStarted;
        }

        private void OnStarted() {
            _model.Started -= OnStarted;
        
            var grid = _model.Entities.GetFirst<Grid>();
            var gridCells = grid.GetComponent<GridCellsComponent>();
            foreach (var cell in gridCells.Cells) {
                Instantiate(_squarePrefab, cell.LocalPosition, Quaternion.identity, transform);
            }
        }
    }
}
