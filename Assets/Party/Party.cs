using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Party : MonoBehaviour
{

    public enum partyState { AVALIABLE, SUCCESS, SUPERSUCCESS, FAIL };
    public partyState thisPartyState = partyState.AVALIABLE;

    [Header("For Testing")]
    [Tooltip("Number of clothing items currently worn which are in Theme")]
    [Range(0, 2)]
    [SerializeField] private int correctItems = 0;

    [Header("Party Hints")]
    [SerializeField] private string[] hints;

    [Header("PopUp Messages")]
    [SerializeField] private GameObject successPopUp;
    [SerializeField] private GameObject superSuccessPopUp;
    [SerializeField] private GameObject failPopUp; //reference the pop-up messages

    [Header("Disasppear Animation")]
    [SerializeField] private GameObject fireworksFX;

    [Header("Tooltips Setup")]
    [SerializeField] private Text nameTooltip;
    [SerializeField] private Text themeTooltip;
    private Transform childObj;
    private string partyName = "Yi Tong's 21st"; //ZAC
    private CompanyManager.trend theme;
    private string themeString;

    private Vector3 InitialScale;
    private Vector3 FinalScale;
    private bool playDisappear = false;

    void Start()
    {
        theme = (CompanyManager.trend)Random.Range(0, 12); // picks a random theme
        themeString = theme.ToString(); // gets the theme name string to display

        nameTooltip.text = partyName;
        themeTooltip.text = themeString;
        
        //tooltip is a child object of this event prefab
        childObj = transform.Find("Tooltip(Canvas)");
        childObj.gameObject.SetActive(false);

        //Appear animation
        InitialScale = new Vector3(0.1f, 0.1f, 0.1f);
        FinalScale = new Vector3(1, 1, 1);
        transform.localScale = InitialScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, FinalScale, Time.deltaTime * 2);
        if (playDisappear)
            transform.localScale = Vector3.Lerp(transform.localScale, InitialScale, Time.deltaTime * 2);
    }

    //Tool tip
    private void OnMouseOver()
    {
        childObj.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        childObj.gameObject.SetActive(false);
    }

    //Fail & Success Conditions
    private void OnMouseDown()
    {
        // If there is a UI open, stop player from clicking on other jobs.
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //Success
            if (correctItems == 1)
            {
                thisPartyState = partyState.SUCCESS;
                var tempPopUp = Instantiate(successPopUp); //pop up message
                tempPopUp.transform.parent = gameObject.transform;

                //AUDIO
                AudioManager.instance.party_success.Play();
            }

            //Fail
            else if (correctItems == 0)
            {
                thisPartyState = partyState.FAIL;
                var tempPopUp = Instantiate(failPopUp); //pop up message
                tempPopUp.transform.parent = gameObject.transform;

                //AUDIO
                AudioManager.instance.job_fail.Play();
            }

            //Super Success
            else if (correctItems == 2)
            {
                thisPartyState = partyState.SUPERSUCCESS;
                var tempPopUp = Instantiate(superSuccessPopUp); //pop up message
                tempPopUp.transform.parent = gameObject.transform;

                //AUDIO
                AudioManager.instance.party_success.Play();
            }

            GameManager.instance.day++;
        }
    }

    public void disappearOnSuccess()
    {
        if (thisPartyState == Party.partyState.SUCCESS || thisPartyState == Party.partyState.SUPERSUCCESS)
        {
            transform.GetComponentInParent<City>().type = City.cityType.none;
            playDisappear = true;
        }
    }
}
