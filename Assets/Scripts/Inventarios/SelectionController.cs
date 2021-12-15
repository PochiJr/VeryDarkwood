using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public Vector3 escala = new Vector3(0, 0, 0);
    private Transform seleccion;
    public GameObject seleccionado;
    private GameObject outline;
    private bool objetoSeleccionadoCreado = false;

    // Control de textura del highlight
    public Material material;

    void Update()
    {
        if (seleccion != null)
        {
            if (objetoSeleccionadoCreado)
            {
                Destroy(outline);
                objetoSeleccionadoCreado = false;
            }
            seleccion = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;

            if (selection.tag == "Seleccionable")
            {
                seleccion = selection;
                // crear outline -->
                if (!objetoSeleccionadoCreado)
                {
                    outline = Instantiate<GameObject>(seleccionado);
                    outline.transform.position = seleccionado.transform.position;
                    outline.transform.rotation = seleccionado.transform.rotation;
                    outline.transform.localScale =  new Vector3(seleccionado.transform.localScale.x * escala.x,
                        seleccionado.transform.localScale.y * escala.y, seleccionado.transform.localScale.z * escala.z);

                    outline.GetComponent<MeshRenderer>().material = material;

                    objetoSeleccionadoCreado = true;
                }
            }
            
        } else
        {
            objetoSeleccionadoCreado = false;
        }

    }


}
