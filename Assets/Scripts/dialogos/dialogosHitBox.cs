using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogosHitBox : MonoBehaviour
{
    public string nombreArchivo = "pruebaDialogos.txt";

    public GameObject GestorDialogos;
    public GameObject panel;

    private float ttl = 3;
    private bool isActivado = false;
    // Start is called before the first frame update
    void Update()
    {
        if (isActivado)
        {
            ttl -= 0.005f;
            if (ttl <= 0)
            {
                Time.timeScale = 1;
                panel.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GestorDialogos.GetComponent<dialogosBase>().nombreArchivo = nombreArchivo;
            GestorDialogos.GetComponent<dialogosBase>().CreateFile();
            panel.SetActive(true);

            isActivado = true;
            Time.timeScale = 0;
        }
    }

}
