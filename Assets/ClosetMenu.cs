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

        if (mouseOver && Input.GetMouseButtonDown(0))
        {
            shopIcon.GetComponent<ShopMenu>().opened = !shopIcon.GetComponent<ShopMenu>().opened;
            shopIcon.GetComponent<ShopMenu>().childObj.gameObject.SetActive(shopIcon.GetComponent<ShopMenu>().opened);

            //AUDIO
            if (shopIcon.GetComponent<ShopMenu>().opened)
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
