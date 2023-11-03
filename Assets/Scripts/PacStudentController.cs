using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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
    public const float unit = 1.25f;
    public float speed = 0.1f;
    public double time = 0;
    public double movementInteval = 0.5;
    Vector2 movement;

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

        StartPosition();

        characterDirection = MovementDirections.Right;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteDirection();
        Debug.Log(transform.position.x);
        WalkingDirection();
        Debug.Log(CanMove());
        GetCurrentPos((double)transform.position.x, (double)transform.position.y);
        Move();
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

    private Vector2 WalkingDirection()
    {
        if (Input.GetKey(KeyCode.W))
        {
            characterDirection = MovementDirections.Up;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            characterDirection = MovementDirections.Left;
        }

        else if (Input.GetKey(KeyCode.S))
        {
            characterDirection = MovementDirections.Down;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            characterDirection = MovementDirections.Right;
        }

        switch (characterDirection)
        {
            case MovementDirections.Left:
                return new Vector2(transform.position.x - unit, transform.position.y);
            case MovementDirections.Right:
                return new Vector2(transform.position.x + unit, transform.position.y);
            case MovementDirections.Up:
                return new Vector2(transform.position.x, transform.position.y + unit);
            case MovementDirections.Down:
                return new Vector2(transform.position.x, transform.position.y - unit);
            default:
                break;
        }
        return new Vector2(transform.position.x, transform.position.y);

    }

    public Boolean CanMove()
    {
        int[] tile = NextTile();
        if (levelMap[tile[0], tile[1]] >= 1 && levelMap[tile[0], tile[1]] <= 4)
        {
            return false;
        }
        return true;
    }

    public int[] NextTile()
    {
        int[] position;

        if (characterDirection == MovementDirections.Right)
        {
            return position = GetCurrentPos((double)transform.position.x + unit, (double)transform.position.y);
        }
        else if (characterDirection == MovementDirections.Left)
        {
            return position = GetCurrentPos((double)transform.position.x - unit, (double)transform.position.y);
        }
        else if (characterDirection == MovementDirections.Up)
        {
            return position = GetCurrentPos((double)transform.position.x, (double)transform.position.y + unit);
        }
        else if (characterDirection == MovementDirections.Down)
        {
            return position = GetCurrentPos((double)transform.position.x, (double)transform.position.y - unit);
        }
        return null;
    }

    public void Move()
    {
        if (CanMove())
        {
            startPosition = transform.position;
            time += Time.deltaTime;

            if (time >= movementInteval)
            {
                movement = WalkingDirection();
                transform.position = movement;
                time = 0.0f;
            }
        }
    }


    public int[] GetCurrentPos(double posX, double posY)
    {
        double x = (posX + 16.875) / unit;
        double y = (-1) * (posY - 16.625) / unit;
        int[] array = new int[2] { (int)x, (int)y };
        array[0] = (int)y;
        array[1] = (int)x;
        return array;
    }

    public void StartPosition()
    {
        characterDirection = MovementDirections.Right;
        transform.position = startPosition;
    }
}