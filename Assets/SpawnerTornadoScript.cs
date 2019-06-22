using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTornadoScript : MonoBehaviour
{
    public GameObject[] spawners;
    public float rate;
    public GameObject tornado;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine("tempete");
    }

    

    IEnumerator tempete()
    {
       yield return new WaitForSeconds(2);
        int i = 0;
        float compteurSeconde = 0;

        while (compteurSeconde<9)
        {
            if (i == spawners.Length)
                i = 0;
            GameObject currentTornado=Instantiate(tornado, spawners[i].transform.position, spawners[i].transform.rotation) as GameObject;
            currentTornado.GetComponent<TornadoScript>().Move();
            Destroy(currentTornado, 3);
            yield return new WaitForSeconds(rate);
            i++;
            compteurSeconde = compteurSeconde + rate;
        }
        compteurSeconde = 0;
        while (compteurSeconde < 9)
        {
            if (i == -1)
                i = spawners.Length-1;
            GameObject currentTornado = Instantiate(tornado, spawners[i].transform.position, spawners[i].transform.rotation) as GameObject;
            currentTornado.GetComponent<TornadoScript>().Move();
            Destroy(currentTornado, 3);
            yield return new WaitForSeconds(rate);
            i--;
            compteurSeconde = compteurSeconde + rate;
        }

    }
}
