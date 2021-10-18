using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasaController : MonoBehaviour
{
    public Transform enemigos;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach (Transform child in enemigos)
            {
                child.GetComponent<EnemyAI>().isPlayerAtHome = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (Transform child in enemigos)
            {
                child.GetComponent<EnemyAI>().isPlayerAtHome = false;
            }
        }
    }
}
