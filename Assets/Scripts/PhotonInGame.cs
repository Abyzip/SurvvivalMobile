using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInGame : MonoBehaviour
{
    private void OnDisconnectedFromPhoton()
    { 
        Debug.Log("Disconnect from the game");
    }
}
