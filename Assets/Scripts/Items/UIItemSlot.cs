using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemSlot : MonoBehaviour
{
    public bool isCursor; // Lo usaremos para detectar objetos arrastrados
    
    public ItemSlot itemSlot;
    public RectTransform slotRect;

    public Image icon;
    public TextMeshProUGUI amount;
    public Image condition;

    private void Awake() // se crea aqui el objeto nuevo para que nunca pueda ser null
    {
        itemSlot = new ItemSlot();
    }

    private void Update()
    {
        if (!isCursor) return;

        transform.position = Input.mousePosition;
    }

    public void ClearSlot()
    {
        itemSlot = new ItemSlot();
        RefreshSlot();
    }

    public void RefreshSlot()
    {
        if (!isCursor)
            UpdateConditionBar(); 
        UpdateIcon();
        UpdateAmount();
    }
    private void UpdateConditionBar()
    {
        // Casos en los que no se usa la barra de duracion
        if (itemSlot == null || !itemSlot.hasItem || !itemSlot.item.isDegradable)
        {
            condition.enabled = false;
        }
        else
        {
            condition.enabled = true;

            // Obtenemos el tanto por uno de la durabilidad
            float conditionPercent = (float)itemSlot.condition / (float)itemSlot.item.maxDuration;

            // Multiplicamos el ancho de la barra maximo por el numero obtenido
            float barWidth = slotRect.rect.width * conditionPercent;

            // Una vez obtenida la asignamos
            condition.rectTransform.sizeDelta = new Vector2(barWidth, condition.rectTransform.sizeDelta.y);

            // Color de la durabilidad de blanco a rojo segun se vaya reduciendo
            condition.color = Color.Lerp(Color.white, Color.red, conditionPercent);
        }
    }

    private void UpdateIcon()
    {
        if (itemSlot == null || !itemSlot.hasItem)
        {
            icon.enabled = false;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = itemSlot.item.icon;
        }
    }

    private void UpdateAmount()
    {
        if (itemSlot == null || !itemSlot.hasItem || itemSlot.amount < 2) // Poniendo esta ultima restriccion no mostraremos la cantidad si solo tenemos una unidad
        {
            amount.enabled = false;
        }
        else
        {
            amount.enabled = true;
            amount.text = itemSlot.amount.ToString();
        }
    }
}
