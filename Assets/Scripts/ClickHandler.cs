using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClickHandler : MonoBehaviour
{
    GraphicRaycaster raycaster;
    PointerEventData pointer;
    EventSystem eventSystem;

    public UIItemSlot cursor;

    private void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        // Se podria hacer publico pero peresa
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>(); ;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Preparamos un pointer event en la posicion del mouse
            pointer = new PointerEventData(eventSystem);
            pointer.position = Input.mousePosition;

            // Creamos una lista para recoger los resultados del Raycast
            List<RaycastResult> results = new List<RaycastResult>();

            // Lanzamos el raycast desde el puntero y pasamos los resultados a la lista
            raycaster.Raycast(pointer, results);

            Debug.Log(results[0].gameObject.tag);
            if (results.Count > 0 && results[0].gameObject.tag == "UIItemSlot")
            {
                
                ProcessClick(results[0].gameObject.GetComponent<UIItemSlot>());
            }
        }
    }

    private void ProcessClick(UIItemSlot clicked)
    {
        // Atrapamos posibilidades null por si acaso 
        if (clicked == null)
        {
            Debug.Log("Elemento de la interfaz con tag UIItemSlot no tiene un componente UIItemSlot asociado.");
            return;
        }

       // Si los slots son diferentes simplemente los swapeamos
       if(!ItemSlot.Compare(cursor.itemSlot, clicked.itemSlot))
        {
            ItemSlot.Swap(cursor.itemSlot, clicked.itemSlot);
            cursor.RefreshSlot();
            return;
        } else
        { // Si los slots son el mismo tipo
           
            if (!cursor.itemSlot.hasItem)
                return;

            // Da igual usar cursor que click pues sabemos a partir de aqui
            // que ambos tienen un objeto y que ademas son del mismo tipo
            if (!cursor.itemSlot.item.isStackable)
                return;

            // Esto significa que si el slot clickeado tiene ya el maximo
            // no podemos Mergear objetos del mismo tipo
            if (clicked.itemSlot.amount == clicked.itemSlot.item.maxStack)
                return;

            // si no, sumamos las cantidades hasta alcanzar el maximo, dejando
            // el resto en el cursor
            int total = cursor.itemSlot.amount + clicked.itemSlot.amount;
            int maxStack = cursor.itemSlot.item.maxStack;

            // En este caso juntamos todo en el slot seleccionado
            if (total <= maxStack)
            {
                clicked.itemSlot.amount = total;
                cursor.itemSlot.Clear();
            } else
            {
                clicked.itemSlot.amount = maxStack;
                cursor.itemSlot.amount = total - maxStack;
            }
            cursor.RefreshSlot();
        }


    }
}
