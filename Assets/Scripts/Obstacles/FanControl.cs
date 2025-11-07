using System.Collections;
using UnityEngine;

public class FanControl : MonoBehaviour
{
    public Animator animator;
    public BoxCollider wind;

    public float waitTime = 2f;

    public void ProcessAnimation(string statue)
    {
        if (statue == "true")
        {
            animator.SetBool("Start", true);
            wind.enabled = true;
        }
        else
        {
            animator.SetBool("Start", false);
            StartCoroutine(TriggerAnimationRoutine());
            wind.enabled = false;
        }
    }

    IEnumerator TriggerAnimationRoutine()
    {
        yield return new WaitForSeconds(waitTime);
        ProcessAnimation("true");
    }
}
