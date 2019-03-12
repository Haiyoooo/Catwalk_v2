using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp_ButtonFunctions : MonoBehaviour
{
    private Transform thisEvent;
    private EventBehavior thisEventBehavior;

    private void Start()
    {
        thisEvent = transform.parent.parent;
        thisEventBehavior = thisEvent.GetComponent<EventBehavior>();
    }

    public void _closePopUp()
    {
        ////call the function in event Manager
        //EventManager.instance.destroyEvent(thisEventBehavior.isJob);

        //update count
        if (thisEventBehavior.isJob == false)
        {
            EventManager.instance.totalJobs--;
        }
        else
        {
            EventManager.instance.totalParties--;
        }

        //Destroy the event prefab
        //and hence pop-up message because that's a child of event Prefab.
        Destroy(thisEvent.gameObject);
    }
}
