using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    public float moveSpeed = 3f;
    public float turnSpeed = 50f;
    int jumpHash = Animator.StringToHash("Jump");
    int walk = Animator.StringToHash("Run");
    int strafeR = Animator.StringToHash("Strafe Right");
    int strafeL = Animator.StringToHash("Strafe Left");
    int pickup = Animator.StringToHash("Pick Up");
    int drinkPot = Animator.StringToHash("Drink Potion");
    public float timer = 0f;
    public float timerLerp = 0;

    public float speed = 6.0F;

    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private bool walking;

    void Start()
    {
        anim = GetComponent<Animator>();
        walking = true;
    }


    void Update()
    {

        float straff = 100 * Input.GetAxis("Mouse X");
        straff *= Time.deltaTime;
        transform.Rotate(0, straff, 0);
        /* if (Input.GetKeyDown(KeyCode.LeftShift))
         {
             SwitchWalk();
         }

         // Ramasser un item au sol
         if (Input.GetKeyDown(KeyCode.LeftControl))
         {
             anim.SetBool(pickup, true);
             //Detruir objet
         }

         // Boire une possion Vie
         if (Input.GetKeyDown(KeyCode.V))
         {
             anim.SetBool(drinkPot, true);
             //Vie +++
         }

         // Boire une possion Mana
         if (Input.GetKeyDown(KeyCode.B))
         {
             anim.SetBool(drinkPot, true);
             //Mana +++
         }*/

        CharacterController controller = GetComponent<CharacterController>();
        float vertical = Input.GetAxis("Vertical");
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(0, 0, vertical);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (vertical != 0)
        {
            anim.SetBool(strafeR, false);
            anim.SetBool(strafeL, false);
            anim.SetBool(walk, true); // 1 is run forward
            //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 1, 0);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -1, 0);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {

             anim.SetBool(strafeR, false);
             anim.SetBool(strafeL, false);
             anim.SetBool(walk, true); // 1 is run forward
             transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
         }
        else
        {
            anim.SetBool(strafeR, false);
            anim.SetBool(strafeL, false);
            anim.SetBool(walk, false);

        }

        /*
       else if (horizontal > 0)
       {
           anim.SetBool(strafeR, false);
           anim.SetBool(walk, false);
           //transform.Rotate(new Vector3(0, -5, 0));

           anim.SetBool(strafeL, true);

           //transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
       }


       else if (horizontal < 0)
       {

           //transform.Rotate(new Vector3(0, -5, 0));
           anim.SetBool(walk, false);
           anim.SetBool(strafeL, false);

           anim.SetBool(strafeR, true);
           //transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);


       }
       


        */

        /* 
         if (Input.GetKeyDown(KeyCode.Space))
         {
             anim.SetBool(walk, false);
             anim.SetTrigger(jumpHash);
             anim.SetBool(walk, true);
         }
     }
     else if (Input.GetKey(KeyCode.S))
     {
         anim.SetBool(walk, true); // 1 is run forward
         transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
     }
     else if (Input.GetKey(KeyCode.Q))
     {
         //transform.Rotate(new Vector3(0, -5, 0));
         anim.SetBool(strafeL, true);
         transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
     }
     else if (Input.GetKey(KeyCode.D))
     {
         //transform.Rotate(new Vector3(0, 5, 0));
         anim.SetBool(strafeR, true);
         transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
     }

     else
     {
         anim.SetBool(walk, false);
         anim.SetBool(strafeL, false);
         anim.SetBool(strafeR, false);
     }


     
     

     */
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pot" && Input.GetKey(KeyCode.LeftControl))
        {
            Destroy(other.gameObject);
        }
    }


}