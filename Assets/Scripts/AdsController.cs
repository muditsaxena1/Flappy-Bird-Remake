using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsController : MonoBehaviour {

	public static AdsController instance;

	private const string SDK_KEY = "pGTwTWy0-l3_0LLIxzlpAtpzQVchQFuEVM8-u5-sq2rrOB8J8u7IlwG9Qusr7k3d5CbzHaq6cey_r5JzzQGiLX";

	void Awake () {
		MakeSingleton();
		SceneManager.sceneLoaded += OnLevelWsLoaded;
	}

	void Start(){
		AppLovin.SetSdkKey(SDK_KEY);
		AppLovin.InitializeSdk();
		AppLovin.SetUnityAdListener(this.gameObject.name);
		StartCoroutine(CallAds());
	}

	void OnLevelWsLoaded(Scene scene, LoadSceneMode loadSceneMode){
		if(SceneManager.GetActiveScene().name == "MainMenu"){
			int random = Random.Range(0, 10);
			if(random < 2){
				ShowVideo();
			}
			else if (random < 6){
				ShowInterstitial();
			}
		}
	}

	IEnumerator CallAds(){
		yield return new WaitForSecondsRealtime(3f);
		LoadInterstitial();
		LoadVideo();

		AppLovin.ShowAd(AppLovin.AD_POSITION_TOP, AppLovin.AD_POSITION_CENTER);
	}
	void MakeSingleton () {
		if(instance != null){
			Destroy(gameObject);
		}
		else{
			instance = this;
			DontDestroyOnLoad(instance);
		}
	}

	public void LoadInterstitial() {
		AppLovin.PreloadInterstitial();
	}

	public void ShowInterstitial(){
		if (AppLovin.HasPreloadedInterstitial()){
			AppLovin.ShowInterstitial();
		}
		else {
			LoadInterstitial();
		}
	}

	public void LoadVideo(){
		AppLovin.LoadRewardedInterstitial();
	}

	public void ShowVideo(){
		AppLovin.ShowRewardedInterstitial();
	}

	void onAppLovinEventReceived(string ev){
	    if(ev.Contains("DISPLAYEDINTER")) {
	        // An ad was shown.  Pause the game.
	    }
	    else if(ev.Contains("HIDDENINTER")) {
	        // Ad ad was closed.  Resume the game.
	        // If you're using PreloadInterstitial/HasPreloadedInterstitial, make a preload call here.
	        AppLovin.PreloadInterstitial();
	    }
	    else if(ev.Contains("LOADEDINTER")) {
	        // An interstitial ad was successfully loaded.
	    }
	    else if(string.Equals(ev, "LOADINTERFAILED")) {
	        // An interstitial ad failed to load.
	    }
		else if(ev.Contains("LOADEDREWARDED")) {
        // A rewarded video was successfully loaded.
	    }
	    else if(ev.Contains("LOADREWARDEDFAILED")) {
	        // A rewarded video failed to load.
	    }
	    else if(ev.Contains("HIDDENREWARDED")) {
	        // A rewarded video was closed.  Preload the next rewarded video.
	        AppLovin.LoadRewardedInterstitial();
	    }
	}

}
