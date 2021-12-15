using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class item : ScriptableObject
{
    public string itemName;

    [TextArea] // Esto hace que a la hora de asignar el valor en Unity salga como una caja enorme en vez de un prompt chikito
    public string itemDescription;

    public Sprite icon; // Icono que aparecera en el inventario

    public int maxStack; // Cantidad maxima de stackeo que tendra el objeto
    public int maxDuration; // Durabilidad maxima del objeto, si es infinito sera de -1

    // Con estas funciones comprobamos de manera rapida si es stackeable y degradable
    public bool isStackable { get { return (maxStack > 1); } }
    public bool isDegradable { get { return (maxDuration > -1); } }
}
