using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diablo : GameElement
{
    public float SpeedY = 8;
    public eDiabloState State = eDiabloState.StandStill;
    public int StateChangePeriod = 2;

    public float ActivationThreshold = 10;
    public bool Activated;

    private Vector2 mDirection = new Vector2();
    private float TimeInState = 0;
    private float TimeInCurDirection = 0;
    private float mInitialPosY;

    protected override void Start()
    {
        mInitialPosY = this.transform.position.y;
        base.Start();
    }

    protected override void Update()
    {
        if (Mathf.Abs(GameManager.DistanceToPlayerInX(this.transform)) < ActivationThreshold)
        {
            Activated = true;
        }

        UpdateState();
        UpdateAnimator();
        UpdateDirection();
        UpdateVelocity();

        base.Update();
    }

    private void UpdateState()
    {
        TimeInState += Time.deltaTime;

        // If player not close enough yet, alwaysin standstill
        if (!Activated)
        {
            this.State = eDiabloState.StandStill;
            return;
        }

        switch (this.State)
        {
            case eDiabloState.StandStill:
                if (TimeInState > StateChangePeriod)
                {
                    this.State = GameManager.FlipCoin() ? eDiabloState.Flying : eDiabloState.Walking;
                    TimeInState = 0;
                }
                break;
            case eDiabloState.Flying:
                if (TimeInState > StateChangePeriod && this.transform.position.y < mInitialPosY + 0.5f)
                {
                    this.State = GameManager.FlipCoin() ? eDiabloState.Flying : eDiabloState.Walking;
                    TimeInState = 0;
                }
                break;
            case eDiabloState.Walking:
                if (TimeInState > StateChangePeriod)
                {
                    this.State = GameManager.FlipCoin() ? eDiabloState.Flying : eDiabloState.Walking;
                    TimeInState = 0;
                }
                break;
        }
    }

    private void UpdateAnimator()
    {
        switch (this.State)
        {
            case eDiabloState.StandStill:
                mAnimator.Play("DiabloStand");
                break;
            case eDiabloState.Flying:
                mAnimator.Play("DiabloFly");
                break;
            case eDiabloState.Walking:
                mAnimator.Play("DiabloWalk");
                break;
        }
    }

    private void UpdateDirection()
    {
        TimeInCurDirection += Time.deltaTime;
        var dirToPlayer = mLookDir.LookLeft ? Vector2.left : Vector2.right;

        switch (this.State)
        {
            case eDiabloState.StandStill:
                mDirection = Vector2.zero;
                break;
            case eDiabloState.Flying:
                // Always go up at the beginning of state cycle
                if (TimeInState < 1)
                {
                    mDirection = Vector2.up;
                }
                else if (TimeInState < 3)
                {
                    if (TimeInCurDirection >= 1)
                    {
                        // In the middle, choose randomly every second
                        mDirection = GameManager.FlipCoin() ? dirToPlayer : Vector2.zero;
                        TimeInCurDirection = 0;
                    }
                }
                else
                {
                    // Always go down at the end of state cycle
                    mDirection = Vector2.down;
                }
                break;
            case eDiabloState.Walking:
                if (TimeInCurDirection >= 1)
                {
                    // Choose randomly every second
                    mDirection = GameManager.FlipCoin() ? dirToPlayer : Vector2.zero;
                    TimeInCurDirection = 0;
                }
                break;
        }
    }

    private void UpdateVelocity()
    {
        switch (this.State)
        {
            case eDiabloState.StandStill:
                mRigidBody.velocity = Vector2.zero;
                this.transform.position = new Vector3(this.transform.position.x, mInitialPosY, this.transform.position.z);
                break;
            case eDiabloState.Flying:
                mRigidBody.velocity = mDirection * SpeedY;
                break;
            case eDiabloState.Walking:
                mRigidBody.velocity = mDirection * SpeedX;
                this.transform.position = new Vector3(this.transform.position.x, mInitialPosY, this.transform.position.z);
                break;
        }
    }

    public override void Destroy()
    {
        base.Destroy();

        GameManager.CurrentLevel.SpawnFxDeathFire(this.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByDiablo();
            Destroy();
        }
    }
}
