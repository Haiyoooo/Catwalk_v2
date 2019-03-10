using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScroll : MonoBehaviour
{

    private bool mouseOver = false;
    //[SerializeField] GameObject shopIcon;
    [SerializeField] GameObject itemManager;
    [SerializeField] bool isUp;
    [SerializeField] bool inStore;
    [SerializeField] Transform shopAnchor;
    [SerializeField] Transform closetAnchor;


    void Start()
    {
        // Find the objects that the arrows reference
        //shopIcon = GameObject.Find("Shop Icon");
        itemManager = GameObject.Find("Item Manager");
        if (inStore)
        {
            shopAnchor = transform.Find("StoreAnchor(Clone)");
        }
        else
        {
            closetAnchor = transform.Find("ClosetAnchor(Clone)");
        }

        // make invisible until inventory is opened
        transform.localScale = Vector3.zero;
    }

    
    void Update()
    {

        
        if (mouseOver && Input.GetMouseButtonDown(0))
        {
            // store scroll
            if (inStore)
            {
                if (isUp)
                {
                    itemManager.GetComponent<ItemManager>().storeIsUp = true;
                }
                else
                {
                    itemManager.GetComponent<ItemManager>().storeIsDown = true;
                }
            }

            // closet scroll
            else
            {
                if (isUp)
                {
                    itemManager.GetComponent<ItemManager>().closetIsUp = true;
                }
                else
                {
                    itemManager.GetComponent<ItemManager>().closetIsDown = true;
                }
            }


        }


        // disable when at the top or bottom
        if (inStore)
        {
            if (isUp)
            {
                if (shopAnchor.transform.position.y < 5)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            else
            {
                if (shopAnchor.transform.position.y > 13)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
        else
        {
            if (isUp)
            {
                if (closetAnchor.transform.position.y < 5)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            else
            {
                if (closetAnchor.transform.position.y > 13)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                }
            }
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
