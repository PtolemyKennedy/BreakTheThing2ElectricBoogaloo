using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour
{

    public GameManager _GameManager;
    public GameObject cameraRotator;
    public Camera mainCamera;

    Vector2 touchPosition;

    /// <summary>
    /// is the player in first person mode
    /// </summary>
    public bool isFirstPerson = false;
    /// <summary>
    /// is the player currently placing an explosive
    /// </summary>
    public bool isPlacingExplosive = false;
    public string explosiveToPlace = "";


    /// <summary>
    /// the rotation speed of the camera
    /// </summary>
    float rotateSpeed = 1.8f;

    /// <summary>
    /// the height change speed of the camera
    /// </summary>
    float heightSpeed = 0.12f;

    /// <summary>
    /// is the player currently zooming in/out
    /// </summary>
    bool isZooming = false;

    /// <summary>
    /// initial pinch length for zooming
    /// </summary>
    float startPinchLength = 0;

    /// <summary>
    /// change in pinch length for zooming
    /// </summary>
    float deltaPinch = 0;

    /// <summary>
    /// the size of the pinch deadzone for zooming
    /// </summary>
    public float pinchDeadzone = 50;

    /// <summary>
    /// minimum zoom distance, uses local position of camera
    /// </summary>
    float minZoom = -5;

    /// <summary>
    /// maximum zoom distance, uses local position of camera
    /// </summary>
    float maxZoom = -50;

    /// <summary>
    /// speed to zoom in/out
    /// </summary>
    float zoomSpeed = 0.3f;

    //Use this for initialization

    void Start()
    {
        _GameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update ()
    {
        //check if the player is placing an explosive
        if (isPlacingExplosive)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                if (/*Input.GetMouseButtonUp(0)*/ Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    //find the position that was clicked
                    Vector3 pos = Vector3.zero;

                    Ray ray = Camera.main.GetComponent<Camera>().ScreenPointToRay(/*Input.GetTouch(0).position*/Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        pos = hit.point;
                    }

                    //instantiate an explosive where clicked
                    Instantiate(Resources.Load(explosiveToPlace), pos, Quaternion.identity);

                    //add 1 to number of bombs used
                    switch (explosiveToPlace)
                    {
                        case "smallExplosive":
                            _GameManager.explosivesUsed[0]++;
                            break;
                        case "mediumExplosive":
                            _GameManager.explosivesUsed[1]++;
                            break;
                        case "largeExplosive":
                            _GameManager.explosivesUsed[2]++;
                            break;
                        case "implosive":
                            _GameManager.explosivesUsed[3]++;

                            break;
                        default:
                            break;
                    }

                    //check to see if the player is over the maximum explosives
                    _GameManager.CheckExplosivesUsed();

                    //no longer placing explosive
                    isPlacingExplosive = false;
                }
            }
        }
        else
        {
            if (!isFirstPerson)
            {
                //rotation and height control
                if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.touchCount == 1)
                {
                    //rotate camera
                    if (touchPosition.x < Input.GetTouch(0).position.x)
                    {
                        //rotate the camera left around the center
                        cameraRotator.GetComponent<Transform>().Rotate(new Vector3(0, rotateSpeed, 0));
                    }
                    else if (touchPosition.x > Input.GetTouch(0).position.x)
                    {
                        //rotate the camera right around the center
                        cameraRotator.GetComponent<Transform>().Rotate(new Vector3(0, -rotateSpeed, 0));
                    }

                    //move camera up and down
                    if (touchPosition.y < Input.GetTouch(0).position.y)
                    {
                        //move the camera up to a limit
                        if (cameraRotator.GetComponent<Transform>().position.y <= 15)
                        {
                            cameraRotator.GetComponent<Transform>().Translate(new Vector3(0, heightSpeed, 0));
                        }
                    }
                    else if (touchPosition.y > Input.GetTouch(0).position.y)
                    {
                        //move the camera down to a limit
                        if (cameraRotator.GetComponent<Transform>().position.y >= 2)
                        {
                            cameraRotator.GetComponent<Transform>().Translate(new Vector3(0, -heightSpeed, 0));
                        }
                    }

                    touchPosition = Input.GetTouch(0).position;
                }
            }
            else //use first person controls
            {
                //mainCamera.transform.rotation = gyro.attitude * rot;

                //float z = Input.acceleration.z;
                //float y = Input.acceleration.y;

                //mainCamera.transform.Rotate(0, y * rotSpeed, z * rotSpeed);

                mainCamera.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, -Input.gyro.rotationRateUnbiased.z);
            }

            //zoom and pinch control
            if (Input.touchCount == 2)
            {
                //check if currently zooming in/out
                if (!isZooming)
                {
                    //if not record the starting pinch length
                    startPinchLength = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    isZooming = true;
                }
                else
                {
                    float pinchLength;
                    pinchLength = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

                    //determine the change in pinch length from the starting pinch length
                    deltaPinch = pinchLength - startPinchLength;

                    //if the change is greater than the deadzone attempt to zoom in/out
                    if (deltaPinch < -pinchDeadzone)
                    {
                        //zoom out
                        if (mainCamera.GetComponent<Transform>().localPosition.z > maxZoom)
                        {
                            mainCamera.GetComponent<Transform>().Translate(new Vector3(0, 0, -zoomSpeed));
                        }
                    }
                    else if (deltaPinch > pinchDeadzone)
                    {
                        //zoom in
                        if (mainCamera.GetComponent<Transform>().localPosition.z < minZoom)
                        {
                            mainCamera.GetComponent<Transform>().Translate(new Vector3(0, 0, zoomSpeed));
                        }
                    }
                }
            }

            //no fingers touching so we stop zooming
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isZooming = false;
            }
        }
    }
}
