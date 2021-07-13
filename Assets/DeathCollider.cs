using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{

    public class KillArgs : EventArgs{
        public GameObject player;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("hi");
        if(other.gameObject.tag == "Player"){
            GameObject.Find("GameManager").GetComponent<GeneralManager>().Die(other.gameObject);
        }
    }
}
