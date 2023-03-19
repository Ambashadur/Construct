using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerEvent : MonoBehaviour {
    public int Id;
    public event Action<int, Collider> OnTriggerEvent; 

    private void OnEnable() => GetComponent<BoxCollider>().isTrigger = true;
    private void OnTriggerEnter(Collider other) => OnTriggerEvent?.Invoke(Id, other);
}
