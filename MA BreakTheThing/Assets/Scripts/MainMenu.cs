using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

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
