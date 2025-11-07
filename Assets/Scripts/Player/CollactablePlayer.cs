using UnityEngine;
using UnityEngine.AI;

public class CollactablePlayer : MonoBehaviour
{
    public GameManager gameManager;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material newMaterial;
    public NavMeshAgent navMeshAgent;
    public GameObject target;
    public Animator animator;
    bool isCollide;

    void LateUpdate()
    {
        if(isCollide)
        navMeshAgent.SetDestination(target.transform.position);
    }

    void Start()
    {

    }

    Vector3 CreateNewPos()
    {
        return new Vector3(transform.position.x, .23f, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SubPlayer") || other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("CollectablePlayer"))
            {
                AnimationMaterialTrigger();
                isCollide = true;
                GetComponent<AudioSource>().Play();
            }
        }
        else if (other.CompareTag("PinCube"))
        {
            gameManager.ProcessDestroyEfect(CreateNewPos());
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Enemy"))
        {
            gameManager.ProcessDestroyEfect(CreateNewPos());
            gameObject.SetActive(false);
        }

        else if (other.CompareTag("ChainNeedles"))
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
    }

    void AnimationMaterialTrigger()
    {
        Material[] mats = skinnedMeshRenderer.materials;
        mats[0] = newMaterial;
        skinnedMeshRenderer.materials = mats;
        animator.SetBool("Attack", true);
        GameManager.currentPlayerAmount++;
        gameObject.tag = "SubPlayer";
    }
    

   
}
