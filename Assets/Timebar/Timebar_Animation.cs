using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timebar_Animation : MonoBehaviour
{
    [Header("Icon Images")]
    [SerializeField] private Sprite noFill; //day hasn't been passed yet
    [SerializeField] private Sprite fill; //day is in the past
    private Image image;
    [SerializeField] private RectTransform deadlineCatRect; //deadline cat icon

    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.sprite = noFill;
    }

    private void Update()
    {
        if (GameManager.instance.day % GameManager.instance.countDown == 1) //change the last day box back to white when new week starts
            if (gameObject.name == "Deadline")
                image.sprite = noFill;
    }

    //fill colour when Arrow handle (current day) hits the Day Marker box
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Time Bar Handle" && !GameManager.instance.backWhite) //the arrow indicator, object name 'handle'
        {
            image.sprite = fill; //day box changes color
        }
        if (other.gameObject.tag == "Time Bar Handle" && GameManager.instance.backWhite) //all the day boxes except the first one change back to white at the first day of each week
        {
            image.sprite = noFill; 
            GameManager.instance.backWhite = false;
        }

        //deadlineCatWriggle();
    }

    //public void deadlineCatWriggle()
    //{
    //    deadlineCatRect.Rotate(new Vector3(0, 0, 45));
    //}
}
