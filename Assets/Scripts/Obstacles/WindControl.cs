using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class WindControl : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SubPlayer"))
        {
            Debug.Log("collider");
            other.GetComponent<Rigidbody>().AddForce(new Vector3(-10, 0, 0), ForceMode.Impulse);
        }
    }
}
