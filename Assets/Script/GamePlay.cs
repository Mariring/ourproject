using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour 
{
    [Header("Hero")]
    public Hero hero;

    [Header("Enemy Pattern Player")]
    public EnemyPatternPlayer ptPlayer;

    [Header("Camera")]
    public CamFollow cam;

    [Header("UI")]
    public GameObject uiObj;

    [Header("Game Playing")]
    [Tooltip("true, directly start")]
    public bool isPlay;

    [Header("Start Sound Effect")]
    public AudioSource startSeAudio;
    
    

	// Use this for initialization
	void Start () 
    {
	    if(isPlay)
        {
            GameStart();

        }
        else
        {
            cam.isForceCam = false;
            cam.SetFollowSpeed(2f);
            uiObj.SetActive(false);
        }

	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if(!isPlay)
        {

            if(Vector2.Distance(cam.transform.position,cam.target.transform.position) <= 0.1f)// == )
            {
                GameStart();
            }

        }


	}

    void GameStart()
    {
        hero.GameStart();
        ptPlayer.GameStart();
        isPlay = true;

        cam.isForceCam = true;
        cam.SetOriginFollowSpeed();
        uiObj.SetActive(true);

        startSeAudio.Play();
    }
}
