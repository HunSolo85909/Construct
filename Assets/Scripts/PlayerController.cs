﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class PlayerController : MonoBehaviour
{
    public float drag_grounded;
    public float drag_inair;

    public DetectObs detectVaultObject; 
    public DetectObs detectVaultObstruction; 
    public DetectObs detectClimbObject; 
    public DetectObs detectClimbObstruction; 


    public DetectObs DetectWallL; 
    public DetectObs DetectWallR; 

    public Animator cameraAnimator;

    public float WallRunUpForce;
    public float WallRunUpForce_DecreaseRate;

    private float upforce;

    public float WallJumpUpVelocity;
    public float WallJumpForwardVelocity;
    public float drag_wallrun;
    public bool WallRunning;
    public bool WallrunningLeft;
    public bool WallrunningRight;
    private bool canwallrun; 
    
    public bool IsParkour;
    private float t_parkour;
    private float chosenParkourMoveTime;

    private bool CanVault;
    public float VaultTime; 
    public Transform VaultEndPoint;

    private bool CanClimb;
    public float ClimbTime; 
    public Transform ClimbEndPoint;

    private RigidbodyFirstPersonController rbfps;
    private Rigidbody rb;
    private Vector3 RecordedMoveToPosition;
    private Vector3 RecordedStartPosition;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rbfps = GetComponent<RigidbodyFirstPersonController>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rbfps.Grounded)
        {
            rb.drag = drag_grounded;
            canwallrun = true;
        }
        else
        {
            rb.drag = drag_inair;
        }
        if(WallRunning)
        {
            rb.drag = drag_wallrun;

        }
        //vault
        if (detectVaultObject.Obstruction && !detectVaultObstruction.Obstruction && !CanVault && !IsParkour && !WallRunning
            && (Input.GetKey(KeyCode.Space) || !rbfps.Grounded) && Input.GetAxisRaw("Vertical") > 0f)
        {
            CanVault = true;
        }

        if (CanVault)
        {
            CanVault = false; 
            rb.isKinematic = true; 
            RecordedMoveToPosition = VaultEndPoint.position;
            RecordedStartPosition = transform.position;
            IsParkour = true;
            chosenParkourMoveTime = VaultTime;

            cameraAnimator.CrossFade("Vault",0.1f);
        }

        //climb
        if (detectClimbObject.Obstruction && !detectClimbObstruction.Obstruction && !CanClimb && !IsParkour && !WallRunning
            && (Input.GetKey(KeyCode.Space) || !rbfps.Grounded) && Input.GetAxisRaw("Vertical") > 0f)
        {
            CanClimb = true;
        }

        if (CanClimb)
        {
            CanClimb = false; 
            rb.isKinematic = true; 
            RecordedMoveToPosition = ClimbEndPoint.position;
            RecordedStartPosition = transform.position;
            IsParkour = true;
            chosenParkourMoveTime = ClimbTime;

            cameraAnimator.CrossFade("Climb",0.1f);
        }


        //Parkour movement
        if (IsParkour && t_parkour < 1f)
        {
            t_parkour += Time.deltaTime / chosenParkourMoveTime;
            transform.position = Vector3.Lerp(RecordedStartPosition, RecordedMoveToPosition, t_parkour);

            if (t_parkour >= 1f)
            {
                IsParkour = false;
                t_parkour = 0f;
                rb.isKinematic = false;

            }
        }


        //Wallrun
        if (DetectWallL.Obstruction && !rbfps.Grounded && !IsParkour && canwallrun) 
        {
            WallrunningLeft = true;
            canwallrun = false;
            upforce = WallRunUpForce; 
        }

        if (DetectWallR.Obstruction && !rbfps.Grounded && !IsParkour && canwallrun) 
        {
            WallrunningRight = true;
            canwallrun = false;
            upforce = WallRunUpForce;
        }
        if (WallrunningLeft && !DetectWallL.Obstruction || Input.GetAxisRaw("Vertical") <= 0f || rbfps.relativevelocity.magnitude < 1f) 
        {
            WallrunningLeft = false;
            WallrunningRight = false;
            canwallrun = true;
        }
        if (WallrunningRight && !DetectWallR.Obstruction || Input.GetAxisRaw("Vertical") <= 0f || rbfps.relativevelocity.magnitude < 1f) 
        {
            WallrunningLeft = false;
            WallrunningRight = false;
            canwallrun = true;
        }

        if (WallrunningLeft || WallrunningRight) 
        {
            WallRunning = true;
            rbfps.Wallrunning = true; 
        }
        else
        {
            WallRunning = false;
            rbfps.Wallrunning = false;
        }

        if (WallrunningLeft)
        {     
            cameraAnimator.SetBool("WallLeft", true); 
        }
        else
        {
            cameraAnimator.SetBool("WallLeft", false);
        }
        if (WallrunningRight)
        {           
            cameraAnimator.SetBool("WallRight", true);
        }
        else
        {
            cameraAnimator.SetBool("WallRight", false);
        }

        if (WallRunning)
        {
            
            rb.velocity = new Vector3(rb.velocity.x, upforce ,rb.velocity.z); 
            upforce -= WallRunUpForce_DecreaseRate * Time.deltaTime; 

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = transform.forward * WallJumpForwardVelocity + transform.up * WallJumpUpVelocity;
                WallrunningLeft = false;
                WallrunningRight = false;
                StartCoroutine(WallrunWait());
            }
            if(rbfps.Grounded)
            {
                WallrunningLeft = false;
                WallrunningRight = false;
            }
        }

        //footstep sound
        if(rbfps.Grounded && rb.velocity.magnitude > 2f && !GameManager.isComplete && !PauseMenu.isPaused || WallRunning && rb.velocity.magnitude > 2f && !GameManager.isComplete && !PauseMenu.isPaused)
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    IEnumerator WallrunWait()
    {
        yield return new WaitForSeconds(0.5f);
        canwallrun = true;
    }
}
