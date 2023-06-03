using Core.Managers;
using UnityEngine;

namespace Core.View {
    public class GridView : MonoBehaviour {
        [SerializeField] private GameModel _model;
        [SerializeField] private GameObject _crossPrefab;
        [SerializeField] private GameObject _circlePrefab;
        [SerializeField] private GameObject _drawingTrail;
        
        [SerializeField] private TrailRenderer _winningTrail;
        [SerializeField] private Color _circleColor;
        [SerializeField] private Color _crossColor;
        
        private GridGenerator _gridGenerator;
        private TurnFinishCrossCircleSpawner _turnFinishCrossCircleSpawner;
        private GameOverWinningLineDrawer _gameOverWinningLineDrawer;

        private void Start() {
            _gridGenerator = new GridGenerator(_model, _drawingTrail);
            _turnFinishCrossCircleSpawner = new TurnFinishCrossCircleSpawner(_model, _crossPrefab, _circlePrefab);
            _gameOverWinningLineDrawer = new GameOverWinningLineDrawer(_model, _winningTrail, _circleColor, _crossColor);
            
            _gridGenerator.Start();
            _turnFinishCrossCircleSpawner.Start();
            _gameOverWinningLineDrawer.Start();
        }
    }
}
