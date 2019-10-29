using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip gameoverClip;
    public void PlayMusic(AudioClip clip)
    {
        FirstSceneController scene = SSDirector.GetInstance().CurrentScenceController as FirstSceneController;
        AudioSource.PlayClipAtPoint(clip, scene.player.transform.position);
    }
    void Gameover()
    {
        PlayMusic(gameoverClip);
    }
}
