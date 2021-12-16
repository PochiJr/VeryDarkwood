using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementManager : MonoBehaviour
{
    // Movement
    public float velocidadMovimiento = 5f;
    public float bonusVelocidad = 1f;
    public float gravedad = 0.05f;
    private CharacterController controller;

    // Mouse rotation 
    public Camera cam;

    // Walking
    public Animator animator;

    // Combate
    public Image barraSalud;
    public Image barraSaludGradual;
    private float velocidadBarraSalud = 0.01f;
    private bool estaArmado = false;
    private bool heSidoAtacado = false;
    private float ayuda = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        Vector3 movimiento = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(movimiento * Time.deltaTime * velocidadMovimiento * bonusVelocidad);

        Vector3 velocity = new Vector3(0, gravedad, 0);
        controller.Move(velocity);

        // Rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 8; 
        Vector3 lookDir = cam.ScreenToWorldPoint(mousePos) - gameObject.transform.position;

        float angle = Mathf.Atan2(lookDir.z, lookDir.x) * Mathf.Rad2Deg -90f;
        gameObject.transform.rotation = Quaternion.Euler(0, -angle, 0);

        // Walking
        if( movimiento.x != 0f || movimiento.z != 0f)
        {
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
        }

        

        
        // Golpear
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("isAttacking", true);
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GS Slash") && !Input.GetMouseButtonDown(1))
        {
            animator.SetBool("isAttacking", false);
            velocidadMovimiento = 5.0f;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("GS Slash"))
        {
            velocidadMovimiento = 0.0f;
        }

        // Recibir Daño (lo hacemos tanto para Golpe Recibido (sin arma) como para GS Impact (con arma)
        if ((!animator.GetCurrentAnimatorStateInfo(0).IsName("Golpe Recibido") ||
            !animator.GetCurrentAnimatorStateInfo(0).IsName("GS Impact")) && heSidoAtacado)
        {
            ayuda -= Time.deltaTime;
            if (ayuda <= 0.0f)
            {
                animator.SetBool("isTakingDamage", false);
                heSidoAtacado = false;
                velocidadMovimiento = 5.0f;
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Golpe Recibido") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("GS Impact"))
        {
            velocidadMovimiento = 0.0f;
        }

        // Barra de salud secundaria
        if (barraSalud.fillAmount < barraSaludGradual.fillAmount)
        {
            barraSaludGradual.fillAmount -= Time.deltaTime;
        }

        /* 
          if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("isAttacking", true);
            ayuda = 2.0f;
        }
         if (animator.GetCurrentAnimatorStateInfo(0).IsName("GS Slash") && !Input.GetMouseButtonDown(1))
         {
             ayuda -= 0.5f * Time.deltaTime;
             Debug.Log(ayuda);
         }
         if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GS Slash") && !Input.GetMouseButtonDown(1))
        {
            animator.SetBool("isAttacking", false);
        }

         if (ayuda <= 0.0f && !Input.GetMouseButton(1))
         {
             animator.SetBool("isAttacking", false);
         }*/

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemigoArbol" || other.tag == "EnemigoArbusto") {

            // Reduccion de salud
            if (other.tag == "EnemigoArbol")
                barraSalud.fillAmount -= 0.3f;
            else if (other.tag == "EnemigoArbusto")
                barraSalud.fillAmount -= 0.1f;

            if (barraSalud.fillAmount <= 0.0f)
            {
                // Morir
                animator.SetBool("isDead", true);
            } else
            {

                animator.SetBool("isTakingDamage", true);
            }
            ayuda = 0.2f;
            heSidoAtacado = true;
        }
    }
}
