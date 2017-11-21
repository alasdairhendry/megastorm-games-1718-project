using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour {

    public static SoundEffectManager singleton;
    [SerializeField] private GameObject sound2DPrefab;

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
}
