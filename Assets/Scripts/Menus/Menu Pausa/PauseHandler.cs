using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{
    public GameObject panel;
    private bool isPausado = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPausado)
        {
            panel.SetActive(true);
            Time.timeScale = 0;
            isPausado = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPausado)
        {
            panel.SetActive(false);
            Time.timeScale = 1;
            isPausado = false;
        }
    }

    public void Reanudar()
    {
        panel.SetActive(false);
        Time.timeScale = 1;
        isPausado = false;
    }
}
