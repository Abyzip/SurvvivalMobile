using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAreaScript : MonoBehaviour
{
    public List<GameObject> playersInArea;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playersInArea.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playersInArea.Remove(col.gameObject);
        }
    }
}
