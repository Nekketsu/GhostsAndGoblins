using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private AudioSource mAudioSource;

    public float TimeRemaining = 120;
    public GameObject PrefabDeathFire;

    public GameObject PrefabVanish;
    public GameObject PrefabSplash;
    public GameObject PrefabDragon;
    public GameObject PrefabDragonShot;
    public GameObject PrefabFlyingGhostShot;
    public GameObject PrefabPickUp;
    public GameObject PrefabTreasureBox;
    public AudioClip StageClearMusic;

    public List<Ladder> Ladders = new List<Ladder>();
    public List<MovingPlatform> MovingPlatforms = new List<MovingPlatform>();

    private void Awake()
    {
        mAudioSource = this.GetComponent<AudioSource>();
        GameManager.CurrentLevel = this;

        // Clean the Ladders Collection. WARNING: This clear should be done BEFORE the ladders start registering themselves in this collection
        Ladders.Clear();
        MovingPlatforms.Clear();
    }

    private void Update()
    {
        this.TimeRemaining -= Time.deltaTime;
        if (TimeRemaining < 0)
        {
            GameManager.Player.DieArthurDie();
        }
    }

    public void StopBackgroundMusic()
    {
        mAudioSource.Stop();
    }

    public string GetRemainingTimeFormatted()
    {
        var time = TimeSpan.FromSeconds(TimeRemaining);

        return time.ToString(@"m\:ss");
    }

    public void SpawnFxDeathFire(Vector3 pPosition)
    {
        var newGO = GameObject.Instantiate(this.PrefabDeathFire, pPosition, Quaternion.identity);
    }

    public void SpawnFxVanish(Vector3 pPosition)
    {
        var newGO = GameObject.Instantiate(this.PrefabVanish, pPosition, Quaternion.identity);
    }

    public void SpawnFxSplash(Vector3 pPosition)
    {
        var newGO = GameObject.Instantiate(this.PrefabSplash, pPosition, Quaternion.identity);
    }

    public void SpawnDragon(Vector3 pPosition)
    {
        var newGO = GameObject.Instantiate(this.PrefabDragon, pPosition, Quaternion.identity);
    }

    public void SpawnDragonShot(Vector3 pPosition, bool pLookLeft)
    {
        var newGO = GameObject.Instantiate(this.PrefabDragonShot, pPosition, Quaternion.identity);
        newGO.GetComponent<LookDirection>().LookLeft = pLookLeft;
    }

    public void SpawnFlyingGhostShot(Vector3 pPosition)
    {
        var newGO = GameObject.Instantiate(this.PrefabFlyingGhostShot, pPosition, Quaternion.identity);
        newGO.GetComponent<LookDirection>().LookLeft = true;
    }

    public void SpawnPickUp(ePickupType pType, Vector2 pPos)
    {
        var newGO = GameObject.Instantiate(this.PrefabPickUp, pPos, Quaternion.identity);
        newGO.GetComponent<Pickup>().Type = pType;
        newGO.transform.position = pPos;
    }

    public void SpawnTreasureBox(ePickupType pType, Vector3 pPos)
    {
        var newGO = GameObject.Instantiate(this.PrefabTreasureBox, pPos, Quaternion.identity);
        newGO.GetComponent<TreasureBox>().Type = pType;
        newGO.transform.position = pPos;
    }
}
