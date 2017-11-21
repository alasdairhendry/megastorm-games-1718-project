using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionMeter : MonoBehaviour {

    [SerializeField] private RectTransform marker;
    [SerializeField] private Light[] spotLights;
    Color spotLightColour = Color.white;

	private void Update () {
        if (GameState.singleton.IsPaused)
            return;

        CheckPosition();
	}

    private void CheckPosition()
    {
        float infectionRatio = EntityRecords.singleton.GetInfectionData();
        float posX = 0.0f;

        //print("Infection " + infectionRatio);

        if (infectionRatio < 0)
        {
            float t = (infectionRatio * -1.0f);
            Color left = Color.green;
            Color middle = Color.white;
            left.a = 1.0f;
            middle.a = 0.5f;

            posX = Mathf.Lerp(0.0f, - 192.0f, t);
            spotLightColour = Color.Lerp(middle, left, t);
        }
        else if (infectionRatio > 0)
        {
            float t = infectionRatio;
            Color right = Color.red;
            Color middle = Color.white;
            right.a = 1.0f;
            middle.a = 0.5f;

            posX = Mathf.Lerp(0.0f, 192.0f, t);
            spotLightColour = Color.Lerp(middle, right, t);
        }
        else
        {
            posX = 0.0f;
            spotLightColour = Color.white;
        }

        SetPosition(posX);
        SetColour(spotLightColour);
    }

    private void SetPosition(float posX)
    {
        float newPosX = marker.anchoredPosition.x;

        newPosX = Vector3.Lerp(new Vector3(newPosX, 0.0f, 0.0f), new Vector3(posX, 0.0f, 0.0f), Time.deltaTime).x;
        marker.anchoredPosition = new Vector2(newPosX, marker.anchoredPosition.y);
    }

    private void SetColour(Color colour)
    {
        Color newColour = spotLights[0].color;

        newColour = Vector4.Lerp(new Vector4(newColour.r, newColour.g, newColour.b, newColour.a), new Vector4(colour.r, colour.g, colour.b, colour.a), Time.deltaTime);
        foreach (Light light in spotLights)
        {
            light.color = newColour;
        }        
    }
}
