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

  public bool isRun = false;
  private bool isInteract = false;

  private Camera playerCamera; 
  private Vector2 currentLookInput;
  private float verticalRotation = 0f;

  // Start is called before the first frame update
  private void Start()
  {
    characterController = GetComponent<CharacterController>();
    playerInput = GetComponent<PlayerInput>();
    moveAction = playerInput.actions["Movement"];
    lookAction = playerInput.actions["Look"];
    runAction = playerInput.actions["Run"];
    interactAction = playerInput.actions["Interact"];

    playerCamera = GetComponentInChildren<Camera>();

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  /*
  public void OnMove(InputAction.CallbackContext context)
  {
    moveInput = context.ReadValue<Vector2>();
  }

  public void OnLook(InputAction.CallbackContext context)
  {
    lookInput = context.ReadValue<Vector2>();
  }

  public void OnRun(InputAction.CallbackContext context)
  {
    isRun = context.ReadValue<float>() > 0.5f;
  }
  public void OnInteract(InputAction.CallbackContext context)
  {
    isInteract = context.ReadValue<float>() > 0.5f;
  }
  */

  // Update is called once per frame
  private void Update()
  {
    HandleMovement();
    HandleLook();
    HandleInteract();
  }

  private void HandleMovement() 
  {
    Vector2 input = moveAction.ReadValue<Vector2>();
    isRun = runAction.ReadValue<float>() > 0.5f;
    Vector3 moveDirection = playerCamera.transform.right * input.x + playerCamera.transform.forward * input.y;
    if(isRun)
      characterController.Move(moveDirection * moveRun * Time.deltaTime);
    characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
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
    isInteract = interactAction.ReadValue<float>() > 0.5f;
  }
}
