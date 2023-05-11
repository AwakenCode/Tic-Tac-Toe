using Components;
using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Systems
{
    public class AnalyzeClickSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsCustomInject<GameState> _gameState;

        private EcsPool<TakenCell> _takenCellPool;
        private EcsPool<CheckWinEvent> _checkWinEventPool;
        private EcsFilter _clickedCells;
        
        public void Init(IEcsSystems systems)
        {
            _takenCellPool = _world.Value.GetPool<TakenCell>();
            _checkWinEventPool = _world.Value.GetPool<CheckWinEvent>();
            _clickedCells = _world.Value
                .Filter<Cell>()
                .Inc<ClickedEvent>()
                .Exc<TakenCell>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _clickedCells)
            {
                _takenCellPool.Add(entity).Value = _gameState.Value.CurrentSign;
                _checkWinEventPool.Add(entity);
                _gameState.Value.CurrentSign = _gameState.Value.CurrentSign == SignType.Circle ? SignType.Cross : SignType.Circle;
            }
        }
    }
}