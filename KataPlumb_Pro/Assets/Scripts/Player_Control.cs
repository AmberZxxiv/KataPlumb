using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{

    private Rigidbody rb;
    public float movSpeed;
    private float movFrontal;

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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        // aqui tengo que hacer algo para que la linterna siga al cursor
    }

    private void FixedUpdate()
    {
        // si pulsamos el clic dch que avance por la alcantarilla
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //actualizamos los valores al transform cuando nos movemos
            Vector3 playerMovement = (transform.forward * movFrontal);
            Vector3 playerSpeed = new Vector3(playerMovement.x * movSpeed, rb.velocity.y, playerMovement.z * movSpeed);
            rb.velocity = playerSpeed;
        }
    }
}
