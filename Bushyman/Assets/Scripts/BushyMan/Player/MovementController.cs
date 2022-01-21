using System;
using UnityEditor;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    private Transform cameraTransform;

    //##ANIMATION#
    private Animator anim;
    bool Idle;
    private float Idle2 = 5;

    //##MOVEMENT##
    private float MovementSpeed;
    private float WalkSpeed = 9;
    private float RunSpeed;
    private float stepOffset;
    
    public Vector3 velocity;
    private float gravity = -20f;
    private CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    private float groundDistance = 0.3f;

    private bool isGrounded;
    private float jumpHeight = 4.3f;

    private float TurnSmoothVelocity;
    private float TurnSmoothTime = 0.08f;

    private bool isFalling;

    public DialogueTrigger testsomeshit;

    //##FLOAT##
    float floatTimer = 7.2f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
        stepOffset = controller.stepOffset;
    }
    private void FixedUpdate()
    {
        //Debug.Log("isGrounded state: " + isGrounded);

        //#### GRAVITY ####
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Debug.Log(velocity);

        //######## WALKING #########
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        //###### ON THE GROUND MOVEMENT #######
        if (inputDir != Vector2.zero)
        {
            float playerRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, playerRotation, ref TurnSmoothVelocity, TurnSmoothTime);

            controller.Move(transform.forward * MovementSpeed * Time.deltaTime);


            //######## SET RUNNING OR WALKING SPEED #########
            RunSpeed = WalkSpeed * 1.5f;
            if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
            {
                MovementSpeed = RunSpeed;
                anim.SetTrigger("isRunning");
            }
            else if (isGrounded) { 
                MovementSpeed = WalkSpeed;
                anim.SetTrigger("isWalking");
            }
            
            Idle = false; 
            Idle2 = 10;
        }
        else Idle = true;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1.5f;
            anim.SetBool("isJumping", false);
            anim.SetBool("isHover", false);
            controller.stepOffset = stepOffset;
        }

        controller.Move(velocity * Time.deltaTime);

        //quick setup to test attack animation
        if (Input.GetKeyDown(KeyCode.Mouse0)) { anim.SetTrigger("isAttacking"); }

        //######## JUMP #########
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            anim.SetBool("isJumping", true);
            floatTimer = 7.2f;
            velocity.y = Mathf.Sqrt(-1f * jumpHeight * gravity);
            controller.stepOffset = 0;
        }

        //######## HOVER #########
        if (!isGrounded && Input.GetButton("Jump") && controller.velocity.y <= 0)
        {
            anim.SetBool("isHover", true);
            anim.SetBool("isJumping", false);

            floatTimer -= Time.deltaTime;
            if(floatTimer >= 0) velocity.y = -0.5f;
        }

        //######## IDLE ANIMATION #########
        if (Idle && isGrounded)
        {
            Idle2 -= Time.deltaTime;

            //decide if normal idle has to be played or the secret idle
            if (Idle2 > 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle2"))
            {
                anim.SetTrigger("isIdle");
            }
            if (Idle2 <= 0)
            {
                anim.SetTrigger("isIdle2");
                Idle2 = 30;
            }
        }

        //###### INTERACTION WITH NPC #######
        RaycastHit hit;
        //Debug.DrawRay(transform.position, transform.forward, Color.green, 5);

        if (DialogueManager.instance.isDialogue() == false && Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5) && hit.transform.CompareTag("NPC"))
            {
                //Debug.Log("hit object " + hit.transform.name);
                hit.transform.gameObject.GetComponent<DialogueTrigger>().TriggerdDialogue();
            }
        }
        else if (DialogueManager.instance.isDialogue() == true && Input.GetKeyDown(KeyCode.E)) 
        {
            DialogueManager.instance.DisplayNextSentence();
        }
    }
}

  
  
