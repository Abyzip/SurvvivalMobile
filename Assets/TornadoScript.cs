using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoScript : MonoBehaviour
{
    public float speed = 10;
    public bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            transform.Translate(Vector3.forward * Time.deltaTime*speed);
    }

    public void Move()
    {
        isMoving = true;
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("OnTriggerEnter");
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetPhotonView().isMine)
            {
                col.gameObject.GetComponent<DeathPlayer>().Death();
            }
        }
    }
}
