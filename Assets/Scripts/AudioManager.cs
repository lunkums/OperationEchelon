using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    private Dictionary<string, Sound> soundTypes;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        AudioSource source;
        soundTypes = new Dictionary<string, Sound>();

        foreach (Sound sound in sounds)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.Clip;
            sound.Source = source;
            soundTypes.Add(sound.Name, sound);
        }

        Instance = this;
    }

    public void Play(string clipName)
    {
        soundTypes[clipName].Play();
    }
}

public enum SoundType
{
    Music,
    SFX
}