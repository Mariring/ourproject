using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class BgmPlayer : MonoBehaviour 
{

    AudioSource aSource;

    public AudioClip[] bgmList;

    int playNum;
	// Use this for initialization
	void Start () 
    {
        aSource = this.GetComponent<AudioSource>();

        if(bgmList.Length ==0)
        {
            this.gameObject.SetActive(false);
            return;
        }

        playNum= 0;
        aSource.clip = bgmList[playNum];
        aSource.loop = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        
        if(!aSource.isPlaying)
        {
            playNum += 1;
            if(bgmList.Length == playNum)
            {
                playNum = 0;
            }
            aSource.clip = bgmList[playNum];
            aSource.Play();
        }
	}
}
