using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public Player_Control _PC;
    public TextMeshProUGUI maxscore;


    public Animator animator;
    public Animator animatorCAM;
    public GameObject alcantarilla;
    public Camera Cam;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame() // recarga la escena de juego
    {
        animator.SetBool("JUGAR", true);
        animatorCAM.SetBool("JUGAR", true);
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("Salgo del .exe");
        Application.Quit();
    }
}
