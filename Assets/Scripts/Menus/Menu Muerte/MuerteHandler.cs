using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MuerteHandler : MonoBehaviour
{
    public Image barraSalud;
    public GameObject panel;
    private Image fondo;

    private float lerp = 0f;
    private void Start()
    {
        fondo = panel.GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        if (barraSalud.fillAmount == 0)
        {
            lerp += 0.004f;
            var tempColor = fondo.color;
            tempColor.a = Mathf.Lerp(0f, 1f, lerp); 
            fondo.color = tempColor;
            if (lerp >= 1.1)
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
}
