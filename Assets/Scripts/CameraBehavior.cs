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
    private bool isTopDown = false; //WIP

    //Input
    private GameInput gameInput;
    private InputAction look;

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

        //Gameobjects and components
        Player = GameObject.Find("Player");

        //Get camera positions
        fPCameraTransform = Player.transform.Find("FPCameraLocation");
        tDCameraTransform = Player.transform.Find("TDCameraLocation");
    }

    private void OnEnable()
    {
        look.Enable();
    }

    private void OnDisable()
    {
        look.Disable();
    }

    private void Update()
    {
        CameraMovement(); //Maybe move to FixedUpate or LateUpdate if not smooth
        


        if (isTopDown)
        {
            CameraTopDownFollow();
        }
        else
        {
            CameraFirsPersonFollow();
        }
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

}
