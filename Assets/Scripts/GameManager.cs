
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("Tracking Variables")]
    public int coinCount;
    public int ballCount;
    public int currentScore ;
    public int bestScore;
    public bool canMove;
    public bool isGameOver;
    public bool toSave;
    public List<GameObject> ballsList;
    public float smoothFactor = 10f;
    bool isTouching;
     float targetAngle = 0;


    [Header("Game Objects & Prefabs")]
    public GameObject ballPrefab;
    public GameObject ballWithTrajectory;
    public GameObject trajectoryBalls;
    public GameObject trajectoryIndicator;
    public GameObject coinPrefab;
    public GameObject blockPrefab;
    public GameObject powerUpPrefab;
    public GameObject StartGameUI;
    public GameObject GameOverUI;
    public GameObject RunningUI;
    public GameObject ballCountUI;
    public Transform ballUIPosition;




    Touch touch;
    Vector2 touchStartPos;
    Vector2 touchEndPos;
    float minTrajectoryBallsSize;
    float maxTrajectoryBallsSize = 3.33f;
    public TMP_Text ballCountUIText;

    [Header("Misc")]
    public BoardTransition board;
    float currentBoardOffset;
    float ballLaunchInterval = 0.1f;
    float trajectoryBallsScaleMultiplier = 0.006f;
    int[] nextBlockRow ;
    int rowLength = 7;
    public LowerBound lB;

    public static GameManager instance;

    private void Awake()
    {
        Application.targetFrameRate = 90;
        instance = this;
        //currentMovePosition = ballPrefab.transform.position;
    }


    void Start()
    {
        StartGameUI.SetActive(true);
        isGameOver = true;
    }


    void Update()
    {
        if(!isGameOver)
        {
            HandleTouch();
        }
        if(bestScore < currentScore)
        {
            toSave = true;
            bestScore = currentScore;
        }
        if (toSave)
        {
            SaveData();
            toSave = false;
        }
    }

    void HandleTouch()
    {
        if (isTouching)
        {
            Vector2 direction = touchStartPos - touch.position;

            if (direction != Vector2.zero)
            {
                targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                ballWithTrajectory.transform.eulerAngles = new Vector3(0, 0, targetAngle - 90);
            }

            float scale = (touchStartPos.y - touch.position.y - 20) * trajectoryBallsScaleMultiplier + minTrajectoryBallsSize;

            if (scale > minTrajectoryBallsSize && targetAngle < 173 && targetAngle > 7)
            {
                trajectoryIndicator.SetActive(true);
                if (scale < maxTrajectoryBallsSize)
                    trajectoryBalls.transform.localScale = new Vector3(scale, scale);

            }

            else
            {
                trajectoryIndicator.SetActive(false);
            }
        }


        if (Input.touchCount > 0 && canMove)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isTouching = true;
            }
            //else if (touch.phase == TouchPhase.Moved)
            //{
            //        float scale = (touchStartPos.y - touch.position.y - 20) * trajectoryBallsScaleMultiplier + minTrajectoryBallsSize;


            //    //float targetAngle = Vector2.Angle(touchStartPos - touch.position, Vector2.right);

            //    //Vector2 direction = touchStartPos  - touch.position;

            //    //if(direction != Vector2.zero)
            //    //{
            //    //    targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //    //    ballWithTrajectory.transform.eulerAngles = new Vector3(0, 0, targetAngle - 90);
            //    //}

            //    //ballWithTrajectory.transform.rotation = Quaternion.Lerp(ballWithTrajectory.transform.rotation, Quaternion.Euler(0, 0, targetAngle - 90), Time.deltaTime * smoothFactor );

            //    //float smoothedAngle = Mathf.LerpAngle(ballWithTrajectory.transform.eulerAngles.z, targetAngle - 90, smoothFactor);



            //    if (scale > minTrajectoryBallsSize && targetAngle < 173 && targetAngle > 7)
            //    {
            //        trajectoryIndicator.SetActive(true);
            //        if (scale < maxTrajectoryBallsSize)
            //            trajectoryBalls.transform.localScale = new Vector3(scale, scale);


            //        //ballWithTrajectory.transform.eulerAngles = new Vector3(0, 0, targetAngle -90);

            //        //ballWithTrajectory.transform.Rotate(0, 0, (targetAngle - 90) - ballWithTrajectory.transform.eulerAngles.z);


            //    }
                
            //    else
            //    {
            //        trajectoryIndicator.SetActive(false);
            //    }
            //}
            else if (touch.phase == TouchPhase.Ended && trajectoryIndicator.activeInHierarchy)
            {
                canMove = false;
                isTouching = false;
                touchEndPos = touch.position;
                trajectoryIndicator.SetActive(false);
                //currentMovePosition = ballUIPosition.transform.position;
                StartCoroutine(PlayerMove());
            }
        }
    }

    IEnumerator PlayerMove()
    {
        Vector2 direction = (touchStartPos - touchEndPos).normalized;
        int currentBallCount = ballCount;

        //while (currentBallCount>0)
        //{
        //    GameObject ballClone = Instantiate(ballPrefab, currentMovePosition, ballWithTrajectory.transform.rotation);
        //    ballClone.SetActive(true);
        //    ballClone.GetComponent<Ball>().AddVelocity(direction);
        //    yield return new WaitForSeconds(ballLaunchInterval);
        //    currentBallCount--;
        //    SetBallCountUI(currentBallCount);
        //}


        for (int i = 0; i < ballsList.Count; i++)
        {
            Ball tempBall = ballsList[i].GetComponent<Ball>();
            tempBall.UnsetTransition();
            tempBall.AddVelocity(direction);
            currentBallCount--;
            SetBallCountUI(currentBallCount);
            yield return new WaitForSeconds(ballLaunchInterval);
        }

        ballWithTrajectory.SetActive(false);
        ballCountUI.SetActive(false);
    }


    //0 for empty || 1 for block || 2 for ball PowerUp || 3 for coin
    public void GenerateNextRow()
    {
        nextBlockRow = new int[rowLength];
        if(currentScore > 1)
        {
            nextBlockRow[Random.Range(0, 7)] = 2;
            
            if(Random.Range(0,2) == 1)                 //check for coin
            {
                int coinIndex = Random.Range(0, 7);
                while (nextBlockRow[coinIndex] != 0)
                {
                    coinIndex = Random.Range(0, 7);
                }
                nextBlockRow[coinIndex] = 3;
            }

            int mustBlockIndex = Random.Range(0, 7);
            while (nextBlockRow[mustBlockIndex] != 0)
            {
                mustBlockIndex = Random.Range(0, 7);
            }
            nextBlockRow[mustBlockIndex] = 1;
        }
        for(int i = 0;  i < rowLength; i++)
        {
            if (nextBlockRow[i] == 0)
            {
                nextBlockRow[i] = Random.Range(0, 2);
            }
        }
    }

    public void SpawnNextRowGameObjects()
    {
        for(int i = 0;i < rowLength; i++)
        {
            GameObject temp;
            if (nextBlockRow[i] == 1)
            {
                temp = Instantiate(blockPrefab, board.transform);
            }
            else if (nextBlockRow[i] == 2)
            {
                temp = Instantiate(powerUpPrefab, board.transform);
            }
            else if (nextBlockRow[i] == 3)
            {
                temp = Instantiate(coinPrefab, board.transform);
            }
            else
            {
                continue;
            }
            temp.SetActive(true);
            temp.transform.localPosition = new Vector2(i - 2.5f, currentBoardOffset);
        }
        currentBoardOffset += 1.0f;
        board.SetTransition(new Vector3(board.transform.position.x, board.transform.position.y - 1.0f,0));
    }
    
    public IEnumerator AddBall(int count, Vector2 nextMovePos)
    {
        for(int i =0 ;i < count;i++)
        {
            GameObject newBall = Instantiate(ballPrefab, nextMovePos, ballWithTrajectory.transform.rotation);
            newBall.SetActive(true);
            ballsList.Add(newBall);
            yield return new WaitForSeconds(ballLaunchInterval);
        }
    }

    public IEnumerator DestroyAllBalls()
    {
        for(int i = 0 ;i < ballsList.Count; i++)
        {
            Destroy(ballsList[i]);
            yield return new WaitForEndOfFrame();
        }
        ballsList.Clear();
    }

    public void StartGame()
    {
        StartGameUI.SetActive(false);
        isGameOver = false;
        LoadData();
        ballCount = 1;
        StartCoroutine(AddBall(ballCount,ballPrefab.transform.position));
        ballCountUIText.text = "x" + ballCount.ToString();
        ballCountUI.SetActive(true);
        currentBoardOffset = 0;
        canMove = false;
        currentScore = 1;
        trajectoryIndicator.SetActive(false);
        minTrajectoryBallsSize = trajectoryBalls.transform.localScale.x;
        GenerateNextRow();
        SpawnNextRowGameObjects();
        RunningUI.SetActive(true);
        
    }
    public void Restart()
    {
        ballUIPosition.transform.position = ballPrefab.transform.position;
        ballWithTrajectory.transform.rotation = ballPrefab.transform.rotation;
        ballCount = 1;
        StartCoroutine(AddBall(ballCount,ballPrefab.transform.position));
        LoadData();
        canMove = false;
        ballCountUIText.text = "x" + ballCount.ToString();
        ballCountUI.SetActive(true);
        currentScore = 1;
        trajectoryIndicator.SetActive(false);
        GenerateNextRow();
        SpawnNextRowGameObjects();
        GameOverUI.SetActive(false);
        isGameOver = false;
        RunningUI.SetActive(true);
        lB.UpdateBallCount();
    }

    public void SetBallCountUI(int count)
    {

        ballCountUIText.text = "x" + count.ToString();
    }

    void SaveData()
    {
        SaveSystem.Save(coinCount, bestScore);
    }

    void LoadData()
    {
        GameData gameData = SaveSystem.Load();
        this.bestScore = gameData.bestScore;
        this.coinCount = gameData.coinCount;

    }
}
