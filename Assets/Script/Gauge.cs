using UnityEngine;
using System.Collections;

public class Gauge : MonoBehaviour 
{
    
    [Header("GaugeDirect X or Y")]
    public bool dirX;


    [HideInInspector]
    public float value;

    [Header("Gauge's Max Value")]
    public float maxValue;

    [Header("Gauge's Color")]
    public bool colorChange;
    public Color startColor = Color.white;
    public Color endColor = Color.white;

    float sprSize;
    SpriteRenderer sprRdr;

    float rGap;
    float gGap;
    float bGap;

	// Use this for initialization
	void Awake () 
    {
        if(dirX)
        {
            sprSize = this.transform.localScale.x;
        }
        else
        {
            sprSize = this.transform.localScale.y;
        }

        if(colorChange)
        {
            sprRdr = this.GetComponent<SpriteRenderer>();

            rGap = endColor.r - startColor.r;
            gGap = endColor.g - startColor.g;
            bGap = endColor.b - startColor.b;


        }




	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (dirX)
        {
            this.transform.localScale = new Vector3((float)sprSize * (float)(value / maxValue), 1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, (float)sprSize * (float)(value / maxValue), 1);
        }


        if(colorChange)
        {

            sprRdr.color = new Color(startColor.r + (rGap * (value / maxValue)), startColor.g + (gGap * (value / maxValue)), startColor.b + (bGap * (value / maxValue)));


        }

	}
}
