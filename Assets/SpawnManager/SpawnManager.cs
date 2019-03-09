using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] cities;
    //public GameObject jobPrefab;
    //public GameObject partyPrefab;
    public GameObject eventPrefab;
    public GameObject emptyPrefab;

    private int[] job;
    private int[] party;
    private int lastParty;
    private int jobNum;
    //private TestCity testCity;

    // Start is called before the first frame update
    void Start()
    {
        job = new int[8];
        for (int i = 0; i < 8; i++) job[i] = -1;
        party = new int[2];
        for (int i = 0; i < 2; i++) party[i] = -1;
        RandomSpawn();
        //testCity = gameObject.GetComponent<TestCity>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckChange();
        Debug.Log(jobNum);
    }

    void ChangeStatus(GameObject obj, int index) //change the status of a city
    {
        GameObject newEvent;
        if (index == 1)
        {
            obj.GetComponent<City>().type = City.cityType.job;
            //obj.GetComponent<Image>().color = Color.blue;
            //newEvent = Instantiate(jobPrefab, obj.transform.position, Quaternion.identity);
            newEvent = Instantiate(eventPrefab, obj.transform.position, Quaternion.identity);
            newEvent.GetComponent<EventBehavior>().isJob = true;
            newEvent.transform.parent = obj.transform;
        }
        if (index == 2)
        {
            obj.GetComponent<City>().type = City.cityType.party;
            //obj.GetComponent<Image>().color = Color.red;
            //newEvent = Instantiate(eventPrefab, obj.transform.position, Quaternion.identity);
            newEvent = Instantiate(eventPrefab, obj.transform.position, Quaternion.identity);
            newEvent.GetComponent<EventBehavior>().isJob = false;
            newEvent.transform.parent = obj.transform;
        }
        if (index == 3)
        {
            obj.GetComponent<City>().type = City.cityType.none;
            //obj.GetComponent<Image>().color = Color.white;
            newEvent = Instantiate(emptyPrefab, obj.transform.position, Quaternion.identity);
            newEvent.transform.parent = obj.transform;
        }
        
    }

    void RandomSpawn() //spawn 1 job and 2 parties at random cities in the begining
    {
        job[0] = Random.Range(0, 10);
        ChangeStatus(cities[job[0]], 1);
        do { party[0] = Random.Range(0, 10); } while (party[0] == job[0]);
        ChangeStatus(cities[party[0]], 2);
        do { party[1] = Random.Range(0, 10); } while (party[1] == job[0] || party[1] == party[0]);
        ChangeStatus(cities[party[1]], 2);
        jobNum = 1;
    }

    void CheckChange() //detect if the job or party in a city has been finished
    {
        //If a job done then destroy it
        for (int i = 0; i < 8; i++)
            if(job[i] >= 0)
                //if (cities[job[i]].GetComponent<Image>().color == Color.white)
                if(cities[job[i]].GetComponent<City>().type == City.cityType.none)
                {
                    Destroy(cities[job[i]].transform.GetChild(0).gameObject, 0.2f);
                    job[i] = -1;
                    Debug.Log("Destroyyyyyyyyyyyyyy" + job[i]);
                    jobNum--;
                }

        //if a party done then destroy it and respawn new jobs new parties
        //if (cities[party[0]].GetComponent<Image>().color == Color.white)
        if (cities[party[0]].GetComponent<City>().type == City.cityType.none)
        {
            if (jobNum < 8) //if there is any empty city
            {
                //destroy the done party
                lastParty = party[0];
                Destroy(cities[lastParty].transform.GetChild(0).gameObject, 0.2f);
                //spawn a new party, not on the other party, not on jobs, not on the city you just finished one party
                do { party[0] = Random.Range(0, 10); } while (CheckConflict(party[0]) || party[0] == party[1] || party[0] == lastParty);
                ChangeStatus(cities[party[0]], 2);
                //spawn a new job, not on the parties, not on other jobs
                do { job[jobNum] = Random.Range(0, 10); } while (CheckConflict(job[jobNum]) || job[jobNum] == party[0] || job[jobNum] == party[1]);
                ChangeStatus(cities[job[jobNum]], 1);
                if(jobNum < 7) //if there are more than 1 empty city then spawn the second new job
                {
                    do { job[jobNum+1] = Random.Range(0, 10); } while (CheckConflict(job[jobNum + 1]) || job[jobNum+1] == party[0] || job[jobNum+1] == party[1]);
                    ChangeStatus(cities[job[jobNum+1]], 1);
                    jobNum++;
                }
                jobNum++;
            }
            else //if no empty city then spawn a new party on the same city
            {
                lastParty = party[0];
                Destroy(cities[lastParty].transform.GetChild(0).gameObject, 0.2f);
                ChangeStatus(cities[party[0]], 2);
            }
                
        }
        //if (cities[party[1]].GetComponent<Image>().color == Color.white)
        if (cities[party[1]].GetComponent<City>().type == City.cityType.none)
        {
            if (jobNum < 8)
            {
                lastParty = party[1];
                Destroy(cities[lastParty].transform.GetChild(0).gameObject, 0.2f);
                do { party[1] = Random.Range(0, 10); } while (CheckConflict(party[1]) || party[1] == party[0] || party[1] == lastParty);
                ChangeStatus(cities[party[1]], 2);
                do { job[jobNum] = Random.Range(0, 10); } while (CheckConflict(job[jobNum]) || job[jobNum] == party[0] || job[jobNum] == party[1]);
                ChangeStatus(cities[job[jobNum]], 1);
                if (jobNum < 7)
                {
                    do { job[jobNum + 1] = Random.Range(0, 10); } while (CheckConflict(job[jobNum + 1]) || job[jobNum + 1] == party[0] || job[jobNum + 1] == party[1]);
                    ChangeStatus(cities[job[jobNum + 1]], 1);
                    jobNum++;
                }
                jobNum++;
            }
            else
            {
                lastParty = party[1];
                Destroy(cities[lastParty].transform.GetChild(0).gameObject, 0.2f);
                ChangeStatus(cities[party[1]], 2);
            }
        }
    }


    //Note to Eunice:
    //If there is no Job on the spot... job[i] = -1
    //If there is a Job on the spot... job[i] = spot's number (eg. job[7] = 8 ...
    //means this is the 7th job spawn, in city number 8
    bool CheckConflict(int n) //check the city doesn't have job when spawn party
    {
        if (cities[n].GetComponent<City>().type != City.cityType.none) return true;
        return false;
    }

}
