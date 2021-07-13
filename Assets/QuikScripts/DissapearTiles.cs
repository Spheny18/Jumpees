using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// using UnityEngine.TilemapModule;

public class DissapearTiles : MonoBehaviour
{
    public bool boolOnDefault = false;
    TilemapCollider2D tileMapCollider2D;
    TilemapRenderer tileMapRenderer;

    public GameObject trigger;
    Interact interact;
    void Start()
    {
        tileMapRenderer = GetComponent<TilemapRenderer>();
        tileMapCollider2D = GetComponent<TilemapCollider2D>();
        interact = trigger.GetComponent<Interact>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(interact.on);
        if(interact.on == true){
            tileMapRenderer.enabled = !boolOnDefault;
            tileMapCollider2D.enabled = !boolOnDefault;
        } else {
            tileMapRenderer.enabled = boolOnDefault;
            tileMapCollider2D.enabled = boolOnDefault;
        }
    }
}
