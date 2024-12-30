using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraBehavior : MonoBehaviour
{

    //View behaviour values
    [SerializeField] float lookSensitivity = 20f;
    [SerializeField] float cameraLerpSpeed = 30f;
    private float maxLookAngle = 60f;
    private Vector3 eulerAngles;
    private Vector3 smoothEulerAngles;

    //TopDown values
    public bool IsTopDown = false;
    public bool IsTransitionOngoing = false;
    private float transitionDuration = 3f;
    
    //Input
    private GameInput gameInput;
    private InputAction look;
    private InputAction topDownAbility;

    //Gameobjects and components
    private GameObject Player;
    private PlayerBehavior playerBehavior;
    private Transform fPCameraTransform;
    private Transform tDCameraTransform;

    //Crosshair-objects
    [SerializeField] Texture2D cursorTD;
    [SerializeField] Material crosshairMaterial;
    private Vector2 cursorPivot;


    private void Awake()
    {
        //Set mouse state
        Cursor.lockState = CursorLockMode.Locked;

        //Input
        gameInput = new GameInput();
        look = gameInput.Player.Look;
        topDownAbility = gameInput.Player.TopDownAbility;

        //Gameobjects and components
        Player = GameObject.FindWithTag("Player");
        playerBehavior = Player.GetComponent<PlayerBehavior>();

        //Get camera positions
        fPCameraTransform = Player.transform.Find("FPCameraLocation");
        tDCameraTransform = Player.transform.Find("TDCameraLocation");

        //Set cursor properties
        Cursor.lockState = CursorLockMode.Locked;
        cursorPivot = new Vector2(cursorTD.width / 2, cursorTD.height / 2);
        Cursor.SetCursor(cursorTD, cursorPivot, CursorMode.Auto);

    }

    private void OnEnable()
    {
        look.Enable();
        topDownAbility.Enable();
    }

    private void OnDisable()
    {
        look.Disable();
        topDownAbility.Disable();
    }

    private void Update()
    {
        //Transition handling
        if (topDownAbility.WasPressedThisFrame() && !IsTopDown && !IsTransitionOngoing)
        {
            StartCoroutine(TransitionToTD());
        }
        else if (topDownAbility.WasPressedThisFrame() && IsTopDown && !IsTransitionOngoing)
        {
            StartCoroutine(TransitionToFP());
        }

        //Camera follow and movement handling
        if (IsTopDown && !IsTransitionOngoing)
        {
            CameraTopDownFollow();
        }
        else if (!IsTopDown && !IsTransitionOngoing)
        {
            //CameraFirsPersonFollow();
            FPCameraMovement();
        }

        //Debug.Log(fPCameraTransform.transform.position);
        //Debug.Log(fPCameraTransform.transform.rotation);

    }

    private void LateUpdate()
    {
        if (!IsTopDown && !IsTransitionOngoing)
        {
            CameraFirsPersonFollow();
        }
    }

    private void FixedUpdate()
    {
       

    }


    /// <summary>
    /// Camera movement in first-person view
    /// </summary>
    private void FPCameraMovement()
    {
        //Get input
        eulerAngles.x -= look.ReadValue<Vector2>().y * lookSensitivity * Time.deltaTime;
        eulerAngles.y += look.ReadValue<Vector2>().x * lookSensitivity * Time.deltaTime;

        //Clamp x input
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, -maxLookAngle, maxLookAngle);

        //smoothen input
        smoothEulerAngles.x = Mathf.Lerp(smoothEulerAngles.x, eulerAngles.x, cameraLerpSpeed * Time.deltaTime);
        smoothEulerAngles.y = Mathf.Lerp(smoothEulerAngles.y, eulerAngles.y, cameraLerpSpeed * Time.deltaTime);

        //Rotate
        transform.eulerAngles = eulerAngles;

    }

    /// <summary>
    /// Follow player in first-person
    /// </summary>
    private void CameraFirsPersonFollow()
    {
        transform.position = fPCameraTransform.position;
    }

    /// <summary>
    /// Follow player in top-down view
    /// </summary>
    private void CameraTopDownFollow()
    {
        transform.position = tDCameraTransform.position;
    }


    /// <summary>
    /// Transitions the player camera to top-down view
    /// </summary>
    /// <returns></returns>
    private IEnumerator TransitionToTD()
    {
        //Crosshair invisible
        crosshairMaterial.color = Color.clear;
           
        float invertedTransitionDuration = (1f / transitionDuration); //to make the transition end faster after the camera is already in it's right place
        fPCameraTransform.rotation = transform.rotation; //save rotation of where player looked before transition -> for more smoothness

        IsTransitionOngoing = true;
        float t = 0f;

        //Transition
        while (t < invertedTransitionDuration)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.position = Vector3.Lerp(transform.position, tDCameraTransform.position, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, tDCameraTransform.rotation, t);

            yield return null;
        }

        IsTransitionOngoing = false;
        IsTopDown = true;

        //Cursor visable
        Cursor.lockState = CursorLockMode.Confined;
        
    }

    /// <summary>
    /// Transitions the player camera to first-person view
    /// </summary>
    /// <returns></returns>
    private IEnumerator TransitionToFP()
    {
        //Cursor invisible
        Cursor.lockState = CursorLockMode.Locked;

        float invertedTransitionDuration = (1f / transitionDuration); //to make the transition end faster after the camera is already in it's right place

        IsTransitionOngoing = true;
        float t = 0f;

        //Transition
        while (t < invertedTransitionDuration)
        {
            t += Time.deltaTime * (Time.timeScale/transitionDuration);

            transform.position = Vector3.Lerp(transform.position, fPCameraTransform.position, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, fPCameraTransform.rotation, t);

            yield return null;
        }

        IsTransitionOngoing = false;
        IsTopDown = false;

        //Crosshair visible
        crosshairMaterial.color = Color.white;
    }
}