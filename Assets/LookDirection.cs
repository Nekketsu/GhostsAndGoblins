using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDirection : MonoBehaviour
{
    private SpriteRenderer mRender;
    private bool mLookLeft = false;

    public bool LookLeft
    {
        get => mLookLeft;
        set
        {
            mLookLeft = value;
            mRender.flipX = mLookLeft;
        }
    }

    private void Awake()
    {
        this.mRender = this.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
