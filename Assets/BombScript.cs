using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombScript : MonoBehaviour
{

    public GameObject AOETimer;
    public GameObject AOE;

    public GameObject particle1;
    public GameObject particle2;

    public bool isActivate = false;
    public float timeToReachTarget = 180f;
    public float distToGround;
    float t;

    public GameObject explosionFX;

    public Transform camera;
    public AudioClip deathClip;
    private GameObject[] playerInArea;

    public bool firstGrounded = false;
    // Start is called before the first frame update
    void Start()
    {
        float distToGround = GetComponent<Collider>().bounds.extents.y;
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        /*AOE.transform.LookAt(new Vector3(0,0,1));
        AOETimer.transform.LookAt(new Vector3(0, 0, 1));*/
        if (IsGrounded() && !firstGrounded)
        {
            firstGrounded = true;
            StartCoroutine(activationBomb());

        }

        if (isActivate)
        {
            t += Time.deltaTime / timeToReachTarget;
            AOETimer.GetComponent<Image>().fillAmount = Mathf.Lerp(0.0f, 100.0f, t);
            GetComponent<Renderer>().material.color= Color.Lerp(Color.black, Color.red, Mathf.PingPong(Time.time*2, 0.5f));
            transform.localScale = Vector3.Lerp(new Vector3(6, 6, 6), new Vector3(8, 8, 8), Mathf.PingPong(Time.time * 2, 0.5f));
            if (t > 0.01f)
                explode();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            explode();
            if (col.gameObject.GetPhotonView().isMine)
            {
                col.gameObject.GetComponent<DeathPlayer>().Death();
            }
            
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
    
    private void explode()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(deathClip);
        GameObject explosionInstance = Instantiate(explosionFX,transform.parent.position,Quaternion.identity) as GameObject;
        Destroy(explosionInstance, 2f);
        foreach(GameObject player in transform.parent.GetComponent<BombAreaScript>().playersInArea)
        {
            player.transform.GetComponent<DeathPlayer>().Death();
        }
        Destroy(transform.parent.gameObject);
    }

    IEnumerator activationBomb()
    {
        
        yield return new WaitForSeconds(1);
        AOE.SetActive(true);
        particle1.GetComponent<ParticleSystem>().Play();
        particle2.GetComponent<ParticleSystem>().Play();
        isActivate = true;
    }
}

