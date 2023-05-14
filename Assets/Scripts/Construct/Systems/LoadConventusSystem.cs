using System.Collections.Generic;
using System.Linq;
using Construct.Components;
using Construct.Model;
using Construct.Services;
using Construct.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace Construct.Systems
{
    public sealed class LoadConventusSystem : IEcsRunSystem 
    {
        private readonly EcsWorld _world;
        private readonly IDbController _controller;
        private readonly EcsFilter _loadConventusFilter;
        private readonly EcsPool<Conventus> _conventusPool;
        private readonly EcsPool<LoadConventus> _loadConventusPool;
        private readonly EcsPool<Singula> _singulaPool;

        private readonly Material _greenOutline;
        private readonly int _singulaLayer;

        public LoadConventusSystem(EcsWorld world, LayerMask singulaLayer, IDbController controller) 
        {
            _world = world;
            _controller = controller;
            _loadConventusFilter = _world.Filter<LoadConventus>().End();
            _conventusPool = _world.GetPool<Conventus>();
            _loadConventusPool = _world.GetPool<LoadConventus>();
            _singulaPool = _world.GetPool<Singula>();

            _greenOutline = Resources.Load<Material>($"Materials/GreenOutline");
            _singulaLayer = (int)Mathf.Log(singulaLayer, 2);
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
                    .ToDictionary(
                        joinDto => joinDto.join_id,
                        joinDto => new Join() {
                            Id = joinDto.join_id,
                            NextJoinIds = joinDto.next_join_ids,
                            LeftJoinId = joinDto.left_join_id,
                            LeftPimples = new List<SingulaJoin>(0),
                            RightJoinId = joinDto.right_join_id,
                            RightPimples = new List<SingulaJoin>(0)
                        }
                    );
                
                foreach (var singulaDto in loadedConventus.singulas) {
                    var singulaEntity = _world.NewEntity();
                    ref var singula = ref _singulaPool.Add(singulaEntity);

                    var singulaObject = GameObject.Instantiate(
                        Resources.Load<GameObject>($"Models/{singulaDto.model}"), 
                        singulaDto.position,
                        Quaternion.identity);

                    singulaObject.layer = _singulaLayer;
                    singulaObject.AddComponent<MeshCollider>().convex = true;
                    singulaObject.AddComponent<Rigidbody>();
                    var singulaMeshRenderer = singulaObject.GetComponent<MeshRenderer>();
                    
                    singula.SingulaView = singulaObject.AddComponent<SingulaView>();
                    singula.SingulaView.Id = singulaDto.singula_id;
                    singula.SingulaView.EcsEntity = singulaEntity;
                    singula.SingulaView.Name = singulaDto.name;
                    singula.Transform = singulaObject.GetComponent<Transform>();
                    singula.Renderer = singulaMeshRenderer;
                    singula.Id = singulaDto.singula_id;
                    singula.Name = singulaDto.name;
                    singula.Pimples = singulaDto.pimples.ToDictionary(
                        pimpleDto => pimpleDto.id,
                        pimpleDto => new Pimple()
                        {
                            Id = pimpleDto.id,
                            Position = pimpleDto.position,
                            JoinId = pimpleDto.join_id,
                        });

                    singula.SingulaView.Pimples = singula.Pimples.Select(x => x.Value).ToArray();
                    singula.ConventusEcsEntity = entity;

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