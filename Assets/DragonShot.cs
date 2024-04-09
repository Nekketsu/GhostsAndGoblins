using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonShot : GameElement
{
    protected override void Update()
    {
        mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, 0f);

        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitDragonShot();
            this.Destroy();
        }
    }
}
