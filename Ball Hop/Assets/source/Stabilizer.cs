using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabilizer : MonoBehaviour
{
    private Vector3 localOffset;
    private Quaternion initialRotation;

    void Start()
    {
        // Simpan offset lokal dari parent ke posisi tengah (biasanya Vector3.zero cukup)
        if (transform.parent != null)
        {
            localOffset = transform.localPosition;
        }

        // Simpan rotasi awal di world space
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (transform.parent != null)
        {
            // Ikuti posisi tengah parent
            transform.position = transform.parent.position + transform.parent.TransformVector(localOffset);
        }

        // Pertahankan rotasi awal di world space
        transform.rotation = initialRotation;
    }
}
