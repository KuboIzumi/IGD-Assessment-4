using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private GameManager gameManager;

    public float speed = 5.0f;

    private Vector2 startPosition = new(-15.624f, 15.376f);

    public enum MovementDirections { Right, Up, Left, Down }
    public MovementDirections characterDirection = MovementDirections.Right;

    int[,] levelMap ={
    {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
    {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
    {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
    {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
    {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
    {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
    {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
    {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
    {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
    {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
    {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        gameManager = FindObjectOfType<GameManager>();

        ResetPosition();

        characterDirection = MovementDirections.Right;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentPos((int)transform.position.x, (int)transform.position.y);
        Move();
    }

    private IEnumerator Move()
    {
        float time = 0;
        float lerpSpeed = 1;
        while (time < 1)
        {
            transform.position = Vector2.Lerp(transform.position, WalkingDirection(), time);
            time += Time.deltaTime * lerpSpeed;
            yield return null;
        }
        transform.Translate(new Vector2(speed, 0f) * Time.deltaTime, Space.Self);
    }

    private Vector2 WalkingDirection()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            characterDirection = MovementDirections.Up;
            return new Vector2(0f, 1.25f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            characterDirection = MovementDirections.Left;
            return new Vector2(-1.25f, 0f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            characterDirection = MovementDirections.Down;
            return new Vector2(0f, -1.25f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            characterDirection = MovementDirections.Right;
            return new Vector2(1.25f, 0f);
        }

        return new Vector2(0, 0);
    }

    public int[] GetCurrentPos(int posX, int posY)
    {
        int[] array = new int[] { (posX - 23), (posY + 14) };
        return array;
    }

    public void ResetPosition()
    {
        characterDirection = MovementDirections.Right;
        transform.position = startPosition;
    }
}
