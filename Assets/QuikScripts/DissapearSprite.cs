using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearSprite : MonoBehaviour
{
    public bool boolOnDefault = false;
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;

    public GameObject trigger;
    Interact interact;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        interact = trigger.GetComponent<Interact>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(interact.on);
        if(interact.on == true){
            spriteRenderer.enabled = !boolOnDefault;
            boxCollider2D.enabled = !boolOnDefault;
        } else {
            spriteRenderer.enabled = boolOnDefault;
            boxCollider2D.enabled = boolOnDefault;
        }
    }
}
