using System.Collections.Generic;
using UnityEngine;

public class PendulumSystem : ParticleSystem {

    private int numParticles = 10;

    private float m = 0.1f;  // mass of one particle
    private float stiffness = 5f;  // stiffness of all springs (k constant)
    private float restLength = 1f;  // rest length of all springs
    private float drag = 0.05f;  // drag constant

    public class Spring {
        List<int> connections;

        public Spring() {
            connections = new List<int>();
        }

        public void AddConnection(int j) {
            connections.Add(j);
        }

        public List<int> GetConnections() {
            return connections;
        }

    }
    Spring[] springs;

    public PendulumSystem() {
        // create multiple points for pendulum
        for (int i = 0; i < numParticles; i++) {
            _state.Add(new Vector3(i, i, 0f));
            _state.Add(new Vector3(0f, 0f, 0f));
        }

        // connect points with springs
        CreateListOfSprings();
        for (int i = 1; i < numParticles; i++) {
            springs[i].AddConnection(i - 1);
            springs[i - 1].AddConnection(i);
        }
    }

    public override List<Vector3> EvalF(List<Vector3> state) {
        List<Vector3> f = new List<Vector3>();

        List<Vector3> P = GetPositions();
        List<Vector3> V = GetVelocities();

        // fix first point
        f.Add(new Vector3(0f, 0f, 0f));
        f.Add(new Vector3(0f, 0f, 0f));

        for (int i = 1; i < P.Count; i++) {
            // calculate gravity
            Vector3 gravity = Physics.gravity * m;

            // calculate drag
            Vector3 dragForce = -drag * V[i];

            // apply spring force
            Vector3 springForce = new Vector3(0f, 0f, 0f);
            List<int> connections = springs[i].GetConnections();
            foreach (int j in connections) {
                Vector3 d = P[i] - P[j];
                Vector3 oneForce = -stiffness * (Vector3.Magnitude(d) - restLength) * d.normalized;
                springForce += oneForce;
            }

            // calculate final force and acceleration
            Vector3 force = gravity + springForce + dragForce;
            Vector3 acceleration = force / m;

            // add velocity and acceleration
            f.Add(V[i]);
            f.Add(acceleration);
        }

        return f;
    }

    private void CreateListOfSprings() {
        springs = new Spring[numParticles];
        for (int i = 0; i < numParticles; i++) {
            springs[i] = new Spring();
        }
    }

    public void MoveFixedPoints(Vector3 position) {
        _state[0] = position;
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
