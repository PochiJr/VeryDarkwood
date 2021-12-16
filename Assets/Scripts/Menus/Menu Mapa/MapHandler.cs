using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public GameObject panel;
    private bool isSacado = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isSacado)
        {
            panel.SetActive(true);
            Time.timeScale = 0;
            isSacado = true;
        }
        else if (Input.GetKeyDown(KeyCode.M) && isSacado)
        {
            panel.SetActive(false);
            Time.timeScale = 1;
            isSacado = false;
        }
    }
}
