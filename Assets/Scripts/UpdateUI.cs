using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    public TMP_Text runningBestScore;
    public TMP_Text runningCurrentScore;
    public TMP_Text runningCoin;
    public TMP_Text gameOverCurrentScore;
    public TMP_Text gameOverBestScore;

    private void Update()
    {
        runningBestScore.text = GameManager.instance.bestScore.ToString();
        runningCurrentScore.text = GameManager.instance.currentScore.ToString();
        runningCoin.text = GameManager.instance.coinCount.ToString();
        gameOverCurrentScore.text = GameManager.instance.currentScore.ToString();
        gameOverBestScore.text = GameManager.instance.bestScore.ToString();
    }
}
