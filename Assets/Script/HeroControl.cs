using UnityEngine;
using System.Collections;

public class HeroControl : Hero 
{


    bool isKeyPressed;

    

    void Awake()
    {
        base.Awake();

        isKeyPressed = false;
    }

    void FixedUpdate()
    {
        base.FixedUpdate();


        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isKeyPressed)
        {
            isKeyPressed = true;
            if(isLeft)
            {
                InputAttackEnemy();
            }
            else
            {
                ChangeDirect(true);
            }
 
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isKeyPressed)
        {


            isKeyPressed = true;
            if (isLeft)
            {
                ChangeDirect(false);
            }
            else
            {
                InputAttackEnemy();
            }
        }



        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isKeyPressed = false;
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow))
        {

            isKeyPressed = false;
        }

        
    }

    void Update()
    {

    }

    public void ChangeDirect()  //방향전환
    {
        if (isLeft)
            isLeft = false;
        else
            isLeft = true;
    }


    public void InputAttackEnemy()  //공격
    {
        if(ropeState.isRopeShooting)
        {
            RopeAttackEnemy();
            return;
        }

        if (isPlayingAtkAni)    //애니메이션 진행중이면 X
            return;


        if (comboNum == 3)
        {
            sePlayer.PlaySE(2);
            StartCoroutine(ShakeCharacter(0.1f));
            return;
        }

        comboNum += 1;
        comboTime = 0.8f;


        AttackMove();



        if (comboNum == 3)
        {
            ComboExhaust();
        }
        //atkBox.enemyInBox.Clear();

    }


    public void InputRopeRide()
    {
        RopeRide();
    }

    public void InputRopeShoot()
    {

        LeaveRope();

    }

    








}
