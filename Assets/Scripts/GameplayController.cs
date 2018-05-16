﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour {

	public static GameplayController instance;

	[SerializeField]
	private Text scoreText, endScore, bestScore, gameoverText;

	[SerializeField]
	private Button restartGameButton, instructionsButton;

	[SerializeField]
	private GameObject pausePanel;

	[SerializeField]
	private GameObject[] birds;

	[SerializeField]
	private Sprite[] medals;

	[SerializeField]
	private Image medalImage;

	void Awake () {
		MakeInstance();
		Time.timeScale = 0f;
	}


	void MakeInstance () {
		if(instance == null){
			instance = this;
		}
	}

	public void PauseGame(){
		if (BirdScript.instance != null){
			if(BirdScript.instance.isAlive){
				pausePanel.SetActive(true);
				gameoverText.gameObject.SetActive(false);
				endScore.text = BirdScript.instance.score.ToString();
				bestScore.text = GameController.instance.GetHighscore() + "";
				Time.timeScale = 0f;
				restartGameButton.onClick.RemoveAllListeners();
				restartGameButton.onClick.AddListener(() => ResumeGame());
			}
		}
	}

	public void GoToMenuButton(){
		//Time.timeScale = 1f;
		SceneFader.instance.FadeIn("MainMenu");
	}

	public void ResumeGame(){
		pausePanel.SetActive(false);
		Time.timeScale = 1f;
	}

	public void RestartGame(){
		SceneFader.instance.FadeIn(SceneManager.GetActiveScene().name);
	}

	public void PlayGame(){
		scoreText.gameObject.SetActive(true);
		birds[GameController.instance.GetSelectedBird()].SetActive(true);
		instructionsButton.gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

	public void SetScore(int score){
		scoreText.text = "" + score;
	}

	public void PlayerDiedShowScore(int score){
		pausePanel.SetActive(true);
		gameoverText.gameObject.SetActive(true);
		scoreText.gameObject.SetActive(false);

		endScore.text = "" + score;
		if(score > GameController.instance.GetHighscore()){
			print("New High score saved");
			GameController.instance.SetHighscore(score);
		}

		bestScore.text = "" + GameController.instance.GetHighscore();

		if (score < 20){
			medalImage.sprite = medals[0];
		}
		else if (score < 40){
			medalImage.sprite = medals[1];

			if(GameController.instance.IsGreenBirdUnlocked() == 0){
				GameController.instance.UnlockGreenBird();
				print("Green Bird Unlocked");
			}
		}
		else{
			medalImage.sprite = medals[2];

			if(GameController.instance.IsGreenBirdUnlocked() == 0){
				GameController.instance.UnlockGreenBird();
				print("Green Bird Unlocked");
			}

			if(GameController.instance.IsRedBirdUnlocked() == 0){
				GameController.instance.UnlockRedBird();
				print("Red Bird Unlocked");
			}
		}

		restartGameButton.onClick.RemoveAllListeners();
		restartGameButton.onClick.AddListener(() => RestartGame());
	}

}
