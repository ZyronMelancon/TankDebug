using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float offset;
    public float smoothSpeed = 0.3f;

    float MouseAim;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    void FixedUpdate()
    {
        float rot = target.rotation.eulerAngles.y * Mathf.PI / 180;

        Vector3 pos = target.position + new Vector3(offset * -Mathf.Sin(rot), 3, -offset * Mathf.Cos(rot));
        Vector3 smoothMove = Vector3.Lerp(transform.position, pos, smoothSpeed);
        Quaternion smoothRotate = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed);

        transform.position = smoothMove;
        transform.rotation = smoothRotate;
    }
}
