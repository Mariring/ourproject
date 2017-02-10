using UnityEngine;
using System.Collections;
using Mariring;

public class HeroControl : Hero 
{


    

    void Awake()
    {
        base.Awake();

    }

    void FixedUpdate()
    {
        if (!isPlay)
            return;
        base.FixedUpdate();
    }

    void Update()
    {
        if (!isPlay)
            return;

        base.Update();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PushLeftButton();

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PushRightButton();
        }


        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            InputRopeShoot();
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            InputRopeShoot();
        }


    }

    public void PushLeftButton()
    {

        //컨트롤 할 수 없는 상태
        if (!GetControlable())
            return;


        if(hState == HeroState.Fever)
        {
            if(!isLeft)
                ChangeDirect(true);

            return;
        }



        //로프를 탈 수 있는 상태
        if (ropeState.ropeRidable && ropeState.ropeLeft)
        {
            InputRopeRide();
            return;
        }


        //얘가 왼쪽이냐 오른쪽이냐.
        if (isLeft)
        {
            //히어로가 로프를 타고있는 중이 아니면 때려
            if (hState != HeroState.RopeRiding)
                InputAttackEnemy();

        }
        else
        {
            if(hState == HeroState.Running)
                ChangeDirect(true);
            //로프슈팅 중이 아닐 때 방향 바꿔
            //if (hState != HeroState.RopeFlying)

        }

    }

    public void PushRightButton()
    {
        if (!GetControlable())
            return;


        if (hState == HeroState.Fever)
        {
            if (isLeft)
                ChangeDirect(false);

            return;
        }



        //로프 탈 수 있음
        if ((ropeState.ropeRidable && !ropeState.ropeLeft))
        {
            InputRopeRide();
            return;
        }

        //얘가 왼쪽이냐 오른쪽이냐.
        if (!isLeft)
        {
            //히어로가 로프를 타고있는 중이 아니면 때려
            if (hState != HeroState.RopeRiding)
                InputAttackEnemy();
        }
        else
        {
            //로프슈팅 중이 아닐 때 방향 바꿔
            if (hState == HeroState.Running)
                ChangeDirect(false);

        }


    }


    public void InputAttackEnemy()
    {

        //공격
        AttackCheck();

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
