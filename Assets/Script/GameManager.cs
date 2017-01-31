using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    static GameManager gManager;
    private static GameObject containObj;


    public static GameManager GetManager()
    {
        if (!gManager)
        {
            containObj = new GameObject();
            containObj.name = "GameManager";
            gManager = containObj.AddComponent(typeof(GameManager)) as GameManager;
        }

        return gManager;



    }

    void Awake()
    {
        UnityEngine.Profiler.maxNumberOfSamplesPerFrame = 60;
        Screen.orientation = (ScreenOrientation)Input.deviceOrientation;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;


        if (gManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gManager = this;
        }
        else if (gManager != this)
        {
            Destroy(gameObject);
        }
    }

}
