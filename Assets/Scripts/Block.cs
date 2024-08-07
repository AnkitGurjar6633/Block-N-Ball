using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    int points;
    public TMP_Text tMP_Text;
    public SpriteRenderer spriteRenderer;
    float r;
    float b;
    float g;
    // Start is called before the first frame update
    void Awake()
    {
        points = GameManager.instance.currentScore;
        if(Random.Range(0, 2) == 1)
        {
            points *= 2;
        }
        tMP_Text.text = points.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(points <= 1)
            {
                Destroy(gameObject);
            }
            points -= 1;
            tMP_Text.text = points.ToString();
        }
    }

    void HandleColor()
    {
        //if(points < )
    }
}
