using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemUser : MonoBehaviour {

    private int iterations = 16;

    private Cloth system;
    private ODEsolver stepper;

    private ClothMeshGenerator meshGenerator;

    private void Start() {
        system = new Cloth();
        stepper = new RungeKutta4();
        meshGenerator = GetComponent<ClothMeshGenerator>();
    }

    private void Update() {
        for (int i = 0; i < iterations; i++) {
            stepper.takeStep(system, Time.deltaTime / iterations);
        }
        system.AddMovement();

        if (meshGenerator != null) {
            meshGenerator.GenerateMesh(system.GetPositions(), system.GetN());
        }
    }


    private void OnDrawGizmos() {
        if (system == null || stepper == null) {
            return;
        }

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.black;
        List<Vector3> positions = system.GetPositions();
        for (int i = 0; i < positions.Count; i++) {
            Gizmos.DrawSphere(positions[i], 0.1f);
        }
    }
}
