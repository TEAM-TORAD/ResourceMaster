using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Remoting.Messaging;
//using UnityEditor.Animations;
using UnityEngine;
using Photon;
using Photon.Pun;

namespace Script.Movements
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviourPun
    {
        #region PUBLIC FIELDS
        [Header("Main Scene Camera")]
        public Transform mainCamera;

        [Header("Chracter Animation")]
        public Animator animator;
        public float damp = 0.3f;

        [Header("Walk / Run Setting")] 
        public float walkSpeed = 10;
        public float runSpeed = 5;

        [Header("Current Player Speed")]
        public float currentSpeed = 10;
        public float currentSpeedFloat;

        [Header("Jump Settings")] 
        public float jumpForce;
        public ForceMode appliedForceMode;
        public float jumpTime;

        [Header("Current State")]
        public bool isGrounded;
        public bool isJumping;
        public bool leftShiftPressed;
        public float jumpTimer;

        [Header("Ground Check Settings")]
        public LayerMask whatIsGround;
        public float distanceGround;

        public bool online = true;

        #endregion

        #region PRIVATE FIELDS

        private float inputLeftRight;
        private float inputForwardBack;
        private Vector3 moveDirection;
        private Rigidbody playerRigidBody;
        private float turnSmoothTime = 0.1f;
        float turnSmoothVelocity;
        
        

        #endregion

        #region MONODEVELOP ROUTINES

        private void Start()
        {
            #region Initializing Components
            if (!online || photonView.IsMine)
            {
                playerRigidBody = GetComponent<Rigidbody>();
                transform.Find("Main Camera").gameObject.SetActive(true);
            }
            

            #endregion
        }

        private void Update()
        {

            if (!online || photonView.IsMine)
            {
                #region Ground Check Raycast

                RaycastHit hit;
                Vector3 downVector = transform.TransformDirection(Vector3.down) * 100f;
                Debug.DrawRay(transform.position, downVector, Color.green);

                #endregion

                #region Ground Detection


                if (Physics.Raycast(transform.position, (downVector), out hit))
                {
                    distanceGround = hit.distance;
                    if (distanceGround > 1.3f)
                    {
                        isGrounded = false;
                        animator.SetBool("isGrounded", false);
                    }
                    else
                    {
                        isGrounded = true;
                        animator.SetBool("isGrounded", true);
                    }
                }


                #endregion

                #region Jump Input

                if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
                {
                    isJumping = true;
                    jumpTimer = jumpTime;
                    PlayerJump(jumpForce, appliedForceMode);

                }

                #endregion

                #region Jump Timer

                if (Input.GetKey(KeyCode.Space) && isJumping == true)
                {


                    if (jumpTimer > 0)
                    {
                        PlayerJump(jumpForce, appliedForceMode);
                        jumpTimer -= Time.deltaTime;

                    }
                    else
                    {
                        isJumping = false;

                    }
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    isJumping = false;

                }

                #endregion

                #region Speed Selection

                currentSpeed = leftShiftPressed ? runSpeed : walkSpeed;
                leftShiftPressed = Input.GetKey(KeyCode.LeftShift);

                #endregion

                #region Animator Setup
                animator.SetFloat("Velocity", getVelocity(), damp, Time.deltaTime);
                #endregion

                //FixedUpdate doesn't run in MonoBehaviorPun, add it here
                AddedUpate();
            }
        }

         void AddedUpate()
        {
            #region Movement Input

            inputForwardBack = Input.GetAxisRaw("Vertical");
            inputLeftRight = Input.GetAxisRaw("Horizontal");

            #endregion

            #region Camera and Movement Script
            //move position
            moveDirection = new Vector3(inputLeftRight, 0f, inputForwardBack).normalized;
            //change rotation
            if (moveDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveCamDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                playerRigidBody.MovePosition(transform.position + moveCamDirection.normalized * currentSpeed * Time.deltaTime);

                if (leftShiftPressed)
                {
                    currentSpeedFloat = 0.5f;
                }
                else
                {
                    currentSpeedFloat = 1f;
                }
            }
            else
            {
                currentSpeedFloat = 0f;
            }

            #endregion


            
        }

        #endregion

        #region HELPER ROUTINES

        #region Jump Force
        private void PlayerJump(float jumpForce, ForceMode forceMode)
        {
            playerRigidBody.AddForce(jumpForce * playerRigidBody.mass * Time.deltaTime * Vector3.up, forceMode);
            
        }

        #endregion
        public float getVelocity()
        {
            return currentSpeedFloat;
        }
        #endregion
    }
}