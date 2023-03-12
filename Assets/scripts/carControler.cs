using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carControler : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;
    bool onGround = false;
    float lastYRotation;
    [Range(0f, 1f)]
    [SerializeField] private float steerHelpValue = 0;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    [SerializeField] private Transform centrOfMass;

    Rigidbody rb;

    WheelCollider[] wheelColliders = new WheelCollider[4];

    [SerializeField] private bool privod;
    [SerializeField] private float nitro;
    [SerializeField] private GameObject nitroEffects;

    [Header("For Smoke From Tires")]
    public float minSpeedForSmoke;
    public float minAngeleForSmoke;
    public ParticleSystem[] tireSmokeEffects; 

    void Start()
    {
       wheelColliders[0]=frontLeftWheelCollider;
        wheelColliders[1]=frontRightWheelCollider;
        wheelColliders [2]=rearLeftWheelCollider;
        wheelColliders[3] =rearRightWheelCollider;

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centrOfMass.localPosition;
    }


    private void FixedUpdate()
    {
        CheekOnGround();
        
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        ManageNitro();
        Smoke();
        SteerHelpAssist();
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        if(privod)
        {
            rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
            rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
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

    void SteerHelpAssist()
    {
        if(!onGround)
        {
            return;
        }
        if(Mathf.Abs(transform.rotation.eulerAngles.y-lastYRotation)<10f)
        {
            float turnAbjest = (transform.rotation.eulerAngles.y - lastYRotation)*steerHelpValue;
            Quaternion rotationHelp = Quaternion.AngleAxis(turnAbjest, Vector3.up);
            rb.velocity = rotationHelp * rb.velocity;
        }
        lastYRotation = transform.rotation.eulerAngles.y;
    }

    void CheekOnGround()
    {
        onGround = true;

        foreach(WheelCollider wheelColl in wheelColliders)
        {
            if (!wheelColl.isGrounded)
            {
                onGround = false;
            }
        }
        
    }

    void ManageNitro()
    {
        if(Input.GetKey(KeyCode.LeftShift) && verticalInput > 0.01f)
        {
            rb.AddForce(transform.forward * nitro);
            nitroEffects.SetActive(true);
        }
        else
        {
            if(nitroEffects.activeSelf)
            {
                nitroEffects.SetActive(false);
            }
        }
    }

    void Smoke()
    {
        if (rb.velocity.magnitude > minSpeedForSmoke)
        {
            float angle = Quaternion.Angle(Quaternion.LookRotation(rb.velocity, Vector3.up), Quaternion.LookRotation(transform.forward, Vector3.up));
            if (angle > minAngeleForSmoke && angle < 160 && onGround)
                EnableParticl(true);
            else
            {
                EnableParticl(false);
            }
        }
        else
            EnableParticl(false);
    }

    void EnableParticl(bool _enable)
    {
        foreach(ParticleSystem ps in tireSmokeEffects)
        {
            ParticleSystem.EmissionModule psEm = ps.emission;
            psEm.enabled = _enable;
        }
    }
}