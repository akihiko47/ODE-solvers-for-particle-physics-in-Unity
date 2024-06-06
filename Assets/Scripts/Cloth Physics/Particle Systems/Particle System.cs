using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleSystem {

    protected List<Vector3> _state = new List<Vector3>();

    public abstract List<Vector3> EvalF(List<Vector3> state);

    public List<Vector3> GetState() {
        return _state;
    }

    public void SetState(List<Vector3> newState) {
        _state = newState;
    }

}
