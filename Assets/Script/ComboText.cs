using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboText : MonoBehaviour
{

    Hero hero;
    Text comboText;
    Animator ani;

    int combo;

	// Use this for initialization
	void Awake () 
    {
        hero = GameObject.Find("Hero").GetComponent<Hero>();
        comboText = this.GetComponent<Text>();//hero.enemyCombo;
        ani = this.GetComponent<Animator>();


        combo = hero.enemyCombo;
	}
	
	// Update is called once per frame
	void Update () 
    {

        if(combo == 0)
        {
            comboText.color = new Color(0, 0, 0, 0);
        }
        else
        {
            comboText.color = Color.white;
        }

        if (combo != hero.enemyCombo)
        {

            combo = hero.enemyCombo;

            if (combo == 0)
                return;

            ani.SetTrigger("Set");
            comboText.text = combo.ToString() + " COMBO";
        }




	}
}
