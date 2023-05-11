using System;
using Common;
using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

namespace Systems
{
    public class CreateTakenSingViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsCustomInject<Configuration> _configuration = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private EcsFilter _takenCells;
        private EcsPool<TakenCell> _takenCellPool;
        private EcsPool<SignViewRef> _signViewRefPool;
        private EcsPool<CellViewRef> _cellViewRefPool;

        public void Init(IEcsSystems systems)
        {
            _takenCellPool = _world.Value.GetPool<TakenCell>();
            _signViewRefPool = _world.Value.GetPool<SignViewRef>();
            _cellViewRefPool = _world.Value.GetPool<CellViewRef>();
            
            _takenCells = _world.Value
                .Filter<TakenCell>()
                .Inc<CellViewRef>()
                .Exc<SignViewRef>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _takenCells)
            {
                var signType = _takenCellPool.Get(entity).Value;
                var position = _cellViewRefPool.Get(entity).Value.transform.position;
                var signView = CreateSingView(signType, position);
                _signViewRefPool.Add(entity).Value = signView;
            }
        }

        private SignView CreateSingView(SignType signType, Vector2 position)
        {
            SignView signView = signType switch
            {
                SignType.Cross => Object.Instantiate(_configuration.Value.CrossSingView, _sceneData.Value.GridTransform),
                SignType.Circle => Object.Instantiate(_configuration.Value.CircleSingView, _sceneData.Value.GridTransform),
                _ => throw new ArgumentOutOfRangeException()
            };

            signView.transform.position = position;

            return signView;
        }
    }
}