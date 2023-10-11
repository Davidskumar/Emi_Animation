// Import necessary libraries
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

// Define the Thirdperson class, which inherits from MonoBehaviour
public class Thirdperson : MonoBehaviour
{
    // Serialized fields that can be modified in the Unity Inspector
    [SerializeField] private bool ground;
    [SerializeField] private float groundcheck;
    [SerializeField] private LayerMask groundmask;
    [SerializeField] private float gravity;
    private Vector3 velocity;
    [SerializeField] private float jheight;

    // Public variables for character control
    public CharacterController controller;
    private Animator animat;
    public Transform came;
    public float movespeed;
    public float walkspeed = 6f;
    public float runspeed = 10f;
    public float Value = 0;
    public int Counter = 0;
    public int State = 0;
    public float turnsmooth = 0.2f;
    float turnsmoothvel;

    private void Start()
    {
        // Initialize the 'animat' variable with the Animator component of this GameObject
        animat = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the character is on the ground
        ground = Physics.CheckSphere(transform.position, groundcheck, groundmask);
        if (ground && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input for movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Check if the character is moving and on the ground
        if (direction.magnitude >= 0.1f && ground)
        {
            // Calculate the target angle for rotation
            float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + came.eulerAngles.y;
            // Smoothly rotate the character towards the target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnsmoothvel, turnsmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Calculate the movement direction based on the target angle
            Vector3 movedir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;

            // Check if the 'C' key is pressed for crouching
            if (Input.GetKey(KeyCode.C) == true && movedir != Vector3.zero)
            {
                Crouch();
                controller.Move(movedir.normalized * movespeed * Time.deltaTime);
                animat.SetInteger("State", 2);
            }
            else if (movedir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
                animat.SetFloat("Speed", 0.4f);
            }
            else if (movedir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
                animat.SetFloat("Speed", 1);
            }

            controller.Move(movedir.normalized * movespeed * Time.deltaTime);
        }

        // Check if the character is not moving
        if (direction == Vector3.zero)
        {
            Idel();
            animat.SetFloat("Speed", 0);
            controller.Move(direction.normalized * movespeed * Time.deltaTime);

            // Check if the space key is pressed for burpees
            if (Input.GetKey("space") == true)
            {
                Burpee();
                animat.SetInteger("Burp", Counter);
            }
            if (Input.GetKey("space") == false)
            {
                animat.SetInteger("Burp", 0);
            }

            // Check if the 'C' key is pressed for crouching
            if (Input.GetKey(KeyCode.C) == true)
            {
                Crouch();
                animat.SetInteger("State", State);
            }
            else if (Input.GetKey(KeyCode.C) == false)
            {
                animat.SetInteger("State", 0);
                controller.height = 2f;
            }
            if(Input.GetMouseButtonDown(0) == true)
            {
                animat.SetInteger("Punch", 1);
            }
            else if(Input.GetMouseButtonDown(0) == false)
            {
                animat.SetInteger("Punch", 0);
            }
        }

        // Apply gravity to the character's velocity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Method to set character state to idle
    public void Idel()
    {
        Value = 0;
    }

    // Method to set character state to walking
    public void Walk()
    {
        movespeed = walkspeed;
        Value = 0.5f;
    }

    // Method to set character state to running
    public void Run()
    {
        movespeed = runspeed;
        Value = 1;
    }

    // Method to trigger burpee animation
    public void Burpee()
    {
        Counter = 1;
    }

    // Method to set character state to crouching
    public void Crouch()
    {
        State = 1;
        movespeed = 5f;
        controller.height = 1.5f;
    }
}
