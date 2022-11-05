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
    public LayerMask whatIsGround = 1 << 0; // � ���� ��� ������� ��������, ��� �� ��� https://gist.github.com/unitycoder/17b82701f3e2f187eff9
    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;



    private void Start()
    {
        // �������� ������ ��������� � �� ��� ��� ������
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // ���������, �������� �� ����� �����
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // ���� ��, �� ������������� ���, ����� �� �������
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
        // �������� �������� ������
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // ���� ����� ����� �� ������ � ����� �� �����
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            // ��������� ����������� ������� ��� ���
            readyToJump = false;
            // �������
            Jump();
            /* ���������� ����������� �������� ����� ��������� �����:
             ������� Invoke �������� ����� ResetJump ����� jumpCooldown ������ */
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKey(sprintKey))
            isRunning = true;
        else
            isRunning = false;
    }

    private void MovePlayer()
    {
        // ������������ ����������� �������� � ������� ������
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float playerSpeed;
        if (isRunning)
            playerSpeed = moveSpeed + sprintSpeed;
        else
            playerSpeed = moveSpeed;

        // ���� ����� �� �����, ������ ������� ���
        if (grounded)
            rb.AddForce(moveDirection.normalized * playerSpeed * 10f, ForceMode.Force);
        // ���� �� � ������ ��������� ��� (�����! airMultiplier ����� ���� ������ �������)
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        /* �������� ������ �������� ������������
        (magnitude ���������� ����� ����� �������, normalized �������� ��� ���������� �� -1 �� 1)
        ��������� ������������, ���� */

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        if (flatVel.magnitude > moveSpeed)
        {
            // ������ ������������ �������� � ����������� � �� ������
            Vector3 limitVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3 (limitVel.x, rb.velocity.y, limitVel.z); 
        }
    }

    private void Jump()
    {
        // ������������� ������
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        // ������������ ������
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
