using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public bool blue;

    GameObject grabbed;
    bool grabbing;
    public LayerMask grabbables;
    LayerMask mask;
    PlayerStateManager psm;

    public LayerMask toLayer;

    Vector3 diffVector;
    void Start()
    {
        // mask = gameObject.layer;
        grabbing = false;
        psm = GetComponent<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(blue){
            if(Input.GetButtonDown("BlueInteract")){
                grab();
                Debug.Log("grab grab");
            }
        } else {
            if(Input.GetButtonDown("PinkInteract")){
                grab();
            }
        }
        if(grabbed){
            Rigidbody2D rb = grabbed.GetComponent<Rigidbody2D>();
            // rb.velocity = transform.position + diffVector;
            grabbed.transform.position = transform.position + diffVector;
        
            if(Vector3.Distance(grabbed.transform.position, transform.position) > 2){
                grabbed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                grabbed.layer = mask;
                grabbing = false;
                grabbed = null;
                
            }
        }
    }

    void grab(){
        if(!grabbing){
            bool makeSound = false;
            //OverlapCircleAll(Vector2 point, float radius, int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity); 
            if(psm.state != State.Move){
                return;
            }
            var hits = Physics2D.OverlapCircleAll(transform.position,1.5f,grabbables);
            bool tmp = false;
            foreach (Collider2D hit in hits){
                Debug.Log(hit.gameObject.tag);
                if(hit.gameObject.tag == "Grabbable" && !tmp){
                    tmp = true;
                    grabbing = true;
                    grabbed = hit.gameObject;
                    mask = grabbed.layer;
                    // grabbed.layer = null;
                    if(blue){
                        grabbed.layer = LayerMask.NameToLayer("TmpBlue");
                    } else {
                        grabbed.layer = LayerMask.NameToLayer("TmpPink");
                    }
                    // grabbed.layer = toLayer;
                    Debug.Log(hit.gameObject.name);
                    grabbed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    diffVector = hit.gameObject.transform.position - transform.position;
                    makeSound = true;
                    continue;
                } else if (hit.gameObject.tag == "Interact"){
                    makeSound = true;
                    hit.gameObject.GetComponent<Interact>().action();
                    Debug.Log("look at that action");
                }
            }
            if(makeSound){
                FindObjectOfType<SoundManager>().Play("Select");
            }
        } else {
            grabbed.layer = mask;
            grabbed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            grabbing = false;
            grabbed = null;
            
        }
    }

    void LateUpdate(){
        
    }
}
