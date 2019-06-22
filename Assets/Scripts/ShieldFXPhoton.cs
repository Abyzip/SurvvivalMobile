using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldFXPhoton : MonoBehaviour
{
    private Vector3 selfPos;
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
        }
    }
}
