using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayers : MonoBehaviour
{
    public Transform camera;
    // Start is called before the first frame update
    void Update()
    {
        transform.LookAt(camera);
    }
    
}
