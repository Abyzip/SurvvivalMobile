using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject mainPlayer;
    // Start is called before the first frame update
    void Start()
    {
        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void spawnPlayer()
    {
        PhotonNetwork.Instantiate(mainPlayer.name, mainPlayer.transform.position, mainPlayer.transform.rotation, 0);
    }
}
