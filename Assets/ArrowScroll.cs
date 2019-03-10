using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScroll : MonoBehaviour
{
    private bool mouseOver = false;
    [SerializeField] GameObject shopIcon;
    [SerializeField] bool isUp; // is it an up or down arrow, decided in the inspector of each arrow prefab
    [SerializeField] bool inStore; // is it for the store of the closet, decided in the inspector of each arrow prefab
    [SerializeField] Transform shopAnchor;
    [SerializeField] Transform closetAnchor;


    void Start()
    {
        // Find the objects that the arrows reference
        shopIcon = GameObject.Find("Shop Icon");
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
                    shopIcon.GetComponent<ItemManager>().storeIsUp = true;
                }
                else
                {
                    shopIcon.GetComponent<ItemManager>().storeIsDown = true;
                }
            }

            // closet scroll
            else
            {
                if (isUp)
                {
                    shopIcon.GetComponent<ItemManager>().closetIsUp = true;
                }
                else
                {
                    shopIcon.GetComponent<ItemManager>().closetIsDown = true;
                }
            }


        }

        // if it is visible, then you can press the arrow
        GetComponent<BoxCollider2D>().enabled = GetComponent<SpriteRenderer>().enabled;

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
