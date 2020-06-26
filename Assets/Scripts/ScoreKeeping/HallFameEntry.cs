using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.ScoreKeeping
{
    [System.Serializable]
    public struct HallFameEntry
    {
        [SerializeField] public string Name;
        [SerializeField] public float Kills;
        [SerializeField] public float Deaths;
        [SerializeField] public float KDRatio;
        [SerializeField] public string GameMode;

        public HallFameEntry(string name, float kills, float deaths, string gameMode)
        {
            Name = name;
            Kills = kills;
            Deaths = deaths;
            KDRatio = (deaths == 0) ? kills : kills / deaths;
            GameMode = gameMode;
        }

        public HallFameEntry CreateSaveData(string name, float kills, float deaths, string gameMode)
        {
            HallFameEntry saveData = new HallFameEntry();

            saveData.Name = name;
            saveData.Kills = kills;
            saveData.Deaths = deaths;
            saveData.KDRatio = (deaths == 0) ? kills : kills / deaths;
            saveData.GameMode = gameMode;

            return saveData;
        }
    }
}