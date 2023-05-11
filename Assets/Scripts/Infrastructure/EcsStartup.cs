using Common;
using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.UnityEditor;
using Systems;
using UnityEngine;

namespace Infrastructure
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private Configuration _configuration;
        [SerializeField] private SceneData _sceneData;
        
        private EcsWorld _world;
        private EcsSystems _systems;
        private GameState _gameState;

        private void Start()
        { 
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _gameState = new GameState();
            
            AddSystems();
            AddOneFrames();
            AddInjections();
            
            _systems.Init();
            GameExtensions.Init(_world);
        }
        
        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if(_systems == null) return;
            
            _systems.Destroy();
            _world.Destroy();
            _systems = null;
            _world = null;
        }

        private void AddSystems()
        {
            _systems
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                .Add(new InitializeCellsSystem())
                .Add(new CreateCellViewSystem())
                .Add(new SetCameraSystem())
                .Add(new InputSystem())
                .Add(new AnalyzeClickSystem())
                .Add(new CreateTakenSingViewSystem())
                .Add(new CheckWinSystem())
                .Add(new WinScreenSystem())
                .Add(new DrawScreenSystem());
        }

        private void AddOneFrames()
        {
            _systems
                .DelHere<UpdateCameraEvent>()
                .DelHere<ClickedEvent>()
                .DelHere<CheckWinEvent>();
        }

        private void AddInjections()
        {
            _systems
                .Inject(_configuration)
                .Inject(_sceneData)
                .Inject(_gameState);
        }
    }
}