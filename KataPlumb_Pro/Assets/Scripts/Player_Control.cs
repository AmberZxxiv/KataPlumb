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

    //animaciones
    private Animator animator;
    public GameObject manos;
    float timer = 0f;
    [SerializeField] bool scrolling = false;

    //linterna
    public Light linterna;
    public float bateria = 10f;
    public float duracion;
    public float masscroll = 0.2f;
    public float luzmax = 5f;
    public float luzmin = 0f;
    public float luzactual;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false ;

        animator = manos.GetComponent<Animator>();
        duracion = bateria;
        luzactual = luzmax;
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
            isMoving = true;
        }
        if (Input.GetMouseButtonUp(1)) // cuando suelto el clic DCH
        {
            isMoving = false;
        }
        if (isMoving) //moverse es avanzar adelante a velocidad constante
        {
            transform.Translate(Vector3.forward * movSpeed * Time.deltaTime);
        }


        FlashLightRecharge();
    }

    void FlashLightRecharge()
    {
        scrolling = Input.GetAxis("Mouse ScrollWheel") != 0 ? true : false; //scrolling es la rueda del raton y si lo esta heciendo es true y si no es false

        if (!scrolling) //si no esta scrolleando suma el timer y si es mayor de 02 baje la intensidad de la linterna y quite la animacion
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                luzactual -= Time.deltaTime/2;
                animator.SetBool("SACAR_LINTERNA", false);
            }
        }
        else //que al hacer scroll se ponga la animacion y suba la intensidad con el timer a 0
        {
            animator.SetBool("SACAR_LINTERNA", true);
            luzactual += Time.deltaTime*2;
            timer = 0;
        }
        luzactual = Mathf.Clamp(luzactual, luzmin, luzmax); //pone la luz actual
        linterna.intensity = luzactual;
    }
}
