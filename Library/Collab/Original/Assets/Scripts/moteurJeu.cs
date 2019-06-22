using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class moteurJeu : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject[] bombSpawners;
    private GameObject[] players;
    private GameObject timer;
    private GameObject panelPause;
    public GameObject winner;
    public int nbDeathPlayer1, nbDeathPlayer2, nbDeathPlayer3, nbDeathPlayer4;
    private bool isGameStarting = false;
    public string difficulty = "LOW";
    private GameObject[] spawnerProjectile;
    private int nbPlayer = 0;
    private bool isMaster = true;
    private PhotonView view;
    public GameObject localPlayer;
    public GameObject player1, player2, player3, player4;
    private GameObject GOPlayer;
    private bool isStacking = false;
    public float stackingPower = 0.0f;

    public static int isReady = 0;

    public GameObject spawner;
    public GameObject FXApocalypse;
    public GameObject FXShield;
    public AudioClip apocalypseCooldownClip;
    public AudioClip tempeteClip;

    public float xMinFxShield, xMaxFxShield, zMinFxShield, zMaxFxShield;
    public GameObject spawnerTornado;

    public int gameDuration;

    public int changeDifficultyMediumTime = 120;
    public int changeDifficultyHardTime = 60;

    public string[] events;
    public string currentEvent = "";

    public GameObject projectilePlaine, projectileIce;

    public GameObject[] spawnersVortex;
    public GameObject FXVortex;
    public GameObject FXPreVortex;

    public GameObject ground;
    public List<EventGame> eventsGameObject=new List<EventGame>();

    public AudioClip punch;
    public AudioClip death;


    // Start is called before the first frame update
    void Start()
    {
        GOPlayer = GameObject.Find("PlayerNamePref");
        view = GetComponent<PhotonView>();
        nbPlayer = PhotonNetwork.room.PlayerCount;
        GameObject currentPlayer=new GameObject();
        switch(PhotonNetwork.player.NickName)
        {
            case "PLAYER 1":
                currentPlayer = PhotonNetwork.Instantiate(GOPlayer.transform.GetChild(0).name, spawner.transform.GetChild(0).position, spawner.transform.GetChild(0).rotation, 0) as GameObject;
                break;
            case "PLAYER 2":
                currentPlayer = PhotonNetwork.Instantiate(GOPlayer.transform.GetChild(0).name, spawner.transform.GetChild(1).position, spawner.transform.GetChild(1).rotation, 0) as GameObject;
                break;
            case "PLAYER 3":
                currentPlayer = PhotonNetwork.Instantiate(GOPlayer.transform.GetChild(0).name, spawner.transform.GetChild(2).position, spawner.transform.GetChild(2).rotation, 0) as GameObject;
                break;
            case "PLAYER 4":
                currentPlayer = PhotonNetwork.Instantiate(GOPlayer.transform.GetChild(0).name, spawner.transform.GetChild(3).position, spawner.transform.GetChild(3).rotation, 0) as GameObject;
                break;
        }
        currentPlayer.name = PhotonNetwork.player.NickName;
        if (!PhotonNetwork.player.IsMasterClient)
        {
            isMaster = false;
        }
        StartCoroutine(loadingPlayers());       
    }

    public void InitEvents()
    {
        float currentTime = 0;
        string currentDifficulty = "LOW";
        int nbSpawnersBomb = bombSpawners.Length;
        EventGame projectileEb = new EventGame("eventProjectile", spawnerProjectile);
        eventsGameObject.Add(projectileEb);
        while (currentTime<gameDuration)
        {
            if (gameDuration-currentTime<changeDifficultyHardTime)
                currentDifficulty = "HARD";
            else if((gameDuration - currentTime < changeDifficultyMediumTime))
                currentDifficulty = "MEDIUM";

            int rand = Random.Range(0, events.Length);
            float durationEvent = 1;
            if (events[rand]=="eventBomb")
            {
                EventGame eb = new EventGame("eventBomb", currentDifficulty, nbSpawnersBomb, bombSpawners);
                eventsGameObject.Add(eb);
                durationEvent = eb.duration;
            }
            else if(events[rand] == "eventTempete")
            {
                EventGame eb = new EventGame("eventTempete", currentDifficulty, spawnerTornado);
                eventsGameObject.Add(eb);
                durationEvent = eb.duration;
            }
            else if (events[rand] == "eventApocalypse")
            {
                EventGame eb = new EventGame("eventApocalypse", currentDifficulty,  xMinFxShield,  xMaxFxShield,  zMinFxShield,  zMaxFxShield);
                eventsGameObject.Add(eb);
                durationEvent = eb.duration;
            }
            else if (events[rand] == "eventVortex")
            {
                EventGame eb = new EventGame("eventVortex", currentDifficulty, spawnersVortex, spawnersVortex.Length);
                eventsGameObject.Add(eb);
                durationEvent = eb.duration;
            }
            currentTime = currentTime + durationEvent;
        }
        string json = JsonHelper.ToJson<EventGame>(eventsGameObject.ToArray(), true);
        int jsonLength = json.Length / 32767;
        string[] jsonTab = Chop(json, 32767);
        Debug.Log(json);



        view.RPC("initEventsRPC", PhotonTargets.All,jsonTab);

    }
    public static string[] Chop(string value, int length)
    {
        int strLength = value.Length;
        int strCount = (strLength + length - 1) / length;
        string[] result = new string[strCount];
        for (int i = 0; i < strCount; ++i)
        {
            result[i] = value.Substring(i * length, Mathf.Min(length, strLength));
            strLength -= length;
        }
        return result;
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (isMaster)
        {
            if (isGameStarting)
            {
                if (currentEvent == "")
                {
                    int rand = Random.Range(0, events.Length);
                    currentEvent = events[rand];
                    if (currentEvent == "eventBomb")
                    {
                        StartCoroutine("bombCoroutine");
                    }
                    else if (currentEvent == "eventApocalypse")
                    {
                        StartCoroutine("apocalypseCoroutine");
                    }
                    else if (currentEvent == "eventTempete")
                    {
                        view.RPC("tempeteCoroutineRPC", PhotonTargets.All);

                    }
                    else if (currentEvent == "eventVortex")
                    {
                        StartCoroutine("vortexCoroutine");
                    }
                }
            }
        }*/
    }

 

    public void StartGame()
    {

        Time.timeScale = 1;
        resetGame();
        if (panelPause != null)
            panelPause.SetActive(false);
        isGameStarting = true;
        view.RPC("timerCoroutineRPC", PhotonTargets.All);

        /*
        StartCoroutine("projectileCoroutine");*/

    }

    private void endGame()
    {
        string winText = "";
        if(winner == player1)
            winText = "Player 1 win !";
        else if (winner == player2)
            winText = "Player 2 win !";
        else if (winner == player3)
            winText = "Player 3 win !";
        else if (winner == player4)
            winText = "Player 4 win !";
       
        panelPause.transform.GetChild(0).GetComponent<Text>().text = winText;
        panelPause.SetActive(true);
        isGameStarting = false;
    }

    private void resetGame()
    {
        isGameStarting = false;
        difficulty = "LOW";
    }
    

   

    

    IEnumerator timerCoroutine()
    {
        yield return new WaitForSeconds(3);
        int i = gameDuration;
        while (i != -1)
        {
            yield return new WaitForSeconds(1);
            timer.GetComponent<Text>().text = ""+i;       
            i--;
            if (i == changeDifficultyMediumTime)
                difficulty = "MEDIUM";
            else if (i == changeDifficultyHardTime)
                difficulty = "HARD";
        }
        endGame();
    }

    IEnumerator projectileCoroutine(EventGame eg)
    {
        Scene m_Scene = SceneManager.GetActiveScene();
        GameObject currentProjectilePrefab;
        if (m_Scene.name=="LevelPlaine")
        {
            currentProjectilePrefab = projectilePlaine;
        }
        else
        {
            currentProjectilePrefab = projectileIce;

        }
        int i = 180;
        int compt = 0;
        while (isGameStarting)
        {
            if (difficulty=="LOW")
                yield return new WaitForSeconds(1);
            else if (difficulty == "MEDIUM")
                yield return new WaitForSeconds(0.5f);
            else if (difficulty == "HARD")
                yield return new WaitForSeconds(0.25f);
            else
                yield return new WaitForSeconds(0.25f);

            GameObject currentSpawner = spawnerProjectile[eg.spawnOrderProjectile[compt]];
            GameObject currentProjectile = Instantiate(currentProjectilePrefab,currentSpawner.transform.position, currentSpawner.transform.rotation) as GameObject;
            Destroy(currentProjectile, 10);
            compt++;
        }
    }
    
    
    
 
    void CameraShake()
    {
         mainCamera.GetComponent<CameraShake>().shakeDuration = 1;
    }
    [PunRPC]
    void timerCoroutineRPC()
    {
        StartCoroutine("timerCoroutine");
    }
    
    [PunRPC]
    void initEventsRPC(string[] listEventJson)
    {
        int i = 0;
        string jsonFinal = "";
        while (i<listEventJson.Length)
        {
            jsonFinal = jsonFinal + listEventJson[i];
            i++;
        }
        EventGame[] eventGame = JsonHelper.FromJson<EventGame>(jsonFinal);
         i = 0;
        List<EventGame> currentList= new List<EventGame>();
        while(i< eventGame.Length)
        {
            currentList.Add(eventGame[i]);
            i++;
        }
        eventsGameObject = currentList;
        view.RPC("isReadyRPC", PhotonTargets.MasterClient);

    }

    [PunRPC]
    void initEventsRPC(string listEventJson)
    {
        int i = 0;
        string jsonFinal = listEventJson;
        
        EventGame[] eventBomb = JsonHelper.FromJson<EventGame>(jsonFinal);
        i = 0;
        List<EventGame> currentList = new List<EventGame>();
        while (i < eventBomb.Length)
        {
            currentList.Add(eventBomb[i]);
            i++;
        }
        eventsGameObject = currentList;
        view.RPC("isReadyRPC", PhotonTargets.MasterClient);

    }

    [PunRPC]
    void isReadyRPC()
    {
        moteurJeu.isReady++;
        if (moteurJeu.isReady == PhotonNetwork.room.MaxPlayers)
        {
            StartCoroutine("waitPlayersCoroutine");
            //view.RPC("startGameRPC", PhotonTargets.All);
            
        }
    }

    IEnumerator waitPlayersCoroutine()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine("gameCoroutine");
    }
    [PunRPC]
    void startGameRPC()
    {
        StartCoroutine("gameCoroutine");
    }
    [PunRPC]
    void initPlayersRPC()
    {
        //StartCoroutine("gameCoroutine");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetPhotonView().isMine)
            {
                localPlayer = player;
            }
            switch (player.GetPhotonView().owner.NickName)
            {
                case "PLAYER 1":
                    player1 = player;
                    break;
                case "PLAYER 2":
                    player2 = player;
                    break;
                case "PLAYER 3":
                    player3 = player;
                    break;
                case "PLAYER 4":
                    player4 = player;
                    break;
            }
        }
        winner = player1;
    }
    IEnumerator gameCoroutine()
    {
        view.RPC("initPlayersRPC", PhotonTargets.All);

       

        int i = 0;
        while(i<eventsGameObject.Count)
        {
            if (eventsGameObject[i].Play()=="bombCoroutine")
            {
                //StartCoroutine(BombCoroutine((EventGame)eventsGameObject[i]));
                view.RPC("EventCoroutineRPC", PhotonTargets.All, i, "bombCoroutine");

            }
            if (eventsGameObject[i].Play() == "projectileCoroutine")
            {
                //StartCoroutine(projectileCoroutine((EventGame)eventsGameObject[i]));
                view.RPC("EventCoroutineRPC", PhotonTargets.All, i, "projectileCoroutine");
            }
            
            else if (eventsGameObject[i].Play() == "tempeteCoroutine")
            {
                //StartCoroutine(tempeteCoroutine((EventGame)eventsGameObject[i]));
                view.RPC("EventCoroutineRPC", PhotonTargets.All, i, "tempeteCoroutine");
            }
            else if (eventsGameObject[i].Play() == "apocalypseCoroutine")
            {
                //StartCoroutine(apocalypseCoroutine((EventGame)eventsGameObject[i]));
                view.RPC("EventCoroutineRPC", PhotonTargets.All, i, "apocalypseCoroutine");
            }
            else if (eventsGameObject[i].Play() == "vortexCoroutine")
            {
                //StartCoroutine(vortexCoroutine((EventGame)eventsGameObject[i]));
                view.RPC("EventCoroutineRPC", PhotonTargets.All, i, "vortexCoroutine");
            }
            yield return new WaitForSeconds(eventsGameObject[i].duration);
            i++;
        }
    }
    [PunRPC]
    void EventCoroutineRPC(int i, string eventName)
    {
        switch(eventName)
        {
            case "bombCoroutine":
                StartCoroutine(BombCoroutine((EventGame)eventsGameObject[i]));
                break;
            case "projectileCoroutine":
                StartCoroutine(projectileCoroutine((EventGame)eventsGameObject[i]));
                break;
            case "tempeteCoroutine":
                StartCoroutine(tempeteCoroutine((EventGame)eventsGameObject[i]));
                break;
            case "apocalypseCoroutine":
                StartCoroutine(apocalypseCoroutine((EventGame)eventsGameObject[i]));
                break;
            case "vortexCoroutine":
                StartCoroutine(vortexCoroutine((EventGame)eventsGameObject[i]));
                break;
        }
        
    }


    IEnumerator apocalypseCoroutine(EventGame eg)
    {
       
        int i = 0;
        GameObject[] instancesFXShield = new GameObject[eg.spawnApocalypseInformations.Count];
        mainCamera.GetComponent<AudioSource>().PlayOneShot(apocalypseCooldownClip);
        while (i < eg.spawnApocalypseInformations.Count)
        {
            GameObject instanceFXShield = Instantiate(FXShield,eg.spawnApocalypseInformations[i].position, Quaternion.identity) as GameObject;
            instancesFXShield[i] = instanceFXShield;
        
            i++;
        }
        Material mGround = ground.GetComponent<MeshRenderer>().material;
        Color initColor = mGround.color;
        float compt = 0;
        while (compt<5) {
            mGround.color = Color.Lerp(initColor, Color.red, compt / 5);
            yield return new WaitForEndOfFrame();
            compt += Time.deltaTime;
        }
        mGround.color = initColor;
        CameraShake();
        if(localPlayer.GetComponent<PlayControllerJoystick>().isSafe == 0)
        {
            localPlayer.GetComponent<DeathPlayer>().Death();
        }
        yield return new WaitForSeconds(2.0f);
        i = 0;
        localPlayer.GetComponent<PlayControllerJoystick>().isSafe = 0;
        while (i < eg.spawnApocalypseInformations.Count)
        {
            Destroy(instancesFXShield[i]);
            i++;
        }
    }
    IEnumerator tempeteCoroutine(EventGame eg)
    {
        Debug.Log("tempeteCoroutine");
        eg.spawnerTempete = this.spawnerTornado;
        spawnerTornado.SetActive(true);
        mainCamera.GetComponent<AudioSource>().PlayOneShot(tempeteClip);
        yield return new WaitForSeconds(2);
        spawnerTornado.transform.GetChild(0).gameObject.SetActive(true);
        if (eg.difficulty == "LOW")
            spawnerTornado.GetComponent<SpawnerTornadoScript>().rate = 0.5f;
        else if (eg.difficulty == "MEDIUM")
            spawnerTornado.GetComponent<SpawnerTornadoScript>().rate = 0.4f;
        else if (eg.difficulty == "HARD")
            spawnerTornado.GetComponent<SpawnerTornadoScript>().rate = 0.3f;

        yield return new WaitForSeconds(eg.duration - 3);
        spawnerTornado.transform.GetChild(0).gameObject.SetActive(false);
        spawnerTornado.SetActive(false);
        Debug.Log("FIN : tempeteCoroutine");

    }

    IEnumerator BombCoroutine(EventGame eb)
    {
        eb.spawners = this.bombSpawners;
        int currentNbBomb = 0;
        while (currentNbBomb != eb.nbBomb)
        {
            eb.spawners[eb.spawnBombInformations[currentNbBomb].spawnerNum].GetComponent<BombSpawnerScript>().shot(eb.spawnBombInformations[currentNbBomb].force);
            currentNbBomb++;
            yield return new WaitForSeconds(eb.rate);
        }
    }

    IEnumerator vortexCoroutine(EventGame eb)
    {
        eb.spawnersVortex = this.spawnersVortex;
        eb.spawnersVortex[eb.spawnVortexInformations.spawnerNum].SetActive(true);
        eb.spawnersVortex[eb.spawnVortexInformations.spawnerNum].GetComponent<VortexScript>().player = localPlayer;
        eb.spawnersVortex[eb.spawnVortexInformations.spawnerNum].GetComponent<VortexScript>().power = eb.spawnVortexInformations.force;

        yield return new WaitForSeconds(eb.duration);

        eb.spawnersVortex[eb.spawnVortexInformations.spawnerNum].SetActive(false);

    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
          
        }
        else
        {
        }
    }

    IEnumerator loadingPlayers()
    {
        yield return new WaitForSeconds(0.2f);
        bombSpawners = GameObject.FindGameObjectsWithTag("spawnerBomb");
         //spawnersVortex = GameObject.FindGameObjectsWithTag("spawnerVortex");
        spawnerProjectile = GameObject.FindGameObjectsWithTag("spawnerProjectile");
        timer = GameObject.FindGameObjectWithTag("timerGame");
        panelPause = GameObject.FindGameObjectWithTag("panelPause");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        ground = GameObject.FindGameObjectWithTag("ground");
        if (panelPause != null)
            panelPause.SetActive(false);
        if (isMaster)
            InitEvents();
        StartGame();
    }

    [PunRPC]
    void incrementDeathRPC(string playerName)
    {
        Debug.Log("playerName : " + playerName);
        switch (playerName)
        {
            case "PLAYER 1":
                nbDeathPlayer1++;
                break;
            case "PLAYER 2":
                nbDeathPlayer2++;
                break;
            case "PLAYER 3":
                nbDeathPlayer3++;
                break;
            case "PLAYER 4":
                nbDeathPlayer4++;
                break;
        }
        List<int> countDeath = new List<int>();
        int MaxPlayers = PhotonNetwork.room.MaxPlayers;
        countDeath.Add(nbDeathPlayer1);
        if(MaxPlayers >= 2)
            countDeath.Add(nbDeathPlayer2);
        if (MaxPlayers >= 3)
            countDeath.Add(nbDeathPlayer3);
        if (MaxPlayers >= 4)
            countDeath.Add(nbDeathPlayer4);
        int min = Mathf.Min(countDeath.ToArray());
        int pos = 1;
        foreach (int count in countDeath)
        {
            if (count == min)
            {
                switch(pos)
                {
                    case 1:
                        view.RPC("getLeaderRPC", PhotonTargets.All, player1.GetPhotonView().owner.NickName);
                        break;
                    case 2:
                        view.RPC("getLeaderRPC", PhotonTargets.All, player2.GetPhotonView().owner.NickName);
                        break;
                    case 3:
                        view.RPC("getLeaderRPC", PhotonTargets.All, player3.GetPhotonView().owner.NickName);
                        break;
                    case 4:
                        view.RPC("getLeaderRPC", PhotonTargets.All, player4.GetPhotonView().owner.NickName);
                        break;
                }
            }
            pos++;
        }
    }

    [PunRPC]
    void getLeaderRPC(string playerName)
    {
        int MaxPlayers = PhotonNetwork.room.MaxPlayers;
        player1.transform.GetChild(4).gameObject.SetActive(false);
        if (MaxPlayers >= 2)
            player2.transform.GetChild(4).gameObject.SetActive(false);
        if (MaxPlayers >= 3)
            player3.transform.GetChild(4).gameObject.SetActive(false);
        if (MaxPlayers >= 4)
            player4.transform.GetChild(4).gameObject.SetActive(false);

        switch (playerName)
        {
            case "PLAYER 1":
                player1.transform.GetChild(4).gameObject.SetActive(true);
                winner = player1;
                break;
            case "PLAYER 2":
                player2.transform.GetChild(4).gameObject.SetActive(true);
                winner = player2;
                break;
            case "PLAYER 3":
                player3.transform.GetChild(4).gameObject.SetActive(true);
                winner = player3;
                break;
            case "PLAYER 14":
                player4.transform.GetChild(4).gameObject.SetActive(true);
                winner = player4;
                break;
        }
    }

    public void incrementDeath(string playerName)
    {
        view.RPC("incrementDeathRPC", PhotonTargets.MasterClient, playerName);
    }

    [PunRPC]
    void activeShieldRPC(string playerName, bool isActive)
    {
        switch (playerName)
        {
            case "PLAYER 1":
                player1.transform.GetChild(2).gameObject.SetActive(isActive);
                    break;
            case "PLAYER 2":
                player2.transform.GetChild(2).gameObject.SetActive(isActive);
                break;
            case "PLAYER 3":
                player3.transform.GetChild(2).gameObject.SetActive(isActive);
                break;
            case "PLAYER 4":
                player4.transform.GetChild(2).gameObject.SetActive(isActive);
                break;
        }
    }

    public void activeShield(string playerName, bool isActive)
    {
        view.RPC("activeShieldRPC", PhotonTargets.All, playerName, isActive);
    }

    [PunRPC]
    void activePushRPC(string playerName, bool isStacking)
    {
        if(playerName == localPlayer.name)
        {
            this.isStacking = isStacking;
            this.stackingPower = 0.0f;
        }

        if (isStacking)
        {
            switch (playerName)
            {
                case "PLAYER 1":
                    player1.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
                    player1.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
                    StartCoroutine(stackSpell(player1));
                    break;
                case "PLAYER 2":
                    player2.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
                    player2.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
                    StartCoroutine(stackSpell(player2));
                    break;
                case "PLAYER 3":
                    player3.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
                    player3.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
                    StartCoroutine(stackSpell(player3));
                    break;
                case "PLAYER 4":
                    player4.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
                    player4.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
                    StartCoroutine(stackSpell(player4));
                    break;
            }
        }
        else
        {
            Color colorAura = new Color();
            if (localPlayer.name == playerName)
            {
                colorAura = new Color(0f, 1f, 0f, 1f);
                /*Debug.Log("forward : " + localPlayer.transform.GetChild(6).forward);
                Vector3 direction = localPlayer.transform.GetChild(6).forward;
                Debug.Log("y : " + localPlayer.transform.GetChild(6).rotation.y);
                Vector3 v = localPlayer.transform.eulerAngles;
                v.y = localPlayer.transform.GetChild(6).eulerAngles.y;
                localPlayer.transform.rotation = Quaternion.Euler(v);
                Debug.Log(" new rotation : " + localPlayer.transform.localRotation);
                Debug.Log("position : " + localPlayer.transform.GetChild(6).GetChild(0).position);
                localPlayer.transform.rotation = Quaternion.Euler(GameObject.FindGameObjectWithTag("Push").GetComponent<PushAbility>().angle);*/
            }
            else
                colorAura = new Color(0f, 0.87f, 1f, 1f);
            switch (playerName)
            {
                case "PLAYER 1":
                    ParticleSystem.MainModule m =   player1.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().main;
                    m.startColor = colorAura;
                    player1.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = colorAura;
                    player1.GetComponent<Animator>().SetTrigger("Melee");
                    player1.transform.GetChild(5).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                    player1.transform.GetChild(5).GetChild(0).GetComponent<ParticleSystem>().Play();
                    StartCoroutine("disableCollider", player1.transform.GetChild(5).GetChild(0).GetComponent<BoxCollider>());
                    break;
                case "PLAYER 2":
                    player2.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = colorAura;
                    player2.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = colorAura;
                    player2.GetComponent<Animator>().SetTrigger("Melee");
                    player2.transform.GetChild(5).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                    player2.transform.GetChild(5).GetChild(0).GetComponent<ParticleSystem>().Play();
                    StartCoroutine("disableCollider", player2.transform.GetChild(5).GetChild(0).GetComponent<BoxCollider>());
                    break;
                case "PLAYER 3":
                    player3.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = colorAura;
                    player3.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = colorAura;
                    player3.GetComponent<Animator>().SetTrigger("Melee");
                    player3.transform.GetChild(5).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                    player3.transform.GetChild(5).GetChild(0).GetComponent<ParticleSystem>().Play();
                    StartCoroutine("disableCollider", player3.transform.GetChild(5).GetChild(0).GetComponent<BoxCollider>());
                    break;
                case "PLAYER 4":
                    player4.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = colorAura;
                    player4.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = colorAura;
                    player4.GetComponent<Animator>().SetTrigger("Melee");
                    player4.transform.GetChild(5).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                    player4.transform.GetChild(5).GetChild(0).GetComponent<ParticleSystem>().Play();
                    StartCoroutine("disableCollider", player4.transform.GetChild(5).GetChild(0).GetComponent<BoxCollider>());
                    break;
            }
        }
    }

    IEnumerator stackSpell(GameObject player)
    {
        MeshRenderer[] tabMesh = player.GetComponentsInChildren<MeshRenderer>();
        Color[] startColors = new Color[tabMesh.Length];
        float timer = 0.0f;

        for (int i = 0; i < tabMesh.Length; i++)
        {
            if (tabMesh[i].name != "Cube")
                startColors[i] = tabMesh[i].material.color;
        }

        while (isStacking && timer < 2)
        {
            foreach (MeshRenderer m in tabMesh)
            {
                m.material.color = Color.Lerp(m.material.color, Color.red, timer / 2);
            }
            timer += Time.deltaTime ;
            yield return new WaitForEndOfFrame();
        }

        while (isStacking)
        {
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < tabMesh.Length; i++)
        {
            tabMesh[i].material.color = startColors[i];
        }
        
        activePush(player.name, false);
        stackingPower += timer / 2;
        Debug.Log("stackingPower = " + stackingPower);
    }

    IEnumerator disableCollider(BoxCollider collider)
    { 
        yield return new WaitForSeconds(0.2f);
        collider.enabled = false;
    }

    public void activePush(string playerName, bool isStacking)
    {
        view.RPC("activePushRPC", PhotonTargets.All, playerName, isStacking);
    }

    [PunRPC]
    void activePushParticleRPC(string playerName, Vector3 direction, string pusherName, float power)
    {
        if(playerName == localPlayer.name)
        {
            switch (pusherName)
            {
                case "PLAYER 1":
                    localPlayer.GetComponent<PlayControllerJoystick>().playerPusher = player1;
                    break;
                case "PLAYER 2":
                    localPlayer.GetComponent<PlayControllerJoystick>().playerPusher = player2;
                    break;
                case "PLAYER 3":
                    localPlayer.GetComponent<PlayControllerJoystick>().playerPusher = player3;
                    break;
                case "PLAYER 4":
                    localPlayer.GetComponent<PlayControllerJoystick>().playerPusher = player4;
                    break;
            }
            Debug.Log("HIT");
            localPlayer.transform.GetComponent<Rigidbody>().velocity = direction/60;
            StartCoroutine("pusherTimer", localPlayer);
        }
        mainCamera.GetComponent<AudioSource>().PlayOneShot(punch);
    }

    IEnumerator pusherTimer(GameObject playerPushed)
    {
        
        while(playerPushed.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        playerPushed.GetComponent<PlayControllerJoystick>().playerPusher = null;
    }

    public void activePushParticle(string playerName, Vector3 direction, string playerPusher, float power)
    {
        view.RPC("activePushParticleRPC", PhotonTargets.All, playerName, direction, playerPusher, power);
    }
}
