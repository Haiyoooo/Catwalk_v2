using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private bool mouseOver = false;
    public bool opened = false;
    public bool itemsOn = false;

    [SerializeField] GameObject tempClosetIcon;
    [SerializeField] GameObject tempStoreWindow;
    [SerializeField] GameObject tempClosetWindow;
    public ItemLog[] AllItemList = new ItemLog[3];
    [SerializeField] List<GameObject> storeItems = new List<GameObject>();
    [SerializeField] List<GameObject> closetItems = new List<GameObject>();
    [SerializeField] GameObject tempShopAnchor;
    [SerializeField] GameObject tempClosetAnchor;

    [SerializeField] GameObject tempStoreUp;
    [SerializeField] GameObject tempStoreDown;
    [SerializeField] GameObject tempClosetUp;
    [SerializeField] GameObject tempClosetDown;

    public Transform childObj;
    private Vector3 windowScale;
    GameObject storeMask;
    GameObject closetMask;

    public bool storeIsUp = false;
    public bool storeIsDown = false;
    public bool closetIsUp = false;
    public bool closetIsDown = false;

    Vector3 shopAnchorSpot;
    Vector3 closetAnchorSpot;

    // Game objects that will be spawned on start
    GameObject closetIcon;
    GameObject storeWindow;
    GameObject closetWindow;
    GameObject shopAnchor;
    GameObject closetAnchor;
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
        shopAnchor = Instantiate(tempShopAnchor, transform.position, Quaternion.identity);
        closetAnchor = Instantiate(tempClosetAnchor, Vector3.Scale(transform.position, new Vector3(-1, 1, 0)), Quaternion.identity); 

        storeUp = Instantiate(tempStoreUp, transform.position, Quaternion.identity);
        storeDown = Instantiate(tempStoreDown, transform.position, Quaternion.identity);
        closetUp = Instantiate(tempClosetUp, transform.position, Quaternion.identity);
        closetDown = Instantiate(tempClosetDown, transform.position, Quaternion.identity);

        // finds and disables the gray out rect
        childObj = transform.Find("Canvas");
        childObj.gameObject.SetActive(false);
        windowScale = new Vector3(1, 0.725f, 1);

        storeMask = GameObject.Find("Store Mask");
        closetMask = GameObject.Find("Closet Mask");


        // Assign the cost to each item
        foreach (ItemLog pair in AllItemList)
        {
            GameObject temp = Instantiate(pair.prefab, transform.position, Quaternion.identity);
            temp.GetComponent<ItemBehavoir>().cost = pair.cost;
        }

        shopAnchorSpot = new Vector3(shopAnchor.transform.position.x + 1.36f, shopAnchor.transform.position.y - 1.1f, 0);
        closetAnchorSpot = new Vector3(closetAnchor.transform.position.x - 5.36f, closetAnchor.transform.position.y - 1.1f, 0);

    }

    void Update()
    {
        // INVENTORY UI
        // opening and closing the inventory
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
            Vector3 targetVector = new Vector3(-transform.position.x, transform.position.y, 0);
            closetIcon.transform.position = Vector3.Lerp(closetIcon.transform.position, targetVector, 0.1f);
            closetWindow.transform.position = Vector3.Lerp(closetWindow.transform.position, targetVector, 0.1f);

            if (closetIcon.transform.position.x > 1) // if the closet icon gets far enough to the right
            {
                // mask info
                Vector3 maskScale = new Vector3(1.4f, 1.7f, 1);

                // store window and mask open and resize
                storeWindow.transform.localScale = Vector3.Lerp(storeWindow.transform.localScale, windowScale, 0.2f);
                storeMask.transform.localScale = Vector3.Lerp(storeMask.transform.localScale, maskScale, 0.2f);

                // closet window and mask open and resize
                closetWindow.transform.localScale = Vector3.Lerp(closetWindow.transform.localScale, windowScale, 0.2f);
                closetMask.transform.localScale = Vector3.Lerp(closetMask.transform.localScale, maskScale, 0.2f);

                // arrows open and resize
                storeUp.transform.position = Vector3.Lerp(storeUp.transform.position, new Vector3(-3.39f, 4.43f, 0), 0.2f);
                storeUp.transform.localScale = Vector3.Lerp(storeUp.transform.localScale, 0.05f * Vector3.one, 0.2f);

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

            // arrows close and resize
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



        // ITEM MANAGEMENT
        if (itemsOn) // if the inventory is open to items
        {
            // divides all items into store or closet lists
            storeItems.Clear();
            closetItems.Clear();
            foreach (ItemBehavoir item in GameObject.FindObjectsOfType<ItemBehavoir>())
            {
                if (item.location == ItemBehavoir.foundIn.store)
                {
                    storeItems.Add(item.gameObject);
                }
                else
                {
                    closetItems.Add(item.gameObject);
                }
            }

            // organize the items in the store
            int i = 0;
            foreach (GameObject item in storeItems)
            {
                // 2 is the distance between any two items, the (i%3) divides them into 3 columns, 
                // the i/3 makes it move to a new row after every 3 items, that is added to the initial anchor spot
                Vector3 targetVector = shopAnchorSpot + new Vector3(2 * (i % 3), -2 * (i / 3), 0);

                if (transform.position != targetVector) // if the item is not where it is supposed to be
                {
                    item.transform.position = Vector3.Lerp(item.transform.position, targetVector, 0.4f);
                    item.transform.localScale = Vector3.Lerp(item.transform.localScale, Vector3.one, 0.1f);
                }
                i++;
            }

            // organize the items in the closet
            int j = 0;
            foreach (GameObject item in closetItems)
            {
                // same as the store organization
                Vector3 targetVector = closetAnchorSpot + new Vector3(2 * (j % 3), -2 * (j / 3), 0);

                if (transform.position != targetVector) // if the item is not where it is supposed to be
                {
                    item.transform.position = Vector3.Lerp(item.transform.position, targetVector, 0.1f); // the closet lerps slower to draw attention to it
                    item.transform.localScale = Vector3.Lerp(item.transform.localScale, Vector3.one, 0.1f);
                }
                j++;
            }
        }

        else // if the inventory is closed to items
        {
            // send all the items back to the shop icon location
            foreach (ItemBehavoir item in GameObject.FindObjectsOfType<ItemBehavoir>())
            {
                item.transform.position = Vector3.Lerp(item.transform.position, transform.position, 0.1f);
                item.transform.localScale = Vector3.Lerp(item.transform.localScale, Vector3.zero, 0.1f);
            }
        }

        // update anchor spots
        shopAnchorSpot = new Vector3(shopAnchor.transform.position.x + 1.36f, shopAnchor.transform.position.y - 1.1f, 0);
        closetAnchorSpot = new Vector3(closetAnchor.transform.position.x - 5.36f, closetAnchor.transform.position.y - 1.1f, 0);

        // store up button
        if ( storeIsUp && (shopAnchor.transform.position.y > 5) )
        {
            // moves the shop anchor upwards, scrolling the shop menu
            shopAnchor.transform.position = shopAnchor.transform.position + new Vector3(0, -2, 0);
            AudioManager.instance.scroll.Play();
        }
        // store down button
        if (storeIsDown && (shopAnchor.transform.position.y < 13))
        {
            // moves the shop anchor downwards, scrolling the shop menu
            shopAnchor.transform.position = shopAnchor.transform.position + new Vector3(0, 2, 0);
            AudioManager.instance.scroll.Play();
        }
        // closet up button
        if (closetIsUp && (closetAnchor.transform.position.y > 5))
        {
            // moves the closet anchor upwards, scrolling the closet menu
            closetAnchor.transform.position = closetAnchor.transform.position + new Vector3(0, -2, 0);
            AudioManager.instance.scroll.Play();
        }
        // closet down button
        if (closetIsDown && (closetAnchor.transform.position.y < 13))
        {
            // moves the closet anchor downwards, scrolling the closet menu
            closetAnchor.transform.position = closetAnchor.transform.position + new Vector3(0, 2, 0);
            AudioManager.instance.scroll.Play();
        }

        // disable the arrows when at the top or bottom
        storeUp.GetComponent<SpriteRenderer>().enabled = shopAnchor.transform.position.y < 5 ? false : true;
        storeDown.GetComponent<SpriteRenderer>().enabled = tooLow(shopAnchor, storeItems) ? false : true;
        closetUp.GetComponent<SpriteRenderer>().enabled = closetAnchor.transform.position.y < 5 ? false : true;
        closetDown.GetComponent<SpriteRenderer>().enabled = tooLow(closetAnchor, closetItems) ? false : true;
        

        // reset the arrow bools
        if (storeIsUp)
        {
            storeIsUp = false;
        }
        if (storeIsDown)
        {
            storeIsDown = false;
        }
        if (closetIsUp)
        {
            closetIsUp = false;
        }
        if (closetIsDown)
        {
            closetIsDown = false;
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

    // Arguments: a GameObject and a List of GameObjects
    // The purpose of this function is to stop the player from scrolling through an empty menu
    // The scrolling menu size is dynamic, thanks to the use of this function
    // When used above, it disables interaction with the scrolling arrows when at the bottom of each menu
    private bool tooLow(GameObject anchor, List<GameObject> contents)
    {
        if ( anchor.transform.position.y > 13 - 2 * ( ((AllItemList.Length+2) / 3) - ((contents.Count+2) / 3) ) )
        {
            return true;
        }
        return false;
    }
    
}


// The ItemLog class stores all the equipable clothing items for the cat and the associated cost of each item
// The purpose of this class is to store all the cost information of all the items in one location, so they 
// could be manipulated in one place in the inspector for tweaking
[System.Serializable]
public class ItemLog
{
    public GameObject prefab;
    public int cost;
}