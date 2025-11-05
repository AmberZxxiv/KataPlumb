using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plumb_Controler : MonoBehaviour
{
    public bool isBroken = false;
    public GameObject childRepared;
    public GameObject childBroken;
    public System_Manager _SM;
    public Player_Control _PC;

    void Start()
    {
        // aqui pillo las referencias de scripts
        _PC = Player_Control.instance;
        _SM = System_Manager.instance;
        if (_SM != null)
        {
            _SM.CountPlumb(this);
        }

        // por alguna razon con el if funciona pero sino no. OK :/
        if (transform.childCount >= 2)
        {
            // cojo los hijos para los estados de la plumb
            childRepared = transform.GetChild(0).gameObject;
            childBroken = transform.GetChild(1).gameObject;
        }
    }

    public void SwitchState()
    {
        if (childBroken != null && childRepared != null)
        {
            // tiene que estar en este orden o sino no funciona jaja saludos
            isBroken = !isBroken;
            childBroken.SetActive(isBroken);
            childRepared.SetActive(!isBroken);
        }
    }
}
