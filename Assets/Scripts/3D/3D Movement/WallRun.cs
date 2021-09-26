﻿using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

public class WallRun : MonoBehaviour
{
    public float wallMaxDistance = 1;
    public float wallSpeedMultiplier = 1.2f;
    public float minimumHeight = 1.2f;
    public float maxAngleRoll = 20;
    [Range(0.0f, 1.0f)]
    public float normalizedAngleThreshold = 0.1f;
    
    public float jumpDuration = 1;
    public float wallBouncing = 3;
    public float cameraTransitionDuration = 1;

    public float wallGravityDownForce = 20f;

    public bool useSprint;


    [Space]
    public Volume wallRunVolume;

    PlayerMovement playerMovement;
    InputManager inpManager;

    Vector3[] directions;
    RaycastHit[] hits;

    public bool isWallRunning = false;
    Vector3 lastWallPosition;
    Vector3 lastWallNormal;
    float elapsedTimeSinceJump = 0;
    float elapsedTimeSinceWallAttach = 0;
    float elapsedTimeSinceWallDetatch = 0;
    bool jumping;
    float lastVolumeValue = 0;
    float noiseAmplitude;

    bool isPlayergrounded() => playerMovement.grounded;

    public bool IsWallRunning() => isWallRunning;

    bool CanWallRun()
    {
        float verticalAxis = Input.GetAxisRaw("Vertical"); // Change later
        bool isSprinting = true; // Change later
        isSprinting = !useSprint ? true : isSprinting;
        return !isPlayergrounded() && verticalAxis > 0 && VerticalCheck() && isSprinting;
    }

    bool VerticalCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeight);
    }


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        inpManager = InputManager.playerInp;

        if(wallRunVolume != null)
        {
            SetVolumeWeight(0);
        }
    }

	private void Update()
	{
        directions = new Vector3[]{
           playerMovement.orientation.right,
           -playerMovement.orientation.right,
       };
    }

	public void LateUpdate()
    {  
        isWallRunning = false;

        if(Input.GetKeyDown(inpManager.jump))
        {
            jumping = true;
        }

        if(CanAttach())
        {
            hits = new RaycastHit[directions.Length];
            for(int i=0; i<directions.Length; i++)
            {
                Vector3 dir = directions[i];
                Physics.Raycast(transform.position, dir, out hits[i], wallMaxDistance);
                if(hits[i].collider != null)
                {
                    Debug.DrawRay(transform.position, dir * hits[i].distance, Color.green);
                }
                else
                {
                    Debug.DrawRay(transform.position, dir * wallMaxDistance, Color.red);
                }
            }

            if(CanWallRun())
            {
                hits = hits.ToList().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();
                if(hits.Length > 0)
                {
                    OnWall(hits[0]);
                    lastWallPosition = hits[0].point;
                    lastWallNormal = hits[0].normal;
                }
            }
        }

        if(isWallRunning)
        {
            elapsedTimeSinceWallDetatch = 0;
            if(elapsedTimeSinceWallAttach == 0 && wallRunVolume != null)
            {
                lastVolumeValue = wallRunVolume.weight;
            }
            elapsedTimeSinceWallAttach += Time.deltaTime;
            //playerMovement.rb.velocity += Vector3.down * wallGravityDownForce * Time.deltaTime;
            playerMovement.rb.AddForce(Vector3.down * wallGravityDownForce * Time.deltaTime, ForceMode.Force);
        }
        else
        {   
            elapsedTimeSinceWallAttach = 0;
            if(elapsedTimeSinceWallDetatch == 0 && wallRunVolume != null)
            {
                lastVolumeValue = wallRunVolume.weight;
            }
            elapsedTimeSinceWallDetatch += Time.deltaTime;
        }

        if(wallRunVolume != null)
        {
            HandleVolume();
        }
    }

    bool CanAttach()
    {
        if(jumping)
        {
            elapsedTimeSinceJump += Time.deltaTime;
            if(elapsedTimeSinceJump > jumpDuration)
            {
                elapsedTimeSinceJump = 0;
                jumping = false;
            }
            return false;
        }
        
        return true;
    }

    void OnWall(RaycastHit hit){
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if(d >= -normalizedAngleThreshold && d <= normalizedAngleThreshold)
        {
            // Vector3 alongWall = Vector3.Cross(hit.normal, Vector3.up);
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 alongWall = transform.TransformDirection(Vector3.forward);

            Debug.DrawRay(transform.position, alongWall.normalized * 10, Color.white);
            Debug.DrawRay(transform.position, lastWallNormal * 10, Color.magenta);

            //playerMovement.rb.velocity = alongWall * vertical * wallSpeedMultiplier;
            playerMovement.rb.AddForce(alongWall * vertical * wallSpeedMultiplier);
            float stickForce = 5f;
            playerMovement.rb.AddForce(-lastWallNormal * stickForce);
            isWallRunning = true;
        }
    }

    float CalculateSide()
    {
        if(isWallRunning)
        {
            Vector3 heading = lastWallPosition - transform.position;
            Vector3 perp = Vector3.Cross(transform.forward, heading);
            float dir = Vector3.Dot(perp, transform.up);
            return dir;
        }
        return 0;
    }

    public float GetCameraRoll()
    {
        float dir = CalculateSide();
        float cameraAngle = playerMovement.playerCam.transform.eulerAngles.z;
        float targetAngle = 0;
        if(dir != 0)
        {
            targetAngle = Mathf.Sign(dir) * maxAngleRoll;
        }
        return Mathf.LerpAngle(cameraAngle, targetAngle, Mathf.Max(elapsedTimeSinceWallAttach, elapsedTimeSinceWallDetatch) / cameraTransitionDuration);
    } 

    public Vector3 GetWallJumpDirection()
    {
        if(isWallRunning)
        {
            Debug.Log((lastWallNormal * wallBouncing + Vector3.up).magnitude);
            return lastWallNormal * wallBouncing + Vector3.up * wallBouncing;
        }
        return Vector3.zero;
    } 

    void HandleVolume()
    {
        float w = 0;
        if(isWallRunning)
        {
            w = Mathf.Lerp(lastVolumeValue, 1, elapsedTimeSinceWallAttach / cameraTransitionDuration);
        }
        else
        {
            w = Mathf.Lerp(lastVolumeValue, 0, elapsedTimeSinceWallDetatch / cameraTransitionDuration);
        }

        SetVolumeWeight(w);
    }

    void SetVolumeWeight(float weight)
    {
        wallRunVolume.weight = weight;
    }
}
