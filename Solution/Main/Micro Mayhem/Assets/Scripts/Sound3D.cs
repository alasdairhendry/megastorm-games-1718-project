using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound3D : MonoBehaviour {

    private AudioSource source;
    private AudioClip clip = null;
    private bool loop = false;
    private float delay = 0.0f;
    private float killDelay = 0.0f;
    private bool hasPlayed = false;

    // Use this for initialization
    public void Init(AudioClip _clip, Transform _transform, bool _loop, float _delay, float volume, float minDistance, float maxDistance)
    {
        clip = _clip;
        loop = _loop;
        delay = _delay;

        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        transform.position = _transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.singleton.IsPaused)
            return;

        delay -= Time.deltaTime;

        if (delay <= 0)
        {
            if (!source.isPlaying)
            {
                if (!hasPlayed)
                {
                    if (loop)
                    {
                        source.loop = true;
                        source.Play();
                        hasPlayed = true;
                    }
                    else
                    {
                        killDelay = source.clip.length * 1.10f;
                        source.Play();
                        hasPlayed = true;

                        killDelay -= Time.deltaTime;

                        if (killDelay <= 0)
                            Destroy(gameObject);
                    }
                }
            }
            else
            {
                if (!loop)
                {
                    killDelay -= Time.deltaTime;

                    if (killDelay <= 0)
                        Destroy(gameObject);
                }
            }
        }
    }
}
