using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTransition : MonoBehaviour
{

    public float boardTransitionSpeed;
    bool isTransitioning;
    public Vector3 nextTransitionDest;
    // Start is called before the first frame update
    void Start()
    {
        nextTransitionDest = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextTransitionDest, boardTransitionSpeed * Time.deltaTime);

        if(isTransitioning && transform.position == nextTransitionDest)
        {
            isTransitioning = false;
            SetAllowMove();
        }

    }
    public void SetTransition(Vector3 dest)
    {
        isTransitioning = true;
        nextTransitionDest = dest;
    }
    void SetAllowMove()
    {
        GameManager.instance.canMove = true;
        GameManager.instance.ballWithTrajectory.SetActive(true);
    }
}
