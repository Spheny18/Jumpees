using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool blue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other){
        if(blue){
            if(other.gameObject.name == "Blue"){
                FindObjectOfType<SoundManager>().Play("Goal");
                GameObject.Find("GameManager").GetComponent<GeneralManager>().BlueGoal = true;
            }
        } else {
            if(other.gameObject.name == "Pink"){
                FindObjectOfType<SoundManager>().Play("Goal");
                GameObject.Find("GameManager").GetComponent<GeneralManager>().PinkGoal = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(blue){
            if(other.gameObject.name == "Blue"){
                GameObject.Find("GameManager").GetComponent<GeneralManager>().BlueGoal = false;
            }
        } else {
            if(other.gameObject.name == "Pink"){
                GameObject.Find("GameManager").GetComponent<GeneralManager>().PinkGoal = false;
            }
        }
    }
}
