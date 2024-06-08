using System.Collections.Generic;
using UnityEngine;

public class SimpleSystem : ParticleSystem {

    public SimpleSystem() {
        _state.Add(new Vector3(1f, 0f, 0f));
    }

    public override List<Vector3> EvalF(List<Vector3> state) {
        List<Vector3> f = new List<Vector3>();

        for (int i = 0; i < state.Count; i++) {
            f.Add(new Vector3(-state[i].y, state[i].x, 0f));
        }

        return f;
    }
}
