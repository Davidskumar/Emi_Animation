using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAttack : MonoBehaviour
{

    [SerializeField] private bool ground;
    [SerializeField] private float groundcheck;
    [SerializeField] private LayerMask groundmask;
    [SerializeField] private float gravity;
    private Vector3 velocity;
    public float lookradius = 5f;
    private Animator animatNPCAttack;
    Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        target =PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animatNPCAttack = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ground = Physics.CheckSphere(transform.position, groundcheck, groundmask);
        if (ground && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;

        float disatance = Vector3.Distance(target.position, transform.position);
        if (disatance <= lookradius)
        {
            agent.SetDestination(target.position);
            animatNPCAttack.SetInteger("NPCOut",1);
        }
        else if (disatance >= lookradius)
        {
            animatNPCAttack.SetInteger("NPCOut",0);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookradius);

    }
}
