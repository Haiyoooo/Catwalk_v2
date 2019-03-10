using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EventBehavior : MonoBehaviour
{
    public enum eventState { AVALIABLE, SUCCESS, SUPERSUCCESS, FAIL };
    public eventState thisEventState = eventState.AVALIABLE;
    public bool isJob;

    private float saturation;

    [Header("For Testing")]
    [Tooltip("Number of clothing items currently worn which the company likes")]
    [Range(0, 2)]
    [SerializeField] private int correctItems = 0;

    [Header("Job Stuff")]
    [SerializeField] public int salary = 20;
    [SerializeField] private float ssBonusMultiplier = 1.5f;

    [Header("Icon Images")]
    [SerializeField] private Sprite jobSprite;
    [SerializeField] private Sprite partySprite;
    private SpriteRenderer eventSpriteRenderer;

    [Header("PopUp Messages")]
    [SerializeField] private GameObject successPopUp;
    [SerializeField] private GameObject superSuccessPopUp;
    [SerializeField] private GameObject failPopUp; //reference the pop-up messages

    [Header("Disappear Animation")]
    [SerializeField] private GameObject fireworksFX;

    [Header("Tooltips Setup")]
    [SerializeField] private Text nameTooltip;
    [SerializeField] private Text salaryTooltip;
    [SerializeField] private Text themeTooltip;

    [SerializeField] private int companyNumber;
    [SerializeField] private string eventName;


    private Transform childObj;
    private CompanyManager.trend theme;
    private string themeString;

    // Animation Stuff
    private Vector3 InitialScale;
    private Vector3 FinalScale;
    private Vector3 BigScale;
    private bool playDisappear = false;
    private bool playBigger = false;
    private bool playStanderd = true;

    public string[] partyNameOptions = new string[20];

    // Start is called before the first frame update
    void Start()
    {
        //COLOR RANDOMIZER
        saturation = 0.4f;
        eventSpriteRenderer = transform.GetComponent<SpriteRenderer>();

        float r = Random.Range(saturation, 1f);     //1 is white & 0 is black
        float g = Random.Range(saturation, 1f);
        float b = Random.Range(saturation, 1f);

        Color newColor = new Color(r, g, b, 1f);

        eventSpriteRenderer.color = newColor;


        assignPartyNames();


        if (isJob)
        {
            companyNumber = Random.Range(0, 9);
            eventName = GameObject.FindGameObjectWithTag("Company Manager").GetComponent<CompanyManager>().CompanyList[companyNumber].name;
            eventSpriteRenderer.sprite = jobSprite;
            salaryTooltip.text = "Salary: " + salary.ToString();
        }
        else // is a party 
        {
            theme = (CompanyManager.trend)Random.Range(0, 12); // picks a random theme
            themeString = "Theme: " + theme.ToString(); // gets the theme name string to display
            eventSpriteRenderer.sprite = partySprite;
            salary = 0;
            themeTooltip.text = themeString;
            eventName = partyNameOptions[Random.Range(0, partyNameOptions.Length)];
        }

        nameTooltip.text = eventName;

        //tooltip is a child object of this event prefab
        childObj = transform.Find("Tooltip(Canvas)");
        childObj.gameObject.SetActive(false);

        //Appear animation
        InitialScale = new Vector3(0.1f, 0.1f, 0.1f);
        FinalScale = new Vector3(1, 1, 1);
        BigScale = new Vector3(1.5f, 1.5f, 1.5f);
        transform.localScale = InitialScale;


        if (transform.parent.name == "City1"
                || transform.parent.name == "City2"
                || transform.parent.name == "City6"
                || transform.parent.name == "City10")
        {
            childObj.gameObject.transform.position = new Vector3(childObj.gameObject.transform.position.x - 0.5f,
                childObj.gameObject.transform.position.y, childObj.gameObject.transform.position.z);
            Debug.Log("left");
        }
    }

    private void Update()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = !FindObjectOfType<ItemManager>().opened;

        if (playStanderd)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, FinalScale, Time.deltaTime * 2);
            if (!playBigger) childObj.gameObject.SetActive(false);
        }
        if (playDisappear)
            transform.localScale = Vector3.Lerp(transform.localScale, InitialScale, Time.deltaTime * 2);
        if (playBigger)
            transform.localScale = Vector3.Lerp(transform.localScale, BigScale, Time.deltaTime * 2);
    }


    //Tool tip
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            childObj.gameObject.SetActive(true);
        }
        playBigger = true;
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            childObj.gameObject.SetActive(false);
        }
        playBigger = false;
        playStanderd = true;
    }


    //Fail & Success Conditions
    private void OnMouseDown()
    {
        // If there is a UI open, stop player from clicking on other jobs.
        /* Description:
         * Checks if the mouse was clicked over a UI element
         * Prevents player from interacting with other jobs if a UI element is blocking the screen
         * (eg. Message pop-up, Shop interface is open) */
        if (!EventSystem.current.IsPointerOverGameObject())
        {

            // Determine Success
            if (isJob)
            {
                correctItems = determineJobSuccess();
            }
            else
            {
                correctItems = determinePartySuccess();
            }


            //Success
            //player is wearing 1 item that the job likes
            if (correctItems == 1)
            {
                thisEventState = eventState.SUCCESS;
                GameManager.instance.fishCoin += salary; //money
                var tempPopUp = Instantiate(successPopUp); //pop up message
                tempPopUp.transform.parent = gameObject.transform;
                Debug.Log("Success." + GameManager.instance.day + " $" + GameManager.instance.fishCoin);

                //AUDIO
                if (isJob)
                {
                    AudioManager.instance.job_success.Play();
                }
                else
                {
                    AudioManager.instance.party_success.Play();
                }

            }

            //Fail
            //player is wearing 0 items that the job likes
            else if (correctItems == 0)
            {
                thisEventState = eventState.FAIL;
                var tempPopUp = Instantiate(failPopUp); //pop up message
                tempPopUp.transform.parent = gameObject.transform;
                Debug.Log("Fail." + GameManager.instance.day + " $" + GameManager.instance.fishCoin);

                //AUDIO
                if (isJob)
                {
                    AudioManager.instance.job_fail.Play();
                }
                else
                {
                    AudioManager.instance.party_fail.Play();
                }

            }

            //Super Success
            //player is wearing 2 items that the job likes
            else if (correctItems == 2)
            {
                thisEventState = eventState.SUPERSUCCESS;
                GameManager.instance.fishCoin += Mathf.FloorToInt(salary * ssBonusMultiplier); //money
                var tempPopUp = Instantiate(superSuccessPopUp); //pop up message
                tempPopUp.transform.parent = gameObject.transform;
                Debug.Log("SUPERSUCCESS." + GameManager.instance.day + " $" + GameManager.instance.fishCoin);

                //AUDIO
                if (isJob)
                {
                    AudioManager.instance.job_success.Play();
                }
                else
                {
                    AudioManager.instance.party_success.Play();
                }
            }

            GameManager.instance.day++;
        }
    }

    public void disappearOnSuccess()
    {
        if (thisEventState == eventState.SUCCESS || thisEventState == eventState.SUPERSUCCESS)
        {
            //var FX = Instantiate(fireworksFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            //Destroy(FX.gameObject, 3f);
            Debug.Log("Disappear" + gameObject.name);
            //Destroy(gameObject, 0.2f);
            transform.GetComponentInParent<City>().type = City.cityType.none;
        }
    }

    public int determineJobSuccess()
    {
        int result = 0;
        CompanyManager.trend playerHeadStyle = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>().headStyle;
        CompanyManager.trend playerBodyStyle = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>().bodyStyle;
        CompanyManager.trend jobWant1 = GameObject.FindGameObjectWithTag("Company Manager").GetComponent<CompanyManager>().CompanyList[companyNumber].itWants[0];
        CompanyManager.trend jobWant2 = GameObject.FindGameObjectWithTag("Company Manager").GetComponent<CompanyManager>().CompanyList[companyNumber].itWants[1];
        
        // only head OR body is what the company likes
        if ((playerHeadStyle == jobWant1) || (playerHeadStyle == jobWant2) ||
             (playerBodyStyle == jobWant1) || (playerBodyStyle == jobWant2))
        {
            result = 1;
        }
        // both head and body is what the company likes
        if (((playerHeadStyle == jobWant1) || (playerHeadStyle == jobWant2)) &&
             ((playerBodyStyle == jobWant1) || (playerBodyStyle == jobWant2)))
        {
            result = 2;
        }

        return result;
    }

    public int determinePartySuccess()
    {
        int result = 0;
        CompanyManager.trend playerHeadStyle = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>().headStyle;
        CompanyManager.trend playerBodyStyle = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>().bodyStyle;

        //  if the players head AND body in the theme
        if ((playerHeadStyle == theme) && (playerBodyStyle == theme))
        {
            result = 2;
        }
        // if the players head OR body in the theme
        else if ((playerHeadStyle == theme) || (playerBodyStyle == theme))
        {
            result = 1;
        }
        
        return result;
    }

    private void assignPartyNames()
    {
        partyNameOptions[0] =  "Some CEO's Birthday Party";
        partyNameOptions[1] =  "Champagne Party";
        partyNameOptions[2] =  "Some Office Party";
        partyNameOptions[3] =  "A Gala Party";
        partyNameOptions[4] =  "A Magazine Party";
        partyNameOptions[5] =  "Fashion Week Party";
        partyNameOptions[6] =  "The Razzle Dazzle";
        partyNameOptions[7] =  "Rockin’ Rollick Party";
        partyNameOptions[8] =  "Funky Fest";
        partyNameOptions[9] =  "Booty Ball";
        partyNameOptions[10] = "Hollapalooza";
        partyNameOptions[11] = "Untamed Night";
        partyNameOptions[12] = "“Bring Your Own Booty” Dance Party";
        partyNameOptions[13] = "Drink Outside the Box";
        partyNameOptions[14] = "Muthalovin Dance Party";
        partyNameOptions[15] = "Thirsty Thursday";
        partyNameOptions[16] = "Fantastic Fridaze";
        partyNameOptions[17] = "Alan’s “Old Fart” Birthday Blowout";
        partyNameOptions[18] = "Sweet Re-Leaf";
        partyNameOptions[19] = "Fall Wound Up";
    }
}
