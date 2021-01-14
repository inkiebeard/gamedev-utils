using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InkiebeardGamedevUtils {
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(AudioSource))]
	public class playerMovement : MonoBehaviour
	{
	    [Header("Movement")]
	    public Transform groundCheck;
	    public float groundDistance = 0.4f;
	    public float jumpHeight = 2f;
	    public LayerMask groundMask;
	    public LayerMask oobMask;
	    public Transform cam;
	    public float speed = 6.0F;
	    public float runMod = 1.4F;
	    public float turnSmoothTime = 0.3f;
	    public float gravity = -9.81f;
	    [Space(10)]

	    [Header("Sound Effects")]
	    public AudioClip[] footsteps;
	    [Range(0.1f, 2.5f)]
	    public float stepOffset = 0.6f;
	    public float runModOffset = 0.7f;

	    private CharacterController controller;
	    private float turnSmoothVel;
	    private Vector3 velocity;
	    private Vector3 startingPoint;
	    private bool isGrounded;
	    private AudioSource audScr;
	    private int footstepCount = 0;
	    private float timer = 0;


	    void Start()
	    {
	        controller = GetComponent<CharacterController>();
	        startingPoint = transform.position;
	        audScr = GetComponent<AudioSource>();
	        Application.targetFrameRate = 120;
	    }

	    void Update()
	    {
	        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
	        float hor = Input.GetAxisRaw("Horizontal");
	        float ver = Input.GetAxisRaw("Vertical");
	        Vector3 direction = new Vector3(hor, 0f, ver).normalized;
	        bool isRunning = isGrounded && Input.GetButton("Sprint");

	        // is moving
	        if (direction.magnitude >= 0.1f)
	        {

	            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
	            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
	            transform.rotation = Quaternion.Euler(0f, angle, 0f);

	            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
	            controller.Move(isRunning ? moveDirection.normalized * speed * runMod * Time.deltaTime : moveDirection.normalized * speed * Time.deltaTime);
	        }

	        // jumping & gravity
	        if (isGrounded && velocity.y < 0)
	        {
	            velocity.y = -2f;
	        }
	        if (Input.GetButtonDown("Jump") && isGrounded)
	        {
	            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
	        }
	        velocity.y += gravity * Time.deltaTime;
	        controller.Move(velocity * Time.deltaTime);

	        // out of bounds check
	        bool isOutOfBounds = Physics.CheckSphere(transform.position, groundDistance, oobMask);
	        if (isOutOfBounds)
	        {
	            transform.position = startingPoint;
	        }

	        // footsteps
	        int stepInterval = Mathf.CeilToInt(60 * (isRunning ? stepOffset * runModOffset : stepOffset));
	        Debug.Log(stepInterval);
	        Debug.Log(timer % stepInterval);
	        if (isGrounded && (timer % stepInterval == 0) && direction.magnitude >= 0.2f)
	        {
	            audScr.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
	        }
	        timer += Time.deltaTime;

	    }
	}
}
