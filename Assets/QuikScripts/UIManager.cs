using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blue;
    public GameObject pink;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play(){
        GetComponent<SoundManager>().Play("maybe");
        
    }

    public void Quit(){
        Application.Quit();
    }
    public void PlayHover(){
        GetComponent<SoundManager>().Play("Hover");
    }

    public void QuitHover(){
        GetComponent<SoundManager>().Play("Click");
    }
    
}
