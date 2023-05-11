using Common;
using Components;
using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Views;

namespace Systems
{
    public class InputSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;
        private readonly EcsCustomInject<GameState> _gameState = default;

        private EcsPool<ClickedEvent> _clickedEventPool;

        public void Init(IEcsSystems systems)
        {
            _clickedEventPool = _world.Value.GetPool<ClickedEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            if(Input.GetMouseButtonDown(0) == false) return;
            if(_gameState.Value.IsGameOver) return;

            var ray = _sceneData.Value.MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit) == false) return;
            
            if (hit.collider.TryGetComponent(out CellView cellView)) 
                _clickedEventPool.Add(cellView.Entity);
        }
    }
}