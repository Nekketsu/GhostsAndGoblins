using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : GameElement
{
    private AudioSource mAudioSource;
    //private CircleCollider2D mCollider;
    public BoxCollider2D mCollider;
    public BoxCollider2D mGroundCheckCollider;
    private List<PlayerShot> mShots = new List<PlayerShot>();
    private ePlayerState mStateBeforeShooting = ePlayerState.Idle;
    private float mGravityScaleOriginaValue;


    public GameObject PrefabShot;

    [Header("Player Status")]
    public int Lives = 3;
    public int Score = 0;
    public ePlayerState State = ePlayerState.Idle;
    public ePlayerArmorState ArmorState = ePlayerArmorState.FullArmor;
    public eWeapon CurrentWeapon = eWeapon.Spear;

    [Header("Dynamics")]
    public float JumpForceNewtons = 1200f;
    public float PlayerSpeedLadder = 10f;

    [Header("Environmental Info")]
    public MovingPlatform InMovingPlatform;


    public Ladder InLadder;
    public Collider2D LadderClimbDownCollider;
    public bool Grounded = false;
    public bool ReadyToClimbUp;
    public bool ReadyToClimbDown;

    [Header("Audio Clips")]
    public AudioClip AudioJump;
    public AudioClip AudioThrow;
    public AudioClip AudioHit;
    public AudioClip AudioDeath;
    public AudioClip AudioArmorPickup;
    public AudioClip AudioTreasurePickup;

    public AudioClip AudioWeaponPickup;

    public bool IsShooting => this.State == ePlayerState.Shoot || this.State == ePlayerState.ShootCrouch;
    public bool IsClimbing => this.State == ePlayerState.Climb;
    public bool IsNaked => this.ArmorState == ePlayerArmorState.Naked;
    public bool IsFrog => this.ArmorState == ePlayerArmorState.Frog;

    protected override void Awake()
    {
        GameManager.Player = this;

        base.Awake();
        //mGroundCheckCollider = this.GetComponent<BoxCollider2D>();
        //mCollider = this.GetComponent<CircleCollider2D>();
        mAudioSource = this.GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        this.Lives = 3;
        this.Score = 0;
        mGravityScaleOriginaValue = this.mRigidBody.gravityScale;
        base.Start();
    }

    public void Pickup(Pickup pickup)
    {
        switch (pickup.Type)
        {
            case ePickupType.Armor:
                this.ArmorState = ePlayerArmorState.FullArmor;
                break;
            case ePickupType.Dagger:
                this.CurrentWeapon = eWeapon.Dagger;
                break;
            case ePickupType.Spear:
                this.CurrentWeapon = eWeapon.Spear;
                break;
            case ePickupType.Torch:
                this.CurrentWeapon = eWeapon.Torch;
                break;
            case ePickupType.Cross:
                this.Score += 200;
                break;
            case ePickupType.Axe:
                this.CurrentWeapon = eWeapon.Axe;
                break;
            case ePickupType.Shield:
                this.CurrentWeapon = eWeapon.Shield;
                break;
            case ePickupType.None:
                break;
        }

        this.mAudioSource.clip = this.AudioTreasurePickup;
        this.mAudioSource.Play();
    }

    public void HitByCrow()
    {
        Hit();
    }

    public void HitByPlant()
    {
        Hit();
    }

    public void HitByPlantShot()
    {
        Hit();
    }

    public void HitByZombie()
    {
        Hit();
    }

    public void HitByKnight()
    {
        Hit();
    }

    public void HitByDiablo()
    {
        Hit();
    }

    public void HitByFlyingGhostShot()
    {
        Hit();
    }

    internal void HitByBoss()
    {
        Hit();
    }

    public void HitDragonShot()
    {
        this.ArmorState = ePlayerArmorState.Frog;
    }


    private void Hit()
    {
        switch (this.ArmorState)
        {
            case ePlayerArmorState.FullArmor:
                this.ArmorState = ePlayerArmorState.Naked;
                break;
            case ePlayerArmorState.Naked:
                this.DieArthurDie();
                break;
            case ePlayerArmorState.Frog:
                this.ArmorState = ePlayerArmorState.Naked;
                break;
        }

        mAudioSource.clip = AudioHit;
        mAudioSource.Play();
    }

    public void DieArthurDie()
    {
        mAudioSource.clip = AudioDeath;
        mAudioSource.Play();

        if (this.Lives > 0)
        {
            this.Lives--;
            return;
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        SearchLadders();
        SearchMovingPlatforms();

        UpdateGroundCheck();
        UpdateInput();
        UpdateAnimator();
        UpdatePhysics();

        base.Update();
    }

    private void UpdatePhysics()
    {
        if (LadderClimbDownCollider)
        {
            Physics2D.IgnoreCollision(mCollider, LadderClimbDownCollider, false);
        }

        switch (this.State)
        {
            case ePlayerState.Climb:
                this.mRigidBody.gravityScale = 0;

                if (LadderClimbDownCollider)
                {
                    Physics2D.IgnoreCollision(mCollider, LadderClimbDownCollider, true);
                }
                break;
            default:
                this.mRigidBody.gravityScale = mGravityScaleOriginaValue;
                break;
        }
    }

    private void SearchMovingPlatforms()
    {
        InMovingPlatform = null;
        foreach (var movingPlatform in GameManager.CurrentLevel.MovingPlatforms)
        {
            if (movingPlatform.TriggerDetector.bounds.Contains(this.mRigidBody.position))
            {
                InMovingPlatform = movingPlatform;
                break;
            }
        }
    }

    private void SearchLadders()
    {
        if (LadderClimbDownCollider != null)
        {
            Physics2D.IgnoreCollision(mCollider, LadderClimbDownCollider, false);
            LadderClimbDownCollider = null;
        }

        InLadder = null;
        ReadyToClimbUp = false;
        ReadyToClimbDown = false;

        foreach (var ladder in GameManager.CurrentLevel.Ladders)
        {
            if (ladder.PrincipalTrigger.bounds.Contains(transform.position))
            {
                InLadder = ladder;
                ReadyToClimbUp = true;
                ReadyToClimbDown = true;
            }
            else if (ladder.UpTrigger.bounds.Contains(transform.position))
            {
                ReadyToClimbDown = true;
            }
            else if (ladder.DownTrigger.bounds.Contains(transform.position))
            {
                ReadyToClimbUp = true;
            }

            if (ReadyToClimbDown)
            {
                LadderClimbDownCollider = ladder.ClimbDownEffector.gameObject.GetComponents<Collider2D>().FirstOrDefault(collider => collider.usedByEffector == true);
            }

            if (ReadyToClimbUp || ReadyToClimbDown)
            {
                break;
            }
        }
    }

    private void UpdateGroundCheck()
    {
        var groundBoxWorldPosition = (Vector2)this.transform.position + this.mGroundCheckCollider.offset;
        var colliders = Physics2D.OverlapBoxAll(groundBoxWorldPosition, this.mGroundCheckCollider.size, 0f);

        Grounded = false;
        foreach (var collider in colliders)
        {
            if (collider.tag != "Player" && collider.tag != "Ladders")
            {
                Grounded = true;
                break;
            }
        }
    }

    private void UpdateInput()
    {
        if (IsShooting)
        {
            return;
        }

        // SPECIAL CASE: When already in climbing state and not pressing any key, velocity should be zero
        if (IsClimbing && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            StopClimb();
        }

        // Parse Input
        if (Input.GetKeyDown(KeyCode.Z) && Grounded)
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Shoot(Input.GetKey(KeyCode.DownArrow));
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !IsClimbing && !ReadyToClimbDown)
        {
            Crouch();
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Grounded)
        {
            MoveRight();
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Grounded)
        {
            MoveLeft();
        }
        else if (ReadyToClimbUp && Input.GetKey(KeyCode.UpArrow))
        {
            ClimbUp();
        }
        else if (ReadyToClimbDown && Input.GetKey(KeyCode.DownArrow))
        {
            ClimbDown();
        }
        else
        {
            if (Grounded)
            {
                Stop();
            }
            if (InMovingPlatform != null)
            {
                StickToMovingPlatform();
            }
        }
    }

    private void StickToMovingPlatform()
    {
        this.mRigidBody.velocity = new Vector2(this.InMovingPlatform.mRigidBody.velocity.x, this.mRigidBody.velocity.y);
    }

    private void StopClimb()
    {
        this.mRigidBody.velocity = Vector2.zero;
    }

    private void ClimbUp()
    {
        this.State = ePlayerState.Climb;
        mRigidBody.velocity = new Vector2(0, PlayerSpeedLadder);
    }

    private void ClimbDown()
    {
        this.State = ePlayerState.Climb;
        mRigidBody.velocity = new Vector2(0, -PlayerSpeedLadder);
    }

    private void Shoot(bool pCrouch)
    {
        if (mShots.Count < 3)
        {
            mStateBeforeShooting = this.State;

            SpawnShot();

            if (pCrouch)
            {
                this.State = ePlayerState.ShootCrouch;
            }
            else
            {
                this.State = ePlayerState.Shoot;
            }

            mAudioSource.clip = this.AudioThrow;
            mAudioSource.Play();
        }
    }

    private void Crouch()
    {
        this.State = ePlayerState.Crouch;
    }

    private void Stop()
    {
        mRigidBody.velocity = new Vector2(0, mRigidBody.velocity.y);
        this.State = ePlayerState.Idle;
    }

    private void Jump()
    {
        mRigidBody.AddForce(new Vector2(0, JumpForceNewtons));

        mAudioSource.clip = this.AudioJump;
        mAudioSource.Play();
    }

    private void MoveLeft()
    {
        mRigidBody.velocity = new Vector2(-SpeedX, mRigidBody.velocity.y);
        mLookDir.LookLeft = true;
        this.State = ePlayerState.Run;
    }

    private void MoveRight()
    {
        mRigidBody.velocity = new Vector2(SpeedX, mRigidBody.velocity.y);
        mLookDir.LookLeft = false;
        this.State = ePlayerState.Run;
    }

    private void UpdateAnimator()
    {
        // Reset AnimatorSpeed to its default value
        mAnimator.speed = 1;

        // Make the Jump animation preceed any other anymation, excep the Shoot animation
        // The second condition (!IsShooting) allows this, to play the shoot animation even if we are in the middle of a Jump
        if (!Grounded && !IsShooting && !IsClimbing)
        {
            if (IsFrog)
            {
                mAnimator.Play("FrogJump");
            }
            else if (Math.Abs(this.mRigidBody.velocity.x) > 1)
            {
                mAnimator.Play(IsNaked ? "JumpRunningNaked" : "JumpRunning");
            }
            else
            {
                mAnimator.Play(IsNaked ? "JumpIdleNaked" : "JumpIdle");
            }

            return;
        }

        // Update animator
        switch (this.State)
        {
            case ePlayerState.Idle:
                switch (this.ArmorState)
                {
                    case ePlayerArmorState.FullArmor:
                        mAnimator.Play("Idle");
                        break;
                    case ePlayerArmorState.Naked:
                        mAnimator.Play("IdleNaked");
                        break;
                    case ePlayerArmorState.Frog:
                        mAnimator.Play("FrogIdle");
                        break;
                }
                break;
            case ePlayerState.Run:
                switch (this.ArmorState)
                {
                    case ePlayerArmorState.FullArmor:
                        mAnimator.Play("Run");
                        break;
                    case ePlayerArmorState.Naked:
                        mAnimator.Play("RunNaked");
                        break;
                    case ePlayerArmorState.Frog:
                        mAnimator.Play("Frog");
                        break;
                }
                break;
            case ePlayerState.Climb:
                mAnimator.Play(IsNaked ? "ClimbNaked" : "Climb");

                // Stop animation if player is not moving up or down
                if (Mathf.Abs(mRigidBody.velocity.y) < 0.01f)
                {
                    mAnimator.speed = 0;
                }
                break;
            case ePlayerState.Crouch:
                switch (this.ArmorState)
                {
                    case ePlayerArmorState.FullArmor:
                        mAnimator.Play("Crouch");
                        break;
                    case ePlayerArmorState.Naked:
                        mAnimator.Play("CrouchNaked");
                        break;
                    case ePlayerArmorState.Frog:
                        mAnimator.Play("Frog");
                        break;
                }
                break;
            case ePlayerState.Die:
                mAnimator.Play("Death");
                break;
            case ePlayerState.LosingArmour:
                mAnimator.Play("ArmourLost");
                break;
            case ePlayerState.Shoot:
                switch (this.ArmorState)
                {
                    case ePlayerArmorState.FullArmor:
                        mAnimator.Play("Shoot");
                        break;
                    case ePlayerArmorState.Naked:
                        mAnimator.Play("ShootNaked");
                        break;
                    case ePlayerArmorState.Frog:
                        mAnimator.Play("Frog");
                        break;
                }
                break;
            case ePlayerState.ShootCrouch:
                switch (this.ArmorState)
                {
                    case ePlayerArmorState.FullArmor:
                        mAnimator.Play("ShootCrouch");
                        break;
                    case ePlayerArmorState.Naked:
                        mAnimator.Play("ShootCrouchNaked");
                        break;
                    case ePlayerArmorState.Frog:
                        mAnimator.Play("Frog");
                        break;
                }
                break;
        }
    }

    private void SpawnShot()
    {
        var offset = this.State == ePlayerState.Crouch ? new Vector3(0, 0.75f, 0) : new Vector3(0, 1.6f, 0);
        var newObj = GameObject.Instantiate(this.PrefabShot, this.transform.position + offset, Quaternion.identity);
        var dir = newObj.GetComponent<LookDirection>();
        dir.LookLeft = this.mLookDir.LookLeft;
        var shot = newObj.GetComponent<PlayerShot>();
        shot.ShotType = this.CurrentWeapon;
        mShots.Add(shot);
    }

    public void DestroyShot(PlayerShot pShot)
    {
        GameObject.Destroy(pShot.gameObject);

        if (mShots.Contains(pShot))
        {
            mShots.Remove(pShot);
        }
    }

    public void ShootAnimationFinished()
    {
        this.State = mStateBeforeShooting;
    }
}