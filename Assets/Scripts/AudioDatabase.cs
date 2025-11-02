using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Audio/AudioDatabase")]
public class AudioDatabase : ScriptableObject
{
    [Serializable]
    public class AudioEntery
    {
        public string Name;
        public AudioClip Clip;
    }

    public List<AudioEntery> MusicList;
    public List<AudioEntery> SfxList;

    private Dictionary<string, AudioClip> musicMap;
    private Dictionary<string, AudioClip> sfxMap;

    public void Initialize()
    {
        musicMap = new Dictionary<string, AudioClip>();
        foreach (var entery in MusicList)
            musicMap[entery.Name] = entery.Clip;

        sfxMap = new Dictionary<string, AudioClip>();
        foreach (var entery in SfxList)
            sfxMap[entery.Name] = entery.Clip;
    }

    public AudioClip GetMusic(string name) => musicMap.TryGetValue(name, out var clip) ? clip : null;
    public AudioClip GetSFX(string name) => sfxMap.TryGetValue(name, out var clip) ? clip : null;
}
