using Attributes;
using Construct.Model;
using UnityEngine;

namespace Construct.Views
{
    public sealed class SingulaView : MonoBehaviour
    {
        [ReadOnly] public int Id;
        [ReadOnly] public string Name;
        [ReadOnly] public int EcsEntity;
        [ReadOnly] public Pimple[] Pimples;
    }
}
