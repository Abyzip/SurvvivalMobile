using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public bool devTesting = false;

    public PhotonView photonView;

    public float moveSpeed = 10f;

    private Vector3 selfPos;

    private Quaternion selfRot;
    // Update is called once per frame
    void Update()
    {
        if (!devTesting) { 
            if (photonView.isMine)
                checkInput();
            else
                smoothNetMovement();
        } else
        {
            checkInput();
        }
    }

    private void checkInput()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += move * moveSpeed * Time.deltaTime;
    }
    private void smoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
        transform.rotation = Quaternion.Lerp(transform.rotation, selfRot, Time.deltaTime * 8);

    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
            selfRot = (Quaternion)stream.ReceiveNext();

        }
    }
}
