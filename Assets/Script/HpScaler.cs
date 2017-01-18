using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class HpScaler : MonoBehaviour {

    public Hero hero;

    Image bar;

    float xMaxSize;
    float yMaxSize;

	// Use this for initialization
	void Awake () 
    {
        bar = this.GetComponent<Image>();

        xMaxSize = bar.gameObject.GetComponent<RectTransform>().rect.width;
        yMaxSize = bar.gameObject.GetComponent<RectTransform>().rect.height;
	}
	
	// Update is called once per frame
	void Update () 
    {
        bar.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(xMaxSize * (hero.hp / 100f), yMaxSize);
        
	    
	}






}
