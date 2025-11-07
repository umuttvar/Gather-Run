using NUnit.Framework;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    NavMeshAgent navMeshAgent;

    bool isAttack = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void LateUpdate()
    {
        if(isAttack)
        navMeshAgent.SetDestination(target.transform.position);
        
    }

    public void ProcessAttack()
    {
        GetComponent<Animator>().SetBool("Attack", true);
        isAttack = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SubPlayer"))
        {
            Vector3 newPos = new Vector3(transform.position.x, 0.3f, transform.position.z);
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().ProcessDestroyEfect(newPos, false, true);
            gameObject.SetActive(false);
        }
    }
}
