using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timebar : MonoBehaviour
{
    public static Timebar instance = null;
    [SerializeField] private GameObject[] dayBox;
    [SerializeField] private Slider barSlider;
    [SerializeField] private Sprite noFill; //day hasn't been passed yet
    [SerializeField] private Sprite fill; //day is in the past
    [SerializeField] private RectTransform deadlineCatRect; //deadline cat icon

    private GameObject[] dayNumberText;
    private int[] dayNumber; //e.p. the first Monday is 1
    private float dayIndex;  //e.p. Monday is 0, Tuesday is 1
    private float goalValue;
    //private Timebar_Animation timebar_Animation;
    private bool dayNumUpdated;
    private bool backWhite;
    private RectTransform[] rectTransform;
    private Image[] image;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        backWhite = false;
        dayIndex = 0;
        dayNumUpdated = false;
        dayNumber = new int[dayBox.Length];
        dayNumberText = new GameObject[dayBox.Length];
        rectTransform = new RectTransform[dayBox.Length];
        image = new Image[dayBox.Length];

        for(int i = 0; i < dayBox.Length; i++)
        {
            dayNumberText[i] = dayBox[i].transform.GetChild(0).gameObject;
            dayNumber[i] = i + 1;
            rectTransform[i] = dayBox[i].GetComponent<RectTransform>();
            image[i] = dayBox[i].GetComponent<Image>();
            image[i].sprite = noFill;
        }
    }

    private void Update()
    {
        dayIndex = GameManager.instance.day % GameManager.instance.countDown - 1;
        goalValue = dayIndex * (1 / (GameManager.instance.countDown - 1));
        TimebarValue();
        if (dayIndex == 0 && !dayNumUpdated && GameManager.instance.day != 1)
        {
            UpdateDayNumbers();
            dayNumUpdated = true;
        }
        ChangeColor();
    }

    private void TimebarValue()
    {
        //Timebar day marks animation
        //if it is the last day of the week....
        if (dayIndex == -1)
            barSlider.value = Mathf.Lerp(barSlider.value, 1, 0.02f);
        else
        {
            //if it is the first day of the week...
            if (dayIndex == 0)
            {
                backWhite = true;
                barSlider.value = 0;
            }
            else //if it is day 2 - day 6...
                barSlider.value = Mathf.Lerp(barSlider.value, goalValue, 0.02f);
        }
    }

    //Display only
    private void UpdateDayNumbers() //Display the index of each day and update when move to next week
    {
        for (int i = 0; i < dayNumberText.Length; i++)
        {
            dayNumber[i] += 7;
            dayNumberText[i].GetComponent<TextMeshProUGUI>().text = "Day " + dayNumber[i];
        }
    }

    private void ChangeColor()
    {
        if (backWhite) //all the day boxes except the first one change back to white at the first day of each week
        {
            for (int i = 1; i < image.Length; i++)
                image[i].sprite = noFill;
            backWhite = false;
        }

        //if (dayIndex == 0) //change the 7th day box back to white when new week starts
        //image[6].sprite = noFill;
        
        //Change that day's box to yellow except the 7th day
        if (barSlider.value >= goalValue - 0.05)
        {
            if (dayIndex != -1)
                image[(int)dayIndex].sprite = fill;
            else
                image[6].sprite = fill;
        } 
    }
}