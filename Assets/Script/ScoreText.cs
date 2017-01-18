using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreText : MonoBehaviour {

    public Hero hero;
    Text uiTxt;
	// Use this for initialization
	void Awake () 
    {
        uiTxt = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        uiTxt.text = hero.score.ToString();
	}
}
