using Common;
using Components;
using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Systems
{
    public class InitializeCellsSystem : IEcsInitSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsCustomInject<Configuration> _configuration = default;
        private readonly EcsCustomInject<GameState> _gameState = default;

        private EcsPool<Cell> _cellPool;
        private EcsPool<Position> _positionPool;
        private EcsPool<UpdateCameraEvent> _updateCameraEventPool;

        public void Init(IEcsSystems systems)
        {
            _cellPool = _world.Value.GetPool<Cell>();
            _positionPool = _world.Value.GetPool<Position>();
            _updateCameraEventPool = _world.Value.GetPool<UpdateCameraEvent>();
            
            for (var x = 0; x < _configuration.Value.LevelWidth; x++)
            {
                for (var y = 0; y < _configuration.Value.LevelHeight; y++)
                {
                    int cell = _world.Value.NewEntity();
                    var position = new Vector2Int(x, y);
                    _positionPool.Add(cell).Value = position;
                    _gameState.Value.Cells[position] = cell;
                    _cellPool.Add(cell);
                }
            }

            int updateCameraEvent = _world.Value.NewEntity();
            _updateCameraEventPool.Add(updateCameraEvent);
        }
    }
}