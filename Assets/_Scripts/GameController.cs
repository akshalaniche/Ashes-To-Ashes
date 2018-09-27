using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject[] food=new GameObject[3];
	public int[] ratio= new int[3];
	private GameObject[] hazards;

	public static int lemonsForCombust = 3;
	private int lemon;
	public GameObject[] lives = new GameObject[lemonsForCombust];

	public int hazardCount;
	public Vector3 spawnValues;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public float changeWait;

	public GUIText energyText;
	private int energy;
	private bool evolve, combust, win, adult;

	public int evolution, flight;

	public GameObject bird;
	private PlayerController control;
	private ChangeScene changeScene;

	// Use this for initialization
	void Start () {

		//set up hazards array with ratio 
		int total = 0;
		for (int i = 0; i < ratio.Length; i++) {
			total += ratio [i];
		}
		hazards = new GameObject[total];

		int j = 0;
		for (int i = 0; i < ratio.Length; i++) {
			int max = j + ratio [i];
			for (; j < max; j++) {
				hazards [j] = food [i];
			}
		}

	
		energy = 0;
		lemon = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
		evolve = false;
		combust = false;
		win = false;
		adult = false;

		UpdateScore();

		changeScene = gameObject.GetComponent <ChangeScene> ();
		control = bird.GetComponent <PlayerController> ();
	}

	IEnumerator SpawnWaves () {
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i = 0; i < hazardCount; i++) {
				GameObject hazard = hazards [Random.Range(0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Instantiate (hazard, spawnPosition, Quaternion.identity);
				if (win || combust || evolve)
					break;
				yield return new WaitForSeconds (spawnWait);
			}
			if (combust || evolve) {
				//Wait for player to evolve
				combust = false;
				evolve = false;
				yield return new WaitForSeconds (changeWait);
			}

			if (win) {
				control.Fly ();
				yield return new WaitForSeconds (changeWait);
				changeScene.NextScene ();
				break;
			}

			yield return new WaitForSeconds (waveWait);
		}
	}


	public void AddEnergy() {
		energy++;

		if (energy >= evolution && !adult) {
			adult = true;
			evolve = true;
			energy = 0;
			control.Evolve ();
		} else if (energy >= flight && adult) {
			win = true;
		}
		UpdateScore ();
	}

	void UpdateScore() {
		energyText.text = "Energy accumulated: " + energy;
	}


	public void AddLemon() {
		GameObject life = lives [lemon];
		life.SetActive (false);
		lemon = lemon + 1;

		if (lemon >= lemonsForCombust) {
			for (int i = 0; i < lives.Length; i++) {
				lives [i].SetActive (true);
			}
			lemon = 0;
			energy = 0;
			combust = true;
			adult = false;
			UpdateScore ();
			control.Combust ();
		}
	}
}
