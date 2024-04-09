using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    public ePickupType Type = ePickupType.None;

    public void Destroy()
    {
        GameManager.CurrentLevel.SpawnPickUp(this.Type, this.transform.position);
        GameObject.Destroy(this.gameObject);
    }
}
