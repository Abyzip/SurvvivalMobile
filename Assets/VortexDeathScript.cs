using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexDeathScript : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("OnTriggerEnter");
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<DeathPlayer>().VortexDeath();
        }
    }
}
