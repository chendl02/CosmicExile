using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel; 
    public Slider volumeSlider;      

    private void Start()
    {
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);

    }


    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }


    public void HideSettings()
    {
        settingsPanel.SetActive(false);
    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
