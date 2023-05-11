using Common;
using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Systems
{
    public class SetCameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsCustomInject<Configuration> _configuration = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private EcsFilter _updateCameraEvents;
        
        public void Init(IEcsSystems systems)
        {
            _updateCameraEvents = _world.Value
                .Filter<UpdateCameraEvent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            if (_updateCameraEvents.GetEntitiesCount() == 0) return;
            
            uint height = _configuration.Value.LevelHeight;
            uint width = _configuration.Value.LevelWidth;
            var camera = _sceneData.Value.MainCamera;

            float sizeX = width / 2f + (width - 1) * _configuration.Value.Offset.x / 2;
            float sizeY = height / 2f + (height - 1) * _configuration.Value.Offset.y / 2;
            camera.orthographicSize = sizeY + 0.1f;
            camera.transform.position = new Vector3(sizeX, sizeY);
        }
    }
}