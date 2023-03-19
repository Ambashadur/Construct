using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class Leg : MonoBehaviour {
    public Vector3[] LegVertecices;

    private Vector3[] _meshVertices;

    public Vector3[] GetVertices() {
        SetVertices();

        var scale = transform.localScale;
        var uniqueVerticies = new List<Vector3>();

        for (int i = -1; ++i < _meshVertices.Length;) {
            if (uniqueVerticies.Contains(_meshVertices[i])) continue;
            uniqueVerticies.Add(_meshVertices[i]);
        }

        var verticies = uniqueVerticies.ToArray();

        for (int i = -1; ++i < verticies.Length;) {
            verticies[i].x *= scale.x;
            verticies[i].y *= scale.y;
            verticies[i].z *= scale.z;

            verticies[i] = transform.rotation * verticies[i];
        }

        return verticies;
    }

    private void OnEnable() {
        SetVertices();
    }

    private void SetVertices() {
        if (_meshVertices != null && _meshVertices.Length > 0) return;

        var collider = GetComponent<MeshCollider>();
        if (collider != null) _meshVertices = collider.sharedMesh.vertices;
    }
}
