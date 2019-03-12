using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndWeek : MonoBehaviour
{
    [SerializeField] private Sprite gameWinSprite; //paid debt
    [SerializeField] private Sprite gameOverSprite;
    [SerializeField] private Image image;
    [SerializeField] private GameObject nextweekButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject endWeekText;

    private void Start()
    {
        //TODO: move all get components here
        nextweekButton.SetActive(false);
        restartButton.SetActive(false);
    }

    public void GameWin()
    {
        endWeekText.GetComponent<TextMeshProUGUI>().text = "YOU WON! Debt free & famous!!!";
        restartButton.SetActive(true);
        image.sprite = gameWinSprite;
    }

    public void GameOver()
    {
        endWeekText.GetComponent<TextMeshProUGUI>().text = "What, not enough money to pay your debt? GO TO JAIL.";
        restartButton.SetActive(true);
        image.sprite = gameOverSprite;
    }

    public void NextWeek()
    {
        endWeekText.GetComponent<TextMeshProUGUI>().text = "Good, you paid " + GameManager.instance.debt + " FishCoins on time. Next payment!!!";
        image.sprite = gameWinSprite;
        nextweekButton.SetActive(true);
    }
}
