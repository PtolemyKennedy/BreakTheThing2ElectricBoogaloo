using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class MainMenu : MonoBehaviour {

    // Use this for initialization
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;

    private void Start()
    {
        Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels"); //Creates array of levels in folder
        foreach (Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;
            container.transform.SetParent(levelButtonContainer.transform,false); //false here sets the location to the container

            string sceneName = thumbnail.name;
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName)); //called every time a button is clicked and changes depending on button
        }
    }
    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7348650003716399/4455741290";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder()
     .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
     .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")  // My test device.
     .Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7348650003716399/8471007928";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        InterstitialAd interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }


    private void LoadLevel (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LookAtMenu(Transform menuTransform)
    {
        Camera.main.transform.LookAt(menuTransform.position);
    }
    private void Update()
    {
        
    }
}
