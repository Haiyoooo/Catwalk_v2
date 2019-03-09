using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    [SerializeField] public enum cityType { job, party, none };
    [SerializeField] public cityType type;

    private void Start()
    {
        type = cityType.none;
    }
}
