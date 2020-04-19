using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentTester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            BreakableComponent[] components = FindObjectsOfType<BreakableComponent>();
            foreach(BreakableComponent component in components) {
                component.SetState(BreakableState.BROKEN);
            }
        }
    }
}
