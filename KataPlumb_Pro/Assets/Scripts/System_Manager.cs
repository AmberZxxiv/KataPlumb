using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class System_Manager : MonoBehaviour
{
    // esto es para poder llamar a este código desde cualquier otro
    public static System_Manager instance;

    // lista en la que contamos los elementos con el Plumb_Controler
    public List<Plumb_Controler> plumbs = new List<Plumb_Controler>();

    // marcamos las probabilidades para que se rompan
    float timeToBreak = 1f;
    float probabilityToBreak = 0.9f;

    // por alguna razon sin este awake no genera la instancia y/o no lo pillan las plumbs
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
    private void Start()
    {
        StartCoroutine("BreakRandomPlumb");
    }

    // recibo la plumb que se quiere unir y compruebo que no esté ya en la lista
    public void CountPlumb (Plumb_Controler newPlumb)
    {
        if (!plumbs.Contains(newPlumb))
        {
            plumbs.Add(newPlumb);
        }
    }

    IEnumerator BreakRandomPlumb()
    {
        yield return new WaitForSeconds(timeToBreak);

        // elijo una plumb random de la lista
        Plumb_Controler plumToBreak = plumbs[Random.Range(0,plumbs.Count)];
        // genero un numero aleatorio y si es válido doy paso a romperla
        bool canWeBreak = Random.Range(0,1) < probabilityToBreak ? true : false;
        // si esta plumb no está rota ya, la rompo
        if (!plumToBreak.isBroken && canWeBreak)
        {
            plumToBreak.SwitchState();
        }

        StartCoroutine(BreakRandomPlumb());
    }
}
