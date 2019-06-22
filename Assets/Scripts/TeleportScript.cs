using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeleportScript : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private GameObject LocalPlayer;
    private PhotonView photonView;
    private Vector3 savedPosition;

    private GameObject savedPositionFX;
    private string savedPositionFXName;

    private Camera camera;
    private bool isTPPlaced = false;

    private string idPlayer;
    
    private GameObject cooldown;
    private bool isAvailable = true;

    void Start()
    {
        cooldown = gameObject.transform.GetChild(1).gameObject;
        StartCoroutine(loadingPlayers());
    }

    public void OnPointerUp(PointerEventData ped)
    {
        if(isAvailable)
            StartCoroutine(teleport());
        
    }
    public void OnPointerDown(PointerEventData ped)
    {

    }

    IEnumerator teleport()
    {
        isAvailable = false;
        LocalPlayer.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        LocalPlayer.transform.GetChild(1).gameObject.SetActive(false);
        Destroy(GameObject.Find(savedPositionFXName + "(Clone)"));
        Instantiate(Resources.Load(savedPositionFXName), new Vector3(LocalPlayer.transform.position.x, 2.0f, LocalPlayer.transform.position.z), Quaternion.identity);
        Vector3 tmpPosition = new Vector3(LocalPlayer.transform.position.x, 2f, LocalPlayer.transform.position.z);
        LocalPlayer.transform.SetPositionAndRotation(savedPosition, Quaternion.identity);
        savedPosition = tmpPosition;
        float cool = 10.0f;
        cooldown.SetActive(true);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        Image image = cooldown.GetComponent<Image>();
        while (cool > 0)
        {
            cool -= Time.deltaTime;
            image.fillAmount = 1 - Mathf.Lerp(1, 0, cool / 10);
            yield return new WaitForEndOfFrame();
        }
        cooldown.SetActive(false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        image.fillAmount = 1;
        isAvailable = true;
    }

    IEnumerator loadingPlayers()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetPhotonView().isMine)
            {
                camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                LocalPlayer = player;
                photonView = player.GetPhotonView();
                idPlayer = photonView.owner.NickName.Substring(7, photonView.owner.NickName.Length - 7);
                savedPositionFXName = "PowerupGlow" + idPlayer;
                Instantiate(Resources.Load(savedPositionFXName), new Vector3(player.transform.position.x, 2.0f, player.transform.position.z), Quaternion.identity);
                savedPosition = new Vector3(player.transform.position.x, 2.0f, player.transform.position.z);
            }
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
