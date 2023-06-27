using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public enum Type{ BattleMusic, TravelMusic, VillageMusic, EventMusic }
    
    [System.Serializable]
    public class MusicCollection{
        public List<AudioClip> battleAudioClips;
        public List<AudioClip> travelAudioClips;
        public List<AudioClip> villageAudioClips;
        public List<AudioClip> eventAudioClips;
        
        public void SetRandomMusic(Type type){
            musicManager.audioData.clip = this.GetAudioClip(type);
        }
        
        public bool SetEventMusic(int index){
            if(index >= eventAudioClips.Count) return false;
            musicManager.audioData.clip = this.eventAudioClips[index];
            return true;
        }
        
        public AudioClip GetAudioClip(Type type){
            int randomIndex = Random.Range(0, GetListCount(type));
            return type switch{
                Type.BattleMusic => battleAudioClips[randomIndex],
                Type.TravelMusic => travelAudioClips[randomIndex],
                Type.VillageMusic => villageAudioClips[randomIndex],
                Type.EventMusic => eventAudioClips[randomIndex]
            };
        }
        
        public int GetListCount(Type type){
            return type switch{
                Type.BattleMusic => battleAudioClips.Count,
                Type.TravelMusic => travelAudioClips.Count,
                Type.VillageMusic => villageAudioClips.Count,
                Type.EventMusic => eventAudioClips.Count
            };
        }
    }
    
    public MusicCollection musicCollection;
    public static MusicManager musicManager;
    public AudioSource audioData;
    
    public bool playMusic;
    public bool stopMusic;
    
    void Awake(){
        musicManager = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Update(){
        if(playMusic) {
            audioData.volume += Time.deltaTime;
            if(audioData.volume == 1f) playMusic = false;
        }
        else if(stopMusic) {
            audioData.volume -= Time.deltaTime;
            if(audioData.volume == 0f) stopMusic = false;
        }
    }
    
    public void PlayMusic(Type type){
        musicCollection.SetRandomMusic(type);
        playMusic = true;
        audioData.Play();
        audioData.volume = 0f;
    }
    
    public void StopMusic(){
        stopMusic = true;
        audioData.Stop();
    }
    
    public bool PlayEventMusic(int index){
        if(!musicCollection.SetEventMusic(index)) return false;
        else {
            audioData.Play();
            return true;
        }
    }
}
