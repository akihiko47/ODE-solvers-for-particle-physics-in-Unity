using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemUser : MonoBehaviour {

    private int iterations = 32;

    private Cloth system;
    private ODEsolver stepper;

    private void Start() {
        system = new Cloth();
        stepper = new RungeKutta4();
    }

    private void Update() {
        for (int i = 0; i < iterations; i++) {
            stepper.takeStep(system, Time.deltaTime / iterations);
        }
        system.MoveFixedPoints(transform.position);
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
