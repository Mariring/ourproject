using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Mariring;

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
        

    }
	

	void FixedUpdate () 
    {


        if (hero.ropeState.ropeRidable || hero.hState == HeroState.RopeRiding)//hero.ropeState.ropeRiding)
        {
            if (hero.ropeState.ropeLeft)
            {
                leftBtn.sprite = ropeBtnSpr;
            }
            else
            {
                rightBtn.sprite = ropeBtnSpr;
            }
        }
        else
        {
            if (hero.isLeft)
            {
                leftBtn.sprite = atkBtnSpr;
                rightBtn.sprite = rightBtnSpr;
            }
            else
            {
                leftBtn.sprite = leftBtnSpr;
                rightBtn.sprite = atkBtnSpr;
            }

        }




	}




    public void PushLeftButton()
    {
        hero.PushLeftButton();

    }

    public void PushRightButton()
    {
        hero.PushRightButton();
    }


    public void PushAttackButton()
    {
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
