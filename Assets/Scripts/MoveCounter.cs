using TMPro;
using UnityEngine;

public class MoveCounter : MonoBehaviour
{
    [SerializeField] private Level level;
    [SerializeField] private TMP_Text text;

    private void Update()
    {
        text.text = "Moves left:\n" + level.MovesLeft;
    }
}
