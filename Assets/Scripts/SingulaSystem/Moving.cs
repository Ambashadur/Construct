using UnityEngine;

namespace SingulaSystem {
    public class Moving : MonoBehaviour {
        [SerializeField] private float _speed = 1.0f;

        private void Update() {
            transform.Translate(0, Time.deltaTime * _speed, 0);
        }
    }
}
