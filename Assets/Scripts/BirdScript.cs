using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdScript : MonoBehaviour {

	public static BirdScript instance;

	[SerializeField]
	private Rigidbody2D myRigidbody;

	[SerializeField]
	private Animator anim;

	[SerializeField]
	private float forwardSpeed = 3f;

	private float bounceSpeed = 5f;

	private bool didFlap;

	public bool isAlive;
	public int score;

	private Button flapButton;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip flapClip, pointClip, diedClip;

	// Use this for initialization
	void Awake () {
		if(instance == null){
			instance = this;
		}
		isAlive = true;

		flapButton = GameObject.FindGameObjectWithTag("FlapButton").GetComponent<Button>();
		flapButton.onClick.AddListener(() => FlapTheBird());

		SetCamerasX();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(isAlive){
			Vector3 temp = transform.position;
			temp.x += forwardSpeed * Time.deltaTime;
			transform.position = temp;

			if(didFlap){
				didFlap = false;
				myRigidbody.velocity = new Vector2(0, bounceSpeed);
				anim.SetTrigger("Flap");
				audioSource.PlayOneShot(flapClip);
			}

			if(myRigidbody.velocity.y >= 0){
				transform.rotation = Quaternion.Euler(0,0,0);
			}
			else{
				float angle = Mathf.Lerp(0, -90, -myRigidbody.velocity.y / 20);
				transform.rotation = Quaternion.Euler(0,0,angle);
			}
		}
	}

	void SetCamerasX(){
		CameraScript.offsetX = Camera.main.transform.position.x - transform.position.x - 1f;
	}
	public void FlapTheBird(){
		//print("FlapTheBird");
		didFlap = true;
	}

	public float GetPositionX(){
		return transform.position.x;
	}

	void OnCollisionEnter2D(Collision2D target){
		if (target.gameObject.tag == "Ground" || target.gameObject.tag == "Pipe"){
			if(isAlive){
				isAlive = false;
				anim.SetTrigger("Bird Died");
				audioSource.PlayOneShot(diedClip);
				GameplayController.instance.PlayerDiedShowScore(score);
				AdsController.instance.ShowInterstitial();
			}
		}
	}
	void OnTriggerEnter2D(Collider2D target){
		if(target.tag == "PipeHolder"){
			score++;
			GameplayController.instance.SetScore(score);
			audioSource.PlayOneShot(pointClip);
		}
	}
}
