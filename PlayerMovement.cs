using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 4;
	public float rotationSpeed = 200.0f;
	public float yaw = 0.0f;

	//animation stuff
	internal Animator animatorPlayer;
	public float v;
	public float h;

	public float angH = 0.0f;
	public float angV = 0.0f;

	float angleInBetween;  //store the angle in between the mouse and the player according to the ground normal
	public Transform m_RiflePosition;
	public Transform m_SMGPosition;
	public Transform m_ShotgunPosition;
    public Rigidbody rb;

    Vector3 lookpos;

    void Start()
    {
		animatorPlayer = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
	{


		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		 
		if (Physics.Raycast(ray, out hit, 360))
		{
			lookpos = hit.point;
		}

		Vector3 lookDir = lookpos - transform.position;
		lookDir.y = 0;


		transform.LookAt(transform.position + lookDir, Vector3.up);
    }

	//used for rigidbody
    void FixedUpdate()
    {
		//up down movement
		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");

		//animator
		//i use the h float value instead of the v, because i accidently mixed the float values up inside the animator it self
		//initialized horizontal first then vertical by accident, whoops!....to lazy to fix. No real issues.
		animatorPlayer.SetFloat ("vertical", h);
		animatorPlayer.SetFloat ("horizontal", v);


		//rotation with the right analog stick controller
		rotationSpeed += Input.GetAxis("Yaw");
		transform.eulerAngles = new Vector3 (0, rotationSpeed, 0);
		yaw = Input.GetAxis ("Yaw") * (rotationSpeed * Time.deltaTime); //* (Time.deltaTime * rotationSpeed);
		transform.eulerAngles = new Vector3 (0, yaw, 0);



		//movement upa nd down
		Vector3 movement = new Vector3(h, 0.0f, v);
		//adding force for movement
        rb.AddForce(movement * speed / Time.deltaTime);
    }
}