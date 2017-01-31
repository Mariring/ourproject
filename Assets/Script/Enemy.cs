using UnityEngine;
using System.Collections;
using Mariring;


[System.Serializable]
public class Enemy : EnemyInfo
{
    
    public bool isLeft;
    public float speed;
    public float atkReadyTime;
    public GameObject hpSpr;

    Wall[] walls;
    protected Hero hero;

    protected float nowHp;
    public float originHp;

    protected float hpSprSize;

  

    [HideInInspector]
    public bool isDead;
    protected bool isMove;
    protected bool isKnockBacking;
    protected bool hitHero;

    protected EnemyAttackBox atkBox;


    //protected int enemyState;
    public EnemyState eState;
    /*
     * 
     * 0 :  달려오기
     * 1 :  달려오며 공격 준비
     * 2 :  서서 공격준비
     * 3 :  공격
     * 4 :  대기
     * 5 :  죽음
     * 6 :  날아감
     * 
     */

    protected float attackCoolTime;



    protected void Awake()
    {
        base.Awake();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), true);
        walls = GameObject.FindObjectsOfType<Wall>();
        hero = GameObject.Find("Hero").GetComponent<Hero>();

        //초기화들
        //originHp = Random.Range(1, 4);
        //nowHp = originHp; 
        hpSprSize = hpSpr.transform.localScale.x;

        isDead = false;
        isMove = true;
        hitHero = false;

        attackCoolTime = 0f;

        atkBox = gameObject.GetComponentInChildren<EnemyAttackBox>();

        //StartCoroutine(AttackHeroRoutine());

    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        base.Awake();

        hpSpr.transform.localScale = new Vector3((float)hpSprSize * (float)(nowHp / originHp), 1, 1);

        //죽으면 안 움직여
        if (isDead)
            return;
       

    }

    protected void Update()
    {

        WallCheck();


        //죽으면 안 움직여
        if (isDead)
        {
            return;
        }


        if (attackCoolTime> 0)
        {
            attackCoolTime -= Time.deltaTime;
        }


        if(!isMove)
        {
            return;
        }

        if (eState == EnemyState.Running || eState == EnemyState.RunningReady)
        {
            if (atkBox.isBoxInHero)
                return;

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




    protected void AttackHero()
    {
        if (isDead)
            return;

        if (atkBox.heroInBox != null)
            atkBox.heroInBox.GetDamage();


    }

    public void Damaged()
    {
        nowHp -= 1;
        isKnockBacking = false;
    }

    public bool EnemyDeathCheck(bool _isFinish)  //죽으면 true
    {
        Damaged();  //ㄷㅔ미지 받고


        if (_isFinish)
        {
            isDead = true;  //죽음 표시
            StartCoroutine(FlyingEnemy());  //날라가

            return true;
        }
        else if(nowHp <=0)
        {
            isDead = true;
            Destroy(this.gameObject);
            return true;
        }


        return false;


    }


    #region Routine


    IEnumerator KnockBackRoutine(Vector2 _target,bool _heroLeft)
    {

        if (_heroLeft)
            _target.x = _target.x - Random.Range(2f, 3f);
        else
            _target.x = _target.x + Random.Range(2f, 3f);

        
        Vector2 _targetPos = new Vector2(_target.x,this.transform.position.y);
        

        isMove = false;
        isKnockBacking = true;
        eState = EnemyState.KnockBack;

        while (Vector2.Distance(this.transform.position,_targetPos)> 0.02f)//Mathf.Abs(this.transform.position.x - _targetPos.x) <= 0.02f)//Mathf.Abs(this.transform.position.x - _targetPos.x)<= 0.02f)
        {

            float _speed = Mathf.Abs(this.transform.position.x - _targetPos.x) * Time.deltaTime * 7f;
            this.transform.position = Vector2.MoveTowards(this.transform.position, _targetPos, _speed);

            yield return null;

            if (!isKnockBacking)
                break;
        }

        isKnockBacking = false;
        isMove = true;

    }

    //죽을때 날라감
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
        while (true)
        {
            time += Time.deltaTime;

            this.transform.position = (dir * 15f * Time.deltaTime) + (Vector2)this.transform.position;
            yield return null;

            if (time > 3f)
            {
                break;
            }
        }

        Destroy(this.gameObject);

    }

    #endregion

    void OnDestroy()
    {
        atkBox.heroInBox = null;
    }


    public void SetInitState(EnemyInitState _state)
    {

        isLeft = !(_state._isLeft);
        originHp = _state._originHp;
        nowHp = originHp;
        speed = _state._speed;
    }


    void WallCheck()
    {

        for(int i=0;i<walls.Length;++i)
        {
            if(walls[i].isLeftWall)
            {
                if(isLeft)
                {
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), walls[i].GetComponent<Collider2D>(),true);
                }
                else if(!isLeft && this.transform.position.x < walls[i].transform.position.x)
                {
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), walls[i].GetComponent<Collider2D>(), true);
                }
                else
                {
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), walls[i].GetComponent<Collider2D>(), false);
                }
            }
            else
            {
                if (isLeft && this.transform.position.x > walls[i].transform.position.x)
                {
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), walls[i].GetComponent<Collider2D>(), true);
                }
                else if (!isLeft)
                {
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), walls[i].GetComponent<Collider2D>(), true);
                }
                else
                {
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), walls[i].GetComponent<Collider2D>(), false);
                }
            }
        }


    }


    void RopeKnockBack(Vector2 _target, bool _heroLeft)
    {

        StartCoroutine(KnockBackRoutine(_target, _heroLeft));
        
    }



    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.CompareTag("Hero"))
        {
            hitHero = true;

            if(coll.gameObject.GetComponent<Hero>() != null)
            {
                if (coll.gameObject.GetComponent<Hero>().hState == HeroState.RopeFlying && !isKnockBacking)
                {
                    
                    RopeKnockBack(coll.gameObject.GetComponent<Hero>().GetRopeTarget(), coll.gameObject.GetComponent<Hero>().isLeft);
                    
                }

            }
        }
    }


    void OnCollisionExit2D(Collision2D coll)
    {

        if (coll.gameObject.CompareTag("Hero"))
        {
            hitHero = false;
        }
    }











}
