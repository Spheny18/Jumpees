using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [HideInInspector]
    public State state;
    Move move;
    void Start()
    {
        move = GetComponent<Move>();
        // state = move.AirMove();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("pre: " + state);
        switch(state){
            case State.Move:
            // Debug.Log("going into: " + state);
                state = move.NormalMove();
                break;
            case State.Airborne:
            // Debug.Log("going into: " + state);
                state = move.AirMove();
                break;
            case State.WallL:
            // Debug.Log("going into: " + state);
                state = move.wallMove(1);
                break;
            case State.WallR:
            // Debug.Log("going into: " + state);
                state = move.wallMove(-1);
                break;
        }
        // Debug.Log("post: " + state);
    }

    public void Die(){
        Debug.Log("Me Ded");
    }
}
public enum State{
        Move,
        Airborne,
        WallR,
        WallL
    }
    