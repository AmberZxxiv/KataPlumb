using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    public NavMeshAgent NavMeshAgent;
    public int actualTarget;
    float playerSpeed;
    public Transform[] targets;
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
    public AudioSource playerSounds;
    public AudioSource backroundSounds;
    public AudioClip dinamo;
    public AudioClip repair;
    public AudioClip mordisco;

    #endregion

    #region //// SCORE ////
    public TextMeshProUGUI scoreTXT;
    public TextMeshProUGUI maxscore;
    private int _score;
    public GameObject water;
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
        backroundSounds.Play();
        rb = GetComponent<Rigidbody>();
        playerSpeed = NavMeshAgent.speed;
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

        #region //// MOV AUTO PLAYER ////
        //le digo que vaya al target actual
        NavMeshAgent.SetDestination(targets[actualTarget].position); 
        // si esta dentro del rango del target, cambia al siguiente
        if (Vector3.Distance(this.transform.position, targets[actualTarget].transform.position)<=1f)
        {
            NextTarget();
        }
        if (Input.GetMouseButton(1)) // mientras clic DCH velocidad
        {
            NavMeshAgent.speed = playerSpeed;
        }
        else // si NO clic DCH, no velocidad
        {
            NavMeshAgent.speed = 0;
        }
        #endregion

        if (Input.GetMouseButtonDown(0)) // CLIC IZQUIERDO
        {
            #region //// LANZAR RAYCAST ////
            // lanzo el raycast desde la camara al centro que es donde esta el cursor locked
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            #endregion

            //en rango colisiono con plumb rota, llamo al switch, sumo puntos y resto agua
            if (Physics.Raycast(ray, out hit, 10f) && hit.collider.CompareTag("plumbroke"))
            {
                playerSounds.clip = repair;
                playerSounds.Play();
                Plumb_Controler plumToRepare = hit.collider.GetComponentInParent<Plumb_Controler>();
                plumToRepare.SwitchState();
                water.transform.position += new Vector3(0, -10, 0) * Time.deltaTime;
                _score += 10;
                scoreTXT.text = "Earned: " + _score.ToString() + " $";
            }

            //dentro del rango colisiono con la verja de salida, activo FIN DE JORNADA
            if (Physics.Raycast(ray, out hit, 10f) && hit.collider.CompareTag("exitdoor"))
            {
                backroundSounds.Stop();
                SetMaxScore();
                Time.timeScale = 0;
                exitMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        // si el agua llega al límite, activamos la muerte por ahogamiento
        if (water.transform.position.y >= 10.35f)
        {
            backroundSounds.Stop();
            maxscore.text = "Employee of the Game: " + PlayerPrefs.GetInt("MaxScore").ToString() + " $";
            Time.timeScale = 0;
            drownedMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // pulsamos ESC para panel de PAUSA
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backroundSounds.Stop();
            maxscore.text = "Employee of the Game: " + PlayerPrefs.GetInt("MaxScore").ToString() + " $";
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
        scrolling = Input.GetAxis("Mouse ScrollWheel") != 0 ? true : false; //scrolling es la rueda del raton y si lo esta haciendo es true y si no es false

        if (!scrolling) //si no esta scrolleando
        {
            timer += Time.deltaTime; // suma el timer
            if (timer > 0.2f) //si es mayor de 02
            {
                luzactual -= Time.deltaTime/5; // cuanto +, + lento baja la intensidad
                animator.SetBool("SACAR_LINTERNA", false); // quite la animacion
            }
        }
        else //que al hacer scroll se ponga la animacion y suba la intensidad con el timer a 0
        {
            playerSounds.clip = dinamo;
            playerSounds.Play();
            animator.SetBool("SACAR_LINTERNA", true);
            luzactual += Time.deltaTime*10; // cuanto +, +rapido recargas
            timer = 0;
        }
        luzactual = Mathf.Clamp(luzactual, luzmin, luzmax); //pone la luz actual
        linterna.intensity = luzactual;
    }

    public void SetMaxScore() // recoge la puntuación, la actualiza y muestra
    {
        if (_score > PlayerPrefs.GetInt("MaxScore") || !PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", _score);
        }
        maxscore.text = "Employee of the Game: " + PlayerPrefs.GetInt("MaxScore").ToString() + " $";
    }

    void NextTarget()
    {
        actualTarget++;
        // si el target es mayor que el máximo de la lista, vuelve al 0
        if (actualTarget > targets.Length - 1)
        {
            actualTarget = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // si te choca el cocodrilo, activo el menu de comido
        if (other.CompareTag("crocodile"))
        {
            backroundSounds.Stop();
            playerSounds.clip = mordisco;
            playerSounds.Play();
            maxscore.text = "Employee of the Game: " + PlayerPrefs.GetInt("MaxScore").ToString() + " $";
            eatedMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    } 

    public void QuitPause() // quita el panel de pausa, reanuda el tiempo y quita el cursor
    {
        backroundSounds.Play();
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
