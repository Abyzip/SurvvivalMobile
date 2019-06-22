using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControllerJoystick : MonoBehaviour
{
    private VHandler jsMovement;
    private Vector3 direction;
    private GameObject joystick;

    Animator anim;
    public float moveSpeed = 3f;
    public float turnSpeed = 50f;
    int walk = Animator.StringToHash("Run");
    public float timer = 0f;
    public float timerLerp = 0;

    public float speed = 6.0F;
    public float maxSpeed = 3;

    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    private Rigidbody playerRigidBody;

    public PhotonView photonView;
    private Vector3 selfPos;
    private Quaternion selfRot;
    private Transform Aura;

    public int isSafe;
    public bool isDying;
    public GameObject playerPusher;
    private int countDeath = 0;

    private bool isVortexActive = false;

    // Start is called before the first frame update
    void Start()
    {
        isSafe = 0;
        isDying = false;
        joystick = GameObject.FindGameObjectWithTag("Joystick");
        jsMovement = joystick.GetComponent<VHandler>();
        playerRigidBody = GetComponent<Rigidbody>();
        /*Aura = transform.GetChild(0);
        if (photonView.isMine)
        {
            Aura.GetComponent<ParticleSystem>().startColor = new Color(0f, 1f, 0f, 1f);
            Aura.GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(0f, 1f, 0f, 1f);
            Aura.GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0f, 1f, 0f, 1f);
        }*/
            anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isDying)
        {
            if (photonView.isMine)
            {
                direction = jsMovement.InputDirection;
                if (direction.x != 0 && direction.y != 0)
                {
                    Vector3 targetDirection = new Vector3(direction.x, 0f, direction.y);
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation = targetRotation;
                    anim.SetBool(walk, true); // 1 is run forward
                                              //playerRigidBody.AddForce(Vector3.forward * moveSpeed * Time.deltaTime);
                                              //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

                    if(playerPusher == null)
                    {
                        playerRigidBody.AddForce(transform.forward * speed);

                        if (playerRigidBody.velocity.magnitude > maxSpeed && !isVortexActive)
                        {
                            playerRigidBody.velocity = playerRigidBody.velocity.normalized * maxSpeed;
                        }
                    }

                }
                else
                {
                    anim.SetBool(walk, false);

                }
            }
            else
            {
                smoothNetMovement();
            }
        }
    }

    private void smoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
        transform.rotation = Quaternion.Lerp(transform.rotation, selfRot, Time.deltaTime * 8);
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(anim.GetBool("Run"));
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
            selfRot = (Quaternion)stream.ReceiveNext();
            anim.SetBool("Run", (bool)stream.ReceiveNext());
        }
    }
}
