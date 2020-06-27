using System.Collections.Generic;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.DataScriptables.ObjectsData
{
    /// <summary>
    /// Stores all static info about an object spawner
    /// </summary>
    [CreateAssetMenu(fileName = "SpawnerData",
    menuName = "Data/Arena/Spawner Data", order = 1)]
    public class SpawnerData : ScriptableObject
    {
        [Header("Action")]
        public List<GameObject> objectsToSpawn;
        public float spawnInterval;
        public int singleSpawnIndex = 0;

        [Header("Rules")]
        [Tooltip("Only spawn object when told to. Doesn't use spawnInterval.")]
        public bool isTriggerAction = false;

        [Tooltip("Spawn Object at the same time as spawner")]
        public bool startSpawn = true;

        [Tooltip("Only spawn one item of the object list at a time")]
        public bool singleSpawn = false;


    }
}