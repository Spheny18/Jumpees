using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    // bool tmpBool = true;
    Move move;
    Closeness closeness;
     SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        move = GetComponent<Move>();
        closeness = GetComponent<Closeness>();
    }

    // Update is called once per frame
    void Update()
    {
        if(closeness.dist < closeness.close){
            animator.SetBool("Close",true);
        } else {
            animator.SetBool("Close",false);
        }
        if(move.velocity.x < 0){
            sr.flipX = true;
        } else if (move.velocity.x > 0){
            sr.flipX = false;
        }
        animator.SetFloat("Xspeed",Mathf.Abs(move.velocity.x));
        animator.SetFloat("Yspeed",move.velocity.y);

            if(move.isGrounded){
                animator.SetBool("Land",true);
            } else {
                animator.SetBool("Land",false);
            }
        
    }

    public void WalkSound(){
        FindObjectOfType<SoundManager>().Play("Walk");
    }

    public void WeakWalk(){
        FindObjectOfType<SoundManager>().Play("WeakWalk");
    }

    public void StrongJump(){
        FindObjectOfType<SoundManager>().Play("StrongJump");
    }
    public void weakJump(){
        FindObjectOfType<SoundManager>().Play("Walk");
    }
}
