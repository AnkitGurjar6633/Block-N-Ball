using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject,0f);
        GameManager.instance.ballCount++;
    }
}
