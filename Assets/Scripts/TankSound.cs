using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSound : MonoBehaviour {

    public AudioSource engine;
    public AudioSource jet;

    Rigidbody self;

	// Use this for initialization
	void Start ()
    {
        self = GetComponentInParent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        engine.pitch = 0.7f + self.velocity.magnitude / 50 + (-Input.GetAxis("Jump") + 1) * 0.2f;
        engine.volume = 0.8f - Input.GetAxis("Jump") * 0.6f;
        jet.pitch = (0.8f + self.velocity.magnitude / 25) * (-Input.GetAxis("Jump") + 1);

    }
}
