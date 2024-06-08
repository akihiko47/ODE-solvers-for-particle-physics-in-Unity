using System.Collections.Generic;
using UnityEngine;

public class Cloth : ParticleSystem {

    private int n = 15;
    private int numParticles;

    private float mass = 0.1f;  // mass of one particle
    private float stiffness = 20f;  // stiffness of all springs (k constant)
    private float drag = 0.4f;  // drag constant

    private static float diag = 1.41421356237f;  // sqrt(2)
    public class Spring {
        List<int> connections;
        public List<float> restLengths;

        public Spring() {
            connections = new List<int>();
            restLengths = new List<float>();
        }

        public void AddConnection(int j, float restLength) {
            connections.Add(j);
            restLengths.Add(restLength);
        }

        public List<int> GetConnections() {
            return connections;
        }

    }
    Spring[] springs;

    public Cloth() {
        numParticles = n * n;

        // create multiple points for pendulum
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                _state.Add(new Vector3(i, j, 0f));
                _state.Add(new Vector3(0f, 0f, 0f));
            }
        }

        // connect points with springs
        CreateListOfSprings();
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                
                // add vertical springs
                if (j < n - 1) {
                    springs[i * n + j].AddConnection(i * n + j + 1, 1f);
                    springs[i * n + j + 1].AddConnection(i * n + j, 1f);
                }

                // add horizontal springs
                if (i < n - 1) {
                    springs[i * n + j].AddConnection((i + 1) * n + j, 1f);
                    springs[(i + 1) * n + j].AddConnection(i * n + j, 1f);
                }

                // diagonal springs
                if (j < n - 1 && i < n - 1) {
                    springs[i * n + j].AddConnection((i + 1) * n + j + 1, diag);
                    springs[(i + 1) * n + j + 1].AddConnection(i * n + j, diag);

                    springs[i * n + j + 1].AddConnection((i + 1) * n + j, diag);
                    springs[(i + 1) * n + j].AddConnection(i * n + j + 1, diag);
                }

                // add long vertical springs
                if (j < n - 2) {
                    springs[i * n + j].AddConnection(i * n + j + 2, 2f);
                    springs[i * n + j + 2].AddConnection(i * n + j, 2f);
                }

                // add long horizontal springs
                if (i < n - 2) {
                    springs[i * n + j].AddConnection((i + 2) * n + j, 2f);
                    springs[(i + 2) * n + j].AddConnection(i * n + j, 2f);
                }
            }
        }
    }

    public override List<Vector3> EvalF(List<Vector3> state) {
        List<Vector3> f = new List<Vector3>();

        List<Vector3> P = GetPositions();
        List<Vector3> V = GetVelocities();

        for (int i = 0; i < P.Count; i++) {
            // calculate gravity
            Vector3 gravity = Physics.gravity * mass;

            // calculate drag
            Vector3 dragForce = -drag * V[i];

            // apply spring force
            Vector3 springForce = new Vector3(0f, 0f, 0f);
            List<int> connections = springs[i].GetConnections();
            int connInd = 0;
            foreach (int j in connections) {
                Vector3 d = P[i] - P[j];
                Vector3 oneForce = -stiffness * (Vector3.Magnitude(d) - springs[i].restLengths[connInd]) * d.normalized;
                springForce += oneForce;
                connInd += 1;
            }

            // calculate final force and acceleration
            Vector3 force = gravity + springForce + dragForce;
            Vector3 acceleration = force / mass;

            // add velocity and acceleration
            f.Add(V[i]);
            f.Add(acceleration);
        }

        // fix cloth points
        f[(numParticles - 1) * 2] = new Vector3(0f, 0f, 0f);
        f[(numParticles - 1) * 2 + 1] = new Vector3(0f, 0f, 0f);

        f[(n - 1) * 2] = new Vector3(0f, 0f, 0f);
        f[(n - 1) * 2 + 1] = new Vector3(0f, 0f, 0f);

        return f;
    }

    private void CreateListOfSprings() {
        springs = new Spring[numParticles];
        for (int i = 0; i < numParticles; i++) {
            springs[i] = new Spring();
        }
    }

    public void AddMovement() {
        _state[(n - 1) * 2] += new Vector3(0f, 0f, Mathf.Sin(Time.realtimeSinceStartup * 4f)) / 2f;
        _state[(numParticles - 1) * 2] += new Vector3(0f, 0f, Mathf.Sin(Time.realtimeSinceStartup * 4f)) / 2f;
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

    public int GetN() {
        return n;
    }
}
