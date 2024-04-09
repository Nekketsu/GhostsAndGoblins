using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Animator mAnimator;
    public ePickupType Type = ePickupType.None;

    private void Awake()
    {
        mAnimator = this.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (this.Type)
        {
            case ePickupType.Armor:
                this.mAnimator.Play("PrizeArmor");
                break;
            case ePickupType.Dagger:
                this.mAnimator.Play("PrizeDagger");
                break;
            case ePickupType.Spear:
                this.mAnimator.Play("PrizeSpear");
                break;
            case ePickupType.Torch:
                this.mAnimator.Play("PrizeFire");
                break;
            case ePickupType.Cross:
                this.mAnimator.Play("PrizeCross");
                break;
            case ePickupType.Axe:
                this.mAnimator.Play("PrizeAxe");
                break;
            case ePickupType.Shield:
                this.mAnimator.Play("PrizeShield");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Pickup(this);

            GameObject.Destroy(this.gameObject);
        }
    }
}
