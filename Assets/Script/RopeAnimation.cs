using UnityEngine;
using System.Collections;
using Mariring;


public class RopeAnimation : MonoBehaviour 
{

    public bool isLeftRope;
    Hero hero;
    Animator ani;



	// Use this for initialization
	void Awake () 
    {
        ani = this.GetComponent<Animator>();
        hero = GameObject.Find("Hero").GetComponent<Hero>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
	

        if(isLeftRope)
        {
            if(hero.hState == HeroState.RopeRiding && !hero.isLeft)
            {
                ani.SetBool("RopeRide", true);
                return;
            }
        }
        else
        {
            if (hero.hState == HeroState.RopeRiding && hero.isLeft)
            {
                ani.SetBool("RopeRide", true);
                return;
            }
        }

        ani.SetBool("RopeRide", false);

	}

}
