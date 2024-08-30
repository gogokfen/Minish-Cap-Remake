using UnityEngine;
public class BGMController : MonoBehaviour
{
    [SerializeField] AudioSource bgm;
    public void ChangeVolume(float NewVolume)
    {
        bgm.volume = NewVolume;
    }
    public void ChangeBGM(AudioClip NewClip)
    {
        bgm.Stop();
        bgm.clip = NewClip;
        bgm.Play();
    }
}
