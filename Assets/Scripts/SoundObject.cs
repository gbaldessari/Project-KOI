using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public static bool listAreInitialized = false;
    private static List<AudioSource> musicSounds;
    private static List<AudioSource> effectsSounds;
    private static List<AudioSource> uiSounds;

    // Start is called before the first frame update
    void Awake()
    {
        if (listAreInitialized) return;

        musicSounds = transform.GetChild(0).GetComponentsInChildren<AudioSource>().ToList();
        effectsSounds = transform.GetChild(1).GetComponentsInChildren<AudioSource>().ToList();
        uiSounds = transform.GetChild(2).GetComponentsInChildren<AudioSource>().ToList();
        listAreInitialized = true;
    }

    /// <summary>
    /// Parará toda canción de fondo y reproducirá la requerida.
    /// </summary>
    /// <param name="level">Un número de nivel o 0 para la musica principal.</param>
    /// <returns></returns>
    public AudioSource GetAndPlayMusicBackground(int level)
    {
        foreach (AudioSource backgroundMusic in musicSounds) backgroundMusic.Stop();
        musicSounds[level].Play();
        musicSounds[level].loop = true;
        return musicSounds[level];
    }

    public void PlayMusicBackground(int level)
    {
        musicSounds[level].Play();
    }

    public void PlayUISelected()
    {
        uiSounds[0].Play();
    }

    public void PlayUIEnter()
    {
        uiSounds[1].Play();
    }

    public void PlayUIBack()
    {
        uiSounds[2].Play();
    }

    public void PlayUISpecial()
    {
        uiSounds[3].Play();
    }

    public void PlayEffectOnHit()
    {
        effectsSounds[0].Play();
    }

    public void PlayEffectFireShooting()
    {
        effectsSounds[1].Play();
    }

    public void PlayEffectProyectileShooting()
    {
        effectsSounds[2].Play();
    }

    public void PlayEffectCoinCollected()
    {
        effectsSounds[3].Play();
    }

    public void PlayEffectBasedMode()
    {
        effectsSounds[4].Play();
    }
}
