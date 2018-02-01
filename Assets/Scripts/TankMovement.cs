using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour {

    public float moveForce = 5f;
    public float hoverForce = 10f;
    public float hoverHeight = 1f;
    public float topSpeed = 25f;

    public ParticleSystem trail;

    Rigidbody self;
    private float MouseAimX;
    Ray ray, rayT, rayL, rayR, rayB;
    Vector3 offset;
    Vector3 offset2;
    GameObject camera;
    
	void Start ()
    {
        self = GetComponent<Rigidbody>();

        camera = GameObject.FindGameObjectWithTag("MainCamera");

        offset = new Vector3(0, 0, 1);
        offset2 = new Vector3(1, 0, 0);
    }
	
	void Update ()
    {
        MouseAimX += Input.GetAxis("Mouse X");
    }

    private void FixedUpdate()
    {
        //Are we over the ground?
        ray = new Ray(transform.position, -transform.up);
        RaycastHit ground;

        Debug.Log(self.velocity.magnitude);

        //Will balance to ground slope up to 5 units above ground
        if (Physics.Raycast(ray, out ground, hoverHeight + 10))
        {
            HoverBalance();
            //Can move and steer up to 2 units above hover height
            if (Input.GetAxis("Jump") <= 0.99f && ground.distance <= hoverHeight + 2)
            {
                Move();
                Steer();
                trail.enableEmission = true;
                if (ground.distance <= hoverHeight)
                    Hover(ground);
                
            }
            else
                trail.enableEmission = false;
        }
        else
            Rebalance();
    }

    void Hover(RaycastHit hit)
    {
        float dist = (hoverHeight - hit.distance) / hoverHeight;
        Vector3 force = new Vector3(0, 1 - (self.velocity.y / 8) + ((Mathf.Abs(self.velocity.x)
            + Mathf.Abs(self.velocity.z)) / 25), 0) * dist * hoverForce * (-Input.GetAxis("Jump") + 1);

        self.AddRelativeForce(force, ForceMode.Acceleration);
    }

    void HoverBalance()
    {
        rayT = new Ray(transform.position + offset * 2f, Vector3.down);
        rayL = new Ray(transform.position - offset2 * 2f, Vector3.down);
        rayR = new Ray(transform.position + offset2 * 2f, Vector3.down);

        RaycastHit balT;
        RaycastHit balL;
        RaycastHit balR;


        if(Physics.Raycast(rayT, out balT, hoverHeight + 2) 
        && Physics.Raycast(rayL, out balL, hoverHeight + 2) 
        && Physics.Raycast(rayR, out balR, hoverHeight + 2))
        {
            Vector3 norm = Vector3.Normalize(balT.normal + balL.normal + balR.normal);

            var bal = Quaternion.FromToRotation(transform.up, norm);
            self.AddTorque(new Vector3(bal.x - 
                (self.angularVelocity.x / 20), bal.y - 
                (self.angularVelocity.y / 20), bal.z - 
                (self.angularVelocity.z / 20)) * 60, 
                ForceMode.Acceleration);
        }

    }

    void Steer()
    {
        var steerdir = Quaternion.FromToRotation(transform.forward, camera.transform.forward);
        self.AddRelativeTorque(new Vector3(0, (steerdir.y - (self.angularVelocity.y / 20)) * (-Input.GetAxis("Jump") + 1), 0) * 75, ForceMode.Impulse);
    }

    void Move()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        Vector3 relDir = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * dir;

        

        if (dir != Vector3.zero)
        {
            self.AddRelativeForce(dir * moveForce * self.mass, ForceMode.Force);
            self.AddForce(-self.velocity * 7.5f);
        }
        else if (dir == Vector3.zero)
            self.AddForce(-self.velocity / 3 * self.mass);
        else
            self.AddRelativeForce(dir * moveForce * Mathf.Max(0, Vector3.Dot(-self.velocity.normalized, relDir)) * self.mass / 2);
    }

    void Rebalance()
    {
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        self.AddTorque(new Vector3(rot.x, rot.y, rot.z) * 200);
    }
}
