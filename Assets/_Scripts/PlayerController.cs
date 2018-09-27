using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private bool flying;
	public float speed;
	public float xmin,xmax;
	private Rigidbody2D rb;
	public GameObject baby, adult, fly;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		flying = false;
	}


	void FixedUpdate ()
	{
		if (!flying) {
		float moveHorizontal = Input.GetAxis ("Horizontal");

		Vector2 movement = new Vector2 (moveHorizontal, 0.0f);

		rb.velocity= (movement * speed);


			rb.position = new Vector2 (
				Mathf.Clamp (rb.position.x, xmin, xmax), 
				rb.transform.position.y
			);
		}
	}



	public void Fly() {
		flying = true;
		Destroy (transform.GetChild (0).gameObject);
		Instantiate (fly, transform);
		rb.velocity= (new Vector2(0.0f, 80.0f));
		Debug.Log (rb.velocity);
	}

	public void Evolve() {
		Destroy (transform.GetChild(0).gameObject);
		Instantiate (adult, transform);
	}

	public void Combust() {
		Destroy (transform.GetChild(0).gameObject);
		Instantiate (baby, transform);
	}
}
