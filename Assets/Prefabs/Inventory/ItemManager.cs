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

    // Previously hard coded number variables
    private float shopAnchorOffSetX;
    private float closetAnchorOffSetX;
    private float anchorOffSetY;
    private float iconEase;
    private float massEase;
    private float itemScaleEase;
    private float storeItemEase;
    private float closetItemEase;
    private float itemRecallEase;
    private float arrowX;
    private float arrowUpY;
    private float arrowDownY;
    private int itemSpacing;
    private int itemsInRow;
    [SerializeField] private float arrowCeiling;
    private float arrowFloor;
    private Vector3 arrowScale;
    private Vector3 windowScale;

    GameObject storeMask;
    GameObject closetMask;

    public bool storeIsUp = false;
    public bool storeIsDown = false;
    public bool closetIsUp = false;
    public bool closetIsDown = false;

    Vector3 shopAnchorSpot;
    Vector3 closetAnchorSpot;

    List<ItemBehavoir> itemScriptList = new List<ItemBehavoir>();

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

        // Previously hard coded numbers
        shopAnchorOffSetX = 1.36f;
        closetAnchorOffSetX = -5.36f;
        anchorOffSetY = -1.1f;
        iconEase = 0.1f;
        massEase = 0.2f;
        itemScaleEase = 0.1f;
        storeItemEase = 0.4f;
        closetItemEase = 0.1f;
        itemRecallEase = 0.1f;
        arrowX = 3.39f;
        arrowUpY = 4.43f;
        arrowDownY = -2.06f;
        arrowScale = 0.05f * Vector3.one;
        itemSpacing = 2;
        itemsInRow = 3;
        arrowCeiling = shopAnchor.transform.position.y + (itemSpacing/2); // 5
        arrowFloor = arrowCeiling + ( (itemSpacing/2) * (AllItemList.Length/itemsInRow) ); // 13
        windowScale = new Vector3(1, 0.725f, 1);
    
        storeMask = GameObject.Find("Store Mask");
        closetMask = GameObject.Find("Closet Mask");


        // Assign the cost to each item
        foreach (ItemLog pair in AllItemList)
        {
            GameObject temp = Instantiate(pair.prefab, transform.position, Quaternion.identity);
            temp.GetComponent<ItemBehavoir>().cost = pair.cost;
        }

        shopAnchorSpot = new Vector3(shopAnchor.transform.position.x + shopAnchorOffSetX, shopAnchor.transform.position.y + anchorOffSetY, 0);
        closetAnchorSpot = new Vector3(closetAnchor.transform.position.x + closetAnchorOffSetX, closetAnchor.transform.position.y + anchorOffSetY, 0);

        //Populate the list of item scripts
        foreach (ItemBehavoir script in FindObjectsOfType<ItemBehavoir>()) {
            itemScriptList.Add(script);
        }

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
            closetIcon.transform.position = Vector3.Lerp(closetIcon.transform.position, targetVector, iconEase);
            closetWindow.transform.position = Vector3.Lerp(closetWindow.transform.position, targetVector, iconEase);

            if (closetIcon.transform.position.x > 1) // if the closet icon gets far enough to the right
            {
                // mask info
                Vector3 maskScale = new Vector3(1.4f, 1.7f, 1);

                // store window and mask open and resize
                storeWindow.transform.localScale = Vector3.Lerp(storeWindow.transform.localScale, windowScale, massEase);
                storeMask.transform.localScale = Vector3.Lerp(storeMask.transform.localScale, maskScale, massEase);

                // closet window and mask open and resize
                closetWindow.transform.localScale = Vector3.Lerp(closetWindow.transform.localScale, windowScale, massEase);
                closetMask.transform.localScale = Vector3.Lerp(closetMask.transform.localScale, maskScale, massEase);

                // arrows open and resize
                storeUp.transform.position = Vector3.Lerp(storeUp.transform.position, new Vector3(-arrowX, arrowUpY, 0), massEase);
                storeUp.transform.localScale = Vector3.Lerp(storeUp.transform.localScale, arrowScale, massEase);

                storeDown.transform.position = Vector3.Lerp(storeDown.transform.position, new Vector3(-arrowX, arrowDownY, 0), massEase);
                storeDown.transform.localScale = Vector3.Lerp(storeDown.transform.localScale, arrowScale, massEase);

                closetUp.transform.position = Vector3.Lerp(closetUp.transform.position, new Vector3(arrowX, arrowUpY, 0), massEase);
                closetUp.transform.localScale = Vector3.Lerp(closetUp.transform.localScale, arrowScale, massEase);

                closetDown.transform.position = Vector3.Lerp(closetDown.transform.position, new Vector3(arrowX, arrowDownY, 0), massEase);
                closetDown.transform.localScale = Vector3.Lerp(closetDown.transform.localScale, arrowScale, massEase);

                // enable the closet icon's circle colider
                closetIcon.GetComponent<CircleCollider2D>().enabled = true;

                itemsOn = true;
            }

        }
        else
        {
            // closet icon moves back behind the shop icon
            Vector3 targetVector = new Vector3(transform.position.x + 0.1f, transform.position.y, 0);
            closetIcon.transform.position = Vector3.Lerp(closetIcon.transform.position, targetVector, iconEase);
            closetWindow.transform.position = Vector3.Lerp(closetWindow.transform.position, targetVector, iconEase);

            // store window closes
            storeWindow.transform.localScale = Vector3.Lerp(storeWindow.transform.localScale, Vector3.zero, iconEase);

            // closet window closes
            closetWindow.transform.localScale = Vector3.Lerp(closetWindow.transform.localScale, Vector3.zero, iconEase);

            // arrows close and resize
            storeUp.transform.position = Vector3.Lerp(storeUp.transform.position, transform.position, massEase);
            storeUp.transform.localScale = Vector3.Lerp(storeUp.transform.localScale, Vector3.zero, massEase);

            storeDown.transform.position = Vector3.Lerp(storeDown.transform.position, transform.position, massEase);
            storeDown.transform.localScale = Vector3.Lerp(storeDown.transform.localScale, Vector3.zero, massEase);

            closetUp.transform.position = Vector3.Lerp(closetUp.transform.position, transform.position, massEase);
            closetUp.transform.localScale = Vector3.Lerp(closetUp.transform.localScale, Vector3.zero, massEase);

            closetDown.transform.position = Vector3.Lerp(closetDown.transform.position, transform.position, massEase);
            closetDown.transform.localScale = Vector3.Lerp(closetDown.transform.localScale, Vector3.zero, massEase);

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
            foreach (ItemBehavoir item in itemScriptList)
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
                // itemSpacing is the distance between any two items, the (i%itemsInRow) divides them into 3 columns, 
                // the i/itemsInRow makes it move to a new row after every itemsInRow items, that is added to the initial anchor spot
                Vector3 targetVector = shopAnchorSpot + new Vector3(itemSpacing * (i % itemsInRow), -itemSpacing * (i / itemsInRow), 0);

                if (transform.position != targetVector) // if the item is not where it is supposed to be
                {
                    item.transform.position = Vector3.Lerp(item.transform.position, targetVector, storeItemEase);
                    item.transform.localScale = Vector3.Lerp(item.transform.localScale, Vector3.one, itemScaleEase);
                }
                i++;
            }

            // organize the items in the closet
            int j = 0;
            foreach (GameObject item in closetItems)
            {
                // same as the store organization
                Vector3 targetVector = closetAnchorSpot + new Vector3(itemSpacing * (j % itemsInRow), -itemSpacing * (j / itemsInRow), 0);

                if (transform.position != targetVector) // if the item is not where it is supposed to be
                {
                    item.transform.position = Vector3.Lerp(item.transform.position, targetVector, closetItemEase); // the closet lerps slower to draw attention to it
                    item.transform.localScale = Vector3.Lerp(item.transform.localScale, Vector3.one, itemScaleEase);
                }
                j++;
            }
        }

        else // if the inventory is closed to items
        {
            // send all the items back to the shop icon location
            foreach (ItemBehavoir item in itemScriptList)
            {
                item.transform.position = Vector3.Lerp(item.transform.position, transform.position, itemRecallEase);
                item.transform.localScale = Vector3.Lerp(item.transform.localScale, Vector3.zero, itemScaleEase);
            }
        }

        // update anchor spots
        shopAnchorSpot = new Vector3(shopAnchor.transform.position.x + shopAnchorOffSetX, shopAnchor.transform.position.y + anchorOffSetY, 0);
        closetAnchorSpot = new Vector3(closetAnchor.transform.position.x + closetAnchorOffSetX, closetAnchor.transform.position.y + anchorOffSetY, 0);

        // store up button
        if ( storeIsUp && (shopAnchor.transform.position.y > arrowCeiling) )
        {
            // moves the shop anchor upwards, scrolling the shop menu
            shopAnchor.transform.position = shopAnchor.transform.position + new Vector3(0, -itemSpacing, 0);
            AudioManager.instance.scroll.Play();
        }
        // store down button
        if (storeIsDown && (shopAnchor.transform.position.y < arrowFloor))
        {
            // moves the shop anchor downwards, scrolling the shop menu
            shopAnchor.transform.position = shopAnchor.transform.position + new Vector3(0, itemSpacing, 0);
            AudioManager.instance.scroll.Play();
        }
        // closet up button
        if (closetIsUp && (closetAnchor.transform.position.y > arrowCeiling))
        {
            // moves the closet anchor upwards, scrolling the closet menu
            closetAnchor.transform.position = closetAnchor.transform.position + new Vector3(0, -itemSpacing, 0);
            AudioManager.instance.scroll.Play();
        }
        // closet down button
        if (closetIsDown && (closetAnchor.transform.position.y < arrowFloor))
        {
            // moves the closet anchor downwards, scrolling the closet menu
            closetAnchor.transform.position = closetAnchor.transform.position + new Vector3(0, itemSpacing, 0);
            AudioManager.instance.scroll.Play();
        }

        // disable the arrows when at the top or bottom
        storeUp.GetComponent<SpriteRenderer>().enabled = shopAnchor.transform.position.y < arrowCeiling ? false : true;
        storeDown.GetComponent<SpriteRenderer>().enabled = tooLow(shopAnchor, storeItems) ? false : true;
        closetUp.GetComponent<SpriteRenderer>().enabled = closetAnchor.transform.position.y < arrowCeiling ? false : true;
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
        if ( anchor.transform.position.y > arrowFloor - itemSpacing * ( ((AllItemList.Length + (itemsInRow - 1)) / itemsInRow) - ((contents.Count + (itemsInRow - 1)) / itemsInRow) ) )
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