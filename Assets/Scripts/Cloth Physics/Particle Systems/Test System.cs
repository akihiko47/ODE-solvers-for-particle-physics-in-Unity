using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSystem : MonoBehaviour {

    private ODEsolvers integrator;

    private Vector3 State;

    private void Start() {
        integrator = new ODEsolvers();
        State = new Vector3(1f, 1f, 0f);
    }

    private void Update() {
        State = integrator.Trapezoidal(State, CalculateDerivative, Time.deltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(State, 0.3f);
    }

    private Vector3 CalculateDerivative(Vector3 X) {
        return new Vector3(-X.y, X.x, 0f);
    }

}
