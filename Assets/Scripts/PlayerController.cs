using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cinemachine; // added Android movment
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
     // public CinemachineFreeLook freeLookCam; // added Android movment


    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;
    public float bounceForce = 8f;


    public int soundToPlay;

    // public float rotChar = 0f;

    private Vector3 moveDirection;

    public CharacterController charController;

    private Camera theCam;

    public GameObject playerModel;
    public float rotateSpeed;

    public Animator anim;

    // public Button jumpButton; // added Android movment

    public bool isKnocking;
    public float knockbackLength = 0.5f;
    private float knockbackCounter;
    public Vector2 knockbackPower;

    public GameObject[] playerPieces;


    public bool stopMove;

    //[SerializeField] private FixedJoystick _joystick; // added Android movment
    //[SerializeField] private VariableJoystick viewJoystick; // added Android movment
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        theCam = Camera.main;
        // Button btn = jumpButton.GetComponent<Button>(); // added Android movment
    }

    // Update is called once per frame
    void Update()
    {
        //if (freeLookCam != null)
        //{
        //    freeLookCam.m_XAxis.Value += -viewJoystick.Direction.x * 2; // added Android movment
        //    freeLookCam.m_YAxis.Value += viewJoystick.Direction.y / 100f; // added Android movment
        //}


        if (!isKnocking && !stopMove)
        {



            float yStore = moveDirection.y;

            // moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
           // moveDirection = (transform.forward * _joystick.Vertical) + (transform.right * _joystick.Horizontal);// added Android movment
            moveDirection.Normalize();
            moveDirection = moveDirection * moveSpeed;
            moveDirection.y = yStore;

            if (charController.isGrounded)
            {
                moveDirection.y = -1f; // Starts 2 animations at once for some reason // fix sometime
                if (Input.GetButtonDown("Jump") )
                {
                    Jump();
                }
            }


            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            // transform.position = transform.position + (moveDirection * Time.deltaTime * moveSpeed);

            charController.Move(moveDirection * Time.deltaTime);
            // || _joystick.Horizontal != 0 || _joystick.Vertical != 0
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 )
            {

                transform.rotation = Quaternion.Euler(0f, theCam.transform.rotation.eulerAngles.y, 0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                // playerModel.transform.rotation = newRotation;
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }
        }
        if(isKnocking)
        {
            knockbackCounter -= Time.deltaTime;


            float yStore = moveDirection.y;
            moveDirection = playerModel.transform.forward * -knockbackPower.x;
            moveDirection.y = yStore;

            if (charController.isGrounded)
            {
                moveDirection.y = -1f; // Starts 2 animations at once for some reason // fix sometime
            }

            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            charController.Move(moveDirection * Time.deltaTime);

            if (knockbackCounter <= 0)
            {
                isKnocking = false;
            }    
        }

        if(stopMove)
        {
            moveDirection = Vector3.zero;
            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            charController.Move(moveDirection);
        }
        
        anim.SetFloat("Speed",Mathf.Abs( moveDirection.x + moveDirection.z));
        anim.SetBool("Grounded", charController.isGrounded);
    }

    public void Knockback()
    {
        isKnocking = true;
        knockbackCounter = knockbackLength;
        UnityEngine.Debug.Log("Knocked Back");
        moveDirection.y = knockbackPower.y;
        charController.Move(moveDirection * Time.deltaTime);
    }

    public void Bounce()
    {
        moveDirection.y = bounceForce;
        charController.Move(moveDirection * Time.deltaTime);
    }

    public void Jump()
    {
        moveDirection.y = jumpForce;
        AudioManager.instance.PlaySFX(soundToPlay);
    }
}
