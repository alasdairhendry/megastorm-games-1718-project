using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLevel : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        List<LevelActor> actors = new List<LevelActor>();

        foreach (Transform child in this.transform.Find("Regenerated"))
        {
            LevelActor actor = new LevelActor(child.name, child.position, child.eulerAngles, child.localScale);
            actors.Add(actor);
        }

        for (int i = 0; i < this.transform.Find("Regenerated").childCount; i++)
        {
            Destroy(this.transform.Find("Regenerated").GetChild(i).gameObject);
        }

        foreach (LevelActor actor in actors)
        {
            if (actor.stub.Contains("(Clone)"))
                actor.stub = actor.stub.Substring(0, actor.stub.Length - 7);

            print(actor.stub);

            GameObject act = Instantiate(Resources.Load("LevelActors/" + actor.stub)) as GameObject;
            act.transform.parent = this.transform.Find("Regenerated");
            act.transform.position = actor.location;
            act.transform.eulerAngles = actor.euler;
            act.transform.localScale = actor.scale;
        }       
    }
}

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
