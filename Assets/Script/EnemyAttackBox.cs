using UnityEngine;
using System.Collections;

public class EnemyAttackBox : MonoBehaviour {

    [HideInInspector]
    public bool isBoxInHero;

    [HideInInspector]
    public Hero heroInBox;


	// Use this for initialization
	void Awake ()
    {
        heroInBox = null;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    
	}


    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Hero"))
        {
            heroInBox = coll.gameObject.GetComponent<Hero>();
            isBoxInHero = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(heroInBox != null)
        {
            if (coll.gameObject == heroInBox.gameObject)
            {
                heroInBox = null;
                isBoxInHero = false;
            }
        }
      
    }
}
