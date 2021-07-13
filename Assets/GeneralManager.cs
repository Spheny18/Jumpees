using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    public int deaths;
    public bool PinkGoal;
    public bool BlueGoal;
    // Start is called before the first frame update
    void Start()
    {
        deaths = 0;
        DontDestroyOnLoad(this.gameObject);
        // DeathCollider dc = GameObject.Find("KillSpot").GetComponent<DeathCollider>();
        // dc.kill += Event_Kill;
    }

    // Update is called once per frame
    void Update()
    {
        if(PinkGoal && BlueGoal){
            PinkGoal = false;
            BlueGoal = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if(Input.GetKeyDown(KeyCode.R)){
            Die(gameObject);
        }
    }
    // void Event_Kill(object sender, KillArgs e){
    //     Debug.Log("kill");
    // }
    public void Die(GameObject obj){
        deaths++;
        PinkGoal = false;
        BlueGoal = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
