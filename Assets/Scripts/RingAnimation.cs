using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingAnimation : MonoBehaviour
{
    private float minSize;
    public float maxSize;
    public float speed = 4f;
    private float toAdd;
    // Start is called before the first frame update
    void Start()
    {
        minSize = this.transform.localScale.x;
        toAdd = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(this.transform.localScale.x > maxSize)
        {
            toAdd = -speed;
        }
        if(this.transform.localScale.x < minSize)
        {
            toAdd = speed;
        }

        this.transform.localScale = new Vector2(this.transform.localScale.x + (toAdd * Time.deltaTime), this.transform.localScale.y + (toAdd * Time.deltaTime));

    }
}
