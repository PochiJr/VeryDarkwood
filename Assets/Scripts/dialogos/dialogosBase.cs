using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class dialogosBase : MonoBehaviour
{
    public string nombreArchivo = "pruebaDialogos.txt";
    public bool panelActivo = false;

    public GameObject panel;
    public TextMeshProUGUI nombreUI;
    public TextMeshProUGUI dialogoUI;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(panelActivo);
        dialogoUI.text = "";
    }


    public void CreateFile()
    {
        string path = @"dialogos/" + nombreArchivo;
        dialogoUI.text = "";

        if (!File.Exists(path))
        {
            // Creamos el fichero en el que escribir el mensaje de error
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("Nombre:Error");
                sw.WriteLine("Dialogo no encontrado!");

            }
        }

        // Abruimos el fichero y leemos de el
        using (StreamReader sr = File.OpenText(path))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                if(s.StartsWith("Nombre:"))
                {
                    Debug.Log(s.Split(':')[1]);
                    nombreUI.text = s.Split(':')[1];
                }
                else
                {
                    Debug.Log(s);
                    dialogoUI.text += s;
                    dialogoUI.text += "\n";
                }

            }
        }

        panelActivo = false;
    }
}
