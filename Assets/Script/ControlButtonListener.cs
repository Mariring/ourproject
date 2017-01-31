using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ControlButtonListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public HeroControlButton controlBtn;
    public bool isLeft;

    [HideInInspector]
    public bool isPressed;
    float pressedTime;

	void Awake () 
    {
        pressedTime = 0;
	}
	
	void FixedUpdate () 
    {
        //if(isPressed)
        //{
        //    pressedTime += Time.deltaTime;
        //}
	}



    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        pressedTime = 0;


        if(isLeft)
        {
            controlBtn.PushLeftButton();
        }
        else
        {
            controlBtn.PushRightButton();
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;


        //로프 타고 있는 중
        controlBtn.PushRopeShootButton();




    }


}
