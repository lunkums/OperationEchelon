using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class History : MonoBehaviour
{
    [SerializeField] private Formation formation;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Level level;
    // SFX data
    [SerializeField] private Sound[] typewriterClacks;
    [Range(0f, 1f)] [SerializeField] private float clackFrequency;
    // Typewriter effect data
    [SerializeField] private float secondsBetweenTypes;
    private float countdown;
    private Queue<char> charQueue;
    // Maps a prefix to the number of rows a move uses
    private Dictionary<int, string> selectionPrefix;
    private int entryCount;

    private void Awake()
    {
        selectionPrefix = new Dictionary<int, string>()
        {
            { 1, " row " },
            { 2, " rows " }
        };
        charQueue = new Queue<char>();
    }

    private void Start()
    {
        Clear();
    }

    private void OnEnable()
    {
        formation.OnMoveAttempt += Append;
        level.OnRestart += Clear;
    }

    private void OnDisable()
    {
        formation.OnMoveAttempt -= Append;
        level.OnRestart -= Clear;
    }

    /* Types another character to the history log if the buffer isn't empty and the countdown
       hits 0. The countdown resets when a character is typed */
    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown > 0)
            return;

        Sound randomClack;

        // Try to pull another character from the queue and type it to the screen
        if (charQueue.TryDequeue(out char c))
        {
            text.text += c;

            if (Random.Range(0, 1f) < clackFrequency)
            {
                randomClack = typewriterClacks[Random.Range(0, typewriterClacks.Length - 1)];
                AudioManager.Instance.Play(randomClack.Name);
            }
        }

        countdown = secondsBetweenTypes;
    }

    private void Clear()
    {
        charQueue.Clear();
        text.text = "";
        countdown = 0;
        entryCount = 0;
    }

    // Appends a Move report message to the history buffer (for eventual typing).
    private void Append(Move move)
    {
        string text = ++entryCount + ") ";
        if (!move.Valid)
            text += "[Failure] ";
        text += GetMessage(move);

        foreach (char c in text)
            charQueue.Enqueue(c);
    }

    // Gets the specific message for a Move using its Operation and selected rows.
    private string GetMessage(Move move)
    {
        int i = 0;
        string text = move.Operation.ToString() + selectionPrefix[move.Operation.SelectionsNeeded()] + move.Selections[i];
        for (i = 1; i < move.Operation.SelectionsNeeded(); i++)
            text += ", " + move.Selections[i];
        Debug.Log(text);
        return text + "\n";
    }
}
