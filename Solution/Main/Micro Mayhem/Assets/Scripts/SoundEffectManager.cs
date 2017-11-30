using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class that allows us to spawn in 2D sounds with great control
/// </summary>
public class SoundEffectManager : MonoBehaviour {

    public static SoundEffectManager singleton;
    [SerializeField] private GameObject sound2DPrefab;
    [SerializeField] private GameObject sound3DPrefab;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    public void Play2DSound(AudioClip clip, bool loop, float delay, float volume)
    {
        GameObject go = Instantiate(sound2DPrefab);
        Sound2D sound = go.GetComponent<Sound2D>();
        sound.Init(clip, loop, delay, volume);
    }

    public void Play3DSound(AudioClip clip, Transform transform, bool loop, float delay, float volume, float minDistance, float maxDistance)
    {
        GameObject go = Instantiate(sound3DPrefab);
        Sound3D sound = go.GetComponent<Sound3D>();
        sound.Init(clip, transform, loop, delay, volume, minDistance, maxDistance);
    }
}
