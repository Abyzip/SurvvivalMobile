using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photonConnect : MonoBehaviour
{
	public string versionName= "0.1";

    public GameObject MenuWindow, serverWindow, sectionView3, levelWindow, storeWindow, playerSelectorRight, playerSelectorLeft, storeButton;
    public GameObject Lock;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);
    }

    public void OnPressBackToMenu()
    {
        MenuWindow.SetActive(true);
        playerSelectorRight.SetActive(true);
        playerSelectorLeft.SetActive(true);
        storeButton.SetActive(true);
        serverWindow.SetActive(false);
    }

    public void OnPressBackToMenuFromLevel()
    {
        MenuWindow.SetActive(true);
        playerSelectorRight.SetActive(true);
        playerSelectorLeft.SetActive(true);
        storeButton.SetActive(true);
        levelWindow.SetActive(false);
    }

    public void OnPressStore()
    {
        playerSelectorRight.SetActive(false);
        playerSelectorLeft.SetActive(false);
        storeButton.SetActive(false);
        storeWindow.SetActive(true);
    }

    public void OnPressBackStore()
    {
        playerSelectorRight.SetActive(true);
        playerSelectorLeft.SetActive(true);
        storeButton.SetActive(true);
        storeWindow.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ConnectToPhoton() {
		//PhotonNetwork.ConnectUsingSettings (versionName);
		Debug.Log ("Connecting to photon...");
	}
    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected to master !");
    }

    private void OnJoinedLobby()
    {
        /*sectionView1.SetActive(false);
        sectionView2.SetActive(true);*/

        Debug.Log("Connected to master !");

    }

    private void OnDisconnectedFromPhoton()
    {
        if (MenuWindow.GetActive())
            MenuWindow.SetActive(false);
        if (serverWindow.GetActive())
            serverWindow.SetActive(false);
        if (playerSelectorRight.GetActive())
            playerSelectorRight.SetActive(false);
        if (playerSelectorLeft.GetActive())
            playerSelectorLeft.SetActive(false);
        if(storeWindow.GetActive())
            storeWindow.SetActive(false);

        sectionView3.SetActive(true);


        Debug.Log("Dis from photon...");
    }

    public void OnPressStart()
    {
        if(!Lock.activeSelf)
        {
            MenuWindow.SetActive(false);
            playerSelectorRight.SetActive(false);
            playerSelectorLeft.SetActive(false);
            storeWindow.SetActive(false);
            levelWindow.SetActive(true);
        }
        else
        {
            StartCoroutine(ShowError());
        }
    }

    public void OnPressInviteFriends()
    {
        if (!Lock.activeSelf)
        {
            MenuWindow.SetActive(false);
            playerSelectorRight.SetActive(false);
            playerSelectorLeft.SetActive(false);
                storeWindow.SetActive(false);
            serverWindow.SetActive(true);
        }
        else
        {
            StartCoroutine(ShowError());
        }
    }

    IEnumerator ShowError()
    {
        Lock.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Lock.transform.GetChild(0).gameObject.SetActive(false);
    }
}
