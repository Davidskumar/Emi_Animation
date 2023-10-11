using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPunch : MonoBehaviour
{
    [SerializeField] private bool ground;
    [SerializeField] private float groundcheck;
    [SerializeField] private LayerMask groundmask;
    [SerializeField] private float gravity;
    private Vector3 velocity;
    private Animator animatNPC;
    private void Start()
    {
        // Initialize the 'animat' variable with the Animator component of this GameObject
        animatNPC = GetComponent<Animator>();
    }
    private void Update()
    {
        ground = Physics.CheckSphere(transform.position, groundcheck, groundmask);
        if (ground && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Punch")
        {
            animatNPC.SetTrigger("Punched");
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Punch")
        {
            animatNPC.SetTrigger("Punched");
        }
    }
}
