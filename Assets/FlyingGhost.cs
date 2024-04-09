using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGhost : GameElement
{
    private float TimeInState = 0;
    private bool Moving = true;
    private bool ReadyToShoot = true;

    protected override void Update()
    {
        // Always Look left
        mLookDir.LookLeft = true;

        if (!IsVisible)
        {
            return;
        }

        // Decide if we should keep moving
        TimeInState += Time.deltaTime;
        if (TimeInState > 2)
        {
            if (Moving && ReadyToShoot)
            {
                Moving = false;
            }
            else
            {
                Moving = true;
                if (ReadyToShoot)
                {
                    this.SpawnShot();
                    ReadyToShoot = false;
                }
            }

            TimeInState = 0;
        }

        UpdateVelocity();
        UpdateAnimator();

        base.Update();
    }

    private void SpawnShot()
    {
        GameManager.CurrentLevel.SpawnFlyingGhostShot(this.transform.position);
    }

    private void UpdateAnimator()
    {
        if (!Moving)
        {
            mAnimator.Play("FlyingGhostStandStill");
        }
        else
        {
            if (ReadyToShoot)
            {
                mAnimator.Play("FlyingGhostMovingWithArrow");
            }
            else
            {
                mAnimator.Play("FlyingGhostMovingNoArrow");
            }
        }
    }

    private void UpdateVelocity()
    {
        if (Moving)
        {
            mRigidBody.velocity = new Vector2(SpeedX, 0f);
        }
        else
        {
            mRigidBody.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByFlyingGhostShot();
            this.Destroy();
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        GameManager.CurrentLevel.SpawnFxDeathFire(this.transform.position);
    }
}
