using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonScript : MonoBehaviour
{
    private CharacterController controller;
    private PlayerStatus stats;
    public Transform cam;
    public float speed = 6f;
    public float runSpeed = 12f;
    private float gravity = -9.81f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    //private Animator animator;
    private Animator animator;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStatus>();
    }
    public void MoveTo(Vector3 pos)
    {
        controller.enabled = false;
        controller.transform.position = pos;
        controller.enabled = true;
    }
    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
    private void Walk()
    {
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);

    }
    private void Run()
    {
        animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);

    }
    void Update()
    {
        if(!stats.isDead)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
 
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 gravForce = new Vector3(0f, gravity, 0f);
            controller.Move(gravForce * Time.deltaTime);

            if (direction.magnitude >= 0.1)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    //Running 
                    controller.Move(moveDirection.normalized * runSpeed * Time.deltaTime);
                    Run();
                }
                else
                {
                    // Walking
                    controller.Move(moveDirection.normalized * speed * Time.deltaTime);
                    Walk();
                }

            }
            else
            {
                Idle();
            }

        }

    }
}
