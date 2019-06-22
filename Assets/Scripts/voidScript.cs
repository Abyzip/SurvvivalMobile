using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voidScript : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("OnTriggerEnter");
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<DeathPlayer>().isInvinsible)
                col.gameObject.GetComponent<DeathPlayer>().isInvinsible = false;
            col.gameObject.GetComponent<DeathPlayer>().Death();
        }
    }
}
