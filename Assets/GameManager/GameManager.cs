using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float day = 0;
    public float countDown = 7;
    public Slider timeBar;
    public GameObject[] dayNumberText;
    public int[] dayNumber;
    public int lastWeek = 4;
    public int currentWeek = 1;
    public bool backWhite = false;
    private float dayIndex = 1;
    private Timebar_Animation timebar_Animation;
    private bool dayNumUpdated = false;

    public int fishCoin = 10;
    //public int debt = 20;
    //public int[] debtList;
    private bool isPaied = false;

    public GameObject cashText;
    //public GameObject debtText;
    //private Text fameStatusText;
    public GameObject endWeek;
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

        //fameStatusText = GameObject.Find("Fame Status").GetComponent<Text>();
        //debt = debtList[0];
    }

    private void Update()
    {
        TimebarValue();
        DisplayCashDebt();
        PayOnDeadline();
        if (day % countDown == 1 && !dayNumUpdated && day != 1)
        {
            UpdateDayNumbers();
            isPaied = false;
            dayNumUpdated = true;
        }
    }

    private void TimebarValue()
    {
        dayIndex = day % countDown - 1;
        float goalValue = dayIndex * (1 / (countDown - 1));

        //Timebar day marks animation
        if (day % countDown == 0)
        {
            timeBar.value = Mathf.Lerp(timeBar.value, 1, 0.02f);
        } 
        else
        {
            timeBar.value = Mathf.Lerp(timeBar.value, goalValue, 0.02f);
            if (day % countDown == 1)
                backWhite = true;
        }
    }

    private void DisplayCashDebt()
    {
        Debug.Log(cashText);
       //debtText.GetComponent<Text>().text = "owe: " + debt;

        cashText.GetComponent<Text>().text = "" + fishCoin;
    }

    private void PayOnDeadline()
    {
        if (day % countDown == 0 && day > 1 && !isPaied)
        {
            //fishCoin -= debt;
            isPaied = true;
            dayNumUpdated = false;

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

                    //imgs
                    gameover.SetActive(false);
                    gamewin.SetActive(true);

                    //SceneManager.LoadScene(3);
                }

                else //Go to next week

                {
                    //endWeekText.GetComponent<TextMeshProUGUI>().text = "Yay, you paid your " + debt + " FishCoin debt on time!";
                    quitButton.SetActive(false);
                    nextweekButton.SetActive(true);
                } 
            }
            endWeek.SetActive(true);

            //debt = debtList[currentWeek];
            currentWeek++;
        }
    }

    private void UpdateDayNumbers() //Display the index of each day and update when move to next week
    {
        for(int i = 0; i < dayNumberText.Length; i++)
        {
            dayNumber[i] += 7;
            dayNumberText[i].GetComponent<TextMeshProUGUI>().text = "Day " + dayNumber[i];
        }
    }
}