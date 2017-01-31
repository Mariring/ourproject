using UnityEngine;
using System.Collections;

public class VectorTest : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.gameObject.transform.position = this.gameObject.transform.position + (Vector3)(Vector2.left * Time.deltaTime * 3); 
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {

            this.gameObject.transform.position = this.gameObject.transform.position + (Vector3)(Vector2.right * Time.deltaTime * 3); 
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Wall"))
            return;

        if(this.transform.position.x >coll.gameObject.transform.position.x)
        {
            coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 1000f);
        }
        else
        {
            coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 1000f);

        }
    }
}
