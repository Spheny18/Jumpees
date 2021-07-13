using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject pink;
    public GameObject blue;
    public float minimumSize;
    public float scaleSpeed = 10;
    float size;
    Camera camera;
    void Start()
    {
        size = minimumSize;
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 diffVector = pink.transform.position - blue.transform.position;
        
        size =  Mathf.Abs(diffVector.y) ;
        if(size < Mathf.Abs(diffVector.x)){
            size = Mathf.Abs(diffVector.x);
        }
        
        // Debug.Log(size + " | " + diffVector);
        if(Mathf.Abs(size) < minimumSize){
            size = minimumSize;
        }
        camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize,Mathf.Abs(size),scaleSpeed*Time.deltaTime);

        transform.position = (blue.transform.position + pink.transform.position)/2;
        transform.position = transform.position - Vector3.forward + Vector3.up;
    }
}
