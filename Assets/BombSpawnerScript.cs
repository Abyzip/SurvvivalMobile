using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawnerScript : MonoBehaviour
{
    public GameObject bomb;
    public float xSpeedMin, ySpeedMin, zSpeedMin, xSpeedMax, ySpeedMax, zSpeedMax, xSpeedTest, ySpeedTest, zSpeedTest;

    public bool devTest = false;
    public bool devTestAlea = false;

    // Start is called before the first frame update
    void Start()
    {
        //shot();
    }

    // Update is called once per frame
    void Update()
    {
        if (devTest)
        {
            if(Input.GetKeyDown("space"))
                shotTest();
        }
    }
    public Vector3 GenerateAleaForce()
    {
        Vector3 force = new Vector3(Random.Range(xSpeedMin, xSpeedMax), Random.Range(ySpeedMin, ySpeedMax), Random.Range(zSpeedMin, zSpeedMax));
        return force;

    }
    public void shot(Vector3 force)
    {
        GameObject bombInstance = Instantiate(bomb, transform.position, Quaternion.identity) as GameObject;
        bombInstance.GetComponent<Rigidbody>().AddForce(force);
    }

    public void shotTest()
    {
        GameObject bombInstance = Instantiate(bomb, transform.position, Quaternion.identity) as GameObject;

        bombInstance.GetComponent<Rigidbody>().AddForce(new Vector3(xSpeedTest, ySpeedTest, zSpeedTest));
        Destroy(bombInstance, 10f);
    }
}
