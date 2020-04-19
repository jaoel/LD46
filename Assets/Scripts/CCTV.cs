using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    public GameObject pivot;

    private void Update() {
        pivot.transform.localRotation = Quaternion.Euler(0f, Mathf.Sin(Time.time * 0.5f) * 45f, 0f);
    }
}
