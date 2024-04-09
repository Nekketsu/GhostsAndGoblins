using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : GameElement
{
    public eWeapon ShotType = eWeapon.Spear;

    [Header("Sprites")]
    public Sprite Dagger;
    public Sprite Spear;
    public Sprite Torch;
    public Sprite Axe;
    public Sprite Shield;

    protected override void Start()
    {
        base.Start();

        switch (ShotType)
        {
            case eWeapon.Dagger:
                //this.mRender.sprite = this.Dagger;
                this.SpeedX = 30;
                this.mRigidBody.gravityScale = 0;
                break;
            case eWeapon.Spear:
                //this.mRender.sprite = this.Spear;
                //this.SpeedX = 40;
                this.SpeedX = 20;
                this.mRigidBody.gravityScale = 0;
                break;
            case eWeapon.Torch:
                //this.mRender.sprite = this.Torch;
                this.SpeedX = 25;
                this.mRigidBody.gravityScale = 16;
                this.mRigidBody.AddForce(new Vector2(mLookDir.LookLeft ? -1000 : 1000, 1000));
                break;
            case eWeapon.Axe:
                //this.mRender.sprite = this.Axe;
                this.SpeedX = 25;
                this.mRigidBody.gravityScale = 16;
                this.mRigidBody.AddForce(new Vector2(mLookDir.LookLeft ? -1000 : 1000, 1000));
                break;
            case eWeapon.Shield:
                //this.mRender.sprite = this.Shield;
                this.SpeedX = 20;
                this.mRigidBody.gravityScale = 0;
                break;
        }

        SpeedX = 30;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (this.ShotType == eWeapon.Dagger || this.ShotType == eWeapon.Shield || this.ShotType == eWeapon.Spear)
        {
            this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, 0);
        }

        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var element = collision.collider.GetComponent<GameElement>();
        if (element != null)
        {
            element.HitByPlayerShot();
        }

        var grv = collision.collider.GetComponent<Grave>();
        if (grv != null)
        {
            grv.HitByPlayerShot();
        }

        var treasure = collision.collider.GetComponent<TreasureBox>();
        if (treasure != null)
        {
            treasure.Destroy();
        }

        this.Destroy();
    }

    public override void Destroy()
    {
        GameManager.Player.DestroyShot(this);
        GameManager.CurrentLevel.SpawnFxVanish(this.transform.position + new Vector3(mLookDir.LookLeft ? -1 : 1, 0, 0));

        // Intentionally avoiding to call the base class, as we overwrite this behavior.
        // base.Destroy();
    }
}
