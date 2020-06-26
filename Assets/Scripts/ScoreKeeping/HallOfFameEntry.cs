using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.ScoreKeeping
{
    [System.Serializable]
    public struct HallOfFameEntry
    {
        [SerializeField] public string Name;
        [SerializeField] public float Kills;
        [SerializeField] public float Deaths;
        [SerializeField] public float KDRatio;
        [SerializeField] public string GameMode;

        public HallOfFameEntry(string name, float kills, float deaths, string gameMode)
        {
            Name = name;
            Kills = kills;
            Deaths = deaths;
            KDRatio = (deaths == 0) ? kills : kills / deaths;
            GameMode = gameMode;
        }

        public HallOfFameEntry CreateSaveData(string name, float kills, float deaths, string gameMode)
        {
            HallOfFameEntry saveData = new HaOfllOfFameEntry();

            saveData.Name = name;
            saveData.Kills = kills;
            saveData.Deaths = deaths;
            saveData.KDRatio = (deaths == 0) ? kills : kills / deaths;
            saveData.GameMode = gameMode;

            return saveData;
        }
    }
}