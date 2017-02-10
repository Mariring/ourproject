using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class HpScaler : MonoBehaviour {

    public Hero hero;

    Image bar;

    float xMaxSize;
    float yMaxSize;

    float showHpValue;
	// Use this for initialization
	void Awake () 
    {
        bar = this.GetComponent<Image>();

        xMaxSize = bar.gameObject.GetComponent<RectTransform>().rect.width;
        yMaxSize = bar.gameObject.GetComponent<RectTransform>().rect.height;

        showHpValue = hero.hp;
	}
	
	// Update is called once per frame
	void Update () 
    {

        showHpValue = Mathf.MoveTowards(showHpValue, hero.hp, Time.deltaTime * Mathf.Abs(showHpValue-hero.hp) * 2f);
        bar.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(xMaxSize * (showHpValue / 100f), yMaxSize);
        

        if(showHpValue <= 0)
        {
            hero.GameOver();
        }
	    
	}






}
