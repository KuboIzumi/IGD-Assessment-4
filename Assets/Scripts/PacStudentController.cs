using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using static PacStudentController;

public class PacStudentController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private GameManager gameManager;

    public float speed = 5.0f;
    public float time = 0;
    public float lerpSpeed = 1;
    public float elaspedTime;

    private Vector2 startPosition = new(-15.625f, 15.375f);

    public enum MovementDirections { Right, Up, Left, Down }
    public MovementDirections characterDirection = MovementDirections.Right;

    int[,] levelMap ={
    { 1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1 },
    { 2,5,5,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,2 },
    { 2,5,3,4,4,3,5,3,4,4,4,3,5,4,4,5,3,4,4,4,3,5,3,4,4,3,5,2 },
    { 2,6,4,0,0,4,5,4,0,0,0,4,5,4,4,5,4,0,0,0,4,5,4,0,0,4,6,2 },
    { 2,5,3,4,4,3,5,3,4,4,4,3,5,3,3,5,3,4,4,4,3,5,3,4,4,3,5,2 },
    { 2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2 },
    { 2,5,3,4,4,3,5,3,3,5,3,4,4,4,4,4,4,3,5,3,3,5,3,4,4,3,5,2 },
    { 2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2 },
    { 2,5,5,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,5,5,2 },
    { 1,2,2,2,2,1,5,4,3,4,4,3,0,4,4,0,3,4,4,3,4,5,1,2,2,2,2,1 },
    { 0,0,0,0,0,2,5,4,3,4,4,3,0,3,3,0,3,4,4,3,4,5,2,0,0,0,0,0 },
    { 0,0,0,0,0,2,5,4,4,0,0,0,0,0,0,0,0,0,0,4,4,5,2,0,0,0,0,0 },
    { 0,0,0,0,0,2,5,4,4,0,3,4,4,0,0,4,4,3,0,4,4,5,2,0,0,0,0,0 },
    { 2,2,2,2,2,1,5,3,3,0,4,0,0,0,0,0,0,4,0,3,3,5,1,2,2,2,2,2 },
    { 0,0,0,0,0,0,5,0,0,0,4,0,0,0,0,0,0,4,0,0,0,5,0,0,0,0,0,0 },
    { 2,2,2,2,2,1,5,3,3,0,4,0,0,0,0,0,0,4,0,3,3,5,1,2,2,2,2,2 },
    { 0,0,0,0,0,2,5,4,4,0,3,4,4,0,0,4,4,3,0,4,4,5,2,0,0,0,0,0 },
    { 0,0,0,0,0,2,5,4,4,0,0,0,0,0,0,0,0,0,0,4,4,5,2,0,0,0,0,0 },
    { 0,0,0,0,0,2,5,4,3,4,4,3,0,3,3,0,3,4,4,3,4,5,2,0,0,0,0,0 },
    { 1,2,2,2,2,1,5,4,3,4,4,3,0,4,4,0,3,4,4,3,4,5,1,2,2,2,2,1 },
    { 2,5,5,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,5,5,2 },
    { 2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2 },
    { 2,5,3,4,4,3,5,3,3,5,3,4,4,4,4,4,4,3,5,3,3,5,3,4,4,3,5,2 },
    { 2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2 },
    { 2,5,3,4,4,3,5,3,4,4,4,3,5,3,3,5,3,4,4,4,3,5,3,4,4,3,5,2 },
    { 2,6,4,0,0,4,5,4,0,0,0,4,5,4,4,5,4,0,0,0,4,5,4,0,0,4,6,2 },
    { 2,5,3,4,4,3,5,3,4,4,4,3,5,4,4,5,3,4,4,4,3,5,3,4,4,3,5,2 },
    { 2,5,5,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,2 },
    { 1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1 },
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
        SpriteDirection();
        Debug.Log(transform.position.x);
        WalkingDirection();

        GetCurrentPos((int)transform.position.x, (int)transform.position.y);
        if (CanMove())
        {
            time += Time.deltaTime;
            float completePercent = time / lerpSpeed;
            transform.position = Vector2.Lerp(transform.position, WalkingDirection(), time);
        }
    }

    private void SpriteDirection()
    {
        switch (characterDirection)
        {
            case MovementDirections.Right:
                spriteRenderer.flipY = false;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
                break;

            case MovementDirections.Up:
                spriteRenderer.flipY = false;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90.0f);
                break;

            case MovementDirections.Left:
                spriteRenderer.flipY = true;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180.0f);
                break;

            case MovementDirections.Down:
                spriteRenderer.flipY = false;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270.0f);
                break;

            default:
                return;
        }
    }

    private void Move()
    {
        transform.Translate(new Vector3(speed, 0f, 0f) * Time.deltaTime, Space.Self);
    }

    private Vector2 WalkingDirection()
    {
        if (Input.GetKey(KeyCode.W))
        {
            characterDirection = MovementDirections.Up;
            return new Vector2(transform.position.x, transform.position.y + 1.25f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            characterDirection = MovementDirections.Left;
            return new Vector2(transform.position.x - 1.25f, transform.position.y);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            characterDirection = MovementDirections.Down;
            return new Vector2(transform.position.x, transform.position.y - 1.25f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            characterDirection = MovementDirections.Right;
            return new Vector2(transform.position.x + 1.25f, transform.position.y);
        }

        return new Vector2(transform.position.x, transform.position.y);
    }

    public Boolean CanMove()
    {
        int[] currentTile = GetCurrentPos((int)transform.position.x, (int)transform.position.y);

        if (characterDirection == MovementDirections.Right)
        {
            if (levelMap[currentTile[0] + 1, currentTile[1]] <= 4 && levelMap[currentTile[0] + 1, currentTile[1]] >= 1)
            {
                return false;
            }
        }
        else if (characterDirection == MovementDirections.Left)
        {
            if (levelMap[currentTile[0] - 1, currentTile[1]] <= 4 && levelMap[currentTile[0] - 1, currentTile[1]] >= 1)
            {
                return false;
            }
        }
        else if (characterDirection == MovementDirections.Up)
        {
            if (levelMap[currentTile[0], currentTile[1] + 1] <= 4 && levelMap[currentTile[0], currentTile[1] + 1] >= 1)
            {
                return false;
            }
        }
        else if (characterDirection == MovementDirections.Down)
        {
            if (levelMap[currentTile[0], currentTile[1] - 1] <= 4 && levelMap[currentTile[0], currentTile[1] - 1] >= 1)
            {
                return false;
            }
        }
        return true;
    }

    public int[] GetCurrentPos(int posX, int posY)
    {
        double x = (posX + 16.875) / 1.25;
        double y = (-1) * (posY - 16.625) / 1.25;
        int[] array = new int[2] { (int)x, (int)y };
        array[0] = (int)x;
        array[1] = (int)y;
        return array;
    }

    public void ResetPosition()
    {
        characterDirection = MovementDirections.Right;
        transform.position = startPosition;
    }
}
