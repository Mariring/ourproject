using UnityEngine;
using System.Collections;
using Mariring;

public class EnemyNormal : Enemy {


    public Sprite[] stateSpr;

    SpriteRenderer sprRdr;

    bool isReady;

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
                    if (atkBox.isBoxInHero && attackCoolTime <= 0)
                    {
                        eState = EnemyState.Ready;
                    }
                    else if (Vector2.Distance(this.transform.position, hero.transform.position) < 5f && attackCoolTime <= 0)
                    {
                        eState = EnemyState.RunningReady;
                    }
                    else
                    {
                        eState = EnemyState.Running;
                    }
                }
                break;


            case EnemyState.Running:
                if (attackCoolTime > 0)
                    return;

                if (Vector2.Distance(this.transform.position, hero.transform.position) < 5f)
                {
                    eState = EnemyState.RunningReady;
                }
                break;

            case EnemyState.RunningReady:
                if(atkBox.isBoxInHero)
                {
                    StartCoroutine(AttackRunningReadyRoutine());
                }
                break;

            case EnemyState.Ready:
                if (!isReady)
                    StartCoroutine(AttackReadyRoutine());
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

        // Debug.Log(enemyState);
        sprRdr.sprite = stateSpr[(int)eState];


	}


    bool CheckAttackableHero()
    {
        if(atkBox.isBoxInHero)
        {
            if (eState == EnemyState.Idle)
            {
                eState = EnemyState.Ready;
                return true;
            }
        }

        return false;
    }



    IEnumerator AttackReadyRoutine()
    {
        isReady = true;
        
        yield return new WaitForSeconds(2f);

        if(eState == EnemyState.Ready)
        {
            eState = EnemyState.Attack;
            AttackHero();
            yield return new WaitForSeconds(0.1f);
        }
        eState = EnemyState.Idle;
        isReady = false;
        attackCoolTime = 2f;
        
    }

    IEnumerator AttackRunningReadyRoutine()
    {


        yield return new WaitForSeconds(0.6f);
        
        if(eState == EnemyState.RunningReady)
        {
            eState = EnemyState.Attack;
            AttackHero();
            yield return new WaitForSeconds(0.1f);
        }

        attackCoolTime = 2f;
        eState = EnemyState.Idle;
    }

}
