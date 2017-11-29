using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private float score = 0.0f;

	// Use this for initialization
	void Start () {
        StartCoroutine(CheckScore());
	}
	
	// Update is called once per frame
	void Update () {
        GetComponentInChildren<Text>().text = score.ToString("000");
	}

    private IEnumerator CheckScore()
    {
        while(true)
        {
            yield return new WaitForSeconds(2.5f);
            float ratio = EntityRecords.singleton.GetInfectionData();
            float lerp = Mathf.InverseLerp(1, -1, ratio);
            lerp += 1;
            score += 5 * lerp;
        }
    }
}
