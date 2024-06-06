using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestSystem : MonoBehaviour {

    private ParticleSystem system;
    private ODEsolver stepper;

    private void Start() {
        system = new SimpleSystem();
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
        for (int i = 0; i < system.GetState().Count; i++) {
            Gizmos.DrawSphere(system.GetState()[i], 0.3f);
        }
    }
}
