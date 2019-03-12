using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float day;
    public float countDown;
    public int lastWeek;
    public int currentWeek;
    public GameObject weekNumText;
    private bool weekNumUpdated;

    public int fishCoin;
    public int fishCoinDisplay;
    public int debt;
    public int[] debtList;
    private bool isPaid;

    public GameObject cashText;
    public GameObject debtText;
    private Text fameStatusText;
    public GameObject endWeekPrefab;
    public GameObject endWeekText;
    public GameObject nextweekButton;
    public GameObject quitButton;
    public Image image;
    [SerializeField] private Sprite gameWinSprite;
    [SerializeField] private GameObject gameover;
    [SerializeField] private GameObject gamewin;
    private EndWeek endWeek;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        day = 1;
        countDown = 7;
        lastWeek = 4;
        currentWeek = 1;
        fishCoin = 10;
        fishCoinDisplay = fishCoin;
        debt = 20;
        isPaid = false;
        weekNumUpdated = false;

        //fameStatusText = GameObject.Find("Fame Status").GetComponent<Text>();
        debt = debtList[0];

        //hide the endWeek pop-up window
        endWeek = GetComponent<EndWeek>();
        endWeekPrefab.SetActive(false);
    }

    private void Update()
    {
        DisplayCashDebt();
        PayOnDeadline();
        UpdateWeekNum();
        weekNumText.GetComponent<TextMeshProUGUI>().text = "Week " + currentWeek + " of " + lastWeek;

        // Rolling Fish Coin Display
        if ( fishCoinDisplay != fishCoin ) { // if the display isn't the same as the actual amount
            if (Mathf.Abs(fishCoinDisplay - fishCoin) < 100) { // if the disparity is small enough (<100)
                if (Time.frameCount % 4 == 0) { // updates every 4 frames (at 15 fps)
                    if (fishCoinDisplay > fishCoin) { // if display is greater than actual
                        fishCoinDisplay--;
                    }
                    else { // if display is lesser than actual
                        fishCoinDisplay++;
                    }
                }
            }
            else { // if the disparity is large enough (>100)
                if (Time.frameCount % 1 == 0) { // updates every frame (at 60 fps)
                    if (fishCoinDisplay > fishCoin) { // if display is greater than actual
                        fishCoinDisplay--;
                    }
                    else { // if display is lesser than actual
                        fishCoinDisplay++;
                    }
                }
            }
            
        }


    }

    private void DisplayCashDebt()
    {
        Debug.Log(cashText);
        debtText.GetComponent<Text>().text = "owe: " + debt;
        cashText.GetComponent<Text>().text = "" + fishCoinDisplay;
    }

    private void PayOnDeadline()
    {
        if (day % countDown == 0 && day > 1 && !isPaid)
        {
            fishCoin -= debt;
            isPaid = true;
            endWeekPrefab.SetActive(true);

            if (fishCoin <= 0)
            {//GAME OVER
                //endWeek.GameOver();
                endWeekText.GetComponent<TextMeshProUGUI>().text = "What, not enough money to pay your debt? GO TO JAIL.";
                quitButton.SetActive(true);
                nextweekButton.SetActive(false);

                //imgs
                gameover.SetActive(true);
                gamewin.SetActive(false);
                ////SceneManager.LoadScene(2);
            }
            else
            {
                if (currentWeek == lastWeek)
                { //Won the game
                    //endWeek.GameWin();
                    endWeekText.GetComponent<TextMeshProUGUI>().text = "YOU WON! Debt free & famous!!!";
                    quitButton.SetActive(true);
                    nextweekButton.SetActive(false);
                    //SceneManager.LoadScene(3);
                }

                else //Go to next week
                {
                    //endWeek.NextWeek();
                    endWeekText.GetComponent<TextMeshProUGUI>().text = "Yay, you paid your " + debt + " FishCoin debt on time!";
                    quitButton.SetActive(false);
                    nextweekButton.SetActive(true);
                }

                ////imgs //TODO: Eunice fix
                image.sprite = gameWinSprite;
            }
            
            print("end week popup");

            debt = debtList[currentWeek];
        }
    }

    private void UpdateWeekNum()
    {
        if (day > 1 && day % countDown == 1 && !weekNumUpdated)
        {
            currentWeek++;
            weekNumUpdated = true;
        }
        if (day % countDown == 0)
            weekNumUpdated = false;
    }
}