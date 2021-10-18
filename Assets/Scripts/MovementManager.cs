using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    // Movement
    public float velocidadMovimiento = 5f;
    public float gravedad = 0.05f;
    private CharacterController controller;

    // Mouse rotation 
    public Camera cam;

    // Walking
    public Animator animator;
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
        controller.Move(movimiento * Time.deltaTime * velocidadMovimiento);

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
        
    }
}
