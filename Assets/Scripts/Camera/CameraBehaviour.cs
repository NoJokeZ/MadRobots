using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBehaviour : MonoBehaviour
{

    //View behaviour values
    private float lookSensitivity = 15f;
    private float cameraLerpSpeed = 30f;
    private float maxLookAngle = 60f;
    private Vector3 eulerAngles;
    private Vector3 smoothEulerAngles;
    private float topDownFollowSpeed = 10f;

    //TopDown values
    public bool IsTopDown { get; private set; } = false;
    public bool IsTransitionOngoing { get; private set; } = false;
    private float transitionDuration = 3f;

    //Input
    private GameInput gameInput;
    private InputAction look;
    private InputAction topDownAbility;

    //Gameobjects and components
    private GameObject player;
    private bool isPlayerAlive = false;
    private Transform fPCameraTransform;
    private Transform tDCameraTransform;

    //Crosshair-objects
    [SerializeField] Texture2D cursorTD;
    [SerializeField] Material crosshairMaterial;
    private Vector2 cursorPivot;


    private void Awake()
    {
        //Input
        gameInput = new GameInput();
        look = gameInput.Player.Look;
        topDownAbility = gameInput.Player.TopDownAbility;

        //Gameobjects and components
        GetPlayerObjects();

        //Set cursor properties
        //cursorDefault = Cursor.
        Cursor.lockState = CursorLockMode.Locked;
        cursorPivot = new Vector2(cursorTD.width / 2, cursorTD.height / 2);
        crosshairMaterial.color = Color.white;

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
        if (isPlayerAlive)
        {
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
        }
        else
        {
            GetPlayerObjects();
        }

    }

    private void LateUpdate()
    {
        if (isPlayerAlive)
        {
            if (!IsTopDown && !IsTransitionOngoing)
            {
                CameraFirstPersonFollow();
            }
        }
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
    private void CameraFirstPersonFollow()
    {
        if (fPCameraTransform != null) transform.position = fPCameraTransform.position;
    }

    /// <summary>
    /// Follow player in top-down view
    /// </summary>
    private void CameraTopDownFollow()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        mousePosition = ray.origin;

        Vector3 newCameraPosition = Vector3.MoveTowards(transform.position, mousePosition, topDownFollowSpeed);
        newCameraPosition.y = tDCameraTransform.position.y;
        transform.position = newCameraPosition;
    }

    /// <summary>
    /// Transitions the player camera to top-down view
    /// </summary>
    /// <returns></returns>
    public IEnumerator TransitionToTD()
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
        Cursor.SetCursor(cursorTD, cursorPivot, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// Transitions the player camera to first-person view
    /// </summary>
    /// <returns></returns>
    public IEnumerator TransitionToFP()
    {
        //Cursor invisible
        Cursor.lockState = CursorLockMode.Locked;

        float invertedTransitionDuration = (1f / transitionDuration); //to make the transition end faster after the camera is already in it's right place

        IsTransitionOngoing = true;
        float t = 0f;

        //Transition
        while (t < invertedTransitionDuration)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.position = Vector3.Lerp(transform.position, fPCameraTransform.position, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, fPCameraTransform.rotation, t);

            yield return null;
        }

        IsTransitionOngoing = false;
        IsTopDown = false;

        //Crosshair visible
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        crosshairMaterial.color = Color.white;
    }

    public void ResetOnPlayerDeath()
    {
        IsTopDown = false;
        IsTransitionOngoing = false;
        isPlayerAlive = false;
    }

    private void GetPlayerObjects()
    {
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            fPCameraTransform = player.transform.Find("UpperBody/FPCameraLocation");
            tDCameraTransform = player.transform.Find("TDCameraLocation");

            isPlayerAlive = true;
        }
        else
        {
            isPlayerAlive = false;
        }

    }
}
