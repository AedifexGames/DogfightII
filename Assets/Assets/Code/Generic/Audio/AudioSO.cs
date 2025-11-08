using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioSO", menuName = "Scriptable Objects/AudioSO")]
public class AudioSO : ScriptableObject
{
    [SerializeField] private List<AudioClip> _clips;
    [SerializeField][Min(0)] private float _volume;
    [SerializeField] private float _volumeVariance;
    [SerializeField][Min(0)] private float _pitch;
    [SerializeField] private float _pitchVariance;

    public Audio GetAudio()
    {
        Audio result;
        result.clip = _clips[Random.Range(0, _clips.Count - 1)];
        result.volume = _volume + Random.Range(0f, _volumeVariance);
        result.pitch = _pitch + Random.Range(0f, _pitchVariance);
        return result;
    }
}
