//using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] musics, sounds;
    IEnumerator classicSoundCO, bigBossSoundCo;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound s in musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        classicSoundCO = null; bigBossSoundCo = null;
        //PlayMusic("Music");
    }

    public float PlaySound(string name)
    {
        Debug.Log(name);
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) {
            s.source.Play();
            return s.source.clip.length;
        }
        else Debug.LogWarning("Sound " + name + " not found");
        return 0;
    }

    public void PlaySound(int i){
        if(i >= sounds.Length) Debug.LogWarning("Music " + i + " not found");
        sounds[i].source.Play();
    }

    void PauseMusic(string name){
        Sound s = Array.Find(musics, music => music.name == name);
        if (s != null) s.source.Pause();
        else Debug.LogWarning("Music " + name + " not found");
    }

    void PlayMusic(string name)
    {
        Sound s = Array.Find(musics, music => music.name == name);
        if (s != null) s.source.Play();
        else Debug.LogWarning("Music " + name + " not found");
    }

    float PlayMusic(int i){
        if(i >= musics.Length) Debug.LogWarning("Music " + i + " not found");
        musics[i].source.Play();
        return musics[i].source.clip.length;
    }

    public void StopAllMusics(){
        if(classicSoundCO != null) StopCoroutine(classicSoundCO);
        foreach (Sound item in musics)
        {
            item.source.Stop();
        }
    }

    #region Random tracks
    IEnumerator PlayRandomClassicSound(){
        while (true)
        {
            yield return new WaitForSecondsRealtime(PlayMusic(UnityEngine.Random.Range(1, musics.Length)));
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("i am in the scene " + SceneManager.GetActiveScene().name);
        StopAllMusics();
        if(SceneManager.GetActiveScene().name == "Menu"){
            PlayMusic("Menu");
        } else {
            PlayMusic("Traffic");
            PlayMusic("Acce");
        }
        // PlayMusic("Music");
        // if(SceneManager.GetActiveScene().name == "Classic") {
        //     PauseMusic("MainMenu");
        //     PlaySound("bird");
        //     classicSoundCO = PlayRandomClassicSound();
        //     StartCoroutine(classicSoundCO);
        // }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    #region settings
    public bool ToggleMusic(){
        foreach (Sound item in musics) item.source.mute = !item.source.mute;
        return musics[0].source.mute;
    }

    public bool ToggleSound(){
        foreach (Sound item in sounds) item.source.mute = !item.source.mute;
        return sounds[0].source.mute;
    }

    public void MusicVolume(float volume){
        foreach (Sound item in musics) item.source.volume = volume;
    }

    public void SoundVolume(float volume){
        foreach (Sound item in sounds) item.source.volume = volume;
    }
    #endregion
}
