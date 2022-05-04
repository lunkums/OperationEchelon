using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Audio/Sound")]
public class Sound : ScriptableObject
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private SoundType type;
    [SerializeField] private string _name;

    public AudioClip Clip => clip;
    public SoundType Type => type;
    public string Name => _name;
    public AudioSource Source { get; set; }
    public float Volume { get => Source.volume; set => Source.volume = value; }

    public void Play()
    {
        Source.Play();
    }
}
