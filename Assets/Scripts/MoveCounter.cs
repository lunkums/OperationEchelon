using TMPro;
using UnityEngine;

public class MoveCounter : MonoBehaviour
{
    [SerializeField] private Level level;
    [SerializeField] private Formation formation;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Sound sound;

    private int movesLeft;

    private void OnEnable()
    {
        formation.OnMoveAttempt += FormationMoveListener;
    }

    private void OnDisable()
    {
        formation.OnMoveAttempt -= FormationMoveListener;
    }

    private void Update()
    {
        text.text = "Moves left:\n" + level.MovesLeft;
    }

    private void FormationMoveListener(Move move)
    {
        if (!move.Valid)
            AudioManager.Instance.Play(sound.Name);
    }
}
