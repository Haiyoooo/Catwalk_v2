using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;
    private GameObject[] spawnpoints;
    [SerializeField] private GameObject eventPrefab;
    [HideInInspector] public int totalJobs = 0;
    [HideInInspector] public int totalParties = 0;
    [SerializeField] private int maxJobs = 0;
    [SerializeField] private int maxParties = 5;
    int rng; //random number

    private void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        spawnpoints = GameObject.FindGameObjectsWithTag("City");
    }


    void Update()
    {
        //spawn jobs/parties on an empty city
        if (totalJobs < maxJobs)
        {
            spawnAsJob(emptyCity(), true); //if true, spawn job. if false, spawn party.
        }

        if (totalParties < maxParties)
        {
            spawnAsJob(emptyCity(), false);
        }

    }

    private Transform emptyCity()
    {
        //find an empty spawn point
        //if the spawnpoint has a child, it means it has an event, so find another random number
        do
        {
            rng = Random.Range(0, spawnpoints.Length);
        }
        while
        (spawnpoints[rng].transform.childCount != 0);
        return spawnpoints[rng].transform;
    }

    private void spawnAsJob(Transform spawn, bool isJob)
    {
        //spawn a new event as job
        var newEvent = Instantiate(eventPrefab, spawn.position, Quaternion.identity);
        newEvent.transform.parent = spawn.transform;
        if (isJob)
        {
            newEvent.GetComponent<EventBehavior>().isJob = true;
            totalJobs++;
        }
        else
        {
            newEvent.GetComponent<EventBehavior>().isJob = false;
            totalParties++;
        }
    }

    //call this function whenever you need to destroy an event
    //updates the counts
    //*** how does it work? is it like copy pasting it somewhere?
    public void destroyEvent(bool isJob)
    {
        if (isJob == false)
        {
            totalJobs--;
        }
        else
        {
            totalParties--;
        }
    }
}
