using System.Linq;
using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;
using Construct.Services;
using Construct.Views;
using Construct.Model;

namespace Construct.Systems {
    sealed class LoadConventusSystem : IEcsRunSystem {
        private readonly EcsWorld _world;
        private readonly IDbController _controller;
        private readonly EcsFilter _loadConventusFilter;
        private readonly EcsPool<Conventus> _conventusPool;
        private readonly EcsPool<LoadConventus> _loadConventusPool;
        private readonly EcsPool<Singula> _singulaPool;

        private readonly Material _greenOutline;

        public LoadConventusSystem(EcsWorld world, IDbController controller) {
            _world = world;
            _controller = controller;
            _loadConventusFilter = _world.Filter<LoadConventus>().End();
            _conventusPool = _world.GetPool<Conventus>();
            _loadConventusPool = _world.GetPool<LoadConventus>();
            _singulaPool = _world.GetPool<Singula>();

            _greenOutline = Resources.Load<Material>($"Materials/GreenOutline");
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _loadConventusFilter) {
                ref var loadConventus = ref _loadConventusPool.Get(entity);
                var loadedConventus = _controller.DonwloadConventus(loadConventus.Id);

                ref var conventus = ref _conventusPool.Add(entity);
                conventus.Id = loadedConventus.conventus_id;
                conventus.Name = loadedConventus.conventus_name;
                conventus.Joins = loadedConventus.joins
                    .Select(joinDto => new Join() {
                        Id = joinDto.join_id,
                        NextJoinIds = joinDto.next_join_ids,
                        PreviousJoinIds = joinDto.previous_join_ids,
                        Position = joinDto.position
                    })
                    .ToArray();
                
                foreach (var singulaDto in loadedConventus.singulas) {
                    var singulaEntity = _world.NewEntity();
                    ref var singula = ref _singulaPool.Add(singulaEntity);

                    var singulaObject = GameObject.Instantiate(
                        Resources.Load<GameObject>($"Models/{singulaDto.model}"), 
                        singulaDto.position,
                        Quaternion.identity);

                    singulaObject.AddComponent<MeshCollider>().convex = true;
                    singulaObject.AddComponent<Rigidbody>();
                    
                    singula.SingulaView = singulaObject.AddComponent<SingulaView>();
                    singula.SingulaView.EcsEntity = singulaEntity;
                    singula.SingulaView.Id = singulaDto.singula_id;
                    singula.SingulaView.Name = singulaDto.name;
                    singula.SingulaView.Joins = loadedConventus.joins
                        .Where(join => singulaDto.joins.Contains(join.join_id))
                        .Select(join => new Join() {
                            Id = join.join_id,
                            Position = join.position,
                            PreviousJoinIds = join.previous_join_ids,
                            NextJoinIds = join.next_join_ids
                        })
                        .ToArray();

                    singula.ConventusEcsEntity = entity;

                    var singulaMeshRenderer = singulaObject.GetComponent<MeshRenderer>();
                    var singulaTexture = singulaMeshRenderer.material.mainTexture;
                    var singulaColor = singulaMeshRenderer.material.color;

                    singulaMeshRenderer.material = _greenOutline;
                    singulaMeshRenderer.material.SetTexture("_Texture2D", singulaTexture);
                    singulaMeshRenderer.material.SetColor("_Color", singulaColor);
                }

                _loadConventusPool.Del(entity);
            }
        }
    }
}