using UnityEngine;
using Leopotam.EcsLite;
using Construct.Systems;

namespace Construct {
    sealed class EcsStartup : MonoBehaviour {
        [SerializeField] private Material _greenTransparent;
        [SerializeField] private PlayerConnection _playerConnection; 

        EcsWorld _world;        
        IEcsSystems _systems;

        void Start () {
            _world = new EcsWorld ();
            _playerConnection.World = _world;
            _systems = new EcsSystems (_world);
            _systems
                .Add(new SingulaSystem(_world))
                .Add(new TriggerEnterSystem(_world, _greenTransparent))
                .Add(new TriggerExitSystem(_world))
                .Add(new JoinSingulaSystem(_world))
                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())
                
                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
        }

        void Update () {
            // process systems here.
            _systems?.Run();
        }

        void OnDestroy () {
            if (_systems != null) {
                // list of custom worlds will be cleared
                // during IEcsSystems.Destroy(). so, you
                // need to save it here if you need.
                _systems.Destroy();
                _systems = null;
            }
            
            // cleanup custom worlds here.
            
            // cleanup default world.
            if (_world != null) {
                _world.Destroy();
                _world = null;
            }
        }
    }
}