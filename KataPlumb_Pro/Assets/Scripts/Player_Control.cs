using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Control : MonoBehaviour
{
    // esto es para poder llamar a este código desde cualquier otro
    public static Player_Control instance;

    #region //// UI PANELS ////
    public GameObject exitMenu;
    public GameObject pauseMenu;
    public GameObject eatedMenu;
    public GameObject drownedMenu;
    #endregion

    #region //// PLAYER MOVEMENT ////
    private Rigidbody rb;
    public float movSpeed;
    private bool isMoving;
    #endregion

    #region //// CAM CONTROL ////
    public float mouseSensitivity;
    private float mouseRotation = 0f;
    public Transform cameraTransform;
    public Transform lanternTransform;
    #endregion

    #region //// ANIMATIONS ////
    private Animator animator;
    public GameObject manos;
    float timer = 0f;
    [SerializeField] bool scrolling = false;
    #endregion

    #region //// LINTERNA ////
    public Light linterna;
    public float bateria = 10f;
    public float duracion;
    public float masscroll = 0.2f;
    public float luzmax = 5f;
    public float luzmin = 0f;
    public float luzactual;
    #endregion

    #region //// SCORE ////
    public TextMeshProUGUI scoreTXT;
    public TextMeshProUGUI maxscore;
    private int _score;
    #endregion

    // sin este awake no genera su instancia y no la pillan las plumbs
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // pa la linterna
        animator = manos.GetComponent<Animator>();
        duracion = bateria;
        luzactual = luzmax;
    }

    // Update is called once per frame
    void Update()
    {
        #region //// CAM FIRST PERSON ////
        // cogemos el valor del cursor para poder darlo de vuelta
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);
        mouseRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseRotation = Mathf.Clamp(mouseRotation, -90f, 90f);
        // lo copiamos en la camara y la linterna, y lo bloqueamos en los polos
        cameraTransform.localRotation = Quaternion.Euler(mouseRotation, 0, 0);
        lanternTransform.localRotation = Quaternion.Euler(mouseRotation, 0, 0);
        #endregion

        if (Input.GetMouseButtonDown(1)) // mientras clic DCH
        {
            isMoving = true;
        }
        if (Input.GetMouseButtonUp(1)) // soltando clic DCH
        {
            isMoving = false;
        }
        if (isMoving) //avanzar adelante a velocidad constante
        {
            transform.Translate(Vector3.forward * movSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0)) // CLIC IZQUIERDO
        {
            #region //// LANZAR RAYCAST ////
            // lanzo el raycast desde la camara al centro que es donde esta el cursor locked
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            #endregion

            //en rango colisiono con plumb rota, llamo a cambio de estado y sumo puntos
            if (Physics.Raycast(ray, out hit, 10f) && hit.collider.CompareTag("plumbroke"))
            {
                Plumb_Controler plumToRepare = hit.collider.GetComponentInParent<Plumb_Controler>();
                plumToRepare.SwitchState();
                _score += 10;
                scoreTXT.text = "Earned: " + _score.ToString() + " $";
            }

            //dentro del rango colisiono con la verja de salida, activo FIN DE JORNADA
            if (Physics.Raycast(ray, out hit, 10f) && hit.collider.CompareTag("exitdoor"))
            {
                SetMaxScore();
                Time.timeScale = 0;
                exitMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        // pulsamos ESC para panel de PAUSA
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetMaxScore();
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // FUNCION DE LINTERNA
        FlashLightRecharge();
        // FUNCION DE LINTERNA
    }

    void FlashLightRecharge()
    {
        scrolling = Input.GetAxis("Mouse ScrollWheel") != 0 ? true : false; //scrolling es la rueda del raton y si lo esta heciendo es true y si no es false

        if (!scrolling) //si no esta scrolleando
        {
            timer += Time.deltaTime; // suma el timer
            if (timer > 0.2f) //si es mayor de 02
            {
                luzactual -= Time.deltaTime/3; // cuanto +, + lento baja la intensidad
                animator.SetBool("SACAR_LINTERNA", false); // quite la animacion
            }
        }
        else //que al hacer scroll se ponga la animacion y suba la intensidad con el timer a 0
        {
            animator.SetBool("SACAR_LINTERNA", true);
            luzactual += Time.deltaTime*3; // cuanto +, +rapido recargas
            timer = 0;
        }
        luzactual = Mathf.Clamp(luzactual, luzmin, luzmax); //pone la luz actual
        linterna.intensity = luzactual;
    }

    public void SetMaxScore() // recoge la puntuación, la actualiza y muestra
    {
        maxscore.text = "Employee of the Game: " + PlayerPrefs.GetInt("MaxScore").ToString() + " $";
        if (_score > PlayerPrefs.GetInt("MaxScore") || !PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", _score);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // si te choca el cocodrilo, activo el menu de comido
        if (other.CompareTag("crocodile"))
        {
            SetMaxScore();
            Time.timeScale = 0;
            eatedMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    } //pa chocar con el cocodrilo

    public void QuitPause() // quita el panel de pausa, reanuda el tiempo y quita el cursor
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void RestartGame() // recarga la escena de juego
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu() // carga la escena de menu inicial
    {
        SceneManager.LoadScene(0);
    }
}
