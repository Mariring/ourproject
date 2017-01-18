using UnityEngine;
using System.Collections;

public class Gauge : MonoBehaviour {

    //SpriteRenderer sprRdr;
    
    public bool dirX;

    float sprSize;

    public float value;
    public float maxValue;

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
	}
}
