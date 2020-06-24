using UnityEngine;

namespace rockpaper_djd
{
    [System.Serializable]
    public struct HallofFameEntry
    {
        [SerializeField] public string Name;
        [SerializeField] public float Kills;
        [SerializeField] public float Deaths;
        [SerializeField] public float KDRatio;
        [SerializeField] public string GameMode;

        public HallofFameEntry(string name, float kills, float deaths, string gameMode)
        {
            Name = name;
            Kills = kills;
            Deaths = deaths;
            KDRatio = (deaths == 0) ?  kills : kills / deaths;
            GameMode = gameMode;
        }

        public HallofFameEntry CreateSaveData(string name, float kills, float deaths, string gameMode)
        {
            HallofFameEntry saveData = new HallofFameEntry();

            saveData.Name = name;
            saveData.Kills = kills;
            saveData.Deaths = deaths;
            saveData.KDRatio = (deaths == 0) ? kills : kills / deaths;
            saveData.GameMode = gameMode;

            return saveData;
        }
    }
}