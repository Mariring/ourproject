using UnityEngine;
using System.Collections;
using Spine.Unity;
using Mariring;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Hero : MonoBehaviour
{
    protected SoundEffectManager sePlayer;

    #region Inspector
    [Header("Hero Setting")]
    public GameObject sight;
    [Tooltip("※ TestMode : 죽지 않는다!")]
    public bool neverDie;
    [Range(0, 100)]
    [Tooltip("※ 체력")]
    public int originHp;

    [Range(0f, 20f)]
    [Tooltip("※ Original Speed")]
    public float originSpeed;

    [Header("Require Script")]
    [Tooltip("※ 카메라")]
    public CamFollow cam;
    [Tooltip("※ 캐릭터에 달릴 로프 게이지")]
    public Gauge ropeGauge;
    [Tooltip("※ 캐릭터 앞 박스")]
    public HeroAttackBox atkBox;
    [Tooltip("※ 캐릭터 뒤 박스")]
    public HeroAttackBox backBox;

    [Header("Attack Styles")]
    public AttackStyle oneCombo;
    public AttackStyle twoCombo;
    public AttackStyle threeCombo;
    public AttackStyle ropeAttack;

    [Header("※ 강유야 너가 해봐...")]
    [Tooltip("hp가 감소하는 기준 시간")]
    public float hpDecreaseTime = 1;
    [Tooltip("~초 마다 감소하는 hp량")]
    public int decreaseHpValue = 10;
    [Tooltip("적을 죽이면 증가하는 hp량")]
    public int increaseHpValue = 20;
    [Tooltip("적한테 맞으면 감소하는 hp량")]
    public int damageValue = 20;
    [Tooltip("적 죽이는 콤보 없어지는 시간")]
    public float enemyComboDeleteTime=2f;
    [Tooltip("피버타임 시간")]
    public float feverTime = 10f;
    [Tooltip("피버 10콤보 유지시간")]
    public float needStayTime = 10f;
    [Tooltip("피버 3콤보 처치 적 수")]
    public int needThreeCompleteNum = 10;
    [Tooltip("피버 로프 처치 적 수")]
    public int needRopeAtkCompleteNum = 10;
    [Tooltip("Origin Speed 에 더하는 (스피드레벨과 곱해지는 계수)")]
    public float addSpeedValue=3;

    [Header("ScoreObject")]
    public GameObject spawnScoreObj;
    public GameObject spawnHighScoreObj;
    #endregion

    #region StateVariable

    //게임 시작함?
    protected bool isPlay;

    protected bool controlable; //컨트롤 가능여부
    protected bool movable;     //이동 가능여부

    protected bool unBeatable;  //무적인가
    protected bool activeUnBeatable;

    //[HideInInspector]
    public HeroState hState;
    [HideInInspector]
    public bool isLeft;
    [HideInInspector]
    public RopeState ropeState;
    #endregion

    #region RequireVariable
    
    [HideInInspector]
    public int hp;

    //로프 점핑 타겟
    Vector2 ropeTarget;     
    public Vector2 GetRopeTarget()
    {
        return ropeTarget;
    }

    //스피드
    float speed;


    //스피드 레벨
    [HideInInspector]
    public int speedLevel; 
    int preSpdLv;

    protected int comboNum;         //어택 콤보
    protected float comboTime;      //콤보 타임
    protected float enemyComboTime; //적 처치 콤보 타임

    //점수
    [HideInInspector]
    public int score;

    //적 처치 콤보
    [HideInInspector]
    public int enemyCombo;  

    //벽
    Wall[] walls;
    #endregion

    #region FeverVariable

    bool isFeverMode = false;
    float nowFeverTime = 0;
    float feverComboStayTime = 0f;
    int threeComCompleteNum = 0;
    int ropeAtkCompleteNum = 0;

    #endregion



    protected void Awake ()
    {
        #region Init

        hp = originHp;
        hState = HeroState.Idle;

        controlable = true;
        movable = true;
        unBeatable = false;
        activeUnBeatable = false;
        //isPlayingAtkAni = false;

        comboTime = 0f;
        enemyComboTime = 0f;
        comboNum = 0;
        enemyCombo = 0;

        speed = originSpeed;
        speedLevel = 0;
        preSpdLv = speedLevel;

        ropeState.ropeRidable = false;
        ropeState.inRope = false;
        ropeState.ropeTime = 0;

        sePlayer = this.GetComponent<SoundEffectManager>();
        walls = GameObject.FindObjectsOfType<Wall>();

        isPlay = false;

        #endregion

        ropeGauge.gameObject.transform.parent.gameObject.SetActive(false);
    }

    #region UpdateFunc

    protected void FixedUpdate()
    {
        if (!isPlay)
            return;

        //콤보시간 계산
        ComboTimeUpdate();
        EnemyComboTimeUpdate();
        FeverCheckUpdate();

        //로프 탈 수 있는지 확인
        CheckAbleToRopeRidingUpdate();

        //움직임
        HeroMoveUpdate();

        //뒤집기
        FlipXUpdate();

        //상태확인업데이트
        StateCheckUpdate();
        
        
	}

    protected void Update()
    {

    }

    void StateCheckUpdate()
    {
        switch(hState)
        {
            case HeroState.Idle:
                if (movable)
                    hState = HeroState.Running;
                break;

            case HeroState.Running:
                break;

            case HeroState.Combo_1:
                break;

            case HeroState.Combo_2:
                break;

            case HeroState.Combo_3:
                break;

            case HeroState.RopeRiding:
                movable = false;
                RopeRidingUpdate();
                break;

            case HeroState.RopeFlying:
                activeUnBeatable = true;
                break;
                
            case HeroState.RopeAttack:
                activeUnBeatable = true;
                break;

            case HeroState.FrontHit:
                break;

            case HeroState.BackHit:
                break;



        }


    }    
    
    //로프 탈 수 있는지 체크
    void CheckAbleToRopeRidingUpdate()
    {


        //무적 상태일때 로프는 탈 수 없는 상태
        if (unBeatable || activeUnBeatable)
        {
            ropeState.ropeRidable = false;
            return;
        }

        //로프 안에 있을 때
        if (ropeState.inRope)
        {
            //로프를 타고있거나 나는중이면 탈 수 있는 상태가 아니야
            if (hState == HeroState.RopeFlying || hState == HeroState.RopeRiding)//ropeState.isRopeShooting || ropeState.ropeRiding)
            {
                ropeState.ropeRidable = false;
                return;
            }

            //박스 안에 적이 없으면 로프를 탈 수 있음
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

    //로프 타는중일때 Update
    void RopeRidingUpdate()
    {

        ropeState.ropeTime += Time.deltaTime;


        float totalSpeed = speed + (speedLevel * addSpeedValue);

        if (isLeft)
        {
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z - (Time.deltaTime * totalSpeed));
            this.transform.position = (Vector2)this.transform.position + (Vector2.right * Time.deltaTime);
        }
        else
        {
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z + (Time.deltaTime * totalSpeed));
            this.transform.position = (Vector2)this.transform.position + (Vector2.left * Time.deltaTime);
        }

        if(speedLevel==0)
        {
            ropeGauge.value = ropeGauge.maxValue * (ropeState.ropeTime / 2);

            if (ropeState.ropeTime > 2f)
                RopeJumpFail();
        }
        else if(speedLevel ==1)
        {
            ropeGauge.value = ropeGauge.maxValue * (ropeState.ropeTime / 1.5f);
            if (ropeState.ropeTime > 1.5f)
                RopeJumpFail();
        }
        else if(speedLevel==2)
        {
            ropeGauge.value = ropeGauge.maxValue * (ropeState.ropeTime / 1f);

            if (ropeState.ropeTime > 1f)
                RopeJumpFail();

        }

    }

    //이동
    void HeroMoveUpdate() 
    {
        if (!movable)
            return;

        float totalSpeed = speed + (speedLevel * 3);


        if ((hState == HeroState.Combo_1 || hState == HeroState.Combo_2 || hState == HeroState.Combo_3)
            && ropeState.inRope)
            return;
            



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
    void FlipXUpdate()
    {

        float _scale = 1;
        if(hState == HeroState.Fever)
        {
            _scale = 1.2f;
        }

        if(isLeft)
        {
            this.gameObject.transform.localScale = new Vector3(_scale, _scale, _scale);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(-_scale, _scale, _scale);
        }
    }

    void WallCheck()
    {

    }
    #endregion



    //공격 버튼 눌렀을 때, 공격할 수 있는 상태인가?
    protected void AttackCheck()
    {
        //로프타는 중이면 나가 
        if (hState == HeroState.RopeRiding || hState == HeroState.RopeAttack)
            return;

        //로프 나는 중이면 로프공격
        if (hState == HeroState.RopeFlying)
        {
            hState = HeroState.RopeAttack;
            //RopeAttackEnemy();
            return;
        }
        

        //애니메이션 진행중이면 X
        if (hState == HeroState.Combo_1 || hState == HeroState.Combo_2 || hState == HeroState.Combo_3)    
            return;

        //콤보가 3으로 꽉 차 있으면 안된다고 흔들흔들
        if (comboNum == 3)
        {
            sePlayer.PlaySE(2); //공격불가 사운드
            StartCoroutine(ShakeCharacterRoutine(0.1f));
            return;
        }

        //콤보 수 올려주고 콤보 타임 초기화
        CalcCombo();

        //공격시 움직임
        AttackMove();

        //어택 상태 세팅
        SetAttackState();

        //어택 애니메이션
        //SetAttackTime();

    }

    //적 한 마리씩 공격
    protected void AttackEnemy(GameObject _enemy)
    {
        //어택 사운드
        sePlayer.PlaySE(1);
        //카메라 흔들림
        cam.ShakeCam(0.12f);

        EnemyValue _eValue = _enemy.GetComponent<Enemy>().eValue;
        Vector2 _pos = _enemy.transform.position;

        if (comboNum > 2)
        {
            if (_enemy.GetComponent<Enemy>().EnemyDeathCheck(true))   //피니쉬로 애를 때렸더니 죽었어
            {
                IncreaseHp();
                SpawnScore(CalcScore(_eValue, true),_pos);          //스코어 2
                IncreaseEnemyCombo();
                threeComCompleteNum += 1;   //피버 조건 올려주기
                //atkBox.enemyInBox.Remove(_enemy.gameObject);
            }

        }
        else
        {
            if (_enemy.GetComponent<Enemy>().EnemyDeathCheck(false)) //그냥 애를 때렸더니 죽었어
            {
                IncreaseHp();
                SpawnScore(CalcScore(_eValue, false),_pos);
                IncreaseEnemyCombo();
                //atkBox.enemyInBox.Remove(_enemy.gameObject);
            }
        }

    }

    //로프 공격(박스안의 모든 적 공격)
    protected void RopeAttackEnemy()
    {

        for(int i=0;i<atkBox.enemyInBox.Count;++i)
        {
            sePlayer.PlaySE(1);
            if (atkBox.enemyInBox[i].GetComponent<Enemy>().EnemyDeathCheck(true))
            {
                IncreaseHp();

                EnemyValue _eValue = atkBox.enemyInBox[i].GetComponent<Enemy>().eValue;
                Vector2 _pos = atkBox.enemyInBox[i].transform.position;
                SpawnScore(CalcScore(_eValue, preSpdLv),_pos);
                IncreaseEnemyCombo();
                ropeAtkCompleteNum += 1;
            }
        }
        atkBox.enemyInBox.Clear();
    }
  
    //공격하면서 움직임 설정
    protected void AttackMove()
    {
        //나중에 공격별로 설정
        if(comboNum == 1)
        {
            StartCoroutine(ImmediateSpeedUpRoutine());
        }
        else if( comboNum ==2)
        {
            StartCoroutine(ImmediateSpeedUpRoutine());
        }
        else if(comboNum==3)
        {
            cam.ForceZoomShot(cam.originZoomSize * 0.75f, 1);
            StartCoroutine(ImmediateSpeedUpRoutine());
        }


    }

    //어택 상태 지정
    void SetAttackState()
    {
        switch (comboNum)
        {
            case 1: 
                hState = HeroState.Combo_1;
                break;

            case 2: 
                hState = HeroState.Combo_2;
                break;

            case 3:
                hState = HeroState.Combo_3;
                break;

        }

    }

    //어택 시간 지정
    void SetAttackTime()
    {
        float _time = 0f;

        switch(comboNum)
        {
            // 원콤 시간지정
            case 1:
                _time = AttackStyles.GetAttackAnimationTime(oneCombo);
                break;

            // 투콤 시간지정
            case 2:
                _time = AttackStyles.GetAttackAnimationTime(twoCombo);
                break;

            // 막타
            case 3:
                _time = AttackStyles.GetAttackAnimationTime(threeCombo);
                break;

        }

        //어택 애니메이션 조절
        //StartCoroutine(AttackAniRoutine(_time));
    }

    //로프 탄다
    protected void RopeRide()
    {
        ropeGauge.gameObject.transform.parent.gameObject.SetActive(true);

        hState = HeroState.RopeRiding;

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

        SetRopeCam(true);

    }

    //로프 떠남
    public void LeaveRope()
    {
        if (hState != HeroState.RopeRiding)//!hState.ropeRiding)
            return;


        ropeGauge.gameObject.transform.parent.gameObject.SetActive(false);

        if(ropeState.ropeTime > 0.5f)
        {
            StartCoroutine(RopeJumpingRoutine(ropeState.ropeTime));

        }
        else
        {
            RopeJumpFail();
        }

    }

    // 로프실패
    public void RopeJumpFail()
    {
        ropeGauge.gameObject.transform.parent.gameObject.SetActive(false);

        hState = HeroState.BackHit;

        SetRopeCam(false);
        cam.ForceChangeOriginZoom();
        cam.ShakeCam(0.1f);

        this.transform.localEulerAngles = new Vector3(0, 0, 0);

        sePlayer.PlaySE(0);

        StartCoroutine(UnbeatableTime(1f));
        StartCoroutine(CantMoveTime(0.5f));
        SpeedDown();

    }


    void SetRopeCam(bool _rope)
    {
        if(_rope)
        {
            cam.dontLimit = true;
            cam.isForceCam = false;
            cam.SetZoomSizeSpeed(3f, 1);
        }
        else
        {
            cam.dontLimit = false;
            cam.isForceCam = true;
            cam.SetOriginZoomSizeSpeed();
        }
    }



    //데미지 받음
    public void GetDamage()
    {
        if (hState == HeroState.Fever)
            return;

        if (unBeatable || activeUnBeatable)
            return;

        //진동
        //Handheld.Vibrate();
        //사운드
        sePlayer.PlaySE(0);



        hState = HeroState.FrontHit;



        //hp감소
        hp -= damageValue;


        //0보다 낮으면 죽어용
        if (hp <= 0)
        {
            hp = 0;

            if (!neverDie)
                GameOver();
        }


        this.transform.localEulerAngles = new Vector3(0, 0, 0);


        //로프게이지 꺼주자 켜져있을 수도 있쟈나?
        ropeGauge.gameObject.transform.parent.gameObject.SetActive(false);
        SetRopeCam(false);
        cam.SetOriginZoomSizeSpeed();
        //맞았을 때 카메라 줌
        cam.ForceZoomShot(cam.originZoomSize * 0.9f);


        //무적타임
        StartCoroutine(UnbeatableTime(1f));
        //movable 조절
        StartCoroutine(CantMoveTime(1f));
        StartCoroutine(CantControlTime(1f));

        //넉백
        StartCoroutine(KnockbackTime(0.5f));

        //스피드 다운
        SpeedDown();

    }



    #region FeverFunction

    void FeverCheckUpdate()
    {

        if (isFeverMode)
        {
            nowFeverTime -= Time.deltaTime;
            activeUnBeatable = true;
            movable = true;
            hState = HeroState.Fever;
            speedLevel = 2;

            hp = originHp;

            if(nowFeverTime <=0f)
            {
                this.GetComponent<Renderer>().material.color = Color.white;
                isFeverMode = false;

                hState = HeroState.Running;
            }
            return;
        }


        if(enemyCombo>=10)
        {
            feverComboStayTime += Time.deltaTime;
        }
        else
        {
            feverComboStayTime = 0;
        }

        
        if(feverComboStayTime >= needStayTime && threeComCompleteNum >= needThreeCompleteNum 
            &&ropeAtkCompleteNum >= needRopeAtkCompleteNum)
        {

            if (hState == HeroState.RopeRiding)
                return;

            isFeverMode = true;
            nowFeverTime = feverTime;
            feverComboStayTime = 0;
            threeComCompleteNum = 0;
            ropeAtkCompleteNum = 0;


        }










    }

    #endregion


    #region Routines

    //어택 애니메이션 조절
    //IEnumerator AttackAniRoutine(float _time)
    //{
    //    isPlayingAtkAni = true;
    //    yield return new WaitForSeconds(_time);
    //    isPlayingAtkAni = false;
    //}

    //대쉬루틴
    IEnumerator ImmediateSpeedUpRoutine()
    {
        speed = originSpeed * 6f;
        yield return new WaitForSeconds(0.1f);
        speed = originSpeed;
    }
    
    //로프 점핑 루틴
    IEnumerator RopeJumpingRoutine(float _power)
    {

        preSpdLv = speedLevel;
        SpeedUp();

        float _dis = 0f;

        float _startPos = this.transform.position.x;

        if (preSpdLv == 0)
            _dis = 10f;
        else if (preSpdLv == 1)
            _dis = 16f;
        else if (preSpdLv == 2)
            _dis = 21f;


        float _xTarget = 0f;
        float _yTarget = this.transform.position.y + 1f;
 
        if(isLeft)
            _xTarget = this.transform.position.x - _dis;// - new Vector2(_dis, 0);
        else
            _xTarget = this.transform.position.x + _dis;

        ropeTarget = new Vector2(_xTarget, _yTarget);


        bool yFinish = false;

        hState = HeroState.RopeFlying;
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
        movable = false;

        cam.SetOriginZoomSizeSpeed();
        cam.SetFollowSpeed(8f);//cam.followSpeed = 8f;

        while(Mathf.Abs(this.transform.position.x - _startPos) < _dis)
        {
            if(hState != HeroState.RopeFlying && hState != HeroState.RopeAttack)
            {
                break;
            }

            float _totalSpeed = (_dis - Mathf.Abs(this.transform.position.x - _startPos))* Time.deltaTime * 3f;

            

            if(_totalSpeed >1.0f)
            {
                _totalSpeed = 0.7f;
            }
            this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(_xTarget, this.transform.position.y), _totalSpeed);

            //Debug.Log(this.transform.position.x);
            if(yFinish)
            {
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 5;
            }
            else
            {
                this.transform.position 
                    = Vector2.MoveTowards(this.transform.position, new Vector2(this.transform.position.x, _yTarget), (((_yTarget - this.transform.position.y)) * 10f) * Time.deltaTime);
            }


            if (!yFinish && _yTarget - this.transform.position.y <= 0.01f)
            {
                yFinish = true;
            }

            
            yield return null;
        }

        SetRopeCam(false);
        cam.SetOriginFollowSpeed();//cam.followSpeed = cam.originFollowSpeed;

        movable = true;
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 8;

    }


    //넉백루틴
    IEnumerator KnockbackTime(float _maxTime)
    {
        float _time = 0;

        while (_maxTime > _time)
        {
            if (isLeft)
            {
                this.gameObject.transform.position = (Vector2.right * Time.deltaTime * (speed * 2) * (_maxTime - _time)) + (Vector2)this.gameObject.transform.position;
            }
            else
            {
                this.gameObject.transform.position = (Vector2.left * Time.deltaTime * (speed * 2) * (_maxTime - _time)) + (Vector2)this.gameObject.transform.position;

            }

            _time += Time.deltaTime;
            yield return null;
        }
    }

    //캐릭터 흔들기
    protected IEnumerator ShakeCharacterRoutine(float _maxTime)
    {
        float shakeTime = 0f;
        while (shakeTime < _maxTime)
        {
            shakeTime += Time.unscaledDeltaTime;
            this.gameObject.transform.position = this.gameObject.transform.position + (Random.insideUnitSphere * 0.1f);
            yield return null;
        }
    }


    IEnumerator UnbeatableBlinkRoutine()
    {
        bool colorChange = false;
        Color wColor = Color.white;
        Color gColor = Color.gray;

        while(true)
        {
            
            if (unBeatable)
            {
                if (colorChange)
                {
                    this.GetComponent<Renderer>().material.color = wColor;
                    colorChange = false;
                }
                else
                {
                    this.GetComponent<Renderer>().material.color = gColor;
                    colorChange = true;
                }


                

            }
            else
            {
                if(colorChange)
                {
                    colorChange = false;
                    this.GetComponent<Renderer>().material.color = wColor;
                }
            }

            yield return new WaitForSeconds(0.065f);

        }
    }

    //Hp 지속 감소 루틴
    IEnumerator DecreaseHpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(hpDecreaseTime);

            if(hState != HeroState.Fever)
                hp -= decreaseHpValue;
        }
    }

    //무적타임 루틴
    IEnumerator UnbeatableTime(float _time)
    {

        unBeatable = true;

        yield return new WaitForSeconds(_time);

        //무적을 풀어 줄 조건들도 필요하겠다

        unBeatable = false;

    }

    //조종할 수 없는 루틴
    IEnumerator CantControlTime(float _time)
    {
        controlable = false;
        yield return new WaitForSeconds(_time);
        controlable = true;

        comboNum = 0;
    }

    //움직일 수 없는 루틴
    protected IEnumerator CantMoveTime(float _time)
    {
        movable = false;
        yield return new WaitForSeconds(_time);
        movable = true;

    }

    #endregion

    #region CalcFunctions

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

    //hp 증가
    public void IncreaseHp()
    {
        hp += increaseHpValue;
        if (hp > 100)
            hp = 100;
    }

    //콤보 계산, 콤보시간 세팅
    void CalcCombo()
    {
        comboNum += 1;

        if (comboNum == 3)
        {
            ComboExhaust();
        }
    }

    //콤보탈진
    protected void ComboExhaust()
    {
        StartCoroutine(CantControlTime(1f));
    }


    //콤보타임 계산
    void ComboTimeUpdate()
    {

        if (hState == HeroState.Combo_1 || hState == HeroState.Combo_2 || hState == HeroState.Combo_3)
            return;

        if (comboTime > 0)
        {
            comboTime -= Time.deltaTime;
        }
        else
            comboNum = 0;
    }

    void IncreaseEnemyCombo()
    {
        enemyCombo += 1;
        enemyComboTime = enemyComboDeleteTime;
    }

    void EnemyComboTimeUpdate()
    {

        if(hState== HeroState.Combo_1 || hState == HeroState.Combo_2 || hState ==HeroState.Combo_3
            || hState == HeroState.RopeRiding || hState == HeroState.RopeAttack || hState == HeroState.Fever)
        {
            return;
        }

        if (enemyComboTime > 0)
        {
            enemyComboTime -= Time.deltaTime;
        }
        else
            enemyCombo = 0;


    }


    //스피드 레벨 업
    void SpeedUp()
    {
        speedLevel += 1;
        if (speedLevel > 2)
            speedLevel = 2;

    }

    //스피드 레벨 다운
    void SpeedDown()
    {
        speedLevel -= 1;
        if (speedLevel < 0)
            speedLevel = 0;
    }


    //방향세팅
    public void ChangeDirect(bool _isLeft)  //true is Left
    {
        if (comboNum != 3)
        {
            comboTime = 0;
            comboNum = 0;
        }

        isLeft = _isLeft;
    }

    #endregion

    #region ColliderFunc

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Rope"))
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
        if (coll.gameObject.CompareTag("Wall"))
        {
            //if (ropeState.ropeRiding)
            //    return;

            //ropeState.isRopeShooting = false;
            //if (hState == HeroState.Combo_1 || hState == HeroState.Combo_2 || hState == HeroState.Combo_3)
            //{
            //    movable = false;
            //    return;
            //}

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
        if (coll.gameObject.CompareTag("Wall"))
        {
            if (hState == HeroState.Combo_1 || hState == HeroState.Combo_2 || hState == HeroState.Combo_3)
                return;

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

    #endregion

    #region SpineEventFunc


    public void AttackEnemyTimingCheckEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        //어택 타이밍
        if (e.data.name == "attack")
        {
            if(hState == HeroState.RopeAttack)
            {
                RopeAttackEnemy();
                return;
            }


            if (atkBox.enemyInBox.Count > 0)
            {
                if (comboNum == 3)
                {
                    for (int i = 0; i < atkBox.enemyInBox.Count; ++i)
                    {
                        //Debug.Log(atkBox.enemyInBox.Count);
                        //Debug.Log(i);
                        AttackEnemy(atkBox.enemyInBox[i]);
                    }
                    atkBox.enemyInBox.Clear();
                }
                else
                {
                    int _num = Random.Range(0, atkBox.enemyInBox.Count);
                    AttackEnemy(atkBox.enemyInBox[_num]);
                    atkBox.enemyInBox.Remove(atkBox.enemyInBox[_num]);
                }
            }
        }

        //어택 시 잠시 멈추게됨, 때리는동안 무적상태
        if(e.data.name == "attackstart")
        {
            if(!isFeverMode)
                movable = false;
            activeUnBeatable = true;
        }





    }

    //애니메이션 끝났을때
    public void AttackAnimationComplete(Spine.TrackEntry trackEntry)
    {
        

        switch(hState)
        {
            case HeroState.Combo_1:
                hState = HeroState.Running;

                movable = true;
                activeUnBeatable = false;

                comboTime = 0.2f;
                
                break;

            case HeroState.Combo_2:
                hState = HeroState.Running;

                movable = true;
                activeUnBeatable = false;

                comboTime = 0.2f;
                break;

            case HeroState.Combo_3:
                hState = HeroState.Running;

                movable = true;
                activeUnBeatable = false;
                
                comboTime = 0.4f;
                break;

            case HeroState.FrontHit:
                hState = HeroState.Idle;
                break;

            case HeroState.BackHit:
                hState = HeroState.Idle;
                break;


            case HeroState.RopeAttack:
                hState = HeroState.Running;
                activeUnBeatable = false;
                break;

            case HeroState.RopeFlying:
                hState = HeroState.Running;
                activeUnBeatable = false;
                break;

            case HeroState.RopeRiding:
                break;





                
                
        }

    }
    




    #endregion

    #region Score

    int CalcScore(EnemyValue _eValue)
    {
        int _score = 500;

        switch(_eValue)
        {
            case EnemyValue.Normal:
                score += _score;
                return _score;
        }
        
        _score = 1000;
        score += 1000;

        return _score;

    }

    int CalcScore(EnemyValue _eValue, bool _isFinish)
    {
        int _score = 0;
        int _baseScore = CalcScore(_eValue);
        int _totalScore = 0;

        if(_isFinish)
        {
            _score += 500;
        }

        _totalScore = _score + _baseScore;
        score += _score;
        return _totalScore;

    }

    int CalcScore(EnemyValue _eValue, int _speedLv)
    {
        int _score = 0;
        int _baseScore = CalcScore(_eValue);
        int _totalScore = 0;

        switch(_speedLv)
        {
            case 0:

                _score += 1000;
                break;

            case 1:
                _score += 2000;
                break;

            case 2:
                _score += 3000;
                break;
        }

        _totalScore = _score + _baseScore;
        score += _score;
        return _totalScore;

    }

    void SpawnScore(int _score, Vector2 _spawnPos)
    {
        GameObject _spawnScoreObj;
        if (_score>=1000)
        {
            _spawnScoreObj = spawnHighScoreObj;
        }
        else
        {
            _spawnScoreObj = spawnScoreObj;
        }


        //if(spawnScoreObj != null)
        //{
            _spawnPos.y += Random.Range(3.5f,4.5f);
            _spawnPos.x += Random.Range(-2f, 2f);
            GameObject _spawnObj = (GameObject)Instantiate(_spawnScoreObj, _spawnPos, Quaternion.identity);
            _spawnObj.GetComponent<Text>().text = _score.ToString();
        //}
    }

    #endregion


    //게임 오버
    public void GameOver()
    {
        if (neverDie)
            return;
        Time.timeScale = 0;
    }

    public void GameStart()
    {
        isPlay = true;
        hState = HeroState.Running;
        
        //루틴 시작
        StartCoroutine(DecreaseHpRoutine());
        StartCoroutine(UnbeatableBlinkRoutine());
    }


    //콤보별 캐릭터 색
    void CharacterComboColorSet()
    {
        if (this.GetComponent<SpriteRenderer>() == null)
            return;

        if (hState == HeroState.RopeFlying)
        {
            this.GetComponent<SpriteRenderer>().color = Color.yellow;
            return;
        }

        switch (comboNum)
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

}
