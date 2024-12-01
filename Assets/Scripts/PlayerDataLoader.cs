using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerDataLoader : MonoBehaviour
{
    public List<Player> Players { get; private set; } = new List<Player>();

    void Start()
    {
        LoadPlayers();
    }

    void LoadPlayers()
    {
        TextAsset csvData = Resources.Load<TextAsset>("all_players");
        if (csvData == null)
        {
            Debug.LogError("CSV file not found. Ensure it is in the Resources folder.");
            return;
        }

        string[] rows = csvData.text.Split('\n');
        for (int i = 1; i < rows.Length; i++) // Skip the header
        {
            string row = rows[i];
            if (string.IsNullOrWhiteSpace(row)) continue;

            string[] columns = row.Split(',');

            if (columns.Length < 41) continue;

            try
            {
                int ovr = int.Parse(columns[4].Trim());
                string position = columns[40].Trim();

                // Exclude goalkeepers
                if (position.Equals("GK", System.StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (ovr >= 75)
                {
                    string league = columns[49].Trim();

                    Players.Add(new Player
                    {
                        Name = columns[3].Trim(),
                        Position = position,
                        OVR = ovr,
                        League = league,
                        PAC = int.Parse(columns[5].Trim()),
                        SHO = int.Parse(columns[6].Trim()),
                        PAS = int.Parse(columns[7].Trim()),
                        DRI = int.Parse(columns[8].Trim()),
                        DEF = int.Parse(columns[9].Trim()),
                        PHY = int.Parse(columns[10].Trim())
                    });
                }
            }
            catch (System.Exception)
            {
                // Handle row parsing errors gracefully
            }
        }
    }

    public void FilterPlayersByTop100()
    {
        Players = Players.OrderByDescending(p => p.OVR).Take(100).ToList();
    }

    public void FilterPlayersByTopLeagues()
    {
        string[] topLeagues = { "Premier League", "Serie A", "LALIGA EA SPORTS", "Bundesliga", "Ligue 1 McDonald's" };
        Players = Players.Where(p => topLeagues.Contains(p.League) && p.OVR >= 80).ToList();
    }

    public void ResetFilter()
    {
        Players.Clear();
        LoadPlayers();
    }
}
