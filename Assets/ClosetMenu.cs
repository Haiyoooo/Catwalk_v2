using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetMenu : MonoBehaviour
{
    private bool mouseOver = false;
    [SerializeField] GameObject shopIcon;

    void Start()
    {
        shopIcon = GameObject.Find("Shop Icon");
    }

    
    void Update()
    {
        // if you click on the closet icon once the inventory is opened
        if (mouseOver && Input.GetMouseButtonDown(0))
        {
            // the inventory will close
            shopIcon.GetComponent<ItemManager>().opened = !shopIcon.GetComponent<ItemManager>().opened;
            // this turns off the gray rectangle behind the inventory UI
            shopIcon.GetComponent<ItemManager>().childObj.gameObject.SetActive(shopIcon.GetComponent<ItemManager>().opened);

            //AUDIO
            if (shopIcon.GetComponent<ItemManager>().opened)
            {
                AudioManager.instance.open_shop.Play();
            }
            else
            {
                AudioManager.instance.close_shop.Play();
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
