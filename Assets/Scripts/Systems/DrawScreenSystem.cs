using Common;
using Components;
using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Systems
{
    public class DrawScreenSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;
        private readonly EcsCustomInject<GameState> _gameState = default;

        private EcsFilter _freeCells;
        private EcsFilter _winners;
        
        public void Init(IEcsSystems systems)
        {
            _freeCells = _world.Value
                .Filter<Cell>()
                .Exc<TakenCell>()
                .End();
            _winners = _world.Value
                .Filter<WinnerSign>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_gameState.Value.IsGameOver) return;
            if (_winners.GetEntitiesCount() > 0 || _freeCells.GetEntitiesCount() > 0) return;
            
            _sceneData.Value.GameOverWindow.SetDraw();
            _sceneData.Value.GameOverWindow.Show(_gameState.Value);
        }
    }
}