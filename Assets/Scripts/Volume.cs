using UnityEngine;
using UnityEngine.UI;
public class Volume : MonoBehaviour
{
    private Slider volumeSlider;
    void Start()
    {
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(AdjustVolume);
    }
    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }
}
