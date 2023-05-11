using Common;
using Components;
using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Systems
{
    public class WinScreenSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;
        private readonly EcsCustomInject<GameState> _gameState = default;

        private EcsPool<TakenCell> _takenCellPool;
        private EcsFilter _winners;
        
        public void Init(IEcsSystems systems)
        {
            _takenCellPool = _world.Value.GetPool<TakenCell>();
            _winners = _world.Value
                .Filter<WinnerSign>()
                .Inc<TakenCell>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_gameState.Value.IsGameOver) return;

            foreach (int cell in _winners)
            {
                _sceneData.Value.GameOverWindow.SetWinner(_takenCellPool.Get(cell).Value);
                _sceneData.Value.GameOverWindow.Show(_gameState.Value);
            }
        }
    }
}