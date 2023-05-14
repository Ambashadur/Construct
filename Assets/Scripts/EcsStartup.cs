using UnityEngine;
using Leopotam.EcsLite;
using Construct.Systems;
using Construct.Services.Impl;

namespace Construct
{
    public sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private PlayerConnection _playerConnection;
        [SerializeField] private LayerMask _singulaLayer;

        EcsWorld _world;
        IEcsSystems _systems;

        void Start ()
        {
            _world = new EcsWorld ();
            _playerConnection.World = _world;
            _systems = new EcsSystems (_world);
            _systems
                .Add(new SingulaSystem(_world))
                .Add(new JoinSingulaSystem(_world))
                .Add(new DetachSingulaSystem(_world))
                .Add(new ReleaseFromHandSystem(_world))
                .Add(new TakeToHandSystem(_world))
                .Add(new EndFocusSystem(_world))
                .Add(new StartFocusSystem(_world))
                .Add(new LoadConventusSystem(_world, _singulaLayer, new DbController()))
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
        }

        void Update ()
        {
            _systems?.Run();
        }

        void OnDestroy ()
        {
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