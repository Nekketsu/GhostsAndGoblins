using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyKnight : GameElement
{
    private float ActivationTime = float.MinValue;

    protected override void Update()
    {
        // Always look left
        this.mLookDir.LookLeft = true;

        if (mRender.isVisible)
        {
            if (ActivationTime < 0)
            {
                ActivationTime = Time.time;
            }
            this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, Mathf.Sin((Time.time - ActivationTime) * 2.5f) * 9f);
        }
        else
        {
            this.mRigidBody.velocity = Vector2.zero;
        }

        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByKnight();
            this.Destroy();
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        GameManager.CurrentLevel.SpawnFxDeathFire(this.transform.position);
    }
}
