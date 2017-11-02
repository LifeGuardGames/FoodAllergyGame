using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelManager : Singleton<LoadLevelManager> {
	private static bool isCreated;

	public TweenToggleDemux loadDemux;
	public Text loadText;
	public Image loadImage;
	public FoodTipController foodTipController;
	public Transform adParentTransform;
	public GameObject logo;
	public float foodTipWait = 1.3f;        // How long to wait if the food tip controller is showing
	public float adWait = 3.0f;

	private bool isLoadingTipWait = false;
	private bool isAdWait = false;
	private string sceneToLoad;
	private GameObject eventSystemObject;
	private AdPerfectlyFree adScript;

	public string GetCurrentSceneName() {
		return SceneManager.GetActiveScene().name;
	}

	void Awake() {
		// Make object persistent
		if(isCreated) {
			// If There is a duplicate in the scene. delete the object and jump Awake
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		isCreated = true;

		FindEventSystem();
		SceneManager.sceneLoaded += OnSceneChange;
	}

	private void FindEventSystem() {
		eventSystemObject = GameObject.Find("EventSystem");
	}

	/// <summary>
	/// Call this to start the transition
	/// </summary>
	/// <param name="sceneName">Scene to be loaded</param>
	public void StartLoadTransition(string sceneName, string additionalTextKey = null, string additionalImageKey = null, bool showRandomTip = false, bool showAdChance = true) {
		isLoadingTipWait = false;
		isAdWait = false;
		if(eventSystemObject != null) {
			eventSystemObject.SetActive(false);
		}

		// Reset everything first
		loadText.text = "";
		loadImage.gameObject.SetActive(false);

		if(additionalTextKey != null) {
			logo.SetActive(true);
			loadText.text = LocalizationText.GetText(additionalTextKey);
		}
		else if(additionalImageKey != null) {
			logo.SetActive(false);
			loadImage.gameObject.SetActive(true);
			loadImage.sprite = SpriteCacheManager.GetLoadingImageData(additionalImageKey);
			isLoadingTipWait = true;
		}
		// Every other case
		else if(showRandomTip || showAdChance) {
			bool showAd = false;

			// Determine if show ad chance overrides random tip
			if(showAdChance && !showRandomTip) {
				showAd = false;
			}
			else if(showAdChance) {		// 20% chance to show ads
				showAd = UnityEngine.Random.Range(0, 5) == 0;
			}

			if(showAd) {    // Show ad
				if(eventSystemObject != null) {
					eventSystemObject.SetActive(true);
				}

				isAdWait = true;
				GameObject adReference = Resources.Load("AdPerfectlyFree") as GameObject;
				GameObject ad = GameObjectUtils.AddChild(adParentTransform.gameObject, adReference);
				adScript = ad.GetComponent<AdPerfectlyFree>();
				adScript.ShowPanel();
			}
			else {			// Show food tip
				isLoadingTipWait = true;
				if(UnityEngine.Random.Range(0, 2) == 0) {       // Show food tip
					logo.SetActive(true);
					foodTipController.ShowFoodTip();
				}
				else {                                          // Show generic image tip
					logo.SetActive(false);
					loadImage.gameObject.SetActive(true);

					int randomIndex = UnityEngine.Random.Range(0, 4);
					Sprite loadingImage = null;
					string loadingText = "";
					switch(randomIndex) {
						case 0:
							loadingImage = SpriteCacheManager.GetLoadingImageData("LoadingImageCrossContact");
							loadingText = LocalizationText.GetText("LoadingKeyCrossContact");
							break;
						case 1:
							loadingImage = SpriteCacheManager.GetLoadingImageData("LoadingImageReadLabels");
							loadingText = LocalizationText.GetText("LoadingKeyReadLabels");
							break;
						case 2:
							loadingImage = SpriteCacheManager.GetLoadingImageData("LoadingImageTellChef");
							loadingText = LocalizationText.GetText("LoadingKeyTellChef");
							break;
						case 3:
							loadingImage = SpriteCacheManager.GetLoadingImageData("LoadingImageWashHands");
							loadingText = LocalizationText.GetText("LoadingKeyWashHands");
							break;
					}
					loadText.text = loadingText;
					loadImage.sprite = loadingImage;
				}
			}
		}
		sceneToLoad = sceneName;
		loadDemux.Show();
		AudioManager.Instance.PlayClip("LoadingOpen");
	}

	/// <summary>
	/// Call this to initiate loading (when ending tween is done), there will be a performance hit here
	/// </summary>
	public void ShowFinishedCallback() {
		if(sceneToLoad != null) {
			if(isLoadingTipWait) {
				Invoke("LoadLevel", foodTipWait);
			}
			else if(isAdWait) {
				Invoke("LoadLevel", adWait);
			}
			else {
				LoadLevel();
			}
		}
		else {
			Debug.LogError("No level name specified");
		}
	}

	private void LoadLevel() {
		if(eventSystemObject != null) {
			eventSystemObject.SetActive(false);
		}
		if(adScript != null) {
			adScript.HidePanel();
			adScript = null;
		}
		SceneManager.LoadScene(sceneToLoad);
	}

	/// <summary>
	/// Hide the demux when the new level is loaded
	public void OnSceneChange(Scene scene, LoadSceneMode mode) {
		loadDemux.Hide();
		AudioManager.Instance.PlayClip("LoadingClose");
		if(isLoadingTipWait) {
			foodTipController.HideFoodTip();
		}
		FindEventSystem();
	}
}
