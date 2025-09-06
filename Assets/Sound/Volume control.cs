using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class Volumecontrol : MonoBehaviour
{
    public SettingsBindings.AudioSettings audioSettings = new();
    public AudioMixer mainAudioMixer ;
    public void Update()
    {
        mainAudioMixer.SetFloat("Volume_Music", audioSettings.MusicVolume);
        mainAudioMixer.SetFloat("Volume_SFX", audioSettings.SfxVolume);
        mainAudioMixer.SetFloat("Volume_Dialogue", audioSettings.Dialogue);
        mainAudioMixer.SetFloat("Volume_Master", audioSettings.Master);
    }


}
