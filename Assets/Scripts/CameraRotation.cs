using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraRotation : MonoBehaviour
{
    // Start is called before the first frame update

    public InputActionAsset cameraActions;
    InputActionMap cameraActionMap;
    InputAction rotateCameraAction;
    float rotationAngle;


    public float speed = 1;
    Vector3 movementVector;

    void Awake()
    {
        cameraActionMap = cameraActions.FindActionMap("Player");
        rotateCameraAction = cameraActionMap.FindAction("Test");

        rotateCameraAction.performed += GetInputAction;
        rotateCameraAction.canceled += GetInputAction;
    }

    private void OnLook(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector2>();

    }

    private void GetInputAction(InputAction.CallbackContext context)
    {
        rotationAngle = context.ReadValue<float>();
        Debug.Log(context);
       

    }
    private void Enable()
    {
        rotateCameraAction.Enable();
    }

    private void Disable()
    {
        rotateCameraAction.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        //rb.AddForce(movement * speed);


        transform.Rotate(movementVector.y*-0.5f, movementVector.x*0.5f , rotationAngle);
    }
}
