using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public Transform orientation;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    bool isRunning;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
   

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround = 1 << 0; // К чему тут битовые операции, сам не ебу https://gist.github.com/unitycoder/17b82701f3e2f187eff9
    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;



    private void Start()
    {
        // Получаем объект ригидбоди и не даём ему упасть
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Проверяем, касается ли игрок земли
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // Если да, то останавливаем его, чтобы не полетел
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        GetInput();
        SpeedControl(); 
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        // Получаем движение игрока
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Если игрок нажал на пробел и стоит на земле
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            // Отключаем возможность прынуть ещё раз
            readyToJump = false;
            // Прыгаем
            Jump();
            /* Возвращаем возможность пругнуть через некоторое время:
             Функция Invoke запустит метод ResetJump через jumpCooldown секунд */
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKey(sprintKey))
            isRunning = true;
        else
            isRunning = false;
    }

    private void MovePlayer()
    {
        // Просчитываем направление движения и двигаем игрока
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float playerSpeed;
        if (isRunning)
            playerSpeed = moveSpeed + sprintSpeed;
        else
            playerSpeed = moveSpeed;

        // Если игрок на земле, просто двигаем его
        if (grounded)
            rb.AddForce(moveDirection.normalized * playerSpeed * 10f, ForceMode.Force);
        // Если он в слегка замедляем его (Важно! airMultiplier дожен быть меньше единицы)
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        /* Получаем вектор скорости передвижения
        (magnitude возвращает длину этого вектора, normalized выражает его отношением от -1 до 1)
        Почитайте документацию, суки */

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        if (flatVel.magnitude > moveSpeed)
        {
            // Создаём максимаьлную скорость и накладываем её на игрока
            Vector3 limitVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3 (limitVel.x, rb.velocity.y, limitVel.z); 
        }
    }

    private void Jump()
    {
        // Останавливаем игрока
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        // Осуществляем прыжок
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
