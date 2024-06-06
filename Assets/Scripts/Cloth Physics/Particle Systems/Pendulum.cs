using System.Collections.Generic;
using UnityEngine;

public class PendulumSystem : ParticleSystem {

    private int numParticles = 2;

    private float m = 0.1f;  // mass of one particle
    private float stiffness = 1.5f;  // stiffness of all springs (k constant)
    private float restLength = 0.5f;  // rest length of all springs

    // struct that contains spring information for every particle
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
        // position and velocity of first point
        _state.Add(new Vector3(0f, 2f, 0f));
        _state.Add(new Vector3(0f, 0f, 0f));

        // position and velocity of second point
        _state.Add(new Vector3(2f, 0f, 0f));
        _state.Add(new Vector3(0f, 0f, 0f));

        // spring from first point to second
        CreateListOfSprings();
        springs[0].AddConnection(1);
        springs[1].AddConnection(0);
    }

    public override List<Vector3> EvalF(List<Vector3> state) {
        List<Vector3> f = new List<Vector3>();

        List<Vector3> P = GetPositions();
        List<Vector3> V = GetVelocities();

        // fix first point
        f.Add(new Vector3(0f, 0f, 0f));
        f.Add(new Vector3(0f, 0f, 0f));

        for (int i = 1; i < P.Count; i++) {
            // apply gravity
            Vector3 gravity = Physics.gravity * m;

            // apply spring force
            Vector3 springForce = new Vector3(0f, 0f, 0f);
            List<int> connections = springs[i].GetConnections();
            foreach (int j in connections) {
                Vector3 d = P[i] - P[j];
                Vector3 oneForce = -stiffness * (Vector3.Magnitude(d) - restLength) * d.normalized;
                springForce += oneForce;
            }

            // calculate final force and acceleration
            Vector3 force = gravity + springForce;
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
