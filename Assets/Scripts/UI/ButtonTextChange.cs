using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace RPS_DJDIII.Assets.Scripts.UI
{
    public class ButtonTextChange : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private GameModeData gameMode;

        [SerializeField] private TextMeshProUGUI gmName;
        [SerializeField] private TextMeshProUGUI gmDescription;
        [SerializeField] private GameObject info;

        private string gameModeName;
        private string gameModeDescription;

        private void Start()
        {
            gameModeName = gameMode.gameModeName;
            gameModeDescription = gameMode.gameModeDescription;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            gmName.text = gameModeName;
            gmDescription.text = gameModeDescription;

            if (info.activeSelf) info.SetActive(false);
        }
    }
}