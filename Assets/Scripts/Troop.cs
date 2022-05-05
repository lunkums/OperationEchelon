using System;
using UnityEngine;

public class Troop : MonoBehaviour
{
    private static readonly string[] signToPrefix = { "Red ", "", "Blue "};

    [Range(0, 4)][SerializeField] private int rank = 0;
    [Range(-1, 1)] [SerializeField] private int sign = 1;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform _transform;

    public Rank Rank => (Rank)rank;
    public int Sign => sign;
    public int SignedRank => rank * sign;
    public Vector3 Position { set => _transform.position = Parent.position + value; }
    public Transform Parent { get => _transform.parent; set => _transform.parent = value; }
    public float Scale { set => _transform.localScale = Vector3.one * value; }

    public event Action OnRankChange;

    private void OnEnable()
    {
        OnRankChange += UpdateSprite;
        OnRankChange += UpdateName;
    }
    private void OnDisable()
    {
        OnRankChange -= UpdateSprite;
        OnRankChange -= UpdateName;
    }

    public bool Equals(Troop troop)
    {
        return (int)Rank * Sign == (int)troop.Rank * troop.Sign;
    }

    public void FlipSign()
    {
        SetSignedRank(SignedRank * -1);
    }

    public void Promote()
    {
        SetSignedRank((rank + 1) % 5 * sign);
    }

    public void Demote()
    {
        SetSignedRank(Math.Max(0, rank - 1) * sign);
    }

    public void Add(Troop troop)
    {
        SetSignedRank(SignedRank + troop.SignedRank);
    }

    public void SetSignedRank(int rank)
    {
        this.rank = Math.Abs(rank);
        sign = Math.Sign(rank);
        OnRankChange.Invoke();
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = TroopSpriteFactory.Instance.GetSprite(this);
    }

    private void UpdateName()
    {
        name = signToPrefix[sign + 1] + rank.ToString();
    }
}

public static class TroopExtensions
{
    public static bool CanAdd(this Troop t1, Troop t2)
    {
        return Math.Abs(t1.SignedRank + t2.SignedRank) <= (int)Rank.General;
    }
}