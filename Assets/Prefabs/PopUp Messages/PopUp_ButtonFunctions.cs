using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp_ButtonFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Button function to close pop-up window
    public void closePopUp()
    {
        Destroy(transform.parent.gameObject); //hide the pop up message
    }
}
