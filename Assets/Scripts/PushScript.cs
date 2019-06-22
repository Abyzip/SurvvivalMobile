using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class PushScript : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData ped)
    {
        //PhotonNetwork.playerList.
        //transform.position = Vector3.zero;
    }

    public void OnPointerUp(PointerEventData ped)
    {
        transform.GetChild(14).gameObject.SetActive(false);
    }
}
