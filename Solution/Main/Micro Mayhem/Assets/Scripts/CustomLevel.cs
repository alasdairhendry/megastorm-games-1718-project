using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used as a Prop-Spawner for Pre-Made custom levels.
/// --As each Custom Level is stored as a prefab, prefab children such as Walls which have variable we may want to change are merged into the parent prefab.
/// --This means when we update the prefab, all of the other props are not updated and we need to manually update them.
/// --To get around this we "Re-Spawn" all of the props at runtime, with their updated Prefab values.
/// </summary>
public class CustomLevel : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        // Initialize a new Actor list.
        List<LevelActor> actors = new List<LevelActor>();

        // Loop through all the level props that are our children.
        foreach (Transform child in this.transform.Find("Regenerated"))
        {
            // Create a new Actor from this object, and add it to our list
            LevelActor actor = new LevelActor(child.name, child.position, child.eulerAngles, child.localScale);
            actors.Add(actor);
        }

        for (int i = 0; i < this.transform.Find("Regenerated").childCount; i++)
        {
            // Destroy all of the previous "Placeholder" props
            Destroy(this.transform.Find("Regenerated").GetChild(i).gameObject);
        }

        // Loop through our Actor list
        foreach (LevelActor actor in actors)
        {
            // Remove the word "Clone" from the GameObjects name, as this is added to the end of any Instantiated object.
            if (actor.stub.Contains("(Clone)"))
                actor.stub = actor.stub.Substring(0, actor.stub.Length - 7);

            // Spawn in the given Actor, set its location, scale and rotation
            GameObject act = Instantiate(Resources.Load("LevelActors/" + actor.stub)) as GameObject;
            act.transform.parent = this.transform.Find("Regenerated");
            act.transform.position = actor.location;
            act.transform.eulerAngles = actor.euler;
            act.transform.localScale = actor.scale;
        }       
    }
}

/// <summary>
/// Abstract class that holds data for each of the props in our level
/// </summary>
public class LevelActor
{
    public string stub;
    public Vector3 location;
    public Vector3 euler;
    public Vector3 scale;

    public LevelActor(string stub, Vector3 location, Vector3 euler, Vector3 scale)
    {
        this.stub = stub;
        this.location = location;
        this.euler = euler;
        this.scale = scale;
    }
}
