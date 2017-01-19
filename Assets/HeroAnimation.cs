using UnityEngine;
using System.Collections;
using Spine.Unity;

[RequireComponent(typeof(Hero))]
public class HeroAnimation : MonoBehaviour 
{

    Hero hero;
    SkeletonAnimation ani;


	void Awake () 
    {
        ani = this.GetComponent<SkeletonAnimation>();
        hero = this.GetComponent<Hero>();

        //ani.AnimationName = "rope";
        
	}


	void Update () 
    {



	}

}
