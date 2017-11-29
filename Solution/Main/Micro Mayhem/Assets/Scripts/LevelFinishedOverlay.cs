using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFinishedOverlay : MonoBehaviour {

    public static LevelFinishedOverlay singleton;
    [SerializeField] private int indexToLoad;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    public void PlayerDied()
    {
        GetComponent<Animator>().SetBool("LevelFinished", true);
        GetComponentInChildren<Text>().text = "You Have Died!";
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void PlayerWon()
    {
        GetComponent<Animator>().SetBool("LevelFinished", true);
        GetComponentInChildren<Text>().text = "Level Complete!";
        StartCoroutine(LoadScene(indexToLoad));
    }

    private IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(index);
    }
}
