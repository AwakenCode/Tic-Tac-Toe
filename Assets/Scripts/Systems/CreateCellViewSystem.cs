using UnityEngine;
using Common;
using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Views;

namespace Systems
{
    public class CreateCellViewSystem : IEcsInitSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsCustomInject<Configuration> _configuration = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default; 

        private EcsFilter _cells;
        private EcsPool<Position> _positionPool;
        private EcsPool<CellViewRef> _cellViewRefPool;

        public void Init(IEcsSystems systems)
        {
            _positionPool = _world.Value.GetPool<Position>();
            _cellViewRefPool = _world.Value.GetPool<CellViewRef>();
            
            _cells = _world.Value
                .Filter<Cell>()
                .Inc<Position>()
                .Exc<CellViewRef>()
                .End();
            
            foreach (int entity in _cells)
            {
                var position = _positionPool.Get(entity).Value;
                float positionX = position.x + _configuration.Value.Offset.x * position.x;
                float positionY = position.y + _configuration.Value.Offset.y * position.y;
                var cellView = Create(new Vector2(positionX, positionY), entity);
                _cellViewRefPool.Add(entity).Value = cellView;
            }
        }

        private CellView Create(Vector2 position, int entity)
        {
            var cellView = Object.Instantiate(_configuration.Value.CellView, _sceneData.Value.GridTransform);
            cellView.transform.position = new Vector3(position.x, position.y); 
            cellView.Entity = entity;
            
            return cellView;
        }
    }
}