using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public bool blue;
    [HideInInspector]
    public Vector2 velocity;
    public float speed = 9;
    public float walkAcceleration = 75;
    public float groundDeceleration = 70;
    public float jumpHeight = 5;
    public float fallingGravity = -75;
    public float jumpingGravity = -30;
    public float wallGravity = -10;
    public bool close;
    public Vector2 wallKick = new Vector2(2,3);
    [HideInInspector]
    public bool isGrounded;
    Controller2D controller;
    void Start()
    {
        close = false;
        isGrounded = false;
        controller = GetComponent<Controller2D>();
        velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public State NormalMove(){
        // Debug.Log(velocity + "ground");
        float moveInput;
        if(blue){
            moveInput = Input.GetAxisRaw("HorizontalBlue");
        }
        else {
            moveInput = Input.GetAxisRaw("HorizontalPink");
        }
        
        velocity.y = 0;
        // bool jumping = false;
        if(blue){
            if (Input.GetButtonDown("BlueJump"))
            {
                if(close){
                    FindObjectOfType<SoundManager>().Play("StrongJump");
                } else {
                    FindObjectOfType<SoundManager>().Play("WeakJump");
                }
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(jumpingGravity));
                // jumping = true;
            }
        } else {
            if (Input.GetButtonDown("PinkJump"))
            {
                if(close){
                    FindObjectOfType<SoundManager>().Play("StrongJump");
                } else {
                    FindObjectOfType<SoundManager>().Play("WeakJump");
                }
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(jumpingGravity));
                // jumping = true;
            }
        }

        velocity.y += fallingGravity * Time.deltaTime;

        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, walkAcceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, groundDeceleration * Time.deltaTime);
        }
        // Debug.Log(velocity + " ground " + gameObject.name);

        controller.Move(velocity * Time.deltaTime);
        isGrounded = controller.collisions["grounded"].isContact;
        // Debug.Log(controller.collisions["grounded"].isContact);
        if(controller.collisions["grounded"].isContact){
            return State.Move;
        } else {
            return State.Airborne;
        }
    }

    public State AirMove(){
        // Debug.Log(velocity + "air");
        float gravity = fallingGravity;
        float moveInput;
        if(blue){
            moveInput = Input.GetAxisRaw("HorizontalBlue");
        }
        else {
            moveInput = Input.GetAxisRaw("HorizontalPink");
        }

        if(velocity.y > 0){
            gravity = jumpingGravity;
        }

        velocity.y += gravity * Time.deltaTime;
        
        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, walkAcceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, groundDeceleration * Time.deltaTime);
        }
        // Debug.Log(velocity + "air");
        if(controller.collisions["topCollision"].isContact && controller.postUpdateVelocity.y > 0){
            velocity.y = 0;
        } else if (controller.collisions["topCollision"].isContact && controller.postUpdateVelocity.y < 0){
            velocity.y += controller.postUpdateVelocity.y/Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
        
        // Debug.Log()
        
        isGrounded = controller.collisions["grounded"].isContact;
        // Debug.Log(controller.collisions["grounded"].isContact);
        if(controller.collisions["grounded"].isContact){
            return State.Move;
        } else {
            if(controller.collisions["wallL"].isContact){
                return State.WallL;
            } else if (controller.collisions["wallR"].isContact){
                return State.WallR;
            }
            return State.Airborne;
        }
    }

    public State wallMove(int direction){

        // bool jumping = false;
        float gravity = fallingGravity;
        float moveInput;
        if(blue){
            moveInput = Input.GetAxisRaw("HorizontalBlue");
        }
        else {
            moveInput = Input.GetAxisRaw("HorizontalPink");
        }

        if(velocity.y > 0){
            gravity = jumpingGravity;
        }
        if(direction == 1){
            moveInput = moveInput > 0 ? moveInput : 0;
        } else {
            moveInput = moveInput < 0 ? moveInput : 0;
        }
        velocity.x = moveInput * speed;

        velocity.y += gravity * Time.deltaTime;

        
        controller.Move(velocity*Time.deltaTime);
        
        if(controller.collisions["grounded"].isContact){
            return State.Move;
        } else {
            if(controller.collisions["wallL"].isContact){
                return State.WallL;
            } else if (controller.collisions["wallR"].isContact){
                return State.WallR;
            }
            return State.Airborne;
        }
    }

}


