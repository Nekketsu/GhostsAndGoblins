using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    private AudioSource mAudioSource;

    public AudioClip AudioHit;
    public int NumShotsToShowDragon = 10;


    private void Awake()
    {
        mAudioSource = this.GetComponent<AudioSource>();
    }

    public void HitByPlayerShot()
    {
        NumShotsToShowDragon--;
        if (NumShotsToShowDragon == 0)
        {
            GameManager.CurrentLevel.SpawnDragon(this.transform.position);
        }

        mAudioSource.clip = AudioHit;
        mAudioSource.Play();
    }
}
