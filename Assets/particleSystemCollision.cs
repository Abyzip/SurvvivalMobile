using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemCollision : MonoBehaviour
{
    public float power;

    private moteurJeu moteur;

    void Start()
    {
        moteur = GameObject.FindGameObjectWithTag("moteurJeu").GetComponent<moteurJeu>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Vector3 direction = gameObject.transform.parent.forward * 600;
            power = GameObject.FindGameObjectWithTag("moteurJeu").GetComponent<moteurJeu>().stackingPower;
            moteur.activePushParticle(other.gameObject.name, direction, gameObject.transform.parent.parent.gameObject.GetPhotonView().owner.NickName, power);
        }
    }
}
