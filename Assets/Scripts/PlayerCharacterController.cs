using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveRun = 7f;
    [SerializeField] private float rotationSpeed = 2f;

    [Header("Camera Settings")]
    [SerializeField] private float lookSensitivity = 1f;
    [SerializeField] private float maxLookAngle = 90f;

    private CharacterController characterController;

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction runAction;
    private InputAction interactAction;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;



    private Camera playerCamera;
    private Vector2 currentLookInput;
    private float verticalRotation = 0f;

    [SerializeField]  private GameObject[] doorKeys = new GameObject[3];
    [SerializeField] private Transform raycastInit;
    RaycastHit hit;
    [SerializeField] Animator anim;
    
    //Door and keys
    private GameObject flashlight;
    private GameObject doorCollided;
    private GameObject doorKey;
    private int doorNumber =-1;
    private int doorKeyNumber =-1;
    public void Init()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Movement"];
        lookAction = playerInput.actions["Look"];
        runAction = playerInput.actions["Run"];
        interactAction = playerInput.actions["Interact"];

        playerCamera = GetComponentInChildren<Camera>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        Init();
      
    }

  

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        if (!GameManager.Instance.hasDead) HandleLook();
        HandleInteract();
    }

    private void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        GameManager.Instance.isDashing = runAction.ReadValue<float>() > 0.5f;
        Vector3 moveDirection = playerCamera.transform.right * input.x + playerCamera.transform.forward * input.y;

        if (!GameManager.Instance.isDashing)
        {
            GameManager.Instance.isSwimming = true;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            GameManager.Instance.ChangeDecreaseRatio(1);
        }
        else if (GameManager.Instance.isDashing)
        {
            GameManager.Instance.isSwimming = false;
            characterController.Move(moveDirection * moveRun * Time.deltaTime);
            GameManager.Instance.ChangeDecreaseRatio(5);
        }

    }

    private void HandleLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>() * lookSensitivity;

        transform.Rotate(Vector3.up * lookInput.x * rotationSpeed);

        verticalRotation -= lookInput.y * rotationSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleInteract()
    {
        if (Physics.SphereCast(raycastInit.position, 0.5f, transform.TransformDirection(Vector3.forward), out hit, 1f))
        {
            Debug.DrawRay(raycastInit.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            if (hit.collider.CompareTag(Constants.INTERACTABLE_TAG) && interactAction.ReadValue<float>() > 0.5f)
            {
                if (hit.collider.gameObject.name.Equals(Constants.FLASHLIGHT))
                {
                    SetTorch();
                }
                else
                    DoorKeys(hit.collider.gameObject);
            }
            if (hit.collider.CompareTag(Constants.DOOR_TAG) && interactAction.ReadValue<float>() > 0.5f) {
                Door();
            }
            
        }


      
    }

    private void SetTorch()
    {
        flashlight = hit.collider.gameObject;
        flashlight.transform.parent = playerCamera.gameObject.transform.GetChild(0).transform;
        flashlight.transform.localPosition = Vector3.zero - new Vector3(0, 0, 0.175f);
        flashlight.transform.localRotation = Quaternion.identity;
        flashlight.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
    }

    private void Door() {
        doorCollided = hit.collider.gameObject;

        if (doorCollided != null)
        {
            if (doorCollided.name.Equals("Exit")) { } else {
                int.TryParse(doorCollided.name, out doorNumber);
                if (doorNumber != -1)
                {
                    CheckDoor(doorNumber);
                    doorNumber = -1;
                }
            }
           
        }
               doorCollided.GetComponent<DoorSystem>().PlaySound();
    }
    private void DoorKeys (GameObject item) {
        if (item != null)
        {
            if (item.name.Equals("exitItem")) doorKeys[2] = item;
            else {
                int.TryParse(item.name, out doorKeyNumber);
                if (doorKeyNumber != -1)
                {
                    doorKeys[doorKeyNumber] = item;
                }
            }
           
        }
    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.RESTPLACE_TAG))
        {
            GameManager.Instance.isResting = true;
            GameManager.Instance.isSwimming = false;
            GameManager.Instance.isDashing = false;
            GameManager.Instance.hasDead = false;
        }
        if (other.gameObject.CompareTag(Constants.ROOM_TAG)) { 
            GameManager.Instance.isInRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.RESTPLACE_TAG))
        {
            GameManager.Instance.isResting = false;
            GameManager.Instance.isSwimming = true;
            GameManager.Instance.isDashing = false;
            GameManager.Instance.hasDead = false;
        }
        if (other.gameObject.CompareTag(Constants.ROOM_TAG))
        {
            GameManager.Instance.isInRoom = false;
        }
    }
    private void CheckDoor(int door){
        if (doorKeys[door] != null) {

            GameManager.Instance.doors[door].transform.GetChild(1).Rotate(0, 0, 150f);
        }
    
    }
}
