using UnityEngine;

public static class GameManager
{
    public static Player Player;
    public static Level CurrentLevel;

    public static float DistanceToPlayerInX(Transform pTransform)
    {
        return Player.transform.position.x - pTransform.position.x;
    }


    public static bool FlipCoin()
    {
        return Random.Range(0f, 1f) > 0.5f;
    }
}
