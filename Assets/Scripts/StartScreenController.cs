using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenController : MonoBehaviour
{
    public Slider DifficultySlider;
    public TMP_Text DifficultyLabel;
    public GameObject StartScreen;
    public GameObject GameScreenCanvas;
    public PlayerDataLoader DataLoader;
    public GameController GameController;

    private int difficultyLevel;

    void Start()
    {
        DifficultySlider.onValueChanged.AddListener(OnDifficultyChanged);
        OnDifficultyChanged(DifficultySlider.value);
    }

    public void OnDifficultyChanged(float value)
    {
        difficultyLevel = (int)value;

        switch (difficultyLevel)
        {
            case 0:
                DifficultyLabel.text = "Easy: Top 100 Players";
                break;
            case 1:
                DifficultyLabel.text = "Medium: Top 5 Leagues";
                break;
            case 2:
                DifficultyLabel.text = "Hard: All Players (75+)";
                break;
        }
    }

    public void StartGame()
    {
        switch (difficultyLevel)
        {
            case 0:
                DataLoader.FilterPlayersByTop100();
                break;
            case 1:
                DataLoader.FilterPlayersByTopLeagues();
                break;
            case 2:
                DataLoader.ResetFilter();
                break;
        }

        StartScreen.SetActive(false);
        GameScreenCanvas.SetActive(true);
        GameController.InitializeGame();
    }
}
