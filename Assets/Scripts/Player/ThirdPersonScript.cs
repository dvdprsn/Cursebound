using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public float runSpeed = 12f;
    private float gravity = -9.81f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Animator animator;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void MoveTo(Vector3 pos)
    {
        controller.Move(pos);
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 gravForce = new Vector3(0f, gravity, 0f);
        controller.Move(gravForce * Time.deltaTime);

        if(direction.magnitude >= 0.1)
        {
            animator.SetBool("isMoving", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(moveDirection.normalized * runSpeed * Time.deltaTime);
                animator.SetBool("isWalking", false);
            }
            else
            {
                controller.Move(moveDirection.normalized * speed * Time.deltaTime);
                animator.SetBool("isWalking", true);
            }

        } else
        {
            animator.SetBool("isMoving", false);

        }
    }
}
