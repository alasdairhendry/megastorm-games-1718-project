using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to control the InfectionMeter HUD
/// </summary>
public class InfectionMeter : MonoBehaviour {

    [SerializeField] private RectTransform marker;
    [SerializeField] private Light[] spotLights;
    Color spotLightColour = Color.white;

	private void Update () {
        if (GameState.singleton.IsPaused)
            return;

        CheckPosition();
	}

    // Check the position that the meter should be at
    private void CheckPosition()
    {
        // Obtain the infection information
        float infectionRatio = EntityRecords.singleton.GetInfectionData();
        float posX = 0.0f;        

        if (infectionRatio < 0)
        {
            // Less than 0 means a healthy(ish) body. Manipulate the meter to be on the left-most side
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
            // Greater than 0 means an unhealthy(ish) body. Manipulate the meter to be on the right-most side
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
            // The infection in the body is balanced so the meter should be centered
            posX = 0.0f;
            spotLightColour = Color.white;
        }

        SetPosition(posX);
        SetColour(spotLightColour);
    }

    // Lerp the position of time for visual effect
    private void SetPosition(float posX)
    {
        float newPosX = marker.anchoredPosition.x;

        newPosX = Vector3.Lerp(new Vector3(newPosX, 0.0f, 0.0f), new Vector3(posX, 0.0f, 0.0f), Time.deltaTime).x;
        marker.anchoredPosition = new Vector2(newPosX, marker.anchoredPosition.y);
    }

    // Lerp the colour of the spotlight in the scene to display the infection rate
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
