using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    private Animator animatNPC;
    private void Start()
    {
        // Initialize the 'animat' variable with the Animator component of this GameObject
        animatNPC = GetComponent<Animator>();
    }   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPCAttack")
        {
            animatNPC.SetInteger("Damage",1);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPCAttack")
        {
            animatNPC.SetInteger("Damage", 0);
        }
    }
}
