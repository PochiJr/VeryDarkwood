using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;


// Este Script por ahora lo pondremos en el Canvas, pero si acabamos añadiendo
// cadaveres a los que lootear, por ejemplo, habria que ponerselo a ellos
public class Loot : MonoBehaviour
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

    // Deteccion de objeto looteable
    GraphicRaycaster raycaster;
    PointerEventData pointer;
    EventSystem eventSystem;

    private bool isAbierto = false;

    private void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        // Se podria hacer publico pero peresa
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>(); ;
    }

    private void Start()
    {
        slotPrefab = Resources.Load<GameObject>("Modelos3D/Prefabs/UIItemSlot");

        item[] tempItems = new item[2]; // Solo hay 2 tipos de objeto por ahora
        tempItems[0] = Resources.Load<item>("Items/HPpotion");
        tempItems[1] = Resources.Load<item>("Items/SPDpotion");

        for (int i = 0; i < 8; i++)
        {
            // Decidimos, de manera aleatoria, si añadir un slot vacio o no
            if (UnityEngine.Random.Range(0, 3) == 0) // 33% de ser vacio
            {
                items.Add(new ItemSlot());
            }
            else
            {
                int index = UnityEngine.Random.Range(0, 2);
                int amount = UnityEngine.Random.Range(1, tempItems[index].maxStack);
                int condition = tempItems[index].maxDuration;

                items.Add(new ItemSlot(tempItems[index].name, amount, condition));
            }
        }
    }

    // A diferencia de "InventarioPrincipal" aqui queremos llamar a OpenContainer
    // cuando el raycast detecte un cofre
    private void Update()        
    {
        if (Input.GetMouseButtonDown(0) )
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform;
                if (selection.tag == "Seleccionable")
                {
                    OpenContainer(items);
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }


    public void OpenContainer(List<ItemSlot> slots)
    {
        parentWindow.SetActive(true);
        // Es el menú de inicio así que no queremos que tenga título porque queda feo
        // title.text = containerName.ToUpper();
        title.text = string.Empty;
        // Loopeamos cada objeto
        for (int i = 0; i < slots.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentWindow);

            // Con esto podemos pasar el nombre de vuelta a entero y obtener
            // el ID de la posicion en la lista
            newSlot.name = i.ToString();

            UISlots.Add(newSlot.GetComponent<UIItemSlot>());
            slots[i].AttachUI(UISlots[i]);
        }
    }

    public void CloseContainer()
    {
        // Vamos a traves de cada slot y detach/eliminarlos
        foreach (UIItemSlot slot in UISlots)
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

