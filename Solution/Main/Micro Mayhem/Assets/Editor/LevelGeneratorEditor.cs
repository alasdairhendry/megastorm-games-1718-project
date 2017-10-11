using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor {

    private LevelGenerator levelGenerator;
    private int currentObjectIndex = -1;
    private bool dimensionsCorrect = false;

    private Vector3 inputPosition = new Vector3();
    private bool inputAvailable = false;

    private GameObject lastObjectPlaced;
    private bool showTransformControls = false;

    Tool LastTool = Tool.None;

    private void OnEnable()
    {
        levelGenerator = target as LevelGenerator;
        LastTool = Tools.current;
        Tools.current = Tool.None;
    }

    void OnDisable()
    {
        Tools.current = LastTool;
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;

        if (e.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }

        GetMousePosition(e);
        InputObject(e);
    }

    private void GetMousePosition(Event e)
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        float planeHeight = 0;
        float distToPlane = (planeHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(distToPlane);

        float tilesPerDimension = (float)levelGenerator.levelDimensions / levelGenerator.tileDimensions;
        if(tilesPerDimension == (int)tilesPerDimension)
        {
            dimensionsCorrect = true;
        }
        else
        {
            dimensionsCorrect = false;
        }

        DrawTiles(mousePosition);
    }

    public void DrawTiles(Vector3 mousePosition)
    {
        if (!dimensionsCorrect)
            return;

        float tilesPerDimension = (float)levelGenerator.levelDimensions / levelGenerator.tileDimensions;
        float td = levelGenerator.tileDimensions;
        float ld = levelGenerator.levelDimensions;

        inputAvailable = false;
        for (int x = 0; x < tilesPerDimension; x++)
        {
            for (int z = 0; z < tilesPerDimension; z++)
            {
                Vector3 handlePos = (levelGenerator.transform.position - new Vector3(ld / 2, 0, ld / 2)) + new Vector3(x * td + (td / 2), 0, z * td + (td / 2));
                if(mousePosition.x > handlePos.x - (td/2) && mousePosition.x < handlePos.x + (td / 2) && mousePosition.z > handlePos.z - (td / 2) && mousePosition.z < handlePos.z + (td / 2))
                {           
                    Handles.color = Color.green;
                    inputPosition = handlePos;
                    inputAvailable = true;
                    HandleUtility.Repaint();
                }
                else
                {
                    Handles.color = Color.white;
                }

                if(levelGenerator.drawHandles || handlePos == inputPosition)
                    Handles.DrawSolidDisc(handlePos, Vector3.up, td / 2);
            }            
        }
    }

    private void InputObject(Event e)
    {
        if (!dimensionsCorrect)
            return;

        if (currentObjectIndex == -1)
            return;

        if (!inputAvailable)
            return;

        if(e.type == EventType.MouseDown && e.button == 0 && e.modifiers == EventModifiers.None)
        {
            GameObject go = Instantiate(levelGenerator.levelObjects[currentObjectIndex]);
            go.transform.position = inputPosition;
            go.name = levelGenerator.levelObjects[currentObjectIndex].name;

            if (levelGenerator.targetParent != null)
                go.transform.parent = levelGenerator.targetParent.transform;

            lastObjectPlaced = go;
            showTransformControls = true;
        }
    }

    public override void OnInspectorGUI()
    {    
        base.OnInspectorGUI();       

        if(GUILayout.Button("None"))
        {
            currentObjectIndex = -1;
            showTransformControls = false;
        }
        for (int i = 0; i < levelGenerator.levelObjects.Count; i++)
        {
            if(GUILayout.Button(levelGenerator.levelObjects[i].name))
            {
                currentObjectIndex = i;
                showTransformControls = false;
            }
        }

        if(currentObjectIndex == -1)
        {
            GUILayout.Label("No object selected");
        }
        else
        {
            GUILayout.Label(levelGenerator.levelObjects[currentObjectIndex].name + " is currently selected");
        }

        GUILayout.Label("Dimensions are eligible: " + dimensionsCorrect.ToString());

        if (showTransformControls)
        {
            if(GUILayout.Button("Rotate Y 90"))
            {                
                lastObjectPlaced.transform.eulerAngles += new Vector3(0, 90, 0);
            }
        }
    }
}
