using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamelogic : MonoBehaviour {

	// Access sprite animations for player
	public Animator animator;

	// Create spell queue
	public List<string> spells = new List<string>();

	// Obstacle logic
	public string key;
	public bool isObstacle = false;
	public Object[] textures;
	public Sprite obstacle;
	public SpriteRenderer obs;
	public bool isMoving = false;
	public float startTime;

	// Create spellqueue sprites
	public GameObject fire_spellqueue1;
	public GameObject water_spellqueue1;
	public GameObject wind_spellqueue1;

	public GameObject fire_spellqueue2;
	public GameObject water_spellqueue2;
	public GameObject wind_spellqueue2;

	public GameObject fire_spellqueue3;
	public GameObject water_spellqueue3;
	public GameObject wind_spellqueue3;

	// Use this for initialization
	void Start () {

		// Load in textures for potential obstacles
		textures = Resources.LoadAll("obstacles", typeof(Texture2D));

		// Find the game objects for the spellqueue sprites
		fire_spellqueue1 = GameObject.Find("Fire_spellqueue1");
		water_spellqueue1 = GameObject.Find("Water_spellqueue1");
		wind_spellqueue1 = GameObject.Find("Wind_spellqueue1");

		fire_spellqueue2 = GameObject.Find("Fire_spellqueue2");
		water_spellqueue2 = GameObject.Find("Water_spellqueue2");
		wind_spellqueue2 = GameObject.Find("Wind_spellqueue2");

		fire_spellqueue3 = GameObject.Find("Fire_spellqueue3");
		water_spellqueue3 = GameObject.Find("Water_spellqueue3");
		wind_spellqueue3 = GameObject.Find("Wind_spellqueue3");

	}
	
	// Update is called once per frame
	void Update () {

		// There is currently an obstacle
		if (isMoving == false) {
			animator.SetBool("obstacle", true);
		} else {
			animator.SetBool("obstacle", false);
		}

		// Call next obstacles
		if (isObstacle == false) {
			loadObstacle();
		}

		if (isMoving == true) {
			float step = (Time.time - startTime) * 5;
			float distance = step / 10;
			obs.transform.position = Vector3.Lerp(new Vector3(4f, 0.07f, 0.0f), new Vector3(0.5f, 0.07f, 0.0f), distance);
			if (obs.transform.position == new Vector3(0.5f, 0.07f, 0.0f)) {
				isMoving = false;
			}
		}

		// Check if spell queue is full
		if (spells.Count == 3) {
			if (isObstacle == true) {
				if (getAnswer(spells, key)) {
					Destroy(obs);
					spells.Clear();
					print("cleared spells");
					isObstacle = false;
				} else {
					spells.Clear();
					print("cleared spells");
				}
			}

			// No spells left in queue
			moveSpellqueue(fire_spellqueue1, -100f);
			moveSpellqueue(fire_spellqueue2, -100f);
			moveSpellqueue(fire_spellqueue3, -100f);

			moveSpellqueue(water_spellqueue1, -100f);
			moveSpellqueue(water_spellqueue2, -100f);
			moveSpellqueue(water_spellqueue3, -100f);

			moveSpellqueue(wind_spellqueue1, -100f);
			moveSpellqueue(wind_spellqueue2, -100f);
			moveSpellqueue(wind_spellqueue3, -100f);
		}

		// Create raycast
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
 		
 		// Check if user clicks on spells
 		if (Input.GetMouseButtonDown(0)) {

			if (hit.collider != null) {
				if (isMoving == false) {
					// Check if fire was clicked
					if (hit.collider.gameObject.name == "Fire_Button") {
						if (spells.Count != 3) {

							// Transform spellqueue sprites
							if (spells.Count == 0) {
								moveSpellqueue(fire_spellqueue1, -0.165f);
							} else if (spells.Count == 1) {
								moveSpellqueue(fire_spellqueue2, 0f);
							} else if (spells.Count == 2) {
								moveSpellqueue(fire_spellqueue3, 0.165f);
							}

							// Add fire spell to list
							spells.Add("f");

						}
					// Check if water was clicked
					} else if (hit.collider.gameObject.name == "Water_Button") {
						if (spells.Count != 3) {

							// Transform spellqueue sprites
							if (spells.Count == 0) {
								moveSpellqueue(water_spellqueue1, -0.165f);
							} else if (spells.Count == 1) {
								moveSpellqueue(water_spellqueue2, 0f);
							} else if (spells.Count == 2) {
								moveSpellqueue(water_spellqueue3, 0.165f);
							}

							// Add water spell to list
							spells.Add("w");

						}
					// Check if wind was clicked
					} else if (hit.collider.gameObject.name == "Wind_Button") {
						if (spells.Count != 3) {

							// Transform spellqueue sprites
							if (spells.Count == 0) {
								moveSpellqueue(wind_spellqueue1, -0.165f);
							} else if (spells.Count == 1) {
								moveSpellqueue(wind_spellqueue2, 0f);
							} else if (spells.Count == 2) {
								moveSpellqueue(wind_spellqueue3, 0.165f);
							}

							// Add wind spell to list
							spells.Add("a");
						}
					}
				}
			}
 		}
	}

	// Transform spell queue sprites
	void moveSpellqueue(GameObject type, float x) {
		type.transform.position = new Vector3(x, -0.35f, 0.0f);
	}
	
	// Load up next obstacle
	void loadObstacle() {
		Texture2D texture = (Texture2D)(textures[Random.Range(0, textures.Length)]);
		obs = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
		obstacle = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
		obs.sprite = obstacle;
		obs.transform.position = new Vector3(0f, 0.07f, 0.0f);
		key = texture.name;
		isObstacle = true;
		isMoving = true;
		startTime = Time.time;
	}

	bool getAnswer(List<string> p, string answer) {
		string guess = p[0] + p[1] + p[2];
		char[] guessarray = guess.ToCharArray();
		char[] answerarray = answer.ToCharArray();
		System.Array.Sort(guessarray);
		System.Array.Sort(answerarray);
		guess = new string(guessarray);
		answer = new string(answerarray);
		if (guess == answer) {
			return true;
		} else {
			return false;
		}
	}


}
