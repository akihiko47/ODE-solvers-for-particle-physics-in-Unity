using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestSystem : MonoBehaviour {

    private PendulumSystem system;
    private ODEsolver stepper;

    private void Start() {
        system = new PendulumSystem();
        stepper = new ExplicitEuler();
    }

    private void Update() {
        stepper.takeStep(system, Time.deltaTime);
    }


    private void OnDrawGizmos() {
        if (system == null || stepper == null) {
            return;
        }

        Gizmos.color = Color.red;
        List<Vector3> positions = system.GetPositions();
        for (int i = 0; i < positions.Count; i++) {
            Gizmos.DrawSphere(positions[i], 0.3f);
        }
    }
}
