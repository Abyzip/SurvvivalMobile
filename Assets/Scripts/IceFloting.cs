using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFloting : MonoBehaviour
{
    private bool floatup;
    void Start()
    {
        int random = Random.Range(0, 2);
        if(random == 1)
        {
            floatup = true;
        }
        else
        {
            floatup = false;
        }
    }

    void Update()
    {
        if (floatup)
            StartCoroutine(floatingup());
        else if (!floatup)
            StartCoroutine(floatingdown());
    }

    IEnumerator floatingup()
    {
        Vector3 p = transform.position;
        p.y += 0.15f * Time.deltaTime;
        transform.position = p;
        yield return new WaitForSeconds(1);
        floatup = false;
    }

    IEnumerator floatingdown()
    {
        Vector3 p = transform.position;
        p.y -= 0.15f * Time.deltaTime;
        transform.position = p;
        yield return new WaitForSeconds(1);
        floatup = true;
    }
}
