using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public PlayerDataLoader DataLoader;

    public GameObject LeftCard;
    public GameObject RightCard;
    public TMP_Text CounterText;
    public GameObject ResultPopup;
    public TMP_Text ResultMessageText;

    public AudioSource audioSource; // Reference to AudioSource
    public AudioClip successClip;  // Success sound
    public AudioClip failClip;     // Fail sound

    private int score = 0;
    private Player currentLeftPlayer;
    private Player currentRightPlayer;
    private string hiddenStat;

    public void InitializeGame()
    {
        if (DataLoader.Players.Count < 2) return;

        NextRound(isFirstRound: true);
    }

    void Start()
    {
        enabled = false;
    }

    void NextRound(bool isFirstRound = false)
    {
        if (DataLoader.Players.Count < 2) return;

        if (!isFirstRound)
        {
            currentLeftPlayer = currentRightPlayer;
            DisplayPlayerStats(LeftCard, currentLeftPlayer, null);
        }
        else
        {
            currentLeftPlayer = DataLoader.Players[Random.Range(0, DataLoader.Players.Count)];
            DataLoader.Players.Remove(currentLeftPlayer);
        }

        currentRightPlayer = DataLoader.Players[Random.Range(0, DataLoader.Players.Count)];
        DataLoader.Players.Remove(currentRightPlayer);

        DisplayPlayerStats(LeftCard, currentLeftPlayer, null);
        hiddenStat = GetRandomStat();
        DisplayPlayerStats(RightCard, currentRightPlayer, hiddenStat);
    }

    void DisplayPlayerStats(GameObject card, Player player, string hiddenStat)
    {
        if (card == null || player == null) return;

        TMP_Text nameText = card.transform.Find("Name")?.GetComponent<TMP_Text>();
        TMP_Text posText = card.transform.Find("POS")?.GetComponent<TMP_Text>();
        TMP_Text ovrText = card.transform.Find("OVR")?.GetComponent<TMP_Text>();
        TMP_Text pacText = card.transform.Find("Pac Words/PAC")?.GetComponent<TMP_Text>();
        TMP_Text shoText = card.transform.Find("SHO Words/SHO")?.GetComponent<TMP_Text>();
        TMP_Text pasText = card.transform.Find("PAS Words/PAS")?.GetComponent<TMP_Text>();
        TMP_Text driText = card.transform.Find("DRI Words/DRI")?.GetComponent<TMP_Text>();
        TMP_Text defText = card.transform.Find("DEF WORDS/DEF")?.GetComponent<TMP_Text>();
        TMP_Text phyText = card.transform.Find("PHY WORDS/PHY")?.GetComponent<TMP_Text>();

        if (nameText != null) nameText.text = player.Name;
        if (posText != null) posText.text = player.Position;
        if (ovrText != null) ovrText.text = player.OVR.ToString();
        if (pacText != null) pacText.text = hiddenStat == "PAC" ? "???" : player.PAC.ToString();
        if (shoText != null) shoText.text = hiddenStat == "SHO" ? "???" : player.SHO.ToString();
        if (pasText != null) pasText.text = hiddenStat == "PAS" ? "???" : player.PAS.ToString();
        if (driText != null) driText.text = hiddenStat == "DRI" ? "???" : player.DRI.ToString();
        if (defText != null) defText.text = hiddenStat == "DEF" ? "???" : player.DEF.ToString();
        if (phyText != null) phyText.text = hiddenStat == "PHY" ? "???" : player.PHY.ToString();
    }

    public void Guess(bool isHigher)
    {
        int leftStat = (int)currentLeftPlayer.GetType().GetField(hiddenStat).GetValue(currentLeftPlayer);
        int rightStat = (int)currentRightPlayer.GetType().GetField(hiddenStat).GetValue(currentRightPlayer);

        if ((isHigher && rightStat > leftStat) || (!isHigher && rightStat < leftStat))
        {
            score++;
            CounterText.text = $"Score: {score}";

            audioSource.PlayOneShot(successClip);

            NextRound();
        }
        else
        {
            audioSource.PlayOneShot(failClip);

            ResultPopup.SetActive(true);
            ResultMessageText.text = $"Game Over! Your score: {score}";
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private string GetRandomStat()
    {
        string[] stats = { "PAC", "SHO", "PAS", "DRI", "DEF", "PHY" };
        return stats[Random.Range(0, stats.Length)];
    }
}
