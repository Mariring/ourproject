using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class SoundEffectManager : MonoBehaviour {

    
    public AudioClip[] seClips;
    AudioSource aSource;

	// Use this for initialization
	void Awake () 
    {
        aSource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void PlaySE(int seNum)
    {
        if (seNum > seClips.Length - 1)
            return;

        aSource.PlayOneShot(seClips[seNum]);
    }

    
}
