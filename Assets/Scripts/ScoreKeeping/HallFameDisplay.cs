using TMPro;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.ScoreKeeping
{

    public class HallFameDisplay : MonoBehaviour
    {
        [SerializeField] private HallFameManager hofManager;

        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI kills;
        [SerializeField] private TextMeshProUGUI deaths;
        [SerializeField] private TextMeshProUGUI KDRatio;
        [SerializeField] private TextMeshProUGUI gameMode;


        HallFameManager.SaveData saveData;
        private void Start()
        {
            hofManager.CreateSaveData();

            saveData = hofManager.LoadSaveData();

            UpdateHallOfFame();
        }


        private void UpdateHallOfFame()
        {
            for (int i = 0; i < saveData.hallOfFameEntryList.Count; i++)
            {
                playerName.text += saveData.hallOfFameEntryList[i].Name + "\n\n";
                kills.text += saveData.hallOfFameEntryList[i].Kills + "\n\n";
                deaths.text += saveData.hallOfFameEntryList[i].Deaths + "\n\n";
                KDRatio.text += saveData.hallOfFameEntryList[i].KDRatio.ToString("F2") + "\n\n";
                gameMode.text += saveData.hallOfFameEntryList[i].GameMode + "\n\n";
            }

            int linesLeft = hofManager.maxEntries - saveData.hallOfFameEntryList.Count;

            if (linesLeft > 0)
            {
                for (int i = 0; i < linesLeft; i++)
                {
                    playerName.text += "\n\n";
                    kills.text += "\n\n";
                    deaths.text += "\n\n";
                    KDRatio.text += "\n\n";
                    gameMode.text += "\n\n";
                }
            }
        }
    }
}
