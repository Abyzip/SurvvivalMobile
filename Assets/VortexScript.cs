using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexScript : MonoBehaviour
{
    public GameObject preFX;
    public GameObject FX;
    public GameObject player;
    public bool isMagnet = false;
    public int power=0;

    public AudioClip spawnVortex;
    public AudioClip vortex;
    // Start is called before the first frame update
    void OnEnable()
    {
        preFX.SetActive(true);
        FX.SetActive(false);
        isMagnet = false;
        StartCoroutine("vortexCoroutine");

    }

     void FixedUpdate()
    {
        if (isMagnet)
        {
            Vector3 direction = new Vector3(transform.position.x - player.transform.position.x, 0, transform.position.z - player.transform.position.z);
            direction=direction.normalized;
            float distance=Vector3.Distance(transform.position, player.transform.position);
            player.GetComponent<Rigidbody>().AddForce(direction/distance*power*20);
        }
    }
    IEnumerator vortexCoroutine()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(spawnVortex);
        yield return new WaitForSeconds(3);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(vortex);
        isMagnet = true;
        preFX.SetActive(false);
        FX.SetActive(true);
  
    }
}
