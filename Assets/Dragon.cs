using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public LookDirection mLookDir;

    private void Awake()
    {
        mLookDir = this.GetComponent<LookDirection>();
    }

    private void Start()
    {
        mLookDir.LookLeft = GameManager.DistanceToPlayerInX(this.transform) > 0;
    }

    public void SpawnDragonShot()
    {
        GameManager.CurrentLevel.SpawnDragonShot(this.transform.position, !this.mLookDir.LookLeft);
    }
}
