using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Testcar : MonoBehaviour
{

    private float vertical;
    private float horizontal;
    private bool Isbreaking;
    private float currentsteerangle;
    private float currentbreakForce;



    [SerializeField]
    private float motorforce = 1000f;
    public float steerangle = 30f;
    public float breakforce = 4000f;

    [SerializeField]
    private GameObject brakelightsleft;
    [SerializeField]
    private GameObject brakelightsright;


    public AudioSource player;
    public AudioClip acceleration;
    public AudioClip deceleration;
    public AudioClip brakesound;
    [SerializeField]
    private AudioClip startsound;

   // private countdown c;


    [Header("wheelColliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [Header("Transform")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    
    void Start()
    {

        player.PlayOneShot(startsound);
        brakelightsleft.SetActive(false);
        brakelightsright.SetActive(false);

    }

    public void getinput()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Isbreaking = Input.GetKey(KeyCode.Space);
    }


    private void FixedUpdate()
    {

      //  if (countdown.instasnce.gamestarted)
       // {

            UpdateWheels();
            getinput();
            steeringangle();
            handlemotor();
            currentbreakForce = Isbreaking ? breakforce : 0f;
            handlebrake();
      // }

    }
    public void steeringangle()
    {
        currentsteerangle = horizontal * steerangle;
        frontLeftWheelCollider.steerAngle = currentsteerangle;
        frontRightWheelCollider.steerAngle = currentsteerangle;
    }
    public void handlemotor()
    {

       accelerationsound();
        rearLeftWheelCollider.motorTorque = vertical * motorforce;
        rearRightWheelCollider.motorTorque = vertical * motorforce;


    }
    public void handlebrake()
    {
        brakelight();
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;

    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void brakelight()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            brakelightsleft.SetActive(true);
            brakelightsright.SetActive(true);

            if (player.clip != brakesound)
            {
                player.PlayOneShot(brakesound);
                player.clip = brakesound;
                player.loop = false;
                player.Play();
            }


        }
        else
        {

            brakelightsleft.SetActive(false);
            brakelightsright.SetActive(false);
        }

    }
    public void accelerationsound()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            //player.PlayOneShot(acceleration);
            if (player.clip != acceleration)
            {
                player.clip = acceleration;
                player.loop = true;
                player.Play();
            }
            //player.clip = acceleration;
            //player.Play();
        }
        else if (rearLeftWheelCollider.motorTorque > 0 && player.clip != deceleration)
        {

            //player.PlayOneShot(deceleration);
            player.clip = deceleration;
            player.Play();
        }

    }


}
