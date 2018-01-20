using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour {

    public float moveForce = 5f;
    public float hoverForce = 10f;
    public float hoverHeight = 1f;

    Rigidbody self;
    private float MouseAim;

	// Use this for initialization
	void Start ()
    {
        self = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        MouseAim = Input.GetAxis("Mouse X");

        Move();
        Hover();
        Steer();

        Debug.Log(transform.rotation.y);
        Debug.Log(MouseAim);
	}

    void Hover()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, hoverHeight))
        {
            float dist = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 force = Vector3.up * dist * hoverForce;
            self.AddRelativeForce(force, ForceMode.Acceleration);
        }
    }

    void Steer()
    {
        self.AddRelativeTorque(new Vector3(0, MouseAim, 0));
    }

    void Move()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        self.AddRelativeForce(dir * moveForce);
    }
}
