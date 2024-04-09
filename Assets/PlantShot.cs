using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantShot : GameElement
{
    public Vector2 Direction;
    public float Speed;
    private Rigidbody2D mRigidbody2D;

    protected override void Awake()
    {
        this.mRigidbody2D = this.GetComponent<Rigidbody2D>();
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        this.mRigidbody2D.velocity = this.Direction * Speed;
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByPlantShot();
            this.Destroy();
        }
    }
}
