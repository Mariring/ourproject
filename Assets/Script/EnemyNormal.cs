using UnityEngine;
using System.Collections;
using Mariring;

public class EnemyNormal : Enemy 
{


    public Sprite[] stateSpr;
    SpriteRenderer sprRdr;


	// Use this for initialization
	void Awake () 
    {
        base.Awake();
        sprRdr = this.GetComponent<SpriteRenderer>();
        isReady = false;
	}
	

    void Update()
    {
        base.Update();
        StateUpdate();
    }

    void StateUpdate()
    {
        switch(eState)
        {
            case EnemyState.Idle:

                if(!CheckAttackableHero())//Vector2.Distance(this.transform.position,hero.transform.position < 5f)
                {
                    if (Vector2.Distance(this.transform.position, hero.transform.position) < 5f && attackCoolTime <= 0)
                    {
                        isReady=false;
                        eState = EnemyState.RunningReady;
                    }
                    else if(!atkBox.isBoxInHero)
                    {
                        eState = EnemyState.Running;
                    }
                }
                else
                {
                    if (atkBox.isBoxInHero && attackCoolTime <= 0)
                    {
                        eState = EnemyState.Ready;
                    }

                }
                break;


            case EnemyState.Running:

                if (atkBox.isBoxInHero)
                {
                    eState = EnemyState.Idle;
                }

                if (attackCoolTime > 0)
                    return;
                if (Vector2.Distance(this.transform.position, hero.transform.position) < 5f)
                {
                    eState = EnemyState.RunningReady;
                }
                break;

            case EnemyState.RunningReady:
                if (!isReady)
                    return;
                if(atkBox.isBoxInHero)
                {
                    StartCoroutine(AttackRunningReadyRoutine());
                }
                break;

            case EnemyState.Ready:
                break;

            //case EnemyState.Attack:
            //    AttackHero();
            //    eState = EnemyState.Idle;
            //    break;

            case EnemyState.KnockBack:
                if (!isKnockBacking)
                    eState = EnemyState.Idle;
                break;

        }
    }

	// Update is called once per frame
	void FixedUpdate () 
    {
        base.FixedUpdate();
        if(sprRdr != null && stateSpr.Length>0)
            sprRdr.sprite = stateSpr[(int)eState];


	}


    bool CheckAttackableHero()
    {
        if(atkBox.isBoxInHero)
        {
            if (eState == EnemyState.Idle)
            {
                return true;
            }
        }

        return false;
    }


    IEnumerator AttackRunningReadyRoutine()
    {

        isReady = true;

        yield return new WaitForSeconds(0.6f);
        
        isReady = false;

        if (eState == EnemyState.RunningReady)
        {
            eState = EnemyState.Attack;
        }
        
    }

}
