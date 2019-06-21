using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostTile : MonoBehaviour
{
    private float alpha = 2f;
    private float floatSpeed = 1000f;
    [HideInInspector]
    public float tileY;
    [HideInInspector]
    public Sprite tile;
    [HideInInspector]
    public bool destroyable; //Prevents the parent ghost tile from being destroyed.

    void Update()
    {
        if(alpha >= 0)
        {
            alpha -= 0.1f;
            tileY += floatSpeed * Time.deltaTime;
            transform.position = new Vector2(transform.position.x,tileY);

            Color color = gameObject.GetComponent<Image>().color;
            color.a = alpha;
            gameObject.GetComponent<Image>().color = color;
        }
        else
        {
            if (destroyable)
            {
                Destroy(gameObject);
            }
        }
    }
}
