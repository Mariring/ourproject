using UnityEngine;
using System.Collections;
using Spine.Unity;
using Mariring;


[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(SkeletonAnimation))]
public class EnemyAnimation : MonoBehaviour 
{

    Enemy enemy;

    SkeletonAnimation ani;

    EnemyState preState;
    public EnemyState newState;

    public string idleAniName;
    public string runAniName;
    public string runReadyAniName;
    public string runReadyAfterAniName;
    public string readyAniName;
    public string attackAniName;
    public string knockBackAniName;
    public string hitAniName;
    public string deadAniName;



	// Use this for initialization
	void Start () 
    {
        ani = this.GetComponent<SkeletonAnimation>();
        enemy = this.GetComponent<Enemy>();


        ani.state.Event += enemy.AttackHeroTimingCheckEvent;
        ani.state.Complete += enemy.AnimationComplete;
        ani.state.Complete += AnimationComplete;
	}
	
	// Update is called once per frame
	void Update () 
    {
        newState = enemy.eState;

        if (preState != newState)
        {
            SetAnimationState();
            preState = newState;

        }

	}



    void SetAnimationState()
    {
        switch(newState)
        {
            case EnemyState.Idle:
                SetAnimation(idleAniName, 1, true);
                break;


            case EnemyState.Running:
                SetAnimation(runAniName, 1, true);
                break;

            case EnemyState.RunningReady:
                SetAnimation(runReadyAniName, 1, false);
                break;

            case EnemyState.Ready:
                SetAnimation(readyAniName, 1, true);
                break;

            case EnemyState.Attack:
                SetAnimation(attackAniName, 1, false);
                break;

            case EnemyState.KnockBack:
                SetAnimation(knockBackAniName, 1, true);
                break;

            case EnemyState.Hit:
                SetAnimation(hitAniName, 1, true);
                break;

            case EnemyState.Dead:
                SetAnimation(deadAniName, 1, true);
                break;

        }
    }

    void SetAnimation(string _name, float _timeScale, bool _loop)
    {
        ani.state.SetAnimation(0, _name, _loop);
        ani.timeScale = _timeScale;
    }


    void AnimationComplete(Spine.TrackEntry trackEntry)
    {
        if(trackEntry.animation.name == runReadyAniName)
        {
            if(newState == EnemyState.RunningReady && enemy.isReady)
            {
                SetAnimation(runReadyAfterAniName, 1, true);
            }
        }

        //switch (enemy.eState)
        //{
        //    case EnemyState.Idle:
        //        break;
        //    case EnemyState.Running:
        //        break;
        //    case EnemyState.RunningReady:
        //        break;
        //    case EnemyState.Ready:
        //        break;
        //    case EnemyState.Attack:
        //        break;
        //    case EnemyState.KnockBack:
        //        break;
        //    case EnemyState.Hit:
        //        break;
        //    case EnemyState.Dead:
        //        break;
        //}
    }

}
