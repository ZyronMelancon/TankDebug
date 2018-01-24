using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour {

    public float moveForce = 5f;
    public float hoverForce = 10f;
    public float hoverHeight = 1f;
    public float topSpeed = 25f;

    Rigidbody self;
    private float MouseAimX;

	// Use this for initialization
	void Start ()
    {
        self = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        MouseAimX += Input.GetAxis("Mouse X");

        Move(Hover());
        //Steer();
	}

    bool Hover()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, hoverHeight))
        {
            float dist = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 force = Vector3.up * dist * hoverForce;
            self.AddRelativeForce(force, ForceMode.Acceleration);
        }

        return Physics.Raycast(ray, out hit, hoverHeight);
    }

    void Steer()
    {

        self.AddRelativeTorque(new Vector3(0, self.rotation.y - MouseAimX * Mathf.PI / 180, 0) * 100);
    }

    void Move(bool isGrounded)
    {
        if (!isGrounded)
            return;

        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        self.AddRelativeForce(dir * moveForce * self.mass);

        if (dir == Vector3.zero)
            self.AddRelativeForce(-self.velocity / 3 * self.mass);
        else
            self.AddRelativeForce(dir * moveForce * Mathf.Max(0, Vector3.Dot(-self.velocity.normalized, dir)) *self.mass);
    }
}
