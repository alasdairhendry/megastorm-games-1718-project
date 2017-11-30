using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public static Tutorial singleton;
    public bool playTutorial = false;
    [SerializeField] private List<TutorialSegment> tutorials = new List<TutorialSegment>();

    [Header("GUI Contents")]
    [SerializeField] private GameObject container;
    [SerializeField] private Image image;
    [SerializeField] private Text headerText;
    [SerializeField] private Text bodyText;

    private Queue<TutorialSegment> upcomingTutorials = new Queue<TutorialSegment>(); // Queue of tutorials
    private bool isShowing = false;

    public enum Stub { MeleeSpawned, MageSpawned, TankSpawned, BoomSpawned, HoverWeaponPickup, PickupCleanser, PickupBlaster, PickupDeadeye, InfectionMeter }

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        //EntityRecords.singleton.onEnemySpawn += OnEnemySpawn;
        DoStartupTutorials();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameState.singleton.IsPaused)
            return;

        if (playTutorial)
            MonitorTutorials();
	}

    private void MonitorTutorials()
    {
        if(upcomingTutorials.Count > 0 && !isShowing)
        {
            isShowing = true;
            GameState.singleton.Pause();
            container.GetComponent<Animator>().SetBool("Show", true);

            TutorialSegment segment = upcomingTutorials.Dequeue();
            headerText.text = segment.GetHeader();
            string body = segment.GetBody();
            int i = body.IndexOf("Tip:");
            if (i > 0)
            {
                body = body.Insert(i, "<color=#f95e5e>");
                body = body.Insert(body.Length, "</color>");
            }
            bodyText.text = body;
            image.sprite = segment.GetImage();
        }
    }

    private TutorialSegment GetTutorial(Tutorial.Stub _stub)
    {
        foreach (TutorialSegment segment in tutorials)
        {
            if(segment.GetTutorial(_stub) != null)
            {
                return segment.GetTutorial(_stub);
            }
        }

        return null;
    }

    private bool CanDoTutorial(TutorialSegment segment)
    {
        return segment.CheckCanDo();
    }

    public void OnClickOkay()
    {
        StartCoroutine(ResetDelay());
        GameState.singleton.Resume();
        container.GetComponent<Animator>().SetBool("Show", false);
    }

    public void OnClickSkip()
    {
        StartCoroutine(ResetDelay());
        playTutorial = false;
        GameState.singleton.Resume();
        container.GetComponent<Animator>().SetBool("Show", false);
    }

    private IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(0.25f);

        while (GameState.singleton.IsPaused)
            yield return null;

        isShowing = false;
    }

    #region TutorialSegments

    private void DoStartupTutorials()
    {
        TutorialSegment segment = GetTutorial(Stub.InfectionMeter);

        if (CanDoTutorial(segment))
        {
            upcomingTutorials.Enqueue(segment);
        }
    }

    public void OnEnemySpawn(EnemyBase enemyType)
    {
        Type t = enemyType.GetType();

        if (t == typeof(EnemyMelee))
        {
            TutorialSegment segment = GetTutorial(Stub.MeleeSpawned);

            if (CanDoTutorial(segment))
            {
                upcomingTutorials.Enqueue(segment);
            }
        }
        else if (t == typeof(EnemyMage))
        {
            print("MAGE SPWANED");
            TutorialSegment segment = GetTutorial(Stub.MageSpawned);

            if (CanDoTutorial(segment))
            {
                upcomingTutorials.Enqueue(segment);
            }
        }
        else if (t == typeof(EnemyTank))
        {
            TutorialSegment segment = GetTutorial(Stub.TankSpawned);

            if (CanDoTutorial(segment))
            {
                upcomingTutorials.Enqueue(segment);
            }
        }
        else if (t == typeof(EnemyBoom))
        {
            TutorialSegment segment = GetTutorial(Stub.BoomSpawned);

            if (CanDoTutorial(segment))
            {
                upcomingTutorials.Enqueue(segment);
            }
        }
    }

    public void OnHoverWeaponPickup()
    {
        TutorialSegment segment = GetTutorial(Stub.HoverWeaponPickup);

        if (CanDoTutorial(segment))
        {
            upcomingTutorials.Enqueue(segment);
        }
    }

    public void OnWeaponPickup(WeaponBase weapon)
    {
        Type t = weapon.GetType();

        if (t == typeof(WeaponBlaster))
        {
            TutorialSegment segment = GetTutorial(Stub.PickupBlaster);

            if (CanDoTutorial(segment))
            {
                upcomingTutorials.Enqueue(segment);
            }
        }
        else if (t == typeof(WeaponTheCleanser))
        {
            TutorialSegment segment = GetTutorial(Stub.PickupCleanser);

            if (CanDoTutorial(segment))
            {
                upcomingTutorials.Enqueue(segment);
            }
        }
        else if (t == typeof(WeaponDeadeye))
        {
            TutorialSegment segment = GetTutorial(Stub.PickupDeadeye);

            if (CanDoTutorial(segment))
            {
                upcomingTutorials.Enqueue(segment);
            }
        }
    }

    #endregion
}

[System.Serializable]
public class TutorialSegment
{    
    private bool hasShown = false;
    private bool hasQueued = false;    
    [SerializeField] private string header = "";
    [SerializeField] private Tutorial.Stub stub;
    [SerializeField] private Sprite image;
    [SerializeField] [TextArea] private string body = "";

    public TutorialSegment GetTutorial(Tutorial.Stub _stub)
    {
        if (_stub == stub)
            return this;
        else
            return null;
    }

    public bool CheckCanDo()
    {
        if (!hasShown)
        {
            if (!hasQueued)
            {
                hasQueued = true;
                return true;
            }
            else return false;
        }
        else return false;
    }

    public Sprite GetImage()
    {
        return image;
    }

    public string GetHeader()
    {
        return header;
    }

    public string GetBody()
    {
        return body;
    }

    public void SetShown()
    {
        hasShown = true;
        hasQueued = true;
    }
}
