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
    string oneComAniName = "attack";


	void Awake () 
    {
        ani = this.GetComponent<SkeletonAnimation>();
        hero = this.GetComponent<Hero>();

        //ani.AnimationName = "rope";

        ani.state.Event += hero.EnemyAttackCheckEvent;
	}


	void Update () 
    {

        //Debug.Log(ani.state);


        newState = hero.hState;



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
                ani.timeScale = 1;
                break;

            case HeroState.Running:
                ani.state.SetAnimation(0,runAniName,true);
                ani.timeScale = 1;
                break;

            case HeroState.Combo_1:
                ani.state.SetAnimation(0, oneComAniName, true);
                ani.timeScale = 2;
                break;

            case HeroState.RopeRiding:
                ani.state.SetAnimation(0,ropeRidingAniName,true);
                ani.timeScale =1;
                break;

            case HeroState.RopeFlying:
                ani.state.SetAnimation(0, ropeFlyingAniName, true);
                ani.timeScale = 1;
                break;
        }
    }
}
