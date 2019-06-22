using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlayer : MonoBehaviour
{

    //public GameObject spawner;
    //public GameObject shield;

    //public float timeRespawn=5;
    //public float timeShield=3;

    /*
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "deathTag")
        {
            Death();
        }
    }*/

    Animator anim;
    int die = Animator.StringToHash("Fly Take Damage");
    public float xMin, xMax, zMin, zMax;
    
    private float timerBerorerespawn = 5.0f;
    private float timerShield = 5.0f;

    public bool isInvinsible = false;
    public AudioClip deathClip;

    private moteurJeu moteur;

    void Start()
    {
        Debug.Log("START");
        moteur = GameObject.FindGameObjectWithTag("moteurJeu").GetComponent<moteurJeu>();
        xMin = moteur.xMinFxShield;
        xMax = moteur.xMaxFxShield;
        zMin = moteur.zMinFxShield;
        zMax = moteur.zMaxFxShield;
        anim = GetComponent<Animator>();
    }

    public void Death()
    {
        if(!isInvinsible)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(deathClip);
            anim.SetBool(die, true);
            GetComponent<PlayControllerJoystick>().isDying = true;
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1000, 1000), 1000, Random.Range(-1000, 1000)));
            GetComponent<BoxCollider>().enabled = false;
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(7).GetComponent<ParticleSystem>().Play();
            moteur.incrementDeath(gameObject.name);
            StartCoroutine(RespawnCoroutine());
        }
    }

    public void VortexDeath()
    {
        if (!isInvinsible)
        {
            gameObject.transform.SetPositionAndRotation(new Vector3(Random.Range(xMin, xMax), -20f, Random.Range(zMin, zMax)), Quaternion.identity);
            GetComponent<BoxCollider>().enabled = false;
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(7).GetComponent<ParticleSystem>().Play();
            moteur.incrementDeath(gameObject.name);
            StartCoroutine(RespawnCoroutine());
        }
    }

    IEnumerator RespawnCoroutine()
    {
        isInvinsible = true;
        GameObject spellPush = GameObject.FindGameObjectWithTag("Push");
        GameObject spellTeleport = GameObject.FindGameObjectWithTag("Teleport");
        spellPush.GetComponent<CanvasGroup>().blocksRaycasts = false;
        spellTeleport.GetComponent<CanvasGroup>().blocksRaycasts = false;
        yield return new WaitForSeconds(timerBerorerespawn);
        GetComponent<BoxCollider>().enabled = true;
        transform.GetChild(3).gameObject.SetActive(false);
        GetComponent<PlayControllerJoystick>().isDying = false;
        gameObject.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.transform.SetPositionAndRotation(new Vector3(Random.Range(xMin, xMax), 0.5f, Random.Range(zMin, zMax)), Quaternion.identity);
        spellPush.GetComponent<CanvasGroup>().blocksRaycasts = true;
        spellTeleport.GetComponent<CanvasGroup>().blocksRaycasts = true;
        StartCoroutine(ShieldCoroutine());
    }
    
    IEnumerator ShieldCoroutine()
    {
        moteur.activeShield(gameObject.name, true);
        yield return new WaitForSeconds(timerShield);
        moteur.activeShield(gameObject.name, false);
        isInvinsible = false;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //stream.SendNext(transform.GetChild(0).GetComponent<ParticleSystem>().main.startColor.colorMin);
        }
        else
        {
            gameObject.transform.position = (Vector3)stream.ReceiveNext();
            gameObject.transform.rotation = (Quaternion)stream.ReceiveNext();
            //Aura = (Transform)stream.ReceiveNext();
        }
    }
}
