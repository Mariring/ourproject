using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreText : MonoBehaviour {

    public Hero hero;
    Text uiTxt;
    float showScoreValue;

	// Use this for initialization
	void Awake () 
    {
        uiTxt = this.GetComponent<Text>();

        showScoreValue = hero.score;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        showScoreValue = Mathf.MoveTowards(showScoreValue, hero.score, Time.deltaTime * 3000f);

        int _showScore = (int)showScoreValue;
        uiTxt.text = _showScore.ToString();


	}
}
