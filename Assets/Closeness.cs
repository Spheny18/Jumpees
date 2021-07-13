using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closeness : MonoBehaviour
{
    public GameObject partner;
    Move move;

    public float close = 4;
    public float closeJump = 7;
    [HideInInspector]
    public float dist;
    public float farJump = 3;
    // public float closeSp
    void Start()
    {
        move = GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diffVector = transform.position - partner.transform.position;
        dist = diffVector.magnitude;
        if(dist < close){
            move.close = true;
            move.jumpHeight = closeJump;
        } else{
            move.close = false;
            move.jumpHeight = farJump;
        }
    }
}
