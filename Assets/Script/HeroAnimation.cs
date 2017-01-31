using UnityEngine;
using System.Collections;
using Spine.Unity;
using Mariring;

[RequireComponent(typeof(Hero))]
public class HeroAnimation : MonoBehaviour 
{

    Hero hero;
    
    SkeletonAnimation ani;

    HeroState preState;
    public HeroState newState;

    //[Header("AnimationName")]

    string idleAniName;
    string runAniName;
    string ropeRidingAniName;
    string ropeFlyingAniName;
    string ropeAttackAniName;

    string oneComAniName;
    string twoComAniName;
    string threeComAniName;

    string frontHitAniName;
    string backHitAniName;


	void Start () 
    {
        ani = this.GetComponent<SkeletonAnimation>();
        hero = this.GetComponent<Hero>();

        //이벤트 추가
        ani.state.Event += hero.AttackEnemyTimingCheckEvent;
        //ani.state.Complete += hero.AttackAnimationComplete;
        ani.state.Complete += hero.AttackAnimationComplete;
        SetAnimationName();
	}


    
    void SetAnimationName()
    {

        idleAniName = "able";
        runAniName = "run";
        ropeRidingAniName = "rope";
        ropeFlyingAniName = "ropeflying";

        frontHitAniName = "fuck01";
        backHitAniName = "fuck02"; 

        oneComAniName = AttackStyles.GetAttackAnimationName(hero.oneCombo);
        twoComAniName = AttackStyles.GetAttackAnimationName(hero.twoCombo);
        threeComAniName = AttackStyles.GetAttackAnimationName(hero.threeCombo);
        ropeAttackAniName = AttackStyles.GetAttackAnimationName(hero.ropeAttack);
        
    }


	void Update () 
    {

        newState = hero.hState;



        if(preState != newState)
        {
            SetAnimationState();
            preState = newState;
        }



	}


    void SetAnimationState()
    {
        switch(newState)
        {
            case HeroState.Idle:
                SetAnimation(idleAniName, 1, true);
                break;

            case HeroState.Running:
                SetAnimation(runAniName, 1, true);
                break;

            case HeroState.Combo_1:

                //ani.state.SetAnimation(1, oneComAniName, false);
                //ani.timeScale = 2.5f;
                SetAnimation(oneComAniName, 2.5f, false);
                break;

            case HeroState.Combo_2:
                
                //ani.state.SetAnimation(1, oneComAniName, false);
                //ani.timeScale = 2.5f;
                SetAnimation(twoComAniName, 2.5f, false);
                break;

            case HeroState.Combo_3:
                SetAnimation(threeComAniName, 1, false);
                break;

            case HeroState.RopeRiding:
                SetAnimation(ropeRidingAniName, 1, false);
                break;

            case HeroState.RopeFlying:
                SetAnimation(ropeFlyingAniName, 1, false);
                break;

            case HeroState.RopeAttack:
                SetAnimation(ropeAttackAniName, 1, false);
                break;


            case HeroState.FrontHit:
                
                SetAnimation(frontHitAniName, 1f, false);
                break;

            case HeroState.BackHit:
                SetAnimation(backHitAniName, 1f, false);
                break;



        }
    }


    void SetAnimation(string _name, float _timeScale, bool _loop)
    {
        ani.state.SetAnimation(0, _name, _loop);
        ani.timeScale = _timeScale;
    }



}
