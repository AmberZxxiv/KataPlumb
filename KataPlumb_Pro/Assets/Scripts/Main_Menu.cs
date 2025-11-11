using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_Menu : MonoBehaviour
{
    public Player_Control _PC;
    public TextMeshProUGUI maxscore;


    public Animator animator;
    public Animator animatorCAM;
    public Animator animatorEsca;
    public GameObject alcantarilla;
    public Camera Cam;
    public GameObject esca;
    public GameObject Canvas;
    public Button boton;
    public GameObject craga;
    public GameObject textos;
    public AudioSource entrance;
    public AudioSource streets;

    // Start is called before the first frame update
    void Start()
    {
        _PC = Player_Control.instance;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        maxscore.text = "Employee of the Game: " + PlayerPrefs.GetInt("MaxScore").ToString() + " $";

        animator = alcantarilla.GetComponent<Animator>();
        animatorCAM = Cam.GetComponent<Animator>();
        animatorEsca = esca.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame() // recarga la escena de juego
    {
        entrance.Play();
        animator.SetBool("JUGAR", true);
        animatorCAM.SetBool("JUGAR", true);
        animatorEsca.SetBool("JUGAR", true);
        maxscore.gameObject.SetActive(false);
        boton.gameObject.SetActive(false);
        streets.Stop();
        StartCoroutine(Espera());
    }

    public void ExitGame()
    {
        Debug.Log("Salgo del .exe");
        Application.Quit();
    }

    IEnumerator Espera() 
    {
        Debug.Log("ESPERA");
        yield return new WaitForSeconds(2);
        textos.SetActive(true);
        craga.SetActive(true);
        Debug.Log("espera a que carge impaciente");
        yield return new WaitForSeconds(5);
        Debug.Log("ya esta");
        SceneManager.LoadScene(1);
    }
}
