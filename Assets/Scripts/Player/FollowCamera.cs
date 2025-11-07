using System;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowCamera : MonoBehaviour
{

    public Transform target;
    public Vector3 targetOffset;
    public GameObject endGamePoint;
    
    public bool isEndGame;

    void Start()
    {
        targetOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (!isEndGame)
            transform.position = Vector3.Lerp(transform.position, target.position + targetOffset, .125f);

        else
            transform.position = Vector3.Lerp(transform.position, endGamePoint.transform.position + targetOffset, .007f);
    }
}
