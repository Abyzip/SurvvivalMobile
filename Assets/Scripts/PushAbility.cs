using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PushAbility : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private GameObject LocalPlayer;
    private GameObject HitArea;
    private GameObject WeaponEffect;
    private Transform Aura;
    public float stackDuration = 1;
    public float stackState = 0;

    public Vector3 angle;

    private PhotonView photonView;

    Color synccolor;
    Vector3 tempcolor;
    
    private bool playersLoaded = false;
    private Vector3 mousePosStart;
    private Vector3 direction;

    private moteurJeu moteur;
    private bool isStacking = false;
    private GameObject cooldown;
    void Start()
    {
        moteur = GameObject.FindGameObjectWithTag("moteurJeu").transform.GetComponent<moteurJeu>();
        cooldown = gameObject.transform.GetChild(1).gameObject;
        StartCoroutine(loadingPlayers());
    }

    void Update()
    {
        if(playersLoaded)
        {
            if (!photonView.isMine)
            {
                Aura.GetChild(0).GetComponent<ParticleSystem>().startColor = synccolor;
                Aura.GetChild(1).GetComponent<ParticleSystem>().startColor = synccolor;
                return;
            }
        }

        if(isStacking)
        {
            
            Vector3 currentPos = Input.mousePosition;
            float angle = Mathf.Atan2(currentPos.y - mousePosStart.y, currentPos.x - mousePosStart.x) * Mathf.Rad2Deg;

            //HitArea.transform.rotation = Quaternion.Euler(new Vector3(0, -angle +90, 0));
            //WeaponEffect.transform.rotation = Quaternion.Euler(new Vector3(0, -angle + 90, 0));
            //this.angle = new Vector3(0, -angle + 90, 0);
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            moteur.activePush(LocalPlayer.name, true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            moteur.activePush(LocalPlayer.name, false);
        }
    }

    public void OnPointerDown(PointerEventData ped)
    {
        mousePosStart = Input.mousePosition;
        HitArea.SetActive(true);
        isStacking = true;
        moteur.activePush(LocalPlayer.name, true);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        mousePosStart = Vector3.zero;
        HitArea.SetActive(false);
        isStacking = false;
        moteur.activePush(LocalPlayer.name, false);
        StartCoroutine("cooldownTimer");
    }

    IEnumerator cooldownTimer()
    {
        float cool = 3.0f;
        cooldown.SetActive(true);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        Image image = cooldown.GetComponent<Image>();
        while (cool > 0)
        {
            cool -= Time.deltaTime;
            image.fillAmount = 1 - Mathf.Lerp(1, 0, cool/3);
            yield return new WaitForEndOfFrame();
        }
        cooldown.SetActive(false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        image.fillAmount = 1;
    }

    IEnumerator loadingPlayers()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetPhotonView().isMine)
            {
                LocalPlayer = player;
                photonView = player.GetPhotonView();
                Aura = player.transform.GetChild(0);
                Aura.GetComponent<ParticleSystem>().startColor = new Color(0f, 1f, 0f, 1f);
                Aura.GetChild(0).GetComponent<ParticleSystem>().startColor= new Color(0f, 1f, 0f, 1f);
                Aura.GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0f, 1f, 0f, 1f);
                playersLoaded = true;
                WeaponEffect = LocalPlayer.transform.GetChild(5).gameObject;
                HitArea = LocalPlayer.transform.GetChild(6).gameObject;
            }
        }
    }
    
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {/*
        if (stream.isWriting)
        {
            //send color
            tempcolor = new Vector3(GetComponent<ParticleSystem>().startColor.r, GetComponent<ParticleSystem>().startColor.g, GetComponent<ParticleSystem>().startColor.b);
            stream.Serialize(ref tempcolor);
        }
        else
        {
            //get color
            stream.Serialize(ref tempcolor);

            synccolor = new Color(tempcolor.x, tempcolor.y, tempcolor.z, 1.0f);

        }*/
    }
}