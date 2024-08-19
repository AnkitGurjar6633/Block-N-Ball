using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
     float speed = 10f;
     float lowerBound = -3.0f;
     bool makeTransition;
     Vector3 nextStartPos;
     Rigidbody2D Rb;
    // Start is called before the first frame update
    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
         if(makeTransition)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextStartPos, speed * Time.deltaTime);
        }
        if (transform.position.y > lowerBound && Rb.velocity.y > -0.1f && Rb.velocity.y < 0.1f)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, -0.2f);
        }
    }
    public void AddVelocity(Vector2 direction)
    {
        Rb.velocity = direction * speed;
    }
    public void SetTransition(Vector2 Pos)
    {
        makeTransition = true;
        nextStartPos = Pos;
    }
    public void UnsetTransition()
    {
        makeTransition = false;
        Rb.velocity = Vector2.zero;
    }
}
