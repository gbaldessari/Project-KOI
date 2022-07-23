using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicContainer : MonoBehaviour
{
    private List<AudioSource> musicas;
    private List<AudioSource> efectos;

    // Start is called before the first frame update
    private void Start()
    {
        musicas = transform.GetChild(0).GetComponentsInChildren<AudioSource>().ToList();
        efectos = transform.GetChild(1).GetComponentsInChildren<AudioSource>().ToList();
    }
    
    public AudioSource GetMusic(string nombreMusica)
    {
        foreach(AudioSource musica in musicas)
        {
            if (musica.gameObject.name == nombreMusica)
            {
                return musica;
            }
        }

        throw new KeyNotFoundException("The specified music was not found");
    }

    public AudioSource GetEffect(string nombreAudio)
    {
        foreach (AudioSource efecto in efectos)
        {
            if (efecto.gameObject.name == nombreAudio)
            {
                return efecto;
            }
        }

        throw new KeyNotFoundException("The specified effect was not found");
    }
}
