using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LowerBound : MonoBehaviour
{
     Vector2 startPos;
     int ballsToCollide;
     int collidedBalls;

    private void Start()
    {
        startPos = GameManager.instance.ballUIPosition.transform.position;
        ballsToCollide = GameManager.instance.ballCount;
        collidedBalls = 0;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        collidedBalls++;
        if (collidedBalls == 1)
        {
            startPos = new Vector2(collision.gameObject.transform.position.x, startPos.y);
            GameManager.instance.ballUIPosition.transform.position = startPos;
            GameManager.instance.ballWithTrajectory.SetActive(true);
        }
        if (collidedBalls == ballsToCollide)
        {
            collidedBalls = 0;
            UpdateBallCount();
            GameManager.instance.SetBallCountUI(ballsToCollide);
            GameManager.instance.currentScore++;
            GameManager.instance.GenerateNextRow();
            GameManager.instance.SpawnNextRowGameObjects();
            GameManager.instance.ballCountUI.SetActive(true);
        }
        collision.gameObject.GetComponent<Ball>().SetTransition(GameManager.instance.ballUIPosition.transform.position);
        Destroy(collision.gameObject, 0.6f);
    }

    public void UpdateBallCount()
    {
        ballsToCollide = GameManager.instance.ballCount;
    }
}
