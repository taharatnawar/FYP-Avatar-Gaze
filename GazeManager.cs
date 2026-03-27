using System.Collections.Generic;
using UnityEngine;

public class GazeManager : MonoBehaviour
{
    [System.Serializable]
    public class GazeEvent
    {
        public float timestamp;
        public string avatar1Role;
        public string avatar1Gaze;
        public string avatar2Role;
        public string avatar2Gaze;
    }

    [Header("CSV")]
    public TextAsset csvFile;

    [Header("Avatar 1")]
    public Animator avatar1Animator;
    public AvatarLookIK avatar1IK;
    public Transform a1Direct;
    public Transform a1Up;
    public Transform a1Down;
    public Transform a1Left;
    public Transform a1Right;

    [Header("Avatar 2")]
    public Animator avatar2Animator;
    public AvatarLookIK avatar2IK;
    public Transform a2Direct;
    public Transform a2Up;
    public Transform a2Down;
    public Transform a2Left;
    public Transform a2Right;

    [Header("Animator Parameter")]
    public string talkingBoolName = "isTalking";

    private List<GazeEvent> events = new List<GazeEvent>();
    private int currentEventIndex = 0;
    private float timer = 0f;

    void Start()
    {
        Debug.Log("GazeManager Start() is running!");

        LoadCSV();

        if (events.Count == 0)
        {
            Debug.LogError("No gaze events loaded.");
            return;
        }

        ApplyEvent(events[0]);
    }

    void Update()
    {
        if (events.Count == 0)
            return;

        timer += Time.deltaTime;

        while (currentEventIndex + 1 < events.Count &&
               timer >= events[currentEventIndex + 1].timestamp)
        {
            currentEventIndex++;
            ApplyEvent(events[currentEventIndex]);
        }
    }

    void ApplyEvent(GazeEvent e)
    {
        if (avatar1Animator != null)
            avatar1Animator.SetBool(talkingBoolName, e.avatar1Role.Trim() == "Talking");

        if (avatar2Animator != null)
            avatar2Animator.SetBool(talkingBoolName, e.avatar2Role.Trim() == "Talking");

        if (avatar1IK != null)
            avatar1IK.currentLookTarget = GetTargetForAvatar1(e.avatar1Gaze);

        if (avatar2IK != null)
            avatar2IK.currentLookTarget = GetTargetForAvatar2(e.avatar2Gaze);

        Debug.Log("Applied: " + e.timestamp + " | A1 " + e.avatar1Role + " " + e.avatar1Gaze +
                  " | A2 " + e.avatar2Role + " " + e.avatar2Gaze);
    }

    Transform GetTargetForAvatar1(string gaze)
    {
        switch (gaze.Trim())
        {
            case "Up":    return a1Up;
            case "Down":  return a1Down;
            case "Left":  return a1Left;
            case "Right": return a1Right;
            default:      return a1Direct;
        }
    }

    Transform GetTargetForAvatar2(string gaze)
    {
        switch (gaze.Trim())
        {
            case "Up":    return a2Up;
            case "Down":  return a2Down;
            case "Left":  return a2Left;
            case "Right": return a2Right;
            default:      return a2Direct;
        }
    }

    void LoadCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV file not assigned in Inspector.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 3; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] cols = line.Split(',');
            if (cols.Length < 5) continue;

            GazeEvent e = new GazeEvent();
            if (!float.TryParse(cols[0].Trim(), out e.timestamp)) continue;

            e.avatar1Role = cols[1].Trim();
            e.avatar1Gaze = cols[2].Trim();
            e.avatar2Role = cols[3].Trim();
            e.avatar2Gaze = cols[4].Trim();

            events.Add(e);
        }

        Debug.Log("Loaded " + events.Count + " gaze events.");
    }
}
