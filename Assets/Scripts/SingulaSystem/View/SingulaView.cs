using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SingulaSystem.Controller;
using SingulaSystem.Model;

public class SingulaView : MonoBehaviour, ISingulaView
{
    public Collider[] SingulaColliders; 

    public void ShowSingulaJoin(Singula singula, Vector3 position) {
        Debug.Log($"Inoke ShowSingulaJoin: singula id = {singula.Id} and position = {position}");
    }

    public void HideSingulaJoin(int singulaId) {
        throw new System.NotImplementedException();
    }

    public void JoinSingulaModels(int masterSingulaId, int slaveSingulaId, Vector3 slaveSingulaPosition) {
        // var otherLimb = collider.GetComponent<Singula>();

        // if (otherLimb != null && otherLimb.HasSlot) {
        //     var joint = collider.GetComponent<FixedJoint>();

        //     if (joint == null) {
        //         var limbTransform = collider.transform;

        //         limbTransform.rotation = _transform.rotation;
        //         limbTransform.position = _transform.TransformPoint(Pimples[index].Position - otherLimb.Slot);

        //         var addedJoint = collider.gameObject.AddComponent<FixedJoint>();
        //         addedJoint.connectedBody = _rigidbody;
        //     } else {
        //         joint.connectedBody = _rigidbody;
        //     }
        // }
    }
}
