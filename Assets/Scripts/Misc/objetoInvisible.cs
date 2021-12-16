using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objetoInvisible : MonoBehaviour
{
    public bool debug = false;
    private void Start()
    {
        if(!debug)
            this.GetComponent<MeshRenderer>().enabled = false;

    }
}
