using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovementScript : MonoBehaviour {

    public CharacterController controller;

    public float speed = 6f;
    public float jumpHeight = 1f;
    public float gravity = -9.81f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Animator anim;
    public Transform cam;
    Vector3 velocity;

    void  Start(){
        anim = GetComponent<Animator>();
    }
    void Update() {
        getInput();
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        addGravity();

        // Adding turning and x/z axis movement
        if (direction.magnitude >= 0.1f){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        OOBCheck();
        //Applying the velocity vector to the controller
        controller.Move(velocity * Time.deltaTime);
    }

    public void getInput()
    {
        if (Input.GetKey("w"))
            anim.SetBool("Moving", true);
        else
            anim.SetBool("Moving", false);

        if (Input.GetKeyDown("space") && controller.isGrounded)
            jump();

        if (Input.GetKeyDown("left shift"))
            speed = 12f;
        else if (Input.GetKeyUp("left shift"))
            speed = 6f;
    }

    public void OOBCheck()
    {
        if (transform.position.y <= -10)
            transform.position = new Vector3(0, 5, 0);
    }

    public void jump(){
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void addGravity(){
        velocity.y += gravity * Time.deltaTime;
        //Capping the falling rate to the velocity of gravity
        if (velocity.y <= gravity)
            velocity.y = gravity;
    }
}
