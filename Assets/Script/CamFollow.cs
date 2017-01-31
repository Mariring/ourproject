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
    public float followSpeed = 3;

    public float originZoomSize;
    public float originZoomSpeed=3;
    
    [HideInInspector]
    public float zoomSize;
    [HideInInspector]
    public float zoomSpeed;
    

    Vector3 camPos;
    

    bool beingZoomShot;

    public bool testMode;

	// Use this for initialization
	void Awake () 
    {
        mainCam = this.GetComponentInChildren<Camera>();
        zoomSize = originZoomSize;
        zoomSpeed = originZoomSpeed;
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
        float camSpeed = followSpeed;

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

    IEnumerator ForceZoomShotRoutine(float _zoomSize)
    {
        if (beingZoomShot)
            yield break;

        beingZoomShot = true;
        float originZoom = mainCam.orthographicSize;
        float zoomSize = _zoomSize;

        mainCam.orthographicSize = zoomSize;

        while(mainCam.orthographicSize != originZoom)
        {
            mainCam.orthographicSize = Mathf.MoveTowards(mainCam.orthographicSize, originZoom, Time.deltaTime * 3f);
            yield return null;
        }

        beingZoomShot = false;
    }

    IEnumerator ForceZoomShotRoutine(float _zoomSize, float _zoomSpeed)
    {
        if(beingZoomShot)
            yield break;

        beingZoomShot = true;
        float originZoom = mainCam.orthographicSize;
        float zoomSize = _zoomSize;

        mainCam.orthographicSize = zoomSize;

        while (mainCam.orthographicSize != originZoom)
        {
            mainCam.orthographicSize = Mathf.MoveTowards(mainCam.orthographicSize, originZoom, Time.deltaTime * _zoomSpeed);
            yield return null;
        }

        beingZoomShot = false;
    }


    public void ForceZoomShot(float _zoomSize)
    {
        if (testMode)
            return;

        StartCoroutine(ForceZoomShotRoutine(_zoomSize));
    }
    public void ForceZoomShot(float _zoomSize , float _zoomSpeed)
    {
        if (testMode)
            return;

        StartCoroutine(ForceZoomShotRoutine(_zoomSize, _zoomSpeed));
    }

    void CameraSizeUpdate()
    {
        if(!beingZoomShot)
        {
            mainCam.orthographicSize = Mathf.MoveTowards(mainCam.orthographicSize, zoomSize, Time.deltaTime * zoomSpeed);
        

        }
    }


    public void SetOriginZoomSizeSpeed()
    {
        zoomSpeed = originZoomSpeed;
        zoomSize = originZoomSize;
    }

    public void SetZoomSizeSpeed(float _zoomSize, float _zoomSpeed)
    {
        zoomSpeed = _zoomSize;
        zoomSize = _zoomSize;
    }
}
