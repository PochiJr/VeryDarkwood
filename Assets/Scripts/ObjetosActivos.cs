using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// Este Script por ahora lo pondremos en el Canvas, pero si acabamos añadiendo
// cadaveres a los que lootear, por ejemplo, habria que ponerselo a ellos
public class ObjetosActivos : MonoBehaviour
{
    public GameObject parentWindow;
    public Transform contentWindow; // El GridLayoutGroup que estamos usando
    public TextMeshProUGUI title;

    public string containerName;
    GameObject slotPrefab;

    // El del video dice que esta lista la podemos guardar en cualquier 
    // otro lado, es el Model, si la queremos mostrar en otro lado
    // podriamos pasarla como parametro
    List<ItemSlot> items = new List<ItemSlot>();
    List<UIItemSlot> UISlots = new List<UIItemSlot>();

    private void Start()
    {
        slotPrefab = Resources.Load<GameObject>("Modelos3D/Prefabs/UIItemSlot");

        item objetoActivoInicial = new item();
        objetoActivoInicial = Resources.Load<item>("Items/Flashlight");
        items.Add(new ItemSlot(objetoActivoInicial.name, objetoActivoInicial.maxStack, objetoActivoInicial.maxDuration));

        parentWindow.SetActive(true);
        // Es el menú de inicio así que no queremos que tenga título porque queda feo
        // title.text = containerName.ToUpper();
        title.text = string.Empty;
        // Loopeamos cada objeto
        for (int i = 0; i < items.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentWindow);

            // Con esto podemos pasar el nombre de vuelta a entero y obtener
            // el ID de la posicion en la lista
            newSlot.name = i.ToString();

            UISlots.Add(newSlot.GetComponent<UIItemSlot>());
            items[i].AttachUI(UISlots[i]);
        }
    }

    private void Update()
    {
        
    }



    public void CloseContainer()
    {
        // Vamos a traves de cada slot y detach/eliminarlos
        foreach(UIItemSlot slot in UISlots)
        {
            // De esta forma los eliminamos de forma segura de la UI
            // sin borrar los datos
            slot.itemSlot.DettachUI();
            Destroy(slot.gameObject);
        }

        UISlots.Clear();
        parentWindow.SetActive(false);
    }
}
