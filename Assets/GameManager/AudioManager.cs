using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioSource[] sounds;

    [HideInInspector]
    public AudioSource equip, unequip, item_select, job_success, job_fail, open_shop, close_shop, party_fail, party_success, error, scroll;


    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        //AUDIO

        sounds = gameObject.GetComponents<AudioSource>();

        equip         = sounds[1];
        unequip       = sounds[2];
        item_select   = sounds[3];
        job_success   = sounds[4];
        job_fail      = sounds[5];
        open_shop     = sounds[6];
        close_shop    = sounds[7];
        party_fail    = sounds[8];
        party_success = sounds[9];
        error         = sounds[10];
        scroll        = sounds[11];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
