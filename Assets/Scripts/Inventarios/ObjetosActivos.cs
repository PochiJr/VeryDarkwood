using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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


    // Gestion del objeto activo
    public GameObject linterna;
    public AudioSource sonidoLinterna;
    public GameObject arma;
    public Animator animator;
    public ParticleSystem particleSystemHP;
    public ParticleSystem particleSystemSPD;
    public Image barraSalud;
    public Image barraSaludGradual;
    public GameObject player;
    private float SPDTimer = 60f;
    private float velocidadMovimientoTemp;
    private bool isEmpocionado = false;
    private int slotIndex;


    private void Start()
    {
        slotPrefab = Resources.Load<GameObject>("Modelos3D/Prefabs/UIItemSlot");
        player = GameObject.Find("Player");

        item objetoActivoInicial = new item();
        objetoActivoInicial = Resources.Load<item>("Items/Flashlight");
        items.Add(new ItemSlot(objetoActivoInicial.name, objetoActivoInicial.maxStack, objetoActivoInicial.maxDuration));
        item objetoActivoInicial2 = new item();
        objetoActivoInicial2 = Resources.Load<item>("Items/WoodPlank");
        items.Add(new ItemSlot(objetoActivoInicial2.name, objetoActivoInicial2.maxStack, objetoActivoInicial2.maxDuration));
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
            newSlot.name = (i+1).ToString();

            UISlots.Add(newSlot.GetComponent<UIItemSlot>());
            items[i].AttachUI(UISlots[i]);
        }
        // Creamos el segundo hueco el cual estara vacio
        /*GameObject newSlotVacio = Instantiate(slotPrefab, contentWindow);
        ItemSlot objetoVacio = new ItemSlot();
        newSlotVacio.name = 2.ToString();
        UISlots.Add(newSlotVacio.GetComponent<UIItemSlot>());
        objetoVacio.AttachUI(UISlots[1]);*/
       
    }

    private void Update()
    {
        // Caso objetos activos vacios
        if (!UISlots[0].itemSlot.hasItem && !UISlots[1].itemSlot.hasItem && linterna.GetComponent<Light>().enabled)
            linterna.GetComponent<Light>().enabled = false;
        
        if (UISlots[0].itemSlot.hasItem)
        {
            // Caso primer objeto con contenido y segundo objeto vacio
            if (UISlots[0].itemSlot.item.itemName != "Flashlight" && !UISlots[1].itemSlot.hasItem && linterna.GetComponent<Light>().enabled)
                linterna.GetComponent<Light>().enabled = false;
            if (UISlots[1].itemSlot.hasItem)
            {
                // Caso ambos objetos con contenido
                if(UISlots[0].itemSlot.item.itemName != "Flashlight" && UISlots[1].itemSlot.item.itemName != "Flashlight" && linterna.GetComponent<Light>().enabled)
                    linterna.GetComponent<Light>().enabled = false;
            }
        }
        if (UISlots[1].itemSlot.hasItem)
        {
            // Caso primer objeto vacio y segundo con contenido
            if (!UISlots[0].itemSlot.hasItem  && UISlots[1].itemSlot.item.name != "Flashlight" && linterna.GetComponent<Light>().enabled)
                linterna.GetComponent<Light>().enabled = false;
        }



        // Hacemos lo mismo con el palo
        if (!UISlots[0].itemSlot.hasItem && !UISlots[1].itemSlot.hasItem && animator.GetBool("weaponChange"))
        {
            animator.SetBool("weaponChange", false);
            arma.GetComponent<MeshRenderer>().enabled = false;
        }

        if (UISlots[0].itemSlot.hasItem)
        {
            if (UISlots[0].itemSlot.item.itemName != "WoodPlank" && !UISlots[1].itemSlot.hasItem && animator.GetBool("weaponChange"))
            {
                animator.SetBool("weaponChange", false);
                arma.GetComponent<MeshRenderer>().enabled = false;
            }
            if (UISlots[1].itemSlot.hasItem)
            {
                if (UISlots[0].itemSlot.item.itemName != "WoodPlank" && UISlots[1].itemSlot.item.itemName != "WoodPlank" && animator.GetBool("weaponChange"))
                {
                    animator.SetBool("weaponChange", false);
                    arma.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        if (UISlots[1].itemSlot.hasItem)
        {
            if (!UISlots[0].itemSlot.hasItem && UISlots[1].itemSlot.item.name != "WoodPlank" && animator.GetBool("weaponChange"))
            {
                animator.SetBool("weaponChange", false);
                arma.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        // Acciones a realizar en funcion del objeto activo
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            //En función de si el usuario ha pulsado 1 o 2 accedemos a ese objeto activo, para ello guardamos la tecla pulsada en una variable indice
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                slotIndex = 0;
            } else
            {
                slotIndex = 1;
            }

            if (UISlots[slotIndex].itemSlot.hasItem)
            {
                switch (UISlots[slotIndex].itemSlot.item.itemName)
                {
                    case "Flashlight":
                        if (linterna.GetComponent<Light>().enabled)
                        {
                            linterna.GetComponent<Light>().enabled = false;
                            sonidoLinterna.Play();
                        }
                        else
                        {
                            linterna.GetComponent<Light>().enabled = true;
                            sonidoLinterna.Play();
                        }
                        break;
                    case "WoodPlank":
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Andar")) 
                        {
                            animator.SetBool("weaponChange", true);
                            arma.GetComponent<MeshRenderer>().enabled = true;

                        } else if (animator.GetCurrentAnimatorStateInfo(0).IsName("GS Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("GS Walk"))
                        {
                            animator.SetBool("weaponChange", false);
                            arma.GetComponent<MeshRenderer>().enabled = false;
                        }
                        break;
                    case "HPpotion":
                        // hacer que cure en cualquier caso:
                        parentWindow.GetComponent<AudioSource>().Play();
                        barraSalud.fillAmount += 0.2f;
                        barraSaludGradual.fillAmount += 0.2f;
                        // 
                        if (UISlots[slotIndex].itemSlot.amount == 1)
                        {
                            UISlots[slotIndex].itemSlot.Clear();
                            particleSystemHP.Play();
                        }
                        else
                        {
                            UISlots[slotIndex].itemSlot.amount -= 1;
                            UISlots[slotIndex].itemSlot.RefreshUISlot();
                            particleSystemHP.Play();
                        }
                        break;
                    case "SPDpotion":
                        // Hacer que de velocidad en cualquier caso
                        parentWindow.GetComponent<AudioSource>().Play();
                        velocidadMovimientoTemp = player.GetComponent<MovementManager>().bonusVelocidad;
                        Debug.Log(velocidadMovimientoTemp);
                        player.GetComponent<MovementManager>().bonusVelocidad = 2f;
                        isEmpocionado = true;

                        SPDTimer = 60.0f;
                        
                        //
                        if (UISlots[slotIndex].itemSlot.amount == 1)
                        {
                            UISlots[slotIndex].itemSlot.Clear();
                            particleSystemSPD.Play();
                        }
                        else
                        {
                            UISlots[slotIndex].itemSlot.amount -= 1;
                            UISlots[slotIndex].itemSlot.RefreshUISlot();
                            particleSystemSPD.Play();
                        }

                        break;
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if (isEmpocionado)
        {
            SPDTimer -= Time.fixedDeltaTime;
            if (SPDTimer <= 0)
            {
                player.GetComponent<MovementManager>().bonusVelocidad = velocidadMovimientoTemp;
                isEmpocionado = false;
            }
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
