using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Animator animator;
    public bool on = false;
    // Start is called before the first frame update
    void Start()
    {
        //  on = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void action(){
        Debug.Log(on);
        on = !on;
        animator.SetBool("On",on);
        Debug.Log(on);
    }
}
