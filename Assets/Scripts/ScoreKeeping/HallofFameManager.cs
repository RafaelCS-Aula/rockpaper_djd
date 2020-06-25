using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.ScoreKeeping
{
    public class HallOfFameManager : MonoBehaviour
    {
        private const string SAVE_FILENAME = "save.dat";

        [SerializeField, HideInInspector] private string _saveFilePath = "";

        [SerializeField, HideInInspector] public int maxEntrys = 0;

        [System.Serializable]
        public struct SaveData
        {
            public List<HallofFameEntry> hallOfFameEntryList;
        }

        private void Awake()
        {
            maxEntrys = 5;
            _saveFilePath = Application.persistentDataPath + "/" + SAVE_FILENAME;
        }

        #region Data Handling
        public void CreateSaveData()
        {
            if (!File.Exists(_saveFilePath))
            {
                SaveData saveData = new SaveData
                {
                    hallOfFameEntryList = new List<HallofFameEntry>()
                };

                StoreSaveData(saveData);
            }
        }

        public void StoreSaveData(SaveData saveData)
        {
            string jsonSaveData = JsonUtility.ToJson(saveData, true);

            File.WriteAllText(_saveFilePath, jsonSaveData);
        }

        public SaveData LoadSaveData()
        {
            string jsonSaveData = File.ReadAllText(_saveFilePath);

            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonSaveData);

            return saveData;
        }
        #endregion


        public void AddEntry(SaveData saveData, CharacterHandler player, string gameMode)
        {
            // If 'scoreList' Isn't Full, Add Player To The List
            if (saveData.hallOfFameEntryList.Count < maxEntrys)
            {
                saveData.hallOfFameEntryList.Add(
                    new HallofFameEntry(player.characterName,
                    player.kills, player.deaths, gameMode));

                SortHighScores(saveData);
                StoreSaveData(saveData);
            }

            //If 'scoreList' Is Full
            else
            {
                // Checks If Player's Score Is Better Than 
                // Any Score In The List
                bool isHigher = false;
                float currentKD = (player.deaths == 0) ? player.kills : player.kills / player.deaths;
                for (int i = 0; i < saveData.hallOfFameEntryList.Count; i++)
                {
                    if (currentKD > saveData.hallOfFameEntryList[i].KDRatio)
                    {
                        isHigher = true;
                    }
                }

                // Adds Score If It's Better
                if (isHigher)
                {
                    saveData.hallOfFameEntryList.Add(
                        new HallofFameEntry(player.characterName,
                        player.kills, player.deaths, gameMode));

                    SortHighScores(saveData);
                    StoreSaveData(saveData);
                }
            }
        }

        public void SortHighScores(SaveData saveData)
        {
            // Sorts HighScores With The Given Comparer

            saveData.hallOfFameEntryList.Sort(new EntryComparer());

            //If A Score Was Added, Remove The Lowest Score
            if (saveData.hallOfFameEntryList.Count > maxEntrys)
            {
                saveData.hallOfFameEntryList.RemoveAt(maxEntrys);
            }
        }

        public void HighScoreManagement(SaveData saveData)
        {
            //AddEntry(saveData, player);
            SortHighScores(saveData);
            StoreSaveData(saveData);
        }
    }
}