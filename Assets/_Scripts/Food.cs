using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

	private GameController controller;
	public float speed;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			controller = gameControllerObject.GetComponent <GameController> ();
		} else {
			Debug.Log ("Cannot find 'GameController' script");
		}

		rb = GetComponent<Rigidbody2D> ();

		rb.velocity = transform.up * -speed;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Destroy (this.gameObject);
		if (other.tag == "Player") {
			if (this.tag == "Lemon") {
				controller.AddLemon ();
			} else {
				controller.AddEnergy ();
			}
		}
	}

}
