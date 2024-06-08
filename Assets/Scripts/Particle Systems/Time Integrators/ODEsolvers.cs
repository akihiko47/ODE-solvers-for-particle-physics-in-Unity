using UnityEngine;
using System.Collections.Generic;


public interface ODEsolver {
    public void takeStep(ParticleSystem particleSystem, float stepSize);
};

public class ExplicitEuler : ODEsolver {
    public void takeStep(ParticleSystem particleSystem, float stepSize) {
        List<Vector3> X = particleSystem.GetState();
        List<Vector3> F = particleSystem.EvalF(X);

        List<Vector3> Xnew = new List<Vector3>();
        for (int i = 0; i < X.Count; i++) {
            Xnew.Add(X[i] + stepSize * F[i]);
        }

        particleSystem.SetState(Xnew);
    }
}

public class Trapezoidal : ODEsolver {
    public void takeStep(ParticleSystem particleSystem, float stepSize) {
        List<Vector3> X = particleSystem.GetState();
        List<Vector3> f0 = particleSystem.EvalF(X);

        List<Vector3> X1 = new List<Vector3>();
        for (int i = 0; i < X.Count; i++) {
            X1.Add(X[i] + stepSize * f0[i]);
        }
        List<Vector3> f1 = particleSystem.EvalF(X1);

        List<Vector3> Xnew = new List<Vector3>();
        for (int i = 0; i < X.Count; i++) {
            Xnew.Add(X[i] + (stepSize / 2) * (f0[i] + f1[i]));
        }

        particleSystem.SetState(Xnew);
    }
}

public class RungeKutta4 : ODEsolver {
    public void takeStep(ParticleSystem particleSystem, float stepSize) {
        List<Vector3> X = particleSystem.GetState();
        List<Vector3> k1 = particleSystem.EvalF(X);

        List<Vector3> X2 = new List<Vector3>();
        for (int i = 0; i < X.Count; i++) {
            X2.Add(X[i] + (stepSize / 2) * k1[i]);
        }
        List<Vector3> k2 = particleSystem.EvalF(X2);

        List<Vector3> X3 = new List<Vector3>();
        for (int i = 0; i < X.Count; i++) {
            X3.Add(X[i] + (stepSize / 2) * k2[i]);
        }
        List<Vector3> k3 = particleSystem.EvalF(X2);

        List<Vector3> X4 = new List<Vector3>();
        for (int i = 0; i < X.Count; i++) {
            X4.Add(X[i] + stepSize * k3[i]);
        }
        List<Vector3> k4 = particleSystem.EvalF(X2);

        List<Vector3> Xnew = new List<Vector3>();
        for (int i = 0; i < X.Count; i++) {
            Xnew.Add(X[i] + (stepSize / 6) * (k1[i] + 2*k2[i] + 2*k3[i] + k4[i]));
        }

        particleSystem.SetState(Xnew);
    }
}

