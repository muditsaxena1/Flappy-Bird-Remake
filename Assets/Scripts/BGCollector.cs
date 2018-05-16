using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGCollector : MonoBehaviour {

	private GameObject[] backgrounds;
	private GameObject[] grounds;

	private float lastBGX;
	private float lastGroundX;

	// Use this for initialization
	void Awake () {

		backgrounds = GameObject.FindGameObjectsWithTag("Background");
		grounds = GameObject.FindGameObjectsWithTag("Ground");

		lastBGX = backgrounds[0].transform.position.x;
		lastGroundX = grounds[0].transform.position.x;

		for(int i = 0; i < backgrounds.Length; i++){
			if(backgrounds[i].transform.position.x > lastBGX){
				lastBGX = backgrounds[i].transform.position.x;
			}
		}
		for(int i = 0; i < grounds.Length; i++){
			if (grounds[i].transform.position.x > lastGroundX){
				lastGroundX = grounds[i].transform.position.x;
			}
		}
	}


	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D target) {
		if(target.tag == "Background"){
			Vector3 temp = target.transform.position;
			float width = ((BoxCollider2D)target).size.x;
			temp.x = lastBGX + width;
			lastBGX = temp.x;

			target.transform.position = temp;
		}
		else if(target.tag == "Ground"){
			Vector3 temp = target.transform.position;
			float width = ((BoxCollider2D)target).size.x;
			temp.x = lastGroundX + width;
			lastGroundX = temp.x;

			target.transform.position = temp;
		}
	}
}
