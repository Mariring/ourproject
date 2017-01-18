using UnityEngine;
using System.Collections;

public class EnemyNormal : Enemy {


    public Sprite[] stateSpr;
    //  0: 일반, 1: 공격준비, 2: 퍽

    SpriteRenderer sprRdr;

	// Use this for initialization
	void Awake () 
    {
        base.Awake();
        sprRdr = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        base.FixedUpdate();

       // Debug.Log(enemyState);
        sprRdr.sprite = stateSpr[enemyState];

	}
}
