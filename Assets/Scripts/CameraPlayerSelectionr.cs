using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerSelectionr : MonoBehaviour
{
    public int initialPos = 2;
    private string playerName;
    private GameObject playerNameGO;
    public GameObject lockImage;

    private GameObject pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = GameObject.Find("Pos" + (initialPos));
        playerNameGO = GameObject.FindGameObjectWithTag("name");
        playerName = pos.transform.GetChild(0).name;
        playerNameGO.transform.GetChild(0).name = playerName;
        transform.LookAt(pos.transform.GetChild(0));
        PlayerRight();
    }

    public void PlayerLeft()
    {
        pos.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        initialPos--;
        if (initialPos < 0)
            initialPos = 5;
        if (initialPos == 3)
            lockImage.SetActive(false);
        else
            lockImage.SetActive(true);
        pos = GameObject.Find("Pos" + initialPos);
        pos.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        playerName = pos.transform.GetChild(0).name;
        playerNameGO.transform.GetChild(0).name = playerName;
        transform.LookAt(pos.transform.GetChild(0));
    }

    public void PlayerRight()
    {
        pos.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        initialPos++;
        if (initialPos > 5)
            initialPos = 0;
        if (initialPos == 3)
            lockImage.SetActive(false);
        else
            lockImage.SetActive(true);
        pos = GameObject.Find("Pos" + initialPos);
        pos.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        playerName = pos.transform.GetChild(0).name;
        playerNameGO.transform.GetChild(0).name = playerName;
        transform.LookAt(pos.transform.GetChild(0));
    }
}
