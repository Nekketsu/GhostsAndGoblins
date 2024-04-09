using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Zombie : GameElement
{
    public ePickupType PickupType = ePickupType.None;
    public eZombieState State = eZombieState.Appearing;
    private BoxCollider2D mBoxCollider;
    public float MaxTimeLiving = 15f;
    private float mTimeLiving;
    private float mOriginalGravityValue;
    public AudioClip AudioDeath;
    public SpriteRenderer PickupIcon;

    protected override void Awake()
    {
        this.mBoxCollider = this.GetComponent<BoxCollider2D>();

        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        this.mOriginalGravityValue = this.mRigidBody.gravityScale;
        this.mBoxCollider.enabled = false;
        this.mTimeLiving = 0;
        this.PickupIcon.enabled = this.PickupType != ePickupType.None;

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        mTimeLiving += Time.deltaTime;
        if (mTimeLiving > MaxTimeLiving)
        {
            this.State = eZombieState.Disappearing;
        }

        switch (this.State)
        {
            case eZombieState.Appearing:
                this.mAnimator.Play("ZombieStart");
                this.mRigidBody.velocity = Vector2.zero;
                this.mRigidBody.gravityScale = 0;
                break;
            case eZombieState.Walking:
                this.mAnimator.Play("ZombieWalk");
                this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, this.mRigidBody.velocity.y);
                this.mRigidBody.gravityScale = mOriginalGravityValue;
                break;
            case eZombieState.Disappearing:
                this.mAnimator.Play("ZombieEnd");
                this.mRigidBody.velocity = Vector2.zero;
                this.mRigidBody.gravityScale = 0;
                break;
        }

        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var grave = collision.collider.GetComponent<Grave>();

        if (grave != null)
        {
            mLookDir.LookLeft = true;
        }

        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByZombie();
            this.Destroy();
        }
    }

    public void StartAnimationFinished()
    {
        this.State = eZombieState.Walking;
        this.mBoxCollider.enabled = true;
    }

    public override void Destroy()
    {
        base.Destroy();
        GameManager.CurrentLevel.SpawnFxSplash(this.transform.position);

        AudioSource.PlayClipAtPoint(AudioDeath, this.transform.position, 1f);

        if (this.PickupType != ePickupType.None)
        {
            GameManager.CurrentLevel.SpawnTreasureBox(this.PickupType, this.transform.position);
        }
    }
}
