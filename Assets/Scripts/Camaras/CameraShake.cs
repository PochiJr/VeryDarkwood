using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    // Animator para ver cuando nos golpean y poder agitar en ese momento
    public Animator animator;

    // Variables del agitado
    public float duracion = 0.3f;
    public float amplitud = 10f;
    public float frecuencia = 2.0f;

    private float elapsedTime = 0.0f;
    private float tiempoLimite = 0.3f;

    // Obtenemos el agitado de Cinemachine de nuestra camara virtual
    public CinemachineVirtualCamera vc;
    private CinemachineBasicMultiChannelPerlin vcNoise;

    void Start()
    {
        // Obtenemos el perfil de ruido de la camara virtual (6D)
        if (vc != null)
        {
            vcNoise = vc.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }

    }

    void Update()
    {
        // Agitaremos la camara cuando se nos golpee
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Golpe Recibido") || animator.GetCurrentAnimatorStateInfo(0).IsName("GS Impact"))
        {
            tiempoLimite -= Time.deltaTime;
            
            if (tiempoLimite >= 0f)
            {
                elapsedTime = duracion;
            }
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Golpe Recibido") && !animator.GetCurrentAnimatorStateInfo(0).IsName("GS Impact"))
        {
            tiempoLimite = 0.3f;
        }

        if (vc != null || vcNoise != null)
        {
            if (elapsedTime > 0)
            {
                // Asignamos los parametros de agitado a nuestra vc
                vcNoise.m_AmplitudeGain = amplitud;
                vcNoise.m_FrequencyGain = frecuencia;

                elapsedTime -= Time.deltaTime;
            } else
            {
                // Si se ha acabado el efecto de agitado, reseteamos las variables
                vcNoise.m_AmplitudeGain = 0f;
                elapsedTime = 0f;
            }
        }
    }
}
