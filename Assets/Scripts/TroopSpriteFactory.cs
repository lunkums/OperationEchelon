using System;
using UnityEngine;

public class TroopSpriteFactory : MonoBehaviour
{
    [SerializeField] private Sprite[] rankSprites;

    public static TroopSpriteFactory Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public Sprite GetSprite(Troop troop)
    {
        return rankSprites[TroopToArrayPos(troop)];
    }

    // Gets the index in the Sprite array using a Troop's rank and sign.
    private int TroopToArrayPos(Troop troop)
    {
        int sign = troop.Sign;
        int log = (int)Math.Log(Math.Abs((int)troop.Rank) << 1, 2);
        log *= sign;
        return log + 4;
    }
}
