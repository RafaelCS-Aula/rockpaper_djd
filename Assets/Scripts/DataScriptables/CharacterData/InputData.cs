using UnityEngine;

namespace rockpaper_djd
{
    [CreateAssetMenu(fileName = "InputData",
    menuName = "Data/Inputs/Input Data", order = 2)]
    public class InputData : ScriptableObject
    {
        [Header("-Info-")]
        [Tooltip("Name for the input type")]
        public string inputName;


        [Header("-Movement Controls-")]
        [Tooltip("Name of the Horizontal Movement Axis")]
        public string hMovAxis;

        [Tooltip("Name of the Horizontal Movement Axis")]
        public string vMovAxis;

        [Tooltip("Name for the input type")]
        public KeyCode jump;

        [Tooltip("Name for the input type")]
        public KeyCode dash;


        [Header("-Camera Controls-")]
        [Tooltip("Name for the input type")]
        public string hCamAxis;

        [Tooltip("Name for the input type")]
        public string vCamAxis;

        [Tooltip("Name for the input type")]
        public KeyCode switchShoulders;


        [Header("-Combat Controls-")]
        [Tooltip("Name for the input type")]
        public KeyCode shoot;

        [Tooltip("Name for the input type")]
        public KeyCode previousType;

        [Tooltip("Name for the input type")]
        public KeyCode nextType;

        [Tooltip("Name for the input type")]
        public string typeScrollAxis;

        [Tooltip("Name for the input type")]
        public KeyCode switchToRock;

        [Tooltip("Name for the input type")]
        public KeyCode switchToPaper;

        [Tooltip("Name for the input type")]
        public KeyCode switchToScissors;
    }
}