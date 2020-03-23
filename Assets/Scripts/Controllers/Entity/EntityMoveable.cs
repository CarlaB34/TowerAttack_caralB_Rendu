using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EntityMoveable : Entity
{
    [Range(1, 50)]
    public float moveSpeed = 1;

    [Header("Target")]
    // Variable target
    public GameObject globalTarget;
    // + 
    //va vers la cible
    public GameObject target = null;

    [Header("Stop Time")]
    // Variable de temps d'arret
    public float timeWaitBeforeMove = 1;
    private float m_CurrentTimeBeforeNextMove = 0;
    
    private NavMeshAgent m_NavMeshAgent;


    //+//Data rangeToDoAttack
    [Header("AttackProps")]
    [SerializeField]
    private float m_rangeToDoAttack = 0.5f;



    // Initialisation - Construction de l'entité
    public override void InitEntity()
    {
        base.InitEntity();        
        Debug.Log("Modif Tealrocks");
        Debug.Log("Coucou Modif Pedro");
        // Initialisation - Construction
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override void RestartEntity()
    {
        base.RestartEntity();

        // Set/Restart properties
        m_NavMeshAgent.speed = moveSpeed;
        SetDestination();
    }

    public void SetGlobalTarget(GameObject target)
    {
        globalTarget = target;
        SetDestination();
    }
    
    //+ 
    //change la valeur, change de target avec le paramettre et lui set un destination
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        SetDestination();
    }

    private void SetDestination()
    {
        if(target)
        {
            m_NavMeshAgent.SetDestination(target.transform.position);
        }
        else if (globalTarget)
        {
            m_NavMeshAgent.SetDestination(globalTarget.transform.position);
        }
    }

    public override void Update()
    {
        base.Update();
        if (m_NavMeshAgent.isStopped)
        {
            if (m_CurrentTimeBeforeNextMove < timeWaitBeforeMove)
            {
                m_CurrentTimeBeforeNextMove += Time.deltaTime;
            }
            else
            {
                m_NavMeshAgent.isStopped = false;
                SetDestination();
            }
        }
        //+
        //on regarde si il est mort
        if(target && target.GetComponent<Entity>().IsValidEntity())
        {
            target = null;
            SetDestination();
        }
    }

    //+
    ///+ // calcule la distance pour la range
    public void OnTriggerStay(Collider other)
    {
        EntityMoveable target = other.GetComponent<EntityMoveable>();
        if(target && alignment != target.alignment)
        {
            
            float dist = Vector3.Distance(other.transform.position, transform.position);
            if(dist <= m_rangeToDoAttack)
            {
                DoAttack(other.GetComponent<Entity>());
            }
            else 
            {
                SetTarget(other.gameObject);
            }
        }
    }

    
    protected override bool DoAttack(Entity targetEntity)
    {
            if (base.DoAttack(targetEntity))
            {
                m_NavMeshAgent.isStopped = true;
                m_CurrentTimeBeforeNextMove = 0;
               
                return true;
            }
        return false;
    }
}
