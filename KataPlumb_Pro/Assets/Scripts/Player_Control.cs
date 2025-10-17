using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{

    private Rigidbody rb;
    public float movSpeed;
    private bool isMoving;

    //mover la camara con el ratón
    public float mouseSensitivity;
    private float mouseRotation = 0f;
    public Transform cameraTransform;
    public Transform lanternTransform;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false ;
    }

    // Update is called once per frame
    void Update()
    {
        // cogemos el valor del cursor para poder darlo de vuelta
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);
        mouseRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseRotation = Mathf.Clamp(mouseRotation, -90f, 90f);
        // lo copiamos en la camara y la linterna, y lo bloqueamos en los polos
        cameraTransform.localRotation = Quaternion.Euler(mouseRotation, 0, 0);
        lanternTransform.localRotation = Quaternion.Euler(mouseRotation, 0, 0);

        if (Input.GetMouseButtonDown(1)) // cuando esta pulsado el clic DCH
        {
            isMoving = true ;
        }
        if (Input.GetMouseButtonUp(1)) // cuando suelto el clic DCH
        {
            isMoving = false;
        }
        if (isMoving) //moverse es avanzar adelante a velocidad constante
        {
            transform.Translate(Vector3.forward * movSpeed * Time.deltaTime);
        }
    }
}
