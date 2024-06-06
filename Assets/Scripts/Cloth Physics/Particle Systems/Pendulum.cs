using System.Collections.Generic;
using UnityEngine;

public class PendulumSystem : ParticleSystem {

    private float m = 1f;

    public PendulumSystem() {
        _state.Add(new Vector3(0f, 0f, 0f));
        _state.Add(new Vector3(0f, 0f, 0f));
        _state.Add(new Vector3(0f, 2f, 0f));
        _state.Add(new Vector3(0f, 0f, 0f));
    }

    public override List<Vector3> EvalF(List<Vector3> state) {
        List<Vector3> f = new List<Vector3>();

        List<Vector3> P = GetPositions();
        List<Vector3> V = GetVelocities();

        for (int i = 0; i < P.Count; i++) {
            Vector3 gravity = Physics.gravity;
            Vector3 force = gravity * m;

            Vector3 acceleration = force / m;

            f.Add(V[i]);
            f.Add(acceleration);
        }

        return f;
    }

    public List<Vector3> GetPositions() {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < _state.Count; i += 2) {
            positions.Add(_state[i]);
        }
        return positions;
    }

    public List<Vector3> GetVelocities() {
        List<Vector3> velocities = new List<Vector3>();
        for (int i = 1; i < _state.Count; i += 2) {
            velocities.Add(_state[i]);
        }
        return velocities;
    }
}
