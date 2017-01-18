using UnityEngine;
using System.Collections;


[System.Serializable]
public class Enemy : EnemyInfo
{

    public bool isLeft;
    public float speed;
    public float atkReadyTime;
    public GameObject hpSpr;

    public float nowHp;
    public float originHp;
    float hpSprSize;


    [HideInInspector]
    public bool isDead;

    EnemyAttackBox atkBox;

    protected int enemyState;
    // 0 : 일반
    // 1 : 공격준비
    // 2 : 공격
    // 3 : 대기 

	protected void Awake ()
    {
        base.Awake();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"),true);

        //초기화들
        //originHp = Random.Range(1, 4);
        //nowHp = originHp;
        hpSprSize = hpSpr.transform.localScale.x;

        isDead = false;
        enemyState = 0;

        atkBox = gameObject.GetComponentInChildren<EnemyAttackBox>();
        StartCoroutine(AttackHeroRoutine());
    }
	
	// Update is called once per frame
    protected void FixedUpdate()
    {
        base.Awake();

        hpSpr.transform.localScale = new Vector3((float)hpSprSize * (float)(nowHp/originHp), 1,1);
     
        //죽으면 안 움직여
        if(isDead)  
        {
            return;
        }

        if (enemyState==0)
        {
            if (isLeft)
            {
                this.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
                this.transform.position = (Vector2.left * speed * Time.deltaTime) + (Vector2)this.transform.position;
            }
            else
            {
                this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                this.transform.position = (Vector2.right * speed * Time.deltaTime) + (Vector2)this.transform.position;
            }
        }
        
	}


    IEnumerator AttackHeroRoutine()
    {
        while(true)
        {

            if (atkBox.isBoxInHero)
            {

                yield return new WaitForSeconds(0.5f);
                if (!CheckAttackableState())
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                enemyState = 1;

                yield return new WaitForSeconds(atkReadyTime);
                if (!CheckAttackableState())
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }
                
                enemyState = 2;
                AttackHero();

                yield return new WaitForSeconds(0.1f);
                if (!CheckAttackableState())
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }
  
                enemyState = 3;

                yield return new WaitForSeconds(1f);
            
            }
            else
            {
                enemyState = 0;
            }

            yield return new WaitForEndOfFrame();
        }
       
    }

    bool CheckAttackableState()
    {
        if(atkBox.isBoxInHero)
        {

            if (atkBox.heroInBox.GetUnbeatableState())
            {
                enemyState = 3;
                return false;
            }
        }


        return true;
    }


    void AttackHero()
    {
        if (isDead)
            return;

        if(atkBox.heroInBox!=null)
            atkBox.heroInBox.GetDamage();


    }

    public void Damaged()
    {
        nowHp -= 1;
    }

    public bool EnemyDeath(bool _isFinish)  //죽으면 true
    {
        Damaged();  //ㄷㅔ미지 받고
        //Debug.Log("size: " + (float)(hpSprSize * (nowHp / originHp)) + ", nowHp/originHp" + (float)(nowHp / originHp));
        if (_isFinish)
        {

            isDead = true;  //죽음 표시
            StartCoroutine(FlyingEnemy());  //날라가
            
            return true;
        }
        else if(nowHp <= 0)
        {
            isDead = true;
            Destroy(this.gameObject);
            return true;
        }

        return false;

       
    }

    IEnumerator FlyingEnemy()
    {
        Destroy(this.GetComponent<BoxCollider2D>());
        Destroy(this.GetComponent<Rigidbody2D>());
        
        float yValue = Random.Range(0.5f, 0.9f);
        Vector2 dir = Vector2.zero;

        if (isLeft)
        {
            dir = new Vector2(1f, yValue);
        }
        else
        {
            dir = new Vector2(-1f, yValue);
        }

        float time = 0;
        while(true)
        {
            time += Time.deltaTime;

            this.transform.position = (dir * 15f * Time.deltaTime) + (Vector2)this.transform.position;
            yield return null;

            if(time > 3f)
            {
                break;
            }
        }

        Destroy(this.gameObject);
       
    }

    void OnDestroy()
    {
        atkBox.heroInBox = null;
    }
    

    public void SetInitState(EnemyInitState _state)
    {
        Debug.Log("ASD");
        isLeft = !(_state._isLeft);
        originHp = _state._originHp;
        speed = _state._speed;
    }







}
