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

/*
    public Vector3 ExplicitEuler(Vector3 X, Func<Vector3, Vector3> f, float h) {
        return X + h * f(X);
    }


    public Vector3 Trapezoidal(Vector3 X, Func<Vector3, Vector3> f, float h) {
        Vector3 f0 = f(X);
        Vector3 f1 = f(X);
        return X + (h / 2f) * (f0 + f1);
    }

    public Vector3 RungeKutta4(Vector3 X, Func<Vector3, Vector3> f, float h) {
        Vector3 k1 = f(X);
        Vector3 k2 = f(X + (h / 2) * k1);
        Vector3 k3 = f(X + (h / 2) * k2);
        Vector3 k4 = f(X + h * k3);
        return X + (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
    }*/


