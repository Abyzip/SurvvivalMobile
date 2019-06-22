using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


public class PhotonButtons : MonoBehaviour
{
    public InputField joinRoomInput;

    public GameObject PopUpErrorServerNotExist, PopUpErrorServerExist, PopUpWaitingPlayers, PopUpWaitingPlayersFriend;
    public GameObject loadinScreen;
    public GameObject slider;
    public Text loadingText;
    public GameObject Lock;

    public Text textI;
    public Text textPlayersConnected, textFriendsConnected, textIDRoomFriend;

    private RoomInfo[] roomsList;

    private string charTab = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private byte NB_PLAYERS = 4;

    private string GenerateID()
    {
        string ID = "";
        for (int i = 0; i < 8; i++)
        {
            ID += charTab[Random.Range(0, charTab.Length)];
        }
        return ID;
    }

    public void CreateRoom(string mode)
    {
        string RoomID = mode;

        RoomID += GenerateID();
        if(roomsList == null)
        {
            PhotonNetwork.autoCleanUpPlayerObjects = false;
            PhotonNetwork.CreateRoom(RoomID, new RoomOptions() { MaxPlayers = NB_PLAYERS }, null);
        }
        else
        {
            RoomInfo room = roomsList.FirstOrDefault(r => r.Name == RoomID);
            bool exists = (room != null);
            if (!exists)
            {
                PhotonNetwork.autoCleanUpPlayerObjects = false;
                PhotonNetwork.CreateRoom(RoomID, new RoomOptions() { MaxPlayers = NB_PLAYERS }, null);
            }
            else
            {
                PhotonNetwork.autoCleanUpPlayerObjects = false;
                CreateRoom(mode);
            }
        }
    }

    public void OnCLickCreateFriendRoom()
    {
        int nb_players = 0;
        string levelName = "";
        GameObject RBPlayer = GameObject.FindGameObjectWithTag("RGPlayer");
        int childrensPlayers = RBPlayer.transform.childCount;
        for (int i = 0; i < childrensPlayers; ++i)
        {
            if (RBPlayer.transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                nb_players = int.Parse(RBPlayer.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text);
                break;
            }
        }

        GameObject RGLevel = GameObject.FindGameObjectWithTag("RGLevel");
        int childrensLevel = RGLevel.transform.childCount;
        for (int i = 0; i < childrensLevel; ++i)
        {
            if (RGLevel.transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                levelName = RGLevel.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text;
                break;
            }
        }

        string modeRoom = "";
        switch (levelName)
        {
            case "Glaçons gelés":
                modeRoom = "F1_";
                break;
            case "Plaine déjantée":
                modeRoom = "F2_";
                break;
        }

        string RoomID = modeRoom;

        RoomID += GenerateID();
        if (roomsList == null)
        {
            PhotonNetwork.autoCleanUpPlayerObjects = false;
            PhotonNetwork.CreateRoom(RoomID, new RoomOptions() { MaxPlayers = (byte) nb_players }, null);
        }
        else
        {
            RoomInfo room = roomsList.FirstOrDefault(r => r.Name == RoomID);
            bool exists = (room != null);
            if (!exists)
            {
                PhotonNetwork.autoCleanUpPlayerObjects = false;
                PhotonNetwork.CreateRoom(RoomID, new RoomOptions() { MaxPlayers = (byte)nb_players }, null);
            }
            else
            {
                OnCLickCreateFriendRoom();
            }
        }
    }

    public void OnClickJoinRoom(Text mode)
    {
        string modeRoom = "";
        switch (mode.text)
        {
            case "Glaçons gelés":
                modeRoom = "1_";
                break;
            case "Plaine déjantée":
                modeRoom = "2_";
                break;
        }

        if(roomsList != null && roomsList.Length > 0)
        {
            foreach (RoomInfo game in roomsList)
            {
                if (modeRoom == "1_")
                {
                    StartCoroutine(ShowError());
                }
                if (game.PlayerCount < game.MaxPlayers && game.Name.StartsWith(modeRoom)  && modeRoom!="1_")
                {
                    PhotonNetwork.JoinRoom(game.Name);
                    return;
                }
            }
        }
        else
        {
            if (modeRoom == "1_")
            {
                StartCoroutine(ShowError());
            }
        }
        if(modeRoom != "1_")
            CreateRoom(modeRoom);
    }

    public void OnClickJoinRoomFriend(Text IDroom)
    {
        /*
        RoomInfo room = roomsList.ToList().FirstOrDefault(r => r.Name == IDroom.text);
        if(room != null)
        {
            PhotonNetwork.JoinRoom(IDroom.text);
        */

        if (roomsList != null)
        {
            foreach (RoomInfo game in roomsList)
            {
                if (game.PlayerCount < game.MaxPlayers && (game.Name == ("F1_" + IDroom.text) || game.Name ==("F2_" + IDroom.text)))
                {
                    PhotonNetwork.JoinRoom(game.Name);
                    return;
                }
            }
        }
    }

    public void HidePopUp(GameObject PopUpGO)
    {
        PopUpGO.SetActive(false);
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.player.NickName = "PLAYER " + PhotonNetwork.room.PlayerCount;
        if (PhotonNetwork.room.PlayerCount == PhotonNetwork.room.MaxPlayers)
        {
           
            switch (PhotonNetwork.room.Name.Substring(0, 2))
            {
                case "1_":
                    StartCoroutine(LoadSceneAsync(1));
                    break;
                case "2_":
                    StartCoroutine(LoadSceneAsync(2));
                    break;

                case "F1":
                    StartCoroutine(LoadSceneAsync(1));
                    break;
                case "F2":
                    StartCoroutine(LoadSceneAsync(2));
                    break;
            }
        }
        else
        {
            PopUpWaitingPlayers.SetActive(true);
            PopUpWaitingPlayersFriend.SetActive(true);
            textPlayersConnected.text = "( " + PhotonNetwork.playerList.Length + " / " + PhotonNetwork.room.MaxPlayers + " )";
            textFriendsConnected.text = "( " + PhotonNetwork.playerList.Length + " / " + PhotonNetwork.room.MaxPlayers + " )";
            textIDRoomFriend.text = "ID : " + PhotonNetwork.room.Name.Substring(PhotonNetwork.room.Name.Length - 8, 8);
        }
    }

    IEnumerator LoadSceneAsync(int indexScene)
    {
        Debug.Log("kjsdbfkjsqbfk");
        AsyncOperation operation = PhotonNetwork.LoadLevelAsync(indexScene);

        loadinScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.transform.GetChild(0).GetComponent<Image>().fillAmount = progress;
            loadingText.text = Mathf.Round(progress * 100f) + "%";
            yield return null;
        }
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if (PhotonNetwork.playerList.Length == PhotonNetwork.room.MaxPlayers)
        {
            switch (PhotonNetwork.room.Name.Substring(0, 2))
            {
                case "1_":
                    PhotonNetwork.LoadLevel("LevelIce");
                    break;
                case "2_":
                    PhotonNetwork.LoadLevel("LevelPlaine");
                    break;
            }
        }
        else
        {
            //PopUpWaitingPlayers.SetActive(true);
            textPlayersConnected.text = "( " + PhotonNetwork.playerList.Length + " / " + PhotonNetwork.room.MaxPlayers + " )";
            textFriendsConnected.text = "( " + PhotonNetwork.playerList.Length + " / " + PhotonNetwork.room.MaxPlayers + " )";
            textIDRoomFriend.text = "ID : " + PhotonNetwork.room.Name.Substring(2, 8);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PopUpWaitingPlayers.SetActive(false);
        PopUpWaitingPlayersFriend.SetActive(false);
    }

    private void Awake()
    {
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 20;
    }

    void OnReceivedRoomListUpdate()
    {
        roomsList = PhotonNetwork.GetRoomList();
    }

    IEnumerator ShowError()
    {
        Lock.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Lock.transform.GetChild(0).gameObject.SetActive(false);
    }
}
