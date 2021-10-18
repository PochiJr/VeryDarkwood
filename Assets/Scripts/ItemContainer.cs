using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// Este Script por ahora lo pondremos en el Canvas, pero si acabamos añadiendo
// cadaveres a los que lootear, por ejemplo, habria que ponerselo a ellos
public class ItemContainer : MonoBehaviour
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
   
        item[] tempItems = new item[3]; // Solo hay 3 tipos de objeto por ahora
        tempItems[0] = Resources.Load<item>("Items/Sword");
        tempItems[1] = Resources.Load<item>("Items/Coin");
        tempItems[2] = Resources.Load<item>("Items/Ring");

        for (int i = 0; i < 10; i++)
        {
            int index = Random.Range(0, 3);
            int amount = Random.Range(1, tempItems[index].maxStack);
            int condition = tempItems[index].maxDuration;

            items.Add(new ItemSlot(tempItems[index].name, amount, condition));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseContainer();
        else if (Input.GetKeyDown(KeyCode.Q))
            OpenContainer(items);
    }


    public void OpenContainer(List<ItemSlot> slots)
    {
        parentWindow.SetActive(true);
        title.text = containerName.ToUpper();

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
