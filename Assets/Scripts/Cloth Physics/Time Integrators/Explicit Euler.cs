using UnityEngine;
using System;

public class ODEsolvers {
    public Vector3 ExplicitEuler(Vector3 X, Func<Vector3, Vector3> f, float h) {
        return X + h * f(X);
    }

    public Vector3 Trapezoidal(Vector3 X, Func<Vector3, Vector3> f, float h) {
        Vector3 f0 = f(X);
        Vector3 f1 = f(X);
        return X + (h / 2f) * (f0 + f1);
    }
}
