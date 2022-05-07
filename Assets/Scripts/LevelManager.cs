using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level currentLevel;
    [SerializeField] private Sound winSfx;
    [SerializeField] private Sound loseSfx;

    public static LevelManager Instance { get; private set; }

    public Level CurrentLevel => currentLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        currentLevel.OnWin += PlayWinSoundEffect;
    }

    private void OnDisable()
    {
        currentLevel.OnWin -= PlayWinSoundEffect;
    }

    private void PlayWinSoundEffect(bool hasWon)
    {
        string sfxName;

        if (hasWon)
            sfxName = winSfx.Name;
        else
            sfxName = loseSfx.Name;

        AudioManager.Instance.Play(sfxName);
    }
}
