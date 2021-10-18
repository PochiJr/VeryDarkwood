using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    // El objeto almacenado en el slot
    public item item;

    // Controlamos el valor maximo de cantidad
    private int _amount;
    public int amount
    {
        get { return _amount; }
        set {
            if (item == null) _amount = 0;
            else if (value > item.maxStack) _amount = item.maxStack;
            else if (value < 1) _amount = 0;
            else _amount = value;

            RefreshUISlot();
        }
    }
    
    // Condicion del objeto, esto se controla pues objetos iguales pero que han sido utilizados
    // no se stackean con los otros, tambien controlamos posibles excepciones aqui
    private int _condition;
    public int condition
    {
        get { return _condition; }
        set
        {
            if (item == null) _condition = 0;
            else if (value > item.maxDuration) _condition = item.maxDuration; // Si la durabilidad pasada como argumento es mayor de la posible, usamos ese maximo como cap
            else _condition = value;
            RefreshUISlot();
        }
    }

    private UIItemSlot uIItemSlot;
    public void AttachUI(UIItemSlot uiSlot) { 
        uIItemSlot = uiSlot;
        uIItemSlot.itemSlot = this;
        RefreshUISlot();
    }

    public void DettachUI() { 
        
        uIItemSlot.ClearSlot();
        uIItemSlot = null;

    }
    public bool isAttachedToUI { get { return (uIItemSlot != null); } }
    public void RefreshUISlot()
    {
        if (!isAttachedToUI)
        {
            return;
        }
        uIItemSlot.RefreshSlot();
    }


    public static bool Compare (ItemSlot slotA, ItemSlot slotB)
    {
        // Comparamos dos objetos y devolvemos false si no son del mismo tipo
        // o si tienen diferente durabilidad aun siendo el mismo tipo de objeto
        if (slotA.item != slotB.item || slotA.condition != slotB.condition){
            return false;
        }
        return true;
    }

    public static void Swap(ItemSlot slotA, ItemSlot slotB)
    {
        // Cacheamos los valores del objeto del slotA
        item _item = slotA.item;
        int _amount = slotA.amount;
        int _condition = slotA.condition;

        // Copiamos los valores del slotB en el slotA
        slotA.item = slotB.item;
        slotA.amount = slotB.amount;
        slotA.condition = slotB.condition;

        // Y a slotB los valores previos de slotA
        slotB.item = _item;
        slotB.amount = _amount;
        slotB.condition = _condition;

        slotA.RefreshUISlot();
        slotB.RefreshUISlot();
    }

    public void Clear() {
        item = null;
        amount = 0;
        condition = 0;
        RefreshUISlot();
    }
    // Comprobador de si el slot esta libre
    public bool hasItem { get { return (item != null); } }

    // Constructor. Los valores asignados en los args de la funcion son por defecto y, por tanto
    // no pasa nada si no se pasan a la funcion, se convierten en opcionales.
    public ItemSlot(string itemName, int _amount = 1, int _condition = 0)
    {
        item _item = FindByName(itemName);
        if (_item == null) // Se pone todo a null o a 0, segun corresponda
        {
            item = null;
            amount = 0;
            condition = 0;
            return;
        } else // Ponemos a los parametros de ItemSlot los valores pasados a la 
                // funcion y el objeto encontrado
        {
            item = _item;
            amount = _amount;
            condition = _condition; 
        }
    }

    // Constructor por defecto
    public ItemSlot ()
    {
        item = null;
        amount = 0;
        condition = 0;
    }

    // Buscador de objetos
    private item FindByName(string itemName)
    {
        itemName = itemName.ToLower();
        item _item = Resources.Load<item>(string.Format("Items/{0}", itemName));

        if (_item == null) // Lanzamos un Warning si no cargó un objeto
            Debug.LogWarning(string.Format("No se pudo encontrar el objeto \"{0}\". El slot esta vacio", itemName));

        return _item;
    }

}
