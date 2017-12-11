using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class MainMenu : MonoBehaviour {
    private BannerView bannerView;
    // Use this for initialization
    public float cameraTransitionSpeed;
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;

    private Transform cameraTransform;
    private Transform cameraDesiredLookAt;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        
        Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels"); //Creates array of levels in folder
        foreach (Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;
            container.transform.SetParent(levelButtonContainer.transform,false); //false here sets the location to the container

            string sceneName = thumbnail.name;
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName)); //called every time a button is clicked and changes depending on button
        }
        RequestBanner();
    }
    private void RequestBanner()
    {

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7348650003716399/4455741290";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder()
    .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
    .AddTestDevice("C08886AC18E0187C9E2CFC90AB3C3129")  // My test device.
    .Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }
    private void Update()
    {
        if(cameraDesiredLookAt != null)
        {
            cameraTransform.rotation = Quaternion.Slerp (cameraTransform.rotation, cameraDesiredLookAt.rotation, cameraTransitionSpeed * Time.deltaTime);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private void LoadLevel (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LookAtMenu(Transform menuTransform)
    {
        cameraDesiredLookAt = menuTransform;
    }
}
