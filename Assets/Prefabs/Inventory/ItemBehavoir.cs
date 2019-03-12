using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemBehavoir : MonoBehaviour
{

    private bool mouseOver = false;
    public bool equipped = false;
    public int cost;
    public enum foundIn { store, closet };
    public foundIn location = foundIn.store;
    [SerializeField] Sprite wornItemSprite;
    [SerializeField] GameObject equipMark;
    GameObject checkMark;

    [HideInInspector]
    public Text costText;

    [HideInInspector]
    public Transform fishcoin;

    //enum trend { Western, Goth, Formal, Neon, Skater, Sporty, Cute, Graceful, Pirate, Southern, MiddleEast, Royal };
    [SerializeField] CompanyManager.trend style; 
    enum putOn { head, body };
    [SerializeField] putOn wornOn;

    private GameObject player;

    List<ItemBehavoir> itemScriptList = new List<ItemBehavoir>();



    void Start()
    {
       
        checkMark = Instantiate(equipMark, this.gameObject.transform);
        checkMark.transform.position = transform.position + 0.65f*Vector3.up + 0.65f*Vector3.right; 

        // start scale at (0, 0 ,0)
        transform.localScale = Vector3.zero;

        costText = gameObject.GetComponentInChildren<Text>();
        fishcoin = gameObject.transform.GetChild(0).GetChild(1);

        player = GameObject.FindGameObjectWithTag("Player");

        //Display Money
        costText.text = "" + cost;

        //Populate the list of item scripts
        foreach (ItemBehavoir script in FindObjectsOfType<ItemBehavoir>()) {
            itemScriptList.Add(script);
        }
    }


    void Update()
    {

        // Buying Code
        if ( mouseOver && (location == foundIn.store) && Input.GetMouseButtonDown(0) && outMaskY() )
        {
            if (GameManager.instance.fishCoin >= cost)
            {
                GameManager.instance.fishCoin -= cost;
                location = foundIn.closet;
                costText.enabled = false;
                fishcoin.GetComponent<SpriteRenderer>().enabled = false;
                AudioManager.instance.job_success.Play();
            }
            else
            {
                Debug.Log("You don't have enough money...");
                AudioManager.instance.error.Play();
            }
        }

        

        // Unequip Code
        else if ( mouseOver && (location == foundIn.closet) && Input.GetMouseButtonDown(0) && equipped && outMaskY() )
        {
            equipped = !equipped;
            AudioManager.instance.unequip.Play();

            if (wornOn == putOn.head) // head
            {
                player.GetComponent<PlayerBehavior>().headItem = null;
                player.GetComponent<PlayerBehavior>().headStyle = CompanyManager.trend.BirthdaySuit;
            }
            else if (wornOn == putOn.body) // body
            {
                player.GetComponent<PlayerBehavior>().bodyItem = null;
                player.GetComponent<PlayerBehavior>().bodyStyle = CompanyManager.trend.BirthdaySuit;
            }
        }



        // Equip Code
        else if ( mouseOver && (location == foundIn.closet) && Input.GetMouseButtonDown(0) && outMaskY() )
        {
            
            // if this is a headpiece, look through all the items that are also 
            // headpieces and unequip the one that was previously equipped
            if (wornOn == putOn.head) // head
            {
                player.GetComponent<PlayerBehavior>().headItem = wornItemSprite;
                player.GetComponent<PlayerBehavior>().headStyle = style;
                foreach (ItemBehavoir item in itemScriptList)
                {
                    if (item.wornOn == putOn.head)
                    {
                        if (item.equipped)
                        {
                            item.equipped = false;
                            continue;
                        }
                    }
                }
            }
            // if this is a bodypiece, look through all the items that are also 
            // bodypieces and unequip the one that was previously equipped
            else if (wornOn == putOn.body) // body
            {
                player.GetComponent<PlayerBehavior>().bodyItem = wornItemSprite;
                player.GetComponent<PlayerBehavior>().bodyStyle = style;
                foreach (ItemBehavoir item in itemScriptList)
                {
                    if (item.wornOn == putOn.body)
                    {
                        if (item.equipped)
                        {
                            item.equipped = false;
                            continue;
                        }
                    }
                }
            }
            equipped = true;
            AudioManager.instance.equip.Play();
        }

        checkMark.SetActive(equipped);

        // hide the cost text
        if (!outMaskY())
        {
            costText.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (!costText.enabled && location == foundIn.store)
        {
            costText.enabled = true;
        }

        if (outMaskY())
        {
            GetComponent<BoxCollider2D>().enabled = true;
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

    private bool outMaskY()
    {
        if ((transform.position.y < -1) || (transform.position.y > 4))
        {
            return false;
        }
        return true;

    }
    
}
