using UnityEngine;

public class AudioManager : SingletonBehaviour<AudioManager>
{
    private int _lifetimeAudioSourceCount = 0;
    public void PlayAudio(Audio audio)
    {
        GameObject sourceObj = new GameObject("Audio Source " + _lifetimeAudioSourceCount + ": " + audio.clip.name);
        AudioSource audioSource = sourceObj.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = audio.volume;
        audioSource.pitch = audio.pitch;
        audioSource.PlayOneShot(audio.clip);
        AutoDestroyTimer timer = sourceObj.AddComponent<AutoDestroyTimer>();
        timer.SetLifetime(audio.clip.length + 0.1f);
    }
}
