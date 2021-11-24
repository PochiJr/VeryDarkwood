using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{

    public GameObject brazo;

    // Gestion de Hitbox
    void enableHitbox() { brazo.GetComponent<BoxCollider>().enabled = true; }
    void disableHitbox() { brazo.GetComponent<BoxCollider>().enabled = false; }
}
