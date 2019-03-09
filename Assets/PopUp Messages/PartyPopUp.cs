using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyPopUp : MonoBehaviour
{
    private EventBehavior eventB;

    //private Job job;

    void Start()
    {
        eventB = transform.parent.parent.GetComponent<EventBehavior>();
        //job = transform.parent.parent.GetComponent<Job>();

    }

    //Button function to close pop-up window
    public void hidePopUp()
    {
        
            eventB.disappearOnSuccess();
        


        Destroy(transform.parent.gameObject); //hide the pop up message
    }
}
