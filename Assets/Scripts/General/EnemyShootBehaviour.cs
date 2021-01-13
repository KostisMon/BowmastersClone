using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This MonoBehaviour is added to the Enemy GameObject on Initialization
// Works like an AI scripts controling the Enemy Npc
public class EnemyShootBehaviour : MonoBehaviour
{
    public enum ShootStates { Aiming, Shoot, Iddle}
    private ShootStates m_CurShootState = ShootStates.Iddle;
    public Player PlayerScript;
    public float m_ShootAngle,m_AimAngle;
    private float m_AimTimer=0f, m_RandomAimDuration, m_ZoomOutDistance, m_RandomZoomOutTarget;

    void Update()
    {
        StateUpdate();
    }
    
    //Used for State controll
    private void StateUpdate()
    {
        switch (m_CurShootState)
        {
            //When aiming starts
            case ShootStates.Aiming:

                //Lerping the aim angle used for rotating the Enemy's arms
                m_AimAngle = Mathf.Lerp(0f, m_ShootAngle, m_AimTimer);
                //Lerping the Zoom distance to give the look of Powering up
                m_ZoomOutDistance = Mathf.Lerp(0f, m_RandomZoomOutTarget, m_AimTimer);
                //Rotating the Arms
                SetArmsRotation(m_AimAngle);
                //Setting the Orthographic size 
                CameraManager.Instance.SetCameraSize(m_ZoomOutDistance);
                //Controlling the lerping
                if (m_AimTimer < 1)
                {
                    //making it last the desired duration
                    m_AimTimer += Time.deltaTime / m_RandomAimDuration;
                }
                else
                {
                    //reseting the timer and setting the next state
                    m_AimTimer = 0f;
                    m_CurShootState = ShootStates.Shoot;
                }

                break;
            case ShootStates.Shoot:
                //Playing the throw sound
                AudioManager.Instance.PlayThrowSound();
                //Shoots the projectile
                Shoot();
                //Changing Game State
                GameManager.Instance.SetState(GameState.EnemyShoots);
                //And Shoot Behaviour state
                m_CurShootState = ShootStates.Iddle;
                break;
            case ShootStates.Iddle:
                break;
            default:
                break;
        }
    }

    //used to rotate arms around a specific axis (Z-axis)
    private void SetArmsRotation(float angle)
    {
        //Here due to the difference of the initial rotation of the arms
        //and the touch rotation I multiply to reach the desired angle
        PlayerScript.LeftArm.transform.rotation = Quaternion.AngleAxis(-angle * 2, Vector3.forward);
        PlayerScript.RightArm.transform.rotation = Quaternion.AngleAxis(-angle * 2, Vector3.forward);
    }

    //Works like initialization method when EnemyAims state is the current one
    public void StartShootSequence()
    {
        //First assinging a random aim duration
        m_RandomAimDuration = Random.Range(0.5f, 1.7f);
        //Then reset the lerp timer
        m_AimTimer = 0f;
        //assigning a random shoot angle
        m_ShootAngle = Random.Range(30f, 70f);
        //and a random zoom out float
        m_RandomZoomOutTarget = Random.Range(3f, 5f);
        //finally start the aim process
        m_CurShootState = ShootStates.Aiming;
    }

    //Used for shooting the Projectile
    public void Shoot()
    {
        //We get the distance between Characters
        float target_Distance = GameManager.Instance.CalculateCharDistance();
        //assing a radom error factor to look like Enemy might miss
        float randomErrorFactor = Random.Range( 0.90f, 1.05f);
        //getting the normalized direction 
        Vector2 direction = ((GameManager.Instance.Player.Body.transform.position) - (transform.position)).normalized;

        // Calculate the velocity needed to throw the object to the target at specified angle.
        // multiplying with the error factor for the random miss chance
        //Based on ballistic trajectory Math
        float projectile_Velocity = target_Distance*randomErrorFactor / (Mathf.Sin(2 * m_ShootAngle * Mathf.Deg2Rad) / (Physics2D.gravity.y * -1));

        // Extract the X Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(m_ShootAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(m_ShootAngle * Mathf.Deg2Rad);

        //creating a velocity Vector
        Vector2 vel = new Vector2(Vx * direction.x, Vy);
        //creating an initial rotation for the projectile to be Instantiated
        //again here as the prefab is created looking to the right I adjust the rotation 
        Quaternion rotation = Quaternion.AngleAxis(90+ m_ShootAngle, Vector3.forward);
        GameObject projectile = Instantiate(GameManager.Instance.ProjectilePrefab, PlayerScript.ProjectileStartPos.transform.position , rotation);
        //Parenting the projectile to keep things tidy
        GameManager.Instance.ParentProjectile(projectile);
        //Applying the velocity force
        projectile.GetComponent<Rigidbody2D>().AddForce(vel, ForceMode2D.Impulse);
        //There is always one current projectile so I setting it up
        GameManager.Instance.SetCurrentProjectile(projectile);
    }
}
