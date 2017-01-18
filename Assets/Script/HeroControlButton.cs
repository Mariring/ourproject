using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroControlButton : MonoBehaviour {

    public HeroControl hero;

    public Image leftBtn;
    public Image rightBtn;



    public Sprite leftBtnSpr;
    public Sprite rightBtnSpr;
    public Sprite ropeBtnSpr;
    public Sprite atkBtnSpr;

	// Use this for initialization
	void Awake () 
    {

	}

    void Start()
    {
        //leftBtn.onClick.AddListener(PushLeftButton);
        //rightBtn.onClick.AddListener(PushRightButton);
        

    }
	

	void FixedUpdate () 
    {


        if (hero.ropeState.ropeRidable || hero.ropeState.ropeRiding)
        {
            if (hero.ropeState.ropeLeft)
            {
                leftBtn.sprite = ropeBtnSpr;
                //leftTxt.text = "Rope";
            }
            else
            {
                rightBtn.sprite = ropeBtnSpr;
                //rightTxt.text = "Rope";
            }
        }
        else
        {
            if (hero.isLeft)
            {
                leftBtn.sprite = atkBtnSpr;
                rightBtn.sprite = rightBtnSpr;
                //leftTxt.text = "Attack";
                //rightTxt.text = "Right";
            }
            else
            {
                leftBtn.sprite = leftBtnSpr;
                rightBtn.sprite = atkBtnSpr;
                //leftTxt.text = "Left";
                //rightTxt.text = "Attack";
            }

        }



	}




    public void PushLeftButton()
    {

        //컨트롤 할 수 없는 상태
        if (!hero.GetControlable())
            return;



        //로프 탈 수 있음
        if(hero.ropeState.ropeRidable && hero.ropeState.ropeLeft)
        {
            PushRopeRideButton();
            return;
        }


        //
        if(hero.isLeft)
        {
            if (!hero.ropeState.ropeRiding)
                PushAttackButton();

        }
        else
        {
            if(!hero.ropeState.isRopeShooting)  //로프슈팅 중이 아닐 때 방향 바꿔
                hero.ChangeDirect(true);
        }

    }

    public void PushRightButton()
    {
        if (!hero.GetControlable())
            return;


        ////로프 타고 있는 중
        //if (hero.ropeState.ropeRiding && hero.isLeft)
        //{
        //    PushRopeShootButton();
        //    return;
        //}

        //로프 탈 수 있음
        if((hero.ropeState.ropeRidable && !hero.ropeState.ropeLeft))
        {
            PushRopeRideButton();
            return;
        }

        
        if (!hero.isLeft)
        {
            PushAttackButton();
        }
        else
        {
            if (!hero.ropeState.isRopeShooting)  //로프슈팅 중이 아닐 때 방향 바꿔
                hero.ChangeDirect(false);
        }


    }


    public void PushAttackButton()
    {
        if (hero.ropeState.ropeRiding)
            return;

        hero.InputAttackEnemy();

    }

    public void PushRopeRideButton()
    {
        hero.InputRopeRide();
    }

    public void PushRopeShootButton()
    {
        hero.InputRopeShoot();
    }



}
