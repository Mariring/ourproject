using UnityEngine;
using System.Collections;
using Mariring;


public class EnemyAngryAnimation : EnemyAnimation 
{
    bool aniUnbeatable;

	// Use this for initialization
	void Start () 
    {
        base.Start();
        aniUnbeatable = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        base.Update();

        StateUpdateCheck();

	}

    void StateUpdateCheck()
    {

        if(enemy.isUnbeatable)//angryEnemy.eState == EnemyState.Running)
        {
            if (!aniUnbeatable)
            {
                SetAnimation(angryWalkAniName, 1, true);
                aniUnbeatable = true;
            }
        }
        else
        {
            aniUnbeatable = false;
        }
    }




}
