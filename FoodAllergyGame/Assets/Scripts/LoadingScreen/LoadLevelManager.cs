using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelManager : Singleton<LoadLevelManager>{

	private static bool isCreated;

	public TweenToggleDemux loadDemux;
	public Text loadText;
	public Image loadImage;
	public FoodTipController foodTipController;
	public GameObject logo;
	public float foodTipWait = 1.3f;		// How long to wait if the food tip controller is showing
	private bool isShowingFoodTip = false;
	private bool isShowingImageTip = false;
	private string sceneToLoad;

	public string GetCurrentSceneName() {
		 return SceneManager.GetActiveScene().name;
	}

	void Awake(){
		// Make object persistent
		if(isCreated){
			// If There is a duplicate in the scene. delete the object and jump Awake
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		isCreated = true;
	}

	/// <summary>
	/// Call this to start the transition
	/// </summary>
	/// <param name="sceneName">Scene to be loaded</param>
	public void StartLoadTransition(string sceneName, string additionalTextKey = null, string additionalImageKey = null, bool showRandomTip = false){
		isShowingFoodTip = false;
		isShowingImageTip = false;

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
			isShowingImageTip = true;
        }
		else if(showRandomTip) {
			if(UnityEngine.Random.Range(0,2) == 0){		// Show food tip
				logo.SetActive(true);
				isShowingFoodTip = true;
				foodTipController.ShowFoodTip();
			}
			else{										// Show generic image tip
				int randomIndex = UnityEngine.Random.Range(0, 4);
				Sprite loadingImage;
				string loadingText;
				switch(randomIndex){
				case 0:
					loadingImage = SpriteCacheManager.GetLoadingImageData("LoadingImage1");
					loadingText = LocalizationText.GetText("Key1");
					break;
				case 1:
					loadingImage = SpriteCacheManager.GetLoadingImageData("LoadingImage1");
					loadingText = LocalizationText.GetText("Key1");
					break;
				case 2:
					loadingImage = SpriteCacheManager.GetLoadingImageData("LoadingImage1");
					loadingText = LocalizationText.GetText("Key1");
					break;
				case 3:
					loadingImage = SpriteCacheManager.GetLoadingImageData("LoadingImage1");
					loadingText = LocalizationText.GetText("Key1");
					break;
				}
				loadText = loadingText;
				loadImage.sprite = loadingImage;
			}
		}
		sceneToLoad = sceneName;
		loadDemux.Show();
	}

	/// <summary>
	/// Call this to initiate loading (when ending tween is done), there will be a performance hit here
	/// </summary>
	public void ShowFinishedCallback(){
		if(sceneToLoad != null){
			if(isShowingFoodTip || isShowingImageTip) {
				Invoke("LoadLevel", foodTipWait);
			}
			else {
				LoadLevel();
			}
		}
		else{
			Debug.LogError("No level name specified");
		}
	}

	private void LoadLevel() {
		SceneManager.LoadScene(sceneToLoad);
	}

	/// <summary>
	/// Hide the demux when the new level is loaded
	void OnLevelWasLoaded(){
		loadDemux.Hide();
		if(isShowingFoodTip) {
			foodTipController.HideFoodTip();
		}
	}
}
