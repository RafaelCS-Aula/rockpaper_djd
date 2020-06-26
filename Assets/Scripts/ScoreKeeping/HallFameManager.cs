using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.ScoreKeeping
{
    public class HallFameManager : MonoBehaviour
    {
        /// <summary>
        /// Name of the save file
        /// </summary>
        private const string SAVE_FILENAME = "save.dat";

        /// <summary>
        /// Path of the save file
        /// </summary>
        [SerializeField, HideInInspector] private string _saveFilePath = "";

        /// <summary>
        /// Maximum entries for the Hall of Fame
        /// </summary>
        [SerializeField, HideInInspector] public int maxEntries = 0;


        /// <summary>
        /// Serializable Struct SaveData that stores a list with al Hall of Fame Entries
        /// </summary>
        [System.Serializable]
        public struct SaveData
        {
            /// <summary>
            /// List that stores all Hall of Fame Entries
            /// </summary>
            public List<HallFameEntry> hallOfFameEntryList;
        }

        /// <summary>
        /// Awake method to initiate variables
        /// </summary>
        private void Awake()
        {
            // Set 'maxEntries' to 5
            maxEntries = 5;

            // Set 'saveFilePath' string 
            _saveFilePath = Application.persistentDataPath + "/" + SAVE_FILENAME;
        }

        #region Data Handling
        /// <summary>
        /// Create and Store a SaveData
        /// </summary>
        public void CreateSaveData()
        {
            // Checks if the file doesn't already exist
            if (!File.Exists(_saveFilePath))
            {
                // Sets a new saveData 
                SaveData saveData = new SaveData
                {
                    // Initializes the saveData list
                    hallOfFameEntryList = new List<HallFameEntry>()
                };

                // Stores the 'SaveData'
                StoreSaveData(saveData);
            }
        }

        /// <summary>
        /// Stores the given SaveData in a file
        /// </summary>
        /// <param name="saveData">SaveData to be stored</param>
        public void StoreSaveData(SaveData saveData)
        {
            // Converts the SaveData to Json
            string jsonSaveData = JsonUtility.ToJson(saveData, true);

            // Writes the converted SaveData on the file in the specified path
            File.WriteAllText(_saveFilePath, jsonSaveData);
        }

        /// <summary>
        /// Loads the SaveData in the specified path and returns its value
        /// </summary>
        /// <returns>SaveData loaded from the specified path</returns>
        public SaveData LoadSaveData()
        {
            // Reads the SaveData from the file in the specified path
            string jsonSaveData = File.ReadAllText(_saveFilePath);

            // Converts the read file to SaveData
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonSaveData);

            // Returns converted SaveData
            return saveData;
        }
        #endregion


        /// <summary>
        /// Evaluates a given Entry, and based on the player's performance and
        /// the already stored performances, evaluated if the entry will be
        /// added to the Hall of Fame, and adds it if that's the case
        /// </summary>
        /// <param name="saveData">SaveData where the current Entries are stored</param>
        /// <param name="player">Player of which the Entry will be evaluated</param>
        /// <param name="gameMode">Name of the current Game Mode for display purposes</param>
        public void AddEntry(SaveData saveData, CharacterHandler player, string gameMode)
        {
            // Check if the Entry list isn't full
            if (saveData.hallOfFameEntryList.Count < maxEntries)
            {
                // Add Entry to the list
                saveData.hallOfFameEntryList.Add(
                    new HallFameEntry(player.characterName,
                    player.kills, player.deaths, gameMode));

                // Runs 'NewEntryManagement' method
                NewEntryManagement(saveData);
            }

            //If the Entry list isn't full
            else
            {
                // Bool to be compared for adding the current Entry
                bool isHigher = false;

                // Get player's KD
                float currentKD = (player.deaths == 0) ? player.kills : player.kills / player.deaths;

                // Get last Entry's KD
                float lastKD = saveData.hallOfFameEntryList[saveData.hallOfFameEntryList.Count - 1].KDRatio;

                // Check if player's KD is higher than the last Entry's KD,
                // and sets 'isHigher' to true if it is
                if (currentKD > lastKD)
                {
                    isHigher = true;
                }
                // If 'isHigher' is true 
                if (isHigher)
                {
                    // Add Entry to the list
                    saveData.hallOfFameEntryList.Add(
                        new HallFameEntry(player.characterName,
                        player.kills, player.deaths, gameMode));

                    // Runs 'NewEntryManagement' method
                    NewEntryManagement(saveData);
                }
            }
        }

        /// <summary>
        /// Sorts Hall of Fame Entries and removes any excess entry
        /// </summary>
        /// <param name="saveData">SaveData with the Entries to be sorted</param>
        public void SortEntries(SaveData saveData)
        {
            // Sort Entries with the given comparer
            saveData.hallOfFameEntryList.Sort(new EntryComparer());

            //Chck if there is more Entries than allowed
            if (saveData.hallOfFameEntryList.Count > maxEntries)
            {
                // Remove the last Entry
                saveData.hallOfFameEntryList.RemoveAt(maxEntries);
            }
        }

        /// <summary>
        /// Defines the order of action after adding an Entry,
        /// Sorting the Entries, then storing the data in the file
        /// </summary>
        /// <param name="saveData">Data saved/to be saved in the save file</param>
        public void NewEntryManagement(SaveData saveData)
        {
            // Calls method 'SortHighScores' with the given SaveData
            SortEntries(saveData);
            // Calls method 'StoreSaveData' with the given SaveData
            StoreSaveData(saveData);
        }
    }
}