using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPrincipalController : MonoBehaviour
{
    public void comenzarJuego()
    {
        Initiate.Fade("SampleScene", Color.black, 1f);
    }

    public void terminarJuego()
    {
        Application.Quit();
    }
}
