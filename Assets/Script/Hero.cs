using UnityEngine;
using System.Collections;

public struct RopeState
{
    public bool ropeRiding;     //로프라이딩 (로프 발사 준비중)
    public bool ropeRidable;    //로프를 탈 수 있는 상태
    public bool ropeLeft;       //로프가 어느 방향이냐
    public bool inRope;         //로프 안에 있어
    public bool isRopeShooting; //로프공격중
    public float ropeTime;      //로프 있었던 시간
    public GameObject rope;
}

[DisallowMultipleComponent]
public class Hero : MonoBehaviour 
{
    [Tooltip("※카메라")]
    public CamFollow cam;
    [Tooltip("※캐릭터에 달릴 로프 게이지")]
    public Gauge ropeGauge;

    [Range(0,100)]
    public int hp;
    public bool neverDie;

    protected SoundEffectManager sePlayer;
    public HeroAttackBox atkBox;
    public HeroAttackBox backBox;

    protected bool controlable;
    protected bool movable;
    protected bool unBeatable;

    [HideInInspector]
    public bool isLeft;

    public float originSpeed;
    float speed;
    int speedLevel;


    protected int comboNum;
    protected float comboTime;

    [HideInInspector]
    public RopeState ropeState;

    [HideInInspector]
    public int score;

    

	protected void Awake () 
    {
        controlable = true;
        movable = true;
        unBeatable = false;
        comboTime = 0;
        comboNum = 0;

        speedLevel = 0;

        ropeState.ropeRiding = false;
        ropeState.ropeRidable = false;
        ropeState.inRope = false;
        ropeState.ropeTime = 0;

        speed = originSpeed;

        sePlayer = this.GetComponent<SoundEffectManager>();

        StartCoroutine(DecreaseHpRoutine());    

    }


    protected void FixedUpdate()
    {
        //콤보시간 계산
        CalcComboTime();

        //콤보별 색상 변경
        CharacterComboColorSet();

        //로프 탈 수 있는지 확인
        CheckAbleToRopeRiding();

        //로프 타고 있을 때
        RopeRidingUpdate();

        //움직임
        Move();

        //뒤집기
        FlipX();

        
        
	}

    //이동
    void Move() 
    {

        if (!movable)
            return;



        float totalSpeed = speed + (speedLevel * 3);

        if (isLeft)
        {
            this.gameObject.transform.position = (Vector2.left * Time.deltaTime * totalSpeed) + (Vector2)this.gameObject.transform.position;
        }
        else
        {
            this.gameObject.transform.position = (Vector2.right * Time.deltaTime * totalSpeed) + (Vector2)this.gameObject.transform.position;
        }

    }

    //게임오브젝트 X방향 뒤집기
    void FlipX()
    {
        if(isLeft)
        {
            this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    //콤보타임 계산
    void CalcComboTime()
    {
        if (comboTime > 0)
        {
            comboTime -= Time.deltaTime;
        }
        else
            comboNum = 0;
    }


    //콤보별 캐릭터 색
    void CharacterComboColorSet()
    {
        if (this.GetComponent<SpriteRenderer>()==null)
            return;

        if(ropeState.isRopeShooting)
        {
            this.GetComponent<SpriteRenderer>().color = Color.yellow;
            return;
        }

        switch(comboNum)
        {
            case 0:
                this.GetComponent<SpriteRenderer>().color = Color.white;
                break;

            case 1:
                this.GetComponent<SpriteRenderer>().color = Color.red;
                break;

            case 2:
                this.GetComponent<SpriteRenderer>().color = Color.blue;
                break;

            case 3:
                this.GetComponent<SpriteRenderer>().color = Color.green;
                break;
        }
    }


    //로프 탈 수 있는지 체크
    void CheckAbleToRopeRiding()
    {
        if (unBeatable)
        {
            ropeState.ropeRidable = false;
            return;
        }
        
        if (ropeState.inRope)
        {
            if(ropeState.isRopeShooting || ropeState.ropeRiding)
            {
                ropeState.ropeRidable = false;
                return;
            }

            if (atkBox.enemyInBox.Count == 0 && backBox.enemyInBox.Count == 0)
            {

                ropeState.ropeRidable = true;
            }
            else
            {
                ropeState.ropeRidable = false;
            }



        }
        else
        {
            ropeState.ropeRidable = false;
        }
    }

    void RopeRidingUpdate()
    {
        if(ropeState.ropeRiding)
        {
            movable = false;
            ropeState.ropeTime += Time.deltaTime;
            ropeGauge.value = ropeGauge.maxValue * (ropeState.ropeTime / 2);


            float totalSpeed = speed + (speedLevel*3);
             
            if(isLeft)
                this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z - (Time.deltaTime * totalSpeed));
            else
                this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z + (Time.deltaTime * totalSpeed));


            if(ropeState.ropeTime > 2f)
            {
                RopeJumpFail();
            }
        }
    }

    //방향세팅
    public void ChangeDirect(bool _isLeft)  //true is Left
    {
        if(comboNum!=3)
        {
            comboTime = 0;
            comboNum = 0;
        }

        isLeft = _isLeft;
    }

    //적 때리기
    protected void AttackEnemy(GameObject _enemy)
    {

        sePlayer.PlaySE(1);
        if(comboNum>2)
        {
            if(_enemy.GetComponent<Enemy>().EnemyDeath(true))   //피니쉬로 애를 때렸더니 죽었어
            {
                IncreaseHp();
                score += 2;     //스코어 2
                atkBox.enemyInBox.Remove(_enemy.gameObject);
            }

        }
        else
        {
            if (_enemy.GetComponent<Enemy>().EnemyDeath(false)) //그냥 애를 때렸더니 죽었어
            {
                IncreaseHp();
                score += 1;     //스코어 1
                atkBox.enemyInBox.Remove(_enemy.gameObject);
            }
        }

    }
    
    protected void RopeAttackEnemy()
    {
        for(int i=0;i<atkBox.enemyInBox.Count;++i)
        {
            sePlayer.PlaySE(1);
            atkBox.enemyInBox[i].GetComponent<Enemy>().EnemyDeath(true);
            IncreaseHp();
            score += 3;
        }
        atkBox.enemyInBox.Clear();
    }

    

    //대쉬루틴
    IEnumerator ImmediateSpeedUpRoutine()
    {
        speed = originSpeed * 3f;
        yield return new WaitForSeconds(0.07f);
        speed = originSpeed;
        
    }
    
    //대쉬루틴 실행 함수
    protected void ImmediateSpeedUp()
    {
        StartCoroutine(ImmediateSpeedUpRoutine());
    }


    //데미지 받음
    public void GetDamage()
    {

        if (unBeatable)
            return;


        hp -= 20;

        if (hp <= 0)
        {
            hp = 0;

            if(!neverDie)
                GameOver();
        }


        sePlayer.PlaySE(0);
        Handheld.Vibrate();
        cam.ForceZoomShot();

        if(ropeState.ropeRiding)
        {
            cam.SetZoomSizeSpeed(4, 3);
            ropeState.ropeRiding = false;
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        StartCoroutine(UnbeatableTime());
        StartCoroutine(CantMoveTime(0.5f));
        StartCoroutine(KnockbackTime(0.5f));
        SpeedDown();

    }

    //hp 증가
    public void IncreaseHp()
    {
        sePlayer.PlaySE(1);
        cam.ShakeCam(0.12f);

        hp += 30;
        if (hp > 100)
            hp = 100;
    }

    //Hp 지속 감소 루틴
    IEnumerator DecreaseHpRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            hp -= 7;
        }
    }

    //무적타임 루틴
    IEnumerator UnbeatableTime()
    {

        unBeatable = true;


        if (this.GetComponent<SpriteRenderer>() != null)
        {

            for (int i = 0; i < 6; ++i)
            {
                if (i % 2 == 0)
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r, this.GetComponent<SpriteRenderer>().color.g, this.GetComponent<SpriteRenderer>().color.b, 0.5f);
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r, this.GetComponent<SpriteRenderer>().color.g, this.GetComponent<SpriteRenderer>().color.b, 1);
                }

                yield return new WaitForSeconds(0.1f);
            }



        }
   
        unBeatable = false;
  
    }

    //넉백루틴
    IEnumerator KnockbackTime(float _maxTime)
    {
        float _time = 0;

        while(_maxTime>_time)
        {
            if(isLeft)
            {
                this.gameObject.transform.position = (Vector2.right * Time.deltaTime * (speed*2) * (_maxTime-_time)) + (Vector2)this.gameObject.transform.position;
            }
            else
            {
                this.gameObject.transform.position = (Vector2.left * Time.deltaTime * (speed * 2) * (_maxTime - _time)) + (Vector2)this.gameObject.transform.position;

            }

            _time += Time.deltaTime;
            yield return null;
        }
    }

    //움직일 수 없는 루틴
    IEnumerator CantMoveTime(float _time)
    {
        movable = false;
        yield return new WaitForSeconds(_time); 
        movable = true;

    }

    //조종할 수 없는 루틴
    IEnumerator CantControlTime(float _time)
    {
        controlable = false;
        yield return new WaitForSeconds(_time);
        controlable = true;

        comboNum = 0; 
    }

    //콤보탈진
    protected void ComboExhaust()
    {
        StartCoroutine(CantControlTime(0.5f));
    }

    //게임 오버
    void GameOver()
    {
        Time.timeScale = 0;
    }

    //방향반환
    public bool GetDirectState()
    {
        return isLeft;
    }

    //무정상태 반환
    public bool GetUnbeatableState()
    {
        return unBeatable;
    }

    //컨트롤가능여부 반환
    public bool GetControlable()
    {
        return controlable;
    }


    //캐릭터 흔들기
    protected IEnumerator ShakeCharacter(float _maxTime)
    {
        float shakeTime = 0f;
        while (shakeTime < _maxTime)
        {
            shakeTime += Time.unscaledDeltaTime;
            this.gameObject.transform.position = this.gameObject.transform.position + (Random.insideUnitSphere * 0.1f);


            yield return null;
        }
    }


    //로프 탄다
    protected void RopeRide()
    {

        ropeState.ropeRiding = true;
        ropeState.ropeTime = 0;

        if(ropeState.ropeLeft)
        {
            ChangeDirect(false);
        }
        else
        {
            ChangeDirect(true);
        }

        this.transform.position = ropeState.rope.transform.position;
        movable = false;

        cam.SetZoomSizeSpeed(2.7f, 1);
    }


    public void LeaveRope()
    {

        cam.SetZoomSizeSpeed(4, 3);
        ropeState.ropeRiding = false;
        //movable = true;

        if(ropeState.ropeTime > 0.5f)
        {
            SpeedUp();
            StartCoroutine(RopeJumping(ropeState.ropeTime));

        }
        else
        { 
            RopeJumpFail();
        }

    }


    public void RopeJumpFail()
    {

        cam.SetZoomSizeSpeed(4, 3);

        this.transform.localEulerAngles = new Vector3(0, 0, 0);

        sePlayer.PlaySE(0);
        cam.ForceZoomShot();

        ropeState.ropeRiding = false;
        StartCoroutine(UnbeatableTime());
        StartCoroutine(CantMoveTime(0.5f));
        SpeedDown();

    }

    void SpeedUp()
    {
        speedLevel += 1;
        if (speedLevel > 2)
            speedLevel = 2;

    }

    void SpeedDown()
    {
        speedLevel -= 1;
        if (speedLevel < 0)
            speedLevel = 0;
    }



    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Rope"))
        {
            ropeState.inRope = true;
            ropeState.rope = coll.gameObject;

            if (coll.GetComponent<Rope>().isLeftRope)
                ropeState.ropeLeft = true;
            else
                ropeState.ropeLeft = false;
        }

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Rope"))
        {
            ropeState.inRope = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.CompareTag("Wall"))
        {
            if (ropeState.ropeRiding)
                return;

            ropeState.isRopeShooting = false;

            if (coll.gameObject.GetComponent<Wall>().isLeftWall && isLeft)
            {
              
                ChangeDirect(false);
            }
            else if ((!coll.gameObject.GetComponent<Wall>().isLeftWall && !isLeft))
            {
                ChangeDirect(true);
            }

            
        }


    }


    void OnCollisionStay2D(Collision2D coll)
    {

    }



    IEnumerator RopeJumping(float _power)
    {

        float originPower = _power * 1.5f;
        float power = originPower;
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.transform.localEulerAngles = new Vector3(0, 0, 0);


        ropeState.isRopeShooting = true;


        while(power>0)
        {
            if (!ropeState.isRopeShooting)
                break;

            float totalSpeed = speed + (speedLevel * 3);
            if(isLeft)
            {
                this.gameObject.transform.position = (Vector2.left * (Time.deltaTime * 2) * totalSpeed * power) + (Vector2)this.gameObject.transform.position;
            }
            else
            {
                this.gameObject.transform.position = (Vector2.right * (Time.deltaTime * 2) * totalSpeed * power) + (Vector2)this.gameObject.transform.position;
            }

            if (power > originPower * 0.75f)
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + ((power-(originPower * 0.75f)) * 10 * Time.deltaTime), this.transform.position.z);
            }
            else
            {
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 8;
            }


            power -= (Time.deltaTime * 2);
            yield return new WaitForEndOfFrame();
        }

        movable = true;
        ropeState.isRopeShooting = false;

        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 8;
    }



}
