using UnityEngine;
using System.Collections;
using Mariring;


public class EnemyRush : Enemy
{

    EnemyRushState rushState;

    #region Inspector
    [Header("Rush Enemy")]
    public float rushCoolTime;
    public float readyTime;

    public float minRushDis;
    public float maxRushDis;

    public float wallMaxDis;

    public float rushSpeed;


    #endregion


    Vector2 rushStartPos;

    float nowReadyTime;
    float nowRushCoolTime;



    void Awake()
    {
        base.Awake();

        eValue = EnemyValue.Rush;
        rushState = EnemyRushState.Ready;

    }

	
	void Start () 
    {
        base.Start();
        



	}

	void FixedUpdate()
    {
        base.FixedUpdate();

    }

	void Update () 
    {
        // wall check, positionCheck, move
        base.Update();

        RushCheckUpdate();
        StateUpdate();
       
	}

    void StateUpdate()
    {

        switch(eState)
        {
            case EnemyState.Idle:
                break;

            case EnemyState.Running:
                break;

            case EnemyState.RunningReady:
                break;

            case EnemyState.RunningReadyComplete:
                break;

            case EnemyState.Ready:
                break;

            case EnemyState.Attack:
                break;

            case EnemyState.KnockBack:
                break;

            case EnemyState.Hit:
                break;

            case EnemyState.Dead:
                break;


            case EnemyState.Rush:
                RushStateUpdate();
                break;

        }

    }

    void RushStateUpdate()
    {

        
        switch(rushState)
        {
            case EnemyRushState.Ready:
                nowReadyTime += Time.deltaTime;

                if (nowReadyTime >= readyTime)
                {
                    rushState = EnemyRushState.Start;
                }

                break;

            case EnemyRushState.Start:
                
                break;

            case EnemyRushState.Rush:
                break;

            case EnemyRushState.Attack:
                break;

        }

    }

    void RushCheckUpdate()
    {


        if (nowRushCoolTime > 0 || eState == EnemyState.Rush)
            return;
        

        if(Vector2.Distance(hero.transform.position,this.transform.position) <= maxRushDis)
        {

            LayerMask wallLayer = 1 << LayerMask.NameToLayer("Wall");
            RaycastHit2D hit = Physics2D.Raycast(hpPos.transform.position, isLeft ? Vector2.left : Vector2.right, wallMaxDis, wallLayer);


            if(hit!=null)
            {
                eState = EnemyState.Rush;
                rushState = EnemyRushState.Ready;

                rushStartPos = this.transform.position;
                
                nowReadyTime = 0f;

            }

                

           // LayerMask ignoreLayer = 1 << LayerMask.NameToLayer("Plat") | 1 << LayerMask.NameToLayer("Wall");
           // RaycastHit2D hit = Physics2D.Raycast(ropeStart.transform.position, direction, Mathf.Infinity, ignoreLayer); //cast downwards




        }


    }






    
}
