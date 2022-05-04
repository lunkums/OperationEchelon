using System;
using UnityEngine;

public class Troop : MonoBehaviour
{
    private static readonly string[] signToPrefix = { "Red ", "", "Blue "};

    [SerializeField] private Rank rank = 0;
    [Range(-1, 1)] [SerializeField] private int sign = 1;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform _transform;

    public Rank Rank => rank;
    public int Sign => sign;
    public int SignedRank => (int)rank * sign;
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
        sign *= -1;
        OnRankChange.Invoke();
    }

    public void Promote()
    {
        SetSignedRank(rank.Next() * sign);
    }

    public void Demote()
    {
        SetSignedRank(rank.Previous() * sign);
    }

    public void Add(Troop troop)
    {
        int myRank = Math.Max(-1, (int)Math.Log((int)Rank, 2)) + 1;
        int theirRank = Math.Max(-1, (int)Math.Log((int)troop.Rank, 2)) + 1;
        SetSignedRank(myRank * Sign+ theirRank * troop.Sign);
    }

    public void SetSignedRank(int rank)
    {
        this.rank = (Rank)Math.Abs(rank);
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
