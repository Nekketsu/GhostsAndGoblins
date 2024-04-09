using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : GameElement
{
    public GameObject PlantShotPrefab;
    public AudioClip AudioDeath;
    public AudioClip AudioShot;
    public float ShotsPeriodSecs = 4f;
    private float mTimeToNextShot;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        mTimeToNextShot = ShotsPeriodSecs;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (mRender.isVisible)
        {
            mTimeToNextShot -= Time.deltaTime;
            if (mTimeToNextShot <= 0)
            {
                SpawnPlantShot();
                mTimeToNextShot = ShotsPeriodSecs;
            }
        }

        base.Update();
    }

    private void SpawnPlantShot()
    {
        var newObj = GameObject.Instantiate(PlantShotPrefab, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        var shot = newObj.GetComponent<PlantShot>();
        shot.Direction = (GameManager.Player.transform.position + new Vector3(0, 1, 0) - this.transform.position).normalized;

        AudioSource.PlayClipAtPoint(AudioShot, this.transform.position, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByPlant();
            this.Destroy();
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        GameManager.CurrentLevel.SpawnFxDeathFire(this.transform.position);

        AudioSource.PlayClipAtPoint(AudioDeath, this.transform.position, 1f);
    }
}
