using RPS_DJDIII.Assets.Scripts.GameManagers;
using System;
using TMPro;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.UI
{
    public class NameSelection : MonoBehaviour
    {
        [SerializeField] private GameModeManager gmManager;

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TMP_InputField inputField;

        private string p1Title;
        private string p2Title;

        private bool p1Done;
        private void Awake()
        {
            p1Done = false;

            p1Title = "1st player's name";
            p2Title = "2nd player's name";
        }
        private void Update()
        {
            if (!p1Done && title.text != p1Title) title.text = p1Title;
            if (p1Done && title.text != p2Title) title.text = p2Title;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (CheckInput(inputField.text))
                {
                    if (!p1Done)
                    {
                        gmManager.p1Name = inputField.text;
                        inputField.text = "";
                        p1Done = true;
                    }
                    else
                    {
                        gmManager.p2Name = inputField.text;
                        DisableGameObject();
                    }
                }
            }
        }

        private bool CheckInput(string name)
        {
            if (name.Length < 4 || name.Length > 6)
            {
                return false;
            }
            foreach (char c in name)
            {
                if (Char.IsDigit(c)) return false;
                if (c == ' ') return false;
            }

            if (p1Done && name == gmManager.p1Name) return false;

            return true;
        }

        private void DisableGameObject()
        {
            gameObject.SetActive(false);
        }
    }
}