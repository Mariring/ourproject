using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

    public GameObject target;
    Camera mainCam;

    public bool isForceCam;

    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    public float speed = 3;

    public float zoomSize;
    float originZoomSize;
    public float zoomSpeed = 3;
    

    Vector3 camPos;
    

    bool beingZoomShot;

    public bool testMode;

	// Use this for initialization
	void Awake () 
    {
        mainCam = this.GetComponentInChildren<Camera>();
        originZoomSize = zoomSize;
        beingZoomShot = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (testMode)
            return;

        if(isForceCam)
        {
            camPos = target.transform.position;

            CamLimit();
            this.transform.position = camPos;
        }
        else
        {
            CamPosSet();
            CamSmoothFollow();

            CamLimit();
        }

        CameraSizeUpdate();
        
	}

    void CamPosSet()
    {
        if (target == null)
        {
            return;
        }


        camPos = target.transform.position;
        camPos.z = -10;



  
    }

    void CamSmoothFollow()
    {
        float camSpeed = speed;

        if ((Vector2.Distance(camPos, this.gameObject.transform.position) > 2f))
        {
            camSpeed = Vector2.Distance(camPos, this.gameObject.transform.position) * 10f;
        }
        else
        {
        }

        transform.position = Vector3.MoveTowards(transform.position, camPos, camSpeed * Time.unscaledDeltaTime);

    }

    void CamLimit()
    {
        camPos = new Vector3(Mathf.Clamp(camPos.x, minX, maxX),
                   Mathf.Clamp(camPos.y, minY, maxY),
                   camPos.z);

    }


    IEnumerator ShakeCameraRoutine(float _maxTime)
    {
        float shakeTime = 0f;
        while (shakeTime < _maxTime)
        {
            shakeTime += Time.unscaledDeltaTime;
            this.gameObject.transform.position = this.gameObject.transform.position + (Random.insideUnitSphere * 0.3f);


            yield return null;
        }
    }

    public void ShakeCam(float _maxTime)
    {
        if (testMode)
            return;
        StartCoroutine(ShakeCameraRoutine(_maxTime));
    }

    IEnumerator ForceZoomShotRoutine()
    {
        beingZoomShot = true;
        float originZoom = mainCam.orthographicSize;
        float zoomSize = originZoom / 1.3f;

        mainCam.orthographicSize = zoomSize;

        while(mainCam.orthographicSize<originZoom)
        {
            mainCam.orthographicSize += (Time.deltaTime * 3f);
            yield return null;
        }

        beingZoomShot = false;
    }


    public void ForceZoomShot()
    {
        if (testMode)
            return;
        StartCoroutine(ForceZoomShotRoutine());
    }


    void CameraSizeUpdate()
    {
        if(!beingZoomShot)
        {
            mainCam.orthographicSize = Mathf.MoveTowards(mainCam.orthographicSize, zoomSize, Time.deltaTime * zoomSpeed);
        

        }
    }

    public void SetZoomSizeSpeed(float _zoomSize, float _zoomSpeed)
    {
        zoomSpeed = _zoomSize;
        zoomSize = _zoomSize;
    }
}
