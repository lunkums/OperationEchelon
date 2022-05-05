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
        return rankSprites[troop.SignedRank + 4];
    }
}
