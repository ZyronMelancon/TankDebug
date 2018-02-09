using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour {

    public FloatVariable health;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            health.Value -= 1;
    }
}
