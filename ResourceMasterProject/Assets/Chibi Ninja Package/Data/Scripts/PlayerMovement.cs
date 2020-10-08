using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Components")]
    public Rigidbody rb;
    public Transform playerCamera;

    [Header("Player Settings")]
    public float playerSpeed;
    public float currentSpeed;

    private float inputForwardBack;
    private float inputLeftRight;
    private Vector3 moveDirection;
    private float turnSmoothTime;
    float turnSmoothVelocity;

    private void Awake()
    {
        // Get Player RigidBody from Self
        rb = GetComponent<Rigidbody>();
        // Player Speed Settings
        playerSpeed = 10;
        // Rotation Smoothing Settings
        turnSmoothTime = 0.1f;
    }
    public void Update()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        #region Player Movement and Rotation

        inputForwardBack = Input.GetAxisRaw("Vertical");
        inputLeftRight = Input.GetAxisRaw("Horizontal");

        //move position
        moveDirection = new Vector3(inputLeftRight, 0f, inputForwardBack).normalized;
        //change rotation
       
        PlayerSpeed();
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveCamDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        rb.MovePosition(transform.position + moveCamDirection.normalized * playerSpeed * currentSpeed * Time.deltaTime);   

        #endregion
    }
    public void PlayerSpeed()
    {
        if (moveDirection.magnitude > 0.1)
        {
            currentSpeed += 1f * Time.deltaTime;

            if (currentSpeed > 2f)
            {
                currentSpeed = 2f;
            }

        }
        else
        {
            currentSpeed = moveDirection.magnitude;
        }
    }

    

}

