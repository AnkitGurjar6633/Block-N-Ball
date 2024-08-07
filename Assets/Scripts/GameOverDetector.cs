using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDetector : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            GameOver();
        }
        else if(collision.CompareTag("Collectables"))
        {
            Destroy(collision.gameObject,0);
        }
    }

    void GameOver()
    {
        GameManager.instance.isGameOver = true;
        GameManager.instance.GameOverUI.SetActive(true);
        GameManager.instance.ballCountUI.SetActive(false);
        StartCoroutine(DestroyAllBlocks());
        StartCoroutine(DestroyAllCollectables());
    }

    IEnumerator DestroyAllBlocks()
    {
        GameObject temp = GameObject.FindWithTag("Block");
        while (temp)
        {
            Destroy(temp);
            yield return new WaitForEndOfFrame();
            temp = GameObject.FindWithTag("Block");
        }
    }

    IEnumerator DestroyAllCollectables()
    {
        GameObject temp = GameObject.FindWithTag("Collectables");
        while (temp)
        {
            Destroy(temp);
            yield return new WaitForEndOfFrame();
            temp = GameObject.FindWithTag("Collectables");
        }
    }
}
