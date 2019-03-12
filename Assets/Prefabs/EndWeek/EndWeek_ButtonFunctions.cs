/* Handles button presses for the pop-up message which appears at the end of each week.
 * 
 * Attached to: 'NextWeekButton' gameobject which is a child object of 'EndWeek' prefab.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndWeek_ButtonFunctions : MonoBehaviour
{

    [SerializeField] private GameObject endWeekPrefab;

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }

    public void NextWeek()
    {
        endWeekPrefab.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}


