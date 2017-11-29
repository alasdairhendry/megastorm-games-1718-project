using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    public static GameState singleton;
    private bool isPaused = false;
    [SerializeField] private float startDelay = 3.5f;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        isPaused = true;
    }

    public void Pause() { isPaused = true; }
    public void Resume() { isPaused = false; }
    public bool IsPaused { get { return isPaused; } }

    private void Start()
    {      
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(startDelay);
        isPaused = false;
    }

}
