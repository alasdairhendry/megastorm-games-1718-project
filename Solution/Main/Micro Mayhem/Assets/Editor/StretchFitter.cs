using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class StretchFitter : MonoBehaviour {

    [MenuItem("StretchFitter/Stretch #&s")]
    static void Boopy()
    {
        RectTransform target = Selection.activeGameObject.GetComponent<RectTransform>();
        RectTransform root = target.transform.parent.GetComponent<RectTransform>();

        float minX = Mathf.Lerp(0, 1, target.anchoredPosition.x / root.sizeDelta.x);
        float minY = Mathf.Lerp(1, 0, (target.anchoredPosition.y + target.sizeDelta.y) / root.sizeDelta.y);
        minY = 0;
        target.anchorMin = new Vector2(minX, minY);
    }

}
