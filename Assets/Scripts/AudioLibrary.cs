
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "AudioLibrary",
    menuName = "Audio/Audio Library"
)]
public class AudioLibrary : ScriptableObject
{
    [Serializable]
    public class Sound
    {
        public string soundName;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;
    }

    [SerializeField] private List<Sound> sounds = new();

    private Dictionary<string, Sound> soundDictionary;

    private void OnEnable()
    {
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        soundDictionary = new Dictionary<string, Sound>(
            StringComparer.OrdinalIgnoreCase
        );

        foreach (Sound sound in sounds)
        {
            if (sound == null ||
                string.IsNullOrWhiteSpace(sound.soundName) ||
                sound.clip == null)
                continue;

            if (!soundDictionary.ContainsKey(sound.soundName))
                soundDictionary.Add(sound.soundName, sound);
            else
                Debug.LogWarning(
                    $"AudioLibrary: Nombre duplicado '{sound.soundName}'.",
                    this
                );
        }
    }

    public bool TryGetSound(string soundName, out Sound sound)
    {
        if (soundDictionary == null)
            BuildDictionary();

        return soundDictionary.TryGetValue(soundName, out sound);
    }
}
