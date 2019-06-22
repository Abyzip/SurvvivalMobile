using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    void LoadSceneAsync()
    {
        AsyncOperation operation = PhotonNetwork.LoadLevelAsync(0);
    }

    public void OnPressQuitGame()
    {
        PhotonNetwork.LeaveRoom();
        LoadSceneAsync();
    }
}
