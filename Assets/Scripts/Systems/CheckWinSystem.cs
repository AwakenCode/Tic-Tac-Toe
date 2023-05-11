using Common;
using Components;
using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Systems
{
    internal class CheckWinSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsCustomInject<Configuration> _configuration = default;
        private readonly EcsCustomInject<GameState> _gameState = default;

        private EcsPool<Position> _positionPool;
        private EcsPool<WinnerSign> _winnerSignPool;
        private EcsFilter _checkWinEvents;
        
        public void Init(IEcsSystems systems)
        {
            _positionPool = _world.Value.GetPool<Position>();
            _winnerSignPool = _world.Value.GetPool<WinnerSign>();
            _checkWinEvents = _world.Value
                .Filter<CheckWinEvent>()
                .Inc<Position>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _checkWinEvents)
            {
                var position = _positionPool.Get(entity).Value;
                int chainLength = _gameState.Value.Cells.GetLongestChain(position);

                if (chainLength >= _configuration.Value.ChainLength) 
                    _winnerSignPool.Add(entity);
            }
        }
    }
}