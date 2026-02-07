using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float defaultMasterVolume = 0f;
    [SerializeField] private float defaultMusicVolume = -10f;
    [SerializeField] private float defaultFXVolume = -5f;

    private const string MasterVolumeKey = "Audio.MasterVolume";
    private const string MusicVolumeKey = "Audio.MusicVolume";
    private const string FXVolumeKey = "Audio.FXVolume";

    private void Start()
    {
        float master = PlayerPrefs.GetFloat(MasterVolumeKey, defaultMasterVolume);
        float music = PlayerPrefs.GetFloat(MusicVolumeKey, defaultMusicVolume);
        float fx = PlayerPrefs.GetFloat(FXVolumeKey, defaultFXVolume);

        audioMixer.SetFloat("MasterVolume", master);
        audioMixer.SetFloat("MusicVolume", music);
        audioMixer.SetFloat("FXVolume", fx);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat(MasterVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("FXVolume", volume);
        PlayerPrefs.SetFloat(FXVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public float GetMasterVolume()
    {
        audioMixer.GetFloat("MasterVolume", out float volume);
        return volume;
    }

    public float GetMusicVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float volume);
        return volume;
    }

    public float GetSFXVolume()
    {
        audioMixer.GetFloat("FXVolume", out float volume);
        return volume;
    }
}
