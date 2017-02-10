using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BgmLoopPlayer : MonoBehaviour {


    AudioSource aSource;

    public AudioClip introBgm;
    public AudioClip loopBgm;

	// Use this for initialization
	void Awake () 
    {

        aSource = this.GetComponent<AudioSource>();

        if(introBgm == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        aSource.clip = introBgm;
        aSource.Play();
        aSource.loop = false;

	}
	
	// Update is called once per frame
	void Update () 
    {
	
        if(!aSource.isPlaying)
        {

            if(aSource.clip != loopBgm)
            {
                aSource.clip = loopBgm;
                aSource.loop = true;
                aSource.Play();
            }



        }


	}
}
