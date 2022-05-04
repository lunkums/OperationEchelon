using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    private Dictionary<string, Sound> soundMap;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        AudioSource source;
        soundMap = new Dictionary<string, Sound>();

        foreach (Sound sound in sounds)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.Clip;
            sound.Source = source;
            soundMap.Add(sound.Name, sound);
        }

        Instance = this;
    }

    public void Play(string clipName)
    {
        soundMap[clipName].Play();
    }
}

public enum SoundType
{
    Music,
    SFX
}