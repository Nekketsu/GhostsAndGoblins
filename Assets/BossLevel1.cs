using System;
using System.Collections;
using UnityEngine;

public class BossLevel1 : GameElement
{
    public float JumpForceNewtons = 1500f;
    private float TimeInState = 0f;
    private float mInitialPosY = 0;
    public eBossLevel1State State = eBossLevel1State.StandStill;
    public AudioClip AudioDeath;
    public AudioClip AudioHit;

    protected override void Start()
    {
        this.mInitialPosY = this.transform.position.y;
        base.Start();
    }

    protected override void Update()
    {
        // Do not update until visible
        if (!IsVisible)
        {
            return;
        }

        UpdateState();
        UpdateAnimator();
        UpdateVelocity();

        base.Update();
    }

    private void UpdateState()
    {
        this.TimeInState += Time.deltaTime;

        if (TimeInState > 1)
        {
            switch (State)
            {
                case eBossLevel1State.StandStill:
                    if (GameManager.FlipCoin())
                    {
                        Walk();
                    }
                    else
                    {
                        Jump();
                    }
                    break;
                case eBossLevel1State.Walk:
                    if (GameManager.FlipCoin())
                    {
                        Stop();
                    }
                    else
                    {
                        Jump();
                    }
                    break;
                case eBossLevel1State.Jump:
                    if (this.transform.position.y < mInitialPosY + 0.1)
                    {
                        Stop();
                    }
                    break;
            }
        }
    }

    private void UpdateAnimator()
    {
        switch (this.State)
        {
            case eBossLevel1State.StandStill:
                mAnimator.Play("BossStandStill");
                break;
            case eBossLevel1State.Walk:
                mAnimator.Play("BossWalk");
                break;
            case eBossLevel1State.Jump:
                mAnimator.Play("BossJump");
                break;
        }
    }

    private void UpdateVelocity()
    {
        switch (this.State)
        {
            case eBossLevel1State.StandStill:
                this.mRigidBody.velocity = Vector2.zero;
                break;
            case eBossLevel1State.Walk:
                this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, 0f);
                break;
            case eBossLevel1State.Jump:
                this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, this.mRigidBody.velocity.y);
                break;
        }
    }

    private void Walk()
    {
        this.State = eBossLevel1State.Walk;
        TimeInState = 0;
    }

    private void Jump()
    {
        this.State = eBossLevel1State.Jump;
        mRigidBody.AddForce(new Vector2(0f, JumpForceNewtons));
        TimeInState = 0;
    }

    private void Stop()
    {
        this.State = eBossLevel1State.StandStill;
        TimeInState = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByBoss();

            this.HitByPlayerShot();
        }
    }

    public override void HitByPlayerShot()
    {
        base.HitByPlayerShot();

        AudioSource.PlayClipAtPoint(AudioHit, this.transform.position, 1f);
    }

    public override void Destroy()
    {
        base.Destroy();
        GameManager.CurrentLevel.SpawnFxDeathFire(this.transform.position);
        GameManager.CurrentLevel.StopBackgroundMusic();

        AudioSource.PlayClipAtPoint(GameManager.CurrentLevel.StageClearMusic, this.transform.position, 1f);
        //StartCoroutine(PlayBossKilled());
    }

    private IEnumerator PlayBossKilled()
    {
        AudioSource.PlayClipAtPoint(AudioDeath, this.transform.position, 1f); // 0.75f
        yield return new WaitForSeconds(AudioDeath.length);
        AudioSource.PlayClipAtPoint(GameManager.CurrentLevel.StageClearMusic, this.transform.position, 1f);
    }
}
