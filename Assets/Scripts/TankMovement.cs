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

        //Are we over the ground?
        ray = new Ray(transform.position, -transform.up);
        RaycastHit ground;


        //If yes, and not grounded, hover as usual
        if (Physics.Raycast(ray, out ground, hoverHeight))
        {
            if (Input.GetAxis("Jump") == 0)
            {
                Hover(ground);
            }
        }
        //If not, try to stay upright until we're over ground again
        else
            Rebalance();

        //Can move and steer up to 1 unit above hover height
        if (Physics.Raycast(ray, out ground, hoverHeight + 1))
            if (Input.GetAxis("Jump") == 0)
            {
                Move();
                Steer();
            }
    }

    void Hover(RaycastHit hit)
    {
        rayT = new Ray(transform.position + offset * 2.5f, -transform.up);
        rayL = new Ray(transform.position - offset2 * 2.5f, -transform.up);
        rayR = new Ray(transform.position + offset2 * 2.5f, -transform.up);
        rayB = new Ray(transform.position - offset * 2.5f, -transform.up);

        float dist = (hoverHeight - hit.distance) / hoverHeight;
        Vector3 force = transform.up * dist * hoverForce;

        self.AddForce(force, ForceMode.Acceleration);


        /* if (Physics.Raycast(rayT, out hit, hoverHeight))
         {
             float dist2 = (hoverHeight - hit.distance);
             Vector3 force2 = -offset2 * dist2;
             self.AddRelativeTorque(force2 * 2);
         }

         if (Physics.Raycast(rayL, out hit, hoverHeight))
         {
             float dist2 = (hoverHeight - hit.distance);
             Vector3 force2 = -offset * dist2;
             self.AddRelativeTorque(force2 * 2);
         }

         if (Physics.Raycast(rayR, out hit, hoverHeight))
         {
             float dist2 = (hoverHeight - hit.distance);
             Vector3 force2 = offset * dist2;
             Debug.Log(dist);
             self.AddRelativeTorque(force2 * 2);
         }

         if (Physics.Raycast(rayB, out hit, hoverHeight))
         {
             float dist2 = (hoverHeight - hit.distance);
             Vector3 force2 = offset2 * dist2;
             self.AddRelativeTorque(force2 * 2);
         }*/
    }

    void Steer()
    {
        var cuck = Quaternion.FromToRotation(transform.forward, camera.transform.forward);
        Debug.Log(cuck);
        self.AddTorque(new Vector3(0, Mathf.Min(cuck.y, 0.15f), 0) * 75, ForceMode.Impulse);
    }

    void Move()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 dirRel = new Vector3()

        self.AddRelativeForce(dir * moveForce * self.mass);

        if (dir == Vector3.zero)
            self.AddForce(-self.velocity / 3 * self.mass);
        else
            self.AddRelativeForce(dir * moveForce * Mathf.Max(0, Vector3.Dot(-self.velocity.normalized, dir)) * self.mass);
    }

    void Rebalance()
    {
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        self.AddTorque(new Vector3(rot.x, rot.y, rot.z) * 60);
    }
}
