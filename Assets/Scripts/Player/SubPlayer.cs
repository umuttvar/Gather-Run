using UnityEngine;
using UnityEngine.AI;

public class SubPlayer : MonoBehaviour
{
    public GameObject target;
    public GameManager gameManager;
    NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void LateUpdate()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }

    Vector3 CreateNewPos()
    {
        return new Vector3(transform.position.x, 0.3f, transform.position.z);

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChainNeedles"))
        {
            gameManager.ProcessDestroyEfect(CreateNewPos());
            gameObject.SetActive(false);
        }

        else if (other.CompareTag("FanNeedles"))
        {
            gameManager.ProcessDestroyEfect(CreateNewPos());
            gameObject.SetActive(false);
        }

        else if (other.CompareTag("Hammer"))
        {
            gameManager.ProcessDestroyEfect(CreateNewPos(), true);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Enemy"))
        {
            gameManager.ProcessDestroyEfect(CreateNewPos(), false, true);
            gameObject.SetActive(false);
        }
         else if (other.CompareTag("CollectablePlayer"))
        {
            gameManager.SubPlayers.Add(other.gameObject);
            GameManager.currentPlayerAmount++;
            other.gameObject.tag = "SubPlayer";
        }
    }
}
