using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour {

    public Text speedText;
    public Text inputText;

    public GameObject tank;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        speedText.text = "Speed: " + tank.GetComponent<Rigidbody>().velocity.magnitude;
        inputText.text = "Input: " + new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
	}
}
