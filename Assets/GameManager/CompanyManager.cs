using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyManager : MonoBehaviour
{

    public enum trend { BirthdaySuit, Western, Goth, Formal, Neon, Skater, Sporty, Cute, Graceful, Pirate, Southern, Fresh, Royal };
    [Header("accept and give money")]
    [Header("It Wants: What styles the company will")]
    [Header("want over all the games")]
    [Header("It Likes: What styles the company CAN")]
    [Header("For each company:")]
    public Company[] CompanyList = new Company[9];

    void Start()
    {
        foreach (Company comp in CompanyList)
        {
            int rand1 = Random.Range(1, comp.itLikes.Length); // first random index
            int rand2;
            do
            {
                rand2 = Random.Range(1, comp.itLikes.Length); // makes a second random int that isn't the first
            } while (rand1 == rand2);

            comp.itWants[0] = comp.itLikes[rand1]; // add the likes index to the wants list
            comp.itWants[1] = comp.itLikes[rand2]; // add the likes index to the wants list
            string test = comp.itWants[1].ToString();

        }


    }


    void Update()
    {

    }


}


[System.Serializable]
public class Company
{
    public string name;
    public CompanyManager.trend[] itLikes = new CompanyManager.trend[4];
    public CompanyManager.trend[] itWants = new CompanyManager.trend[2];
}

