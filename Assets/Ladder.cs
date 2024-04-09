using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public PlatformEffector2D ClimbDownEffector;
    public BoxCollider2D PrincipalTrigger;
    public BoxCollider2D UpTrigger;
    public BoxCollider2D DownTrigger;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure this ladder is registered in the Ladders collection of the level
        // WARNING: This MUST happen AFTER the ladders collectionis cleared in the Level, which is done in the AWAKE. So this must
        //          happen in the Start method.
        if (!GameManager.CurrentLevel.Ladders.Contains(this))
        {
            GameManager.CurrentLevel.Ladders.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
