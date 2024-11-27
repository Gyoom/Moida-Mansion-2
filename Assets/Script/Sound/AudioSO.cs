using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSO", menuName = "Scriptable Objects/AudioSO")]
public class AudioSO : ScriptableObject
{
    [field: Header("-----Audio-----")]
    [field: SerializeField] public List<SoundManager.Sound> soundList;
}
