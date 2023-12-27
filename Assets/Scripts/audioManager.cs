using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public AudioClip bgmusic;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = bgmusic;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.I.isstart == true && !audioSource.isPlaying){
            audioSource.Play();
        }
    }

    public void pauseMusic(){
        audioSource.Pause();
    }

    public void playMusic(){
        audioSource.Play();
    }

    public void stopMusic(){
        audioSource.Stop();
    }
}
