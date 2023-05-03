using UnityEngine;
using Leopotam.EcsLite;
using Construct.Systems;

namespace Construct {
    sealed class EcsStartup : MonoBehaviour {
        [SerializeField] private Material _greenTransparent;
        [SerializeField] private PlayerConnection _playerConnection; 
        [SerializeField] private Material _greenOutline;

        EcsWorld _world;        
        IEcsSystems _systems;

        void Start () {
            _world = new EcsWorld ();
            _playerConnection.World = _world;
            _systems = new EcsSystems (_world);
            _systems
                .Add(new SingulaSystem(_world, _greenOutline))
                .Add(new TriggerEnterSystem(_world, _greenTransparent))
                .Add(new TriggerExitSystem(_world))
                .Add(new JoinSingulaSystem(_world))
                .Add(new StartFocusSystem(_world))
                .Add(new EndFocusSystem(_world))
                .Add(new DetachSingulaSystem(_world))
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
        }

        void Update () {
            _systems?.Run();
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy();
                _systems = null;
            }
            
            if (_world != null) {
                _world.Destroy();
                _world = null;
            }
        }
    }
}