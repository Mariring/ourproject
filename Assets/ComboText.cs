using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboText : MonoBehaviour
{

    Hero hero;
    Text comboText;

	// Use this for initialization
	void Awake () 
    {
        hero = GameObject.Find("Hero").GetComponent<Hero>();
        comboText = this.GetComponent<Text>();//hero.enemyCombo;
	}
	
	// Update is called once per frame
	void Update () 
    {


        comboText.text = hero.enemyCombo.ToString() + " COMBO";


	}
}
