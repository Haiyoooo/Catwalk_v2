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

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.sprite = noFill;
    }

    private void Update()
    {
        ResetDayMarkerColor();
    }

    //fill colour when Arrow handle (current day) hits the Day Marker box
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Time Bar Handle") //the arrow indicator, object name 'handle'
        {
            image.sprite = fill;
        }

        //deadlineCatWriggle();
    }

    //reset all to no fill
    private void ResetDayMarkerColor()
    {
        if (GameManager.instance.backWhite)
        {
            image.sprite = noFill;
            GameManager.instance.backWhite = false;
            Debug.Log("backwhite");
        }
           
    }

    //public void deadlineCatWriggle()
    //{
    //    deadlineCatRect.Rotate(new Vector3(0, 0, 45));
    //}
}
