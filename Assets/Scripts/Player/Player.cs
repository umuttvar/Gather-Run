using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public float forwardSpeed = 0.5f;
    public float horizontalSensitivity = 0.1f;
    public float horizontalLerp = 0.3f;
    public FollowCamera followCamera;
    public GameObject endGamePoint;
    public Slider slider;
    public GameObject endGameTriggerPoint;
    public bool isEndGame;

    void Start()
    {
        float sliderGap = Vector3.Distance(gameObject.transform.position, endGameTriggerPoint.transform.position);
        slider.maxValue = sliderGap;
    }

    void FixedUpdate()
    {
        if (!isEndGame)
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    void Update()
    {
        if(Time.timeScale != 0)
        {
            
        if (isEndGame)
        {
            transform.position = Vector3.Lerp(transform.position, endGamePoint.transform.position, .007f);
            if (slider.value != 0)
                slider.value -= 0.01f;
        }
        else
        {
            float sliderGap = Vector3.Distance(gameObject.transform.position, endGameTriggerPoint.transform.position);
            slider.value = sliderGap;

            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                Vector2 delta = Mouse.current.delta.ReadValue();

                Vector3 targetPos = new Vector3(
                    transform.position.x + delta.x * horizontalSensitivity,
                    transform.position.y,
                    transform.position.z

            );
                transform.position = Vector3.Lerp(transform.position, targetPos, horizontalLerp);
            }
        }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Addition") || other.CompareTag("Subtraction") || other.CompareTag("Divison") || other.CompareTag("Multiplication"))
        {
            int number = int.Parse(other.name);
            gameManager.SubPlayerManagement(other.tag, number, other.transform);

        }

        else if (other.CompareTag("EndGame"))
        {
            followCamera.isEndGame = true;
            gameManager.GetComponent<GameManager>().EnemyTrigger();
            isEndGame = true;
        }

        else if (other.CompareTag("CollectablePlayer"))
        {
            gameManager.SubPlayers.Add(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Column") || collision.gameObject.CompareTag("PinCube") || collision.gameObject.CompareTag("FanNeedles"))
        {
             if (transform.position.x > 0)
            {
                transform.position = new Vector3(transform.position.x - .4f, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x + .4f, transform.position.y, transform.position.z);
            }
        }
    }
}
