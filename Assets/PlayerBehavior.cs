using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] public Sprite headItem;
    [SerializeField] public Sprite bodyItem;

    public CompanyManager.trend headStyle;
    public CompanyManager.trend bodyStyle;


    void Start()
    {
        //headItem = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        //bodyItem = this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
    }

    
    void Update()
    {
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = headItem;
        this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = bodyItem;
    }
}
