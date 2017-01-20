using UnityEngine;
using System.Collections;
using Spine.Unity;

[RequireComponent(typeof(Hero))]
public class HeroAnimation : MonoBehaviour 
{

    Hero hero;
    SkeletonAnimation ani;

    public HeroState preState;
    public HeroState newState;


    string idleAniName = "able";
    string runAniName = "run";
    string ropeRidingAniName = "rope";
    string ropeFlyingAniName = "dropkick";
	void Awake () 
    {
        ani = this.GetComponent<SkeletonAnimation>();
        hero = this.GetComponent<Hero>();

        //ani.AnimationName = "rope";
        
	}


	void Update () 
    {

        Debug.Log(ani.state);

        

        if(hero.ropeState.ropeRiding)
        {
            newState = HeroState.RopeRiding;
        }
        else if(hero.ropeState.isRopeShooting)
        {
            newState = HeroState.RopeFlying;
        }
        else if(hero.isRunning)
        {
            newState = HeroState.Running;
        }
        else
        {
            newState = HeroState.Idle;
        }




        if(preState != newState)
        {
            SetAnimation();
            preState = newState;
        }

	}

    //IEnumerator OneComboRoutine()
    //{

    //}



    void SetAnimation()
    {
        switch(newState)
        {
            case HeroState.Idle:
                ani.state.SetAnimation(0,idleAniName,true);
                break;

            case HeroState.Running:
                ani.state.SetAnimation(0,runAniName,true);
                break;

            case HeroState.RopeRiding:
                ani.state.SetAnimation(0,ropeRidingAniName,true);
                break;

            case HeroState.RopeFlying:
                ani.state.SetAnimation(0, ropeFlyingAniName, true);
                break;
        }
    }
}
