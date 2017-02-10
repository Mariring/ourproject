using UnityEngine;
using System.Collections;
using Spine.Unity;
using Mariring;


[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(SkeletonAnimation))]
[System.Serializable]
public class EnemyAnimation : MonoBehaviour
{
    #region Variable

    [SerializeField]
    public Enemy enemy;
    [SerializeField]
    public SkeletonAnimation ani;

    EnemyState preState;
    public EnemyState newState;

    [SerializeField]
    public string enteranceAniName;
    [SerializeField]
    public string idleAniName;
    [SerializeField]
    public string runAniName;
    [SerializeField]
    public string runReadyAniName;
    [SerializeField]
    public string runReadyAfterAniName;
    [SerializeField]
    public string readyAniName;
    [SerializeField]
    public string attackAniName;
    [SerializeField]
    public string knockBackAniName;
    [SerializeField]
    public string hitAniName;
    [SerializeField]
    public string deadAniName;
    [SerializeField]
    public string flyingAniName;


    [SerializeField]
    public string readyRushAniName;
    [SerializeField]
    public string startRushAniName;
    [SerializeField]
    public string rushAniName;
    [SerializeField]
    public string rushAttackAniName;

    [SerializeField]
    public string angryWalkAniName;

    #endregion

    //angry
    bool aniUnbeatable;


    // Use this for initialization
	protected void Start () 
    {
        //ani = this.GetComponent<SkeletonAnimation>();
        //enemy = this.GetComponent<Enemy>();

        aniUnbeatable = false;

        ani.state.Event += enemy.AttackHeroTimingCheckEvent;
        ani.state.Complete += enemy.AnimationComplete;
        ani.state.Complete += AnimationComplete;

        SetAnimationState();
	}
	
	// Update is called once per frame
    protected void Update() 
    {
        newState = enemy.eState;

        if (preState != newState)
        {
            SetAnimationState();
            preState = newState;

        }

        if (enemy.eValue == EnemyValue.Angry)
            AngryStateUpdateCheck();

	}



    void SetAnimationState()
    {
        switch(newState)
        {
            case EnemyState.Idle:
                SetAnimation(idleAniName, 1, true);
                break;


            case EnemyState.Running:
                {
                    SetAnimation(runAniName, 1, true);
                    break;

                }

            case EnemyState.RunningReady:
                SetAnimation(runReadyAniName, 1, false);
                break;

            case EnemyState.RunningReadyComplete:
                SetAnimation(runReadyAfterAniName, 1, true);
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

    void AngryStateUpdateCheck()
    {

        if (enemy.isUnbeatable)//angryEnemy.eState == EnemyState.Running)
        {
            if (!aniUnbeatable)
            {
                SetAnimation(angryWalkAniName, 1, true);
                aniUnbeatable = true;
            }
        }
        else
        {
            aniUnbeatable = false;
        }
    }


    protected void SetAnimation(string _name, float _timeScale, bool _loop)
    {
        ani.state.SetAnimation(0, _name, _loop);
        ani.timeScale = _timeScale;
    }


    void AnimationComplete(Spine.TrackEntry trackEntry)
    {
        if(trackEntry.animation.name == runReadyAniName)
        {




            //if(newState == EnemyState.RunningReady)
            //{
               
            //}
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
