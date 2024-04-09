using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LookDirection))]
[RequireComponent(typeof(SpriteRenderer))]
public class GameElement : MonoBehaviour
{
    public int NumberOfHitsToDie = 1;
    [HideInInspector]
    protected Rigidbody2D mRigidBody;
    protected SpriteRenderer mRender;
    protected Animator mAnimator;
    protected LookDirection mLookDir;
    public bool DestroyWhenNotVisible = false;
    public bool AlwaysLookToPlayer = false;
    public float SpeedX = 5f;

    public bool IsVisible => this.mRender.isVisible;
    public bool HasBeenVisible;

    protected virtual void Awake()
    {
        mRigidBody = this.GetComponent<Rigidbody2D>();
        mRender = this.GetComponent<SpriteRenderer>();
        mAnimator = this.GetComponent<Animator>();
        mLookDir = this.GetComponent<LookDirection>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        // Always look at the player
        if (AlwaysLookToPlayer)
        {
            mLookDir.LookLeft = GameManager.DistanceToPlayerInX(this.transform) < 0;
        }

        if (IsVisible)
        {
            HasBeenVisible = true;
        }

        if (DestroyWhenNotVisible && !IsVisible && HasBeenVisible)
        {
            Destroy();
        }

    }

    public virtual void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

    public virtual void HitByPlayerShot()
    {
        NumberOfHitsToDie--;

        if (NumberOfHitsToDie <= 0)
        {
            this.Destroy();
        }
    }
}