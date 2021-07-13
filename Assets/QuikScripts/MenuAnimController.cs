using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAnimController : MonoBehaviour
{
    public Animator animator;
    public float maxY;
    public float playY;
    public float playSpd;
    bool jumpingAnim;
    bool jumping;
    public float spd;
    bool play;
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        play = false;
        startingPos = transform.position;
        jumping = false;
        jumpingAnim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(jumpingAnim){
            if(startingPos == transform.position){
                Debug.Log("its true");
                jumping = true;
                animator.SetBool("Jumping",jumping);
            } else if(transform.position == startingPos + Vector3.up*maxY){
                Debug.Log("its false");
                jumping = false;
                animator.SetBool("Jumping",jumping);
            }

            if(jumping){
                //Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta);
                transform.position = Vector3.MoveTowards(transform.position, startingPos + Vector3.up*maxY, spd * Time.deltaTime);
            } else {
                transform.position =  Vector3.MoveTowards(transform.position, startingPos, spd * Time.deltaTime);
            }
        }
        if(play){
            if(transform.position == startingPos + Vector3.up*playY){
                Debug.Log("next level");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            transform.position = Vector3.MoveTowards(transform.position, startingPos + Vector3.up*playY, playSpd * Time.deltaTime);

        }
    }
    public void HoverPlay(){
        if(play){
            return;
        }
        jumpingAnim = true;
        animator.SetTrigger("HoverPlay");
    }

    public void HoverQuit(){
        if(play){
            return;
        }
        animator.SetTrigger("HoverQuit");
    }

    public void HoverExit(){
        if(play){
            return;
        }
        transform.position = startingPos;
        jumpingAnim = false;
        animator.SetTrigger("HoverExit");
    }
    public void Play(){
        jumpingAnim = false;
        play = true;
    }
}
