using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// Este Script por ahora lo pondremos en el Canvas, pero si acabamos a?adiendo
// cadaveres a los que lootear, por ejemplo, habria que ponerselo a ellos
public class InventarioPrincipal : MonoBehaviour
{
    public GameObject parentWindow;
    public Transform contentWindow; // El GridLayoutGroup que estamos usando
    public TextMeshProUGUI title;

    public string containerName;
    GameObject slotPrefab;

    private bool isOpen = false;
    
    List<ItemSlot> items = new List<ItemSlot>();
    List<UIItemSlot> UISlots = new List<UIItemSlot>();

    private void Start()
    {
        slotPrefab = Resources.Load<GameObject>("Modelos3D/Prefabs/UIItemSlot");

        item[] tempItems = new item[2]; // Solo hay 2 tipos de objeto por ahora
        tempItems[0] = Resources.Load<item>("Items/HPpotion");
        tempItems[1] = Resources.Load<item>("Items/SPDpotion");

        for (int i = 0; i < 8; i++)
        {
            // Decidimos, de manera aleatoria, si a?adir un slot vacio o no
            if(Random.Range(0, 3) == 0) // 33% de ser vacio
            {
                items.Add(new ItemSlot());
            } else
            {
                int index = Random.Range(0, 2);
                int amount = Random.Range(1, tempItems[index].maxStack);
                int condition = tempItems[index].maxDuration;

                items.Add(new ItemSlot(tempItems[index].name, amount, condition));
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isOpen)
        {
            OpenContainer(items);
            isOpen = true;
        } else if (Input.GetKeyDown(KeyCode.Q) && isOpen)
        {
            CloseContainer();
            isOpen = false;
        }
            
    }


    public void OpenContainer(List<ItemSlot> slots)
    {
        parentWindow.SetActive(true);
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

