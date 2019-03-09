using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrendManager : MonoBehaviour
{
    public static TrendManager instance = null;
    float degree;
    int[] style;
    public float inSeason;
    public float passSeason;
    public float nextSeason;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.day = 1;
        style = new int[12];
        inSeason = 1;
        passSeason = 12;
        nextSeason = 2;
    }

    // Update is called once per frame
    void Update()
    {
        degree = (GameManager.instance.day - 1) * 30;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, degree);
        StyleChange();
    }

    void StyleChange()
    {
        if (GameManager.instance.day % 12 == 0)
            inSeason = 12;
        else
            inSeason = GameManager.instance.day % 12;
        if (GameManager.instance.day % 12 - 1 == 0)
            passSeason = 12;
        else if (GameManager.instance.day % 12 - 1 == -1)
            passSeason = 11;
        else
            passSeason = GameManager.instance.day % 12 - 1;
        nextSeason = GameManager.instance.day % 12 + 1;
    }
}
