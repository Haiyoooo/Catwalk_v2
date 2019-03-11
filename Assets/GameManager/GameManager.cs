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

    public int fishCoin;
    public int debt;
    public int[] debtList;
    private bool isPaid;

    public GameObject cashText;
    //public GameObject debtText;
    //private Text fameStatusText;
    public GameObject endWeekPrefab;
    public GameObject endWeekText;
    public GameObject nextweekButton;
    public GameObject quitButton;
    [SerializeField] private GameObject gameover;
    [SerializeField] private GameObject gamewin;
    
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
        debt = 20;
        isPaid = false;

        //fameStatusText = GameObject.Find("Fame Status").GetComponent<Text>();
        debt = debtList[0];

        //hide the endWeek pop-up window
        endWeekPrefab.SetActive(false);
    }

    private void Update()
    {
        DisplayCashDebt();
        PayOnDeadline();
    }

    private void DisplayCashDebt()
    {
        Debug.Log(cashText);
        //debtText.GetComponent<Text>().text = "owe: " + debt;
        cashText.GetComponent<Text>().text = "" + fishCoin;
    }

    private void PayOnDeadline()
    {
        if (day % countDown == 0 && day > 1 && !isPaid)
        {
            //fishCoin -= debt;
            isPaid = true;
            endWeekPrefab.SetActive(true);

            if (fishCoin <= 0)
            {//GAME OVER
                endWeekText.GetComponent<TextMeshProUGUI>().text = "What, not enough money to pay your debt? GO TO JAIL.";
                quitButton.SetActive(true);
                nextweekButton.SetActive(false);

                //imgs
                gameover.SetActive(true);
                gamewin.SetActive(false);
                //SceneManager.LoadScene(2);
            }
            else
            {
                if (currentWeek == lastWeek)
                { //Won the game
                    endWeekText.GetComponent<TextMeshProUGUI>().text = "YOU WON! Debt free & famous!!!";
                    quitButton.SetActive(true);
                    nextweekButton.SetActive(false);
                    //SceneManager.LoadScene(3);
                }

                else //Go to next week
                {
                    endWeekText.GetComponent<TextMeshProUGUI>().text = "Yay, you paid your " + debt + " FishCoin debt on time!";
                    quitButton.SetActive(false);
                    nextweekButton.SetActive(true);
                }

                //imgs
                gameover.SetActive(false);
                gamewin.SetActive(true);
            }
            
            print("end week popup");

            debt = debtList[currentWeek];
            currentWeek++;
        }
    }
}