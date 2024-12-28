using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private Transform fPCameraTransform;
    private Transform tDCameraTransform;


    private void Awake()
    {
        //Set mouse state
        Cursor.lockState = CursorLockMode.Locked;

        //Input
        gameInput = new GameInput();
        look = gameInput.Player.Look;
        topDownAbility = gameInput.Player.TopDownAbility;

        //Gameobjects and components
        Player = GameObject.Find("Player");

        //Get camera positions
        fPCameraTransform = Player.transform.Find("FPCameraLocation");
        tDCameraTransform = Player.transform.Find("TDCameraLocation");
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
        if (topDownAbility.WasPressedThisFrame() && !IsTopDown && !IsTransitionOngoing)
        {
            StartCoroutine(TransitionToTD());
        }

        if (topDownAbility.WasPressedThisFrame() && IsTopDown && !IsTransitionOngoing)
        {
            StartCoroutine(TransitionToFP());
        }


        if (IsTopDown && !IsTransitionOngoing)
        {
            CameraTopDownFollow();
        }
        
        if (!IsTopDown && !IsTransitionOngoing)
        {
            CameraFirsPersonFollow();
            CameraMovement();
        }

        Debug.Log(fPCameraTransform.transform.position);
        Debug.Log(fPCameraTransform.transform.rotation);

    }

    private void FixedUpdate()
    {
       

    }

    private void CameraMovement()
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

    private void CameraFirsPersonFollow()
    {
        transform.position = fPCameraTransform.position;
    }
    
    private void CameraTopDownFollow()
    {
        transform.position = tDCameraTransform.position;
    }

    private IEnumerator TransitionToTD()
    {

        float invertedTransitionDuration = (1f / transitionDuration);
        fPCameraTransform.rotation = transform.rotation;

        IsTransitionOngoing = true;
        float t = 0f;

        while (t < invertedTransitionDuration)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.position = Vector3.Lerp(transform.position, tDCameraTransform.position, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, tDCameraTransform.rotation, t);

            yield return null;
        }

        IsTransitionOngoing = false;
        IsTopDown = true;
    }

    private IEnumerator TransitionToFP()
    {
        float invertedTransitionDuration = (1f / transitionDuration);

        IsTransitionOngoing = true;
        float t = 0f;

        while (t < invertedTransitionDuration)
        {
            t += Time.deltaTime * (Time.timeScale/transitionDuration);

            transform.position = Vector3.Lerp(transform.position, fPCameraTransform.position, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, fPCameraTransform.rotation, t);

            yield return null;
        }

        IsTransitionOngoing = false;
        IsTopDown = false;
    }
}
