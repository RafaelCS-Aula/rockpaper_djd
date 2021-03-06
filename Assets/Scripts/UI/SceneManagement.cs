﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPS_DJDIII.Assets.Scripts.GameManagers
{
    public class SceneManagement : MonoBehaviour
    {
        public void NextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        public void PreviousScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}