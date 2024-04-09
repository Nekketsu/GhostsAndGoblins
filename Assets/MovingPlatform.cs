using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D mRigidBody;
    private int Direction = 1;
    public float SpeedX;
    public float RangeM = 5;
    private float mInitialPosX;
    public BoxCollider2D TriggerDetector;


    private void Awake()
    {
        mRigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mInitialPosX = this.transform.position.x;

        // Ensure this platform is registered in the MovingPlatforms collection of the Level
        // WARNING: This MUST happen AFTER the MovingPlatforms collection is cleared in the Level, which is done in the AWAKE. So this must
        //          happen in the Start method.
        if (!GameManager.CurrentLevel.MovingPlatforms.Contains(this))
        {
            GameManager.CurrentLevel.MovingPlatforms.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mRigidBody.velocity = new Vector2(Direction * SpeedX, 0);

        float dif = this.transform.position.x - mInitialPosX;
        if (dif > RangeM)
        {
            Direction = -1;
        }
        if (dif < -RangeM)
        {
            Direction = 1;
        }
    }
}
