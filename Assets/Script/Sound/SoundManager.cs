using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public AudioSO audioSO;
    [SerializeField] private MyAudioSource sourceGO;
    
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }


    public void SpawnAudio2D(Vector3 position, int audioID, bool loop = false)
    {
        MyAudioSource audioSource = Instantiate(sourceGO, position, Quaternion.identity, transform);

        Sound s;
        s = audioSO.soundList.Find(x => x.audioId - 1 == audioID);
        audioSource.aSource.clip = audioSO.soundList[audioID].audioClip;
        audioSource.timeBeforeDestroy = audioSO.soundList[audioID].audioDuration;
        audioSource.gameObject.name = audioSO.soundList[audioID].soundName;
            
        audioSource.aSource.volume = 0;
        audioSource.aSource.DOFade(1, .125f).SetUpdate(true);
        audioSource.aSource.loop = loop;
        audioSource.aSource.spatialBlend = 0;
            
        audioSource.aSource.Play();
    }
    
    public void SpawnAudio3D(Vector3 position, int audioID, float dopplerLevel = 1, float spread = 0, float minDist = 1, float maxDist = 75, bool loop = false)
    {
        MyAudioSource audioSource = Instantiate(sourceGO, position, Quaternion.identity, transform);

        Sound s;
        s = audioSO.soundList.Find(x => x.audioId - 1 == audioID);
        audioSource.aSource.clip = audioSO.soundList[audioID].audioClip;
        audioSource.timeBeforeDestroy = audioSO.soundList[audioID].audioDuration;
        audioSource.gameObject.name = audioSO.soundList[audioID].soundName;
            
        audioSource.aSource.volume = 0;
        audioSource.aSource.DOFade(1, .125f).SetUpdate(true);
        audioSource.aSource.loop = loop;
        audioSource.aSource.spatialBlend = 1;
        audioSource.aSource.dopplerLevel = dopplerLevel;
        audioSource.aSource.spread = spread;
        audioSource.aSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.aSource.minDistance = minDist;
        audioSource.aSource.maxDistance = maxDist;
            
        audioSource.aSource.Play();
    }
    
    
    [System.Serializable]
    public class Sound
    {
        [HideInInspector]public int audioId;
        public string soundName;
        public float audioDuration;
        public AudioClip audioClip;
    }
}
