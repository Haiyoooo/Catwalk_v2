using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    private bool mouseOver = false;
    public bool opened = false;
    public bool itemsOn = false;
    [SerializeField] GameObject tempClosetIcon;
    [SerializeField] GameObject tempStoreWindow;
    [SerializeField] GameObject tempClosetWindow;
    public Transform childObj;
    private Vector3 windowScale;
    GameObject storeMask;
    GameObject closetMask;

    [SerializeField] GameObject tempStoreUp;
    [SerializeField] GameObject tempStoreDown;
    [SerializeField] GameObject tempClosetUp;
    [SerializeField] GameObject tempClosetDown;

    // Game objects that will be spawned on start
    GameObject closetIcon;
    GameObject storeWindow;
    GameObject closetWindow;
    GameObject storeUp;
    GameObject storeDown;
    GameObject closetUp;
    GameObject closetDown;

    void Start()
    {
        // Instantiate all the necesary prefabs for the inventory
        closetIcon = Instantiate(tempClosetIcon, transform.position, Quaternion.identity);
        storeWindow = Instantiate(tempStoreWindow, transform.position, Quaternion.identity);
        storeWindow.transform.localScale = Vector3.zero;
        closetWindow = Instantiate(tempClosetWindow, transform.position, Quaternion.identity);
        closetWindow.transform.localScale = Vector3.zero;
        //storeUp = Instantiate(tempStoreUp, transform.position, Quaternion.identity);
        //storeDown = Instantiate(tempStoreDown, transform.position, Quaternion.identity);
        //closetUp = Instantiate(tempClosetUp, transform.position, Quaternion.identity);
        //closetDown = Instantiate(tempClosetDown, transform.position, Quaternion.identity);

        // finds and disables the gray out rect
        childObj = transform.Find("Canvas");
        childObj.gameObject.SetActive(false);
        windowScale = new Vector3(1, 0.725f, 1);

        storeMask = GameObject.Find("Store Mask");
        closetMask = GameObject.Find("Closet Mask");
    }

    
    void Update()
    {
        if (mouseOver && Input.GetMouseButtonDown(0))
        {
            opened = !opened;
            childObj.gameObject.SetActive(opened);

            //AUDIO
            if (opened)
            {
                AudioManager.instance.open_shop.Play();
            }
            else
            {
                AudioManager.instance.close_shop.Play();
            }
            
        }

        if (opened)
        {

            // closet icon moves to the top right of the screen
            Vector3 targetVector = new Vector3( -transform.position.x, transform.position.y, 0 );
            closetIcon.transform.position = Vector3.Lerp(closetIcon.transform.position, targetVector, 0.1f);
            closetWindow.transform.position = Vector3.Lerp(closetWindow.transform.position, targetVector, 0.1f);

            if (closetIcon.transform.position.x > 1) // if the closet icon gets far enough to the right
            {
                // mask info
                Vector3 maskScale = new Vector3(1.4f, 1.7f, 1);

                // store window opens
                storeWindow.transform.localScale = Vector3.Lerp(storeWindow.transform.localScale, windowScale, 0.2f);
                storeMask.transform.localScale = Vector3.Lerp(storeMask.transform.localScale, maskScale, 0.2f);

                // closet window opens
                closetWindow.transform.localScale = Vector3.Lerp(closetWindow.transform.localScale, windowScale, 0.2f);
                closetMask.transform.localScale = Vector3.Lerp(closetMask.transform.localScale, maskScale, 0.2f);

                // arrows open
                storeUp.transform.position = Vector3.Lerp(storeUp.transform.position, new Vector3(-3.39f, 4.43f, 0), 0.2f);
                storeUp.transform.localScale = Vector3.Lerp(storeUp.transform.localScale, 0.05f*Vector3.one, 0.2f);

                storeDown.transform.position = Vector3.Lerp(storeDown.transform.position, new Vector3(-3.39f, -2.06f, 0), 0.2f);
                storeDown.transform.localScale = Vector3.Lerp(storeDown.transform.localScale, 0.05f * Vector3.one, 0.2f);

                closetUp.transform.position = Vector3.Lerp(closetUp.transform.position, new Vector3(3.39f, 4.43f, 0), 0.2f);
                closetUp.transform.localScale = Vector3.Lerp(closetUp.transform.localScale, 0.05f * Vector3.one, 0.2f);

                closetDown.transform.position = Vector3.Lerp(closetDown.transform.position, new Vector3(3.39f, -2.06f, 0), 0.2f);
                closetDown.transform.localScale = Vector3.Lerp(closetDown.transform.localScale, 0.05f * Vector3.one, 0.2f);

                // enable the closet icon's circle colider
                closetIcon.GetComponent<CircleCollider2D>().enabled = true;

                itemsOn = true;
            }
            
        }
        else
        {
            // closet icon moves back behind the shop icon
            Vector3 targetVector = new Vector3(transform.position.x + 0.1f, transform.position.y, 0);
            closetIcon.transform.position = Vector3.Lerp(closetIcon.transform.position, targetVector, 0.1f);
            closetWindow.transform.position = Vector3.Lerp(closetWindow.transform.position, targetVector, 0.1f);

            // store window closes
            storeWindow.transform.localScale = Vector3.Lerp(storeWindow.transform.localScale, Vector3.zero, 0.1f);

            // closet window closes
            closetWindow.transform.localScale = Vector3.Lerp(closetWindow.transform.localScale, Vector3.zero, 0.1f);

            // arrows close
            storeUp.transform.position = Vector3.Lerp(storeUp.transform.position, transform.position, 0.2f);
            storeUp.transform.localScale = Vector3.Lerp(storeUp.transform.localScale, Vector3.zero, 0.2f);

            storeDown.transform.position = Vector3.Lerp(storeDown.transform.position, transform.position, 0.2f);
            storeDown.transform.localScale = Vector3.Lerp(storeDown.transform.localScale, Vector3.zero, 0.2f);

            closetUp.transform.position = Vector3.Lerp(closetUp.transform.position, transform.position, 0.2f);
            closetUp.transform.localScale = Vector3.Lerp(closetUp.transform.localScale, Vector3.zero, 0.2f);

            closetDown.transform.position = Vector3.Lerp(closetDown.transform.position, transform.position, 0.2f);
            closetDown.transform.localScale = Vector3.Lerp(closetDown.transform.localScale, Vector3.zero, 0.2f);

            // disable the closet icon's circle colider
            closetIcon.GetComponent<CircleCollider2D>().enabled = false;

            itemsOn = false;
        }

    }

    private void OnMouseEnter()
    {
        mouseOver = true;
    }
    private void OnMouseExit()
    {
        mouseOver = false;
    }

}
