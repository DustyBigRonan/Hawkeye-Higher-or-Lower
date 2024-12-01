using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlaylistController : MonoBehaviour
{
    public static PlaylistController Instance; // Singleton to persist across scenes
    public AudioSource audioSource;           // The AudioSource to play the audio
    public AudioClip[] playlist;             // Array of audio clips (assign in Inspector)

    public string startScreenButtonName = "SkipButtonStartScreen";
    public string gameScreenButtonName = "SkipButtonGameScreen";

    private List<int> shuffledIndices;
    private int currentTrackIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (playlist.Length == 0) return;

        if (shuffledIndices == null || shuffledIndices.Count == 0)
        {
            ShufflePlaylist();
            PlayTrack(shuffledIndices[currentTrackIndex]);
        }

        AssignSkipButtons();
    }

    private void OnEnable()
    {
        AssignSkipButtons();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && playlist.Length > 0)
        {
            NextTrack();
        }
    }

    private void PlayTrack(int index)
    {
        if (index < 0 || index >= playlist.Length) return;

        if (audioSource.clip != playlist[index])
        {
            audioSource.clip = playlist[index];
            audioSource.Play();
        }
    }

    public void NextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % shuffledIndices.Count;
        PlayTrack(shuffledIndices[currentTrackIndex]);
    }

    private void ShufflePlaylist()
    {
        shuffledIndices = new List<int>();

        for (int i = 0; i < playlist.Length; i++)
        {
            shuffledIndices.Add(i);
        }

        for (int i = 0; i < shuffledIndices.Count; i++)
        {
            int randomIndex = Random.Range(0, shuffledIndices.Count);
            int temp = shuffledIndices[i];
            shuffledIndices[i] = shuffledIndices[randomIndex];
            shuffledIndices[randomIndex] = temp;
        }
    }

    private void AssignSkipButtons()
    {
        Button skipButtonStartScreen = GameObject.Find(startScreenButtonName)?.GetComponent<Button>();
        if (skipButtonStartScreen != null)
        {
            skipButtonStartScreen.onClick.RemoveAllListeners();
            skipButtonStartScreen.onClick.AddListener(NextTrack);
        }

        Button skipButtonGameScreen = GameObject.Find(gameScreenButtonName)?.GetComponent<Button>();
        if (skipButtonGameScreen != null)
        {
            skipButtonGameScreen.onClick.RemoveAllListeners();
            skipButtonGameScreen.onClick.AddListener(NextTrack);
        }
    }
}
