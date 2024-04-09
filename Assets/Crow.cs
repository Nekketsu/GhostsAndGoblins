using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : GameElement
{
    private bool IsFlying;
    public float ActivationThreshold = 15;
    public AudioClip AudioDeath;

    protected override void Start()
    {
        base.Start();

        // Crows shouldn't destroy when they are outside the screen if they are in flying state. Initially, when they are
        // standing on a grave, they should not be destroyed (they can be far away in the level).
        this.DestroyWhenNotVisible = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (IsFlying)
        {
            mAnimator.Play("CrowFly");
            this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, Mathf.Sin(Time.time * 2f) * 1.6f);
            this.DestroyWhenNotVisible = true;
        }
        else
        {
            mAnimator.Play("CrowStand");
            mRigidBody.velocity = Vector2.zero;
            mLookDir.LookLeft = GameManager.DistanceToPlayerInX(this.transform) < 0;
            this.DestroyWhenNotVisible = false;

            IsFlying = Mathf.Abs(GameManager.DistanceToPlayerInX(this.transform)) < ActivationThreshold;
        }

        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByCrow();
            this.Destroy();
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        GameManager.CurrentLevel.SpawnFxSplash(this.transform.position);

        AudioSource.PlayClipAtPoint(AudioDeath, this.transform.position, 1f);
    }
}
