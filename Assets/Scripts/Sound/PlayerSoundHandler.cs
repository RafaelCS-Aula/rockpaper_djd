using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSoundHandler : MonoBehaviour, IDataUser<PlayerSoundSetData>,
    ISoundHolder
    {
        private List<AudioSource> sources;
        private AudioSource sourceComponent;
        [SerializeField] private PlayerSoundSetData _soundSet;
        public PlayerSoundSetData DataHolder
        {
            get => _soundSet;
            set => value = _soundSet;
        }

        [HideInInspector] public AudioClip dJump;
        [HideInInspector] public AudioClip dDoubleJump;
        [HideInInspector] public AudioClip dShot;
        [HideInInspector] public AudioClip dHurt;
        [HideInInspector] public AudioClip dDash;
        [HideInInspector] public AudioClip dDie;

        void Awake()
        {
            sources = new List<AudioSource>();
            sourceComponent = gameObject.GetComponent<AudioSource>();
            GetData();
        }

        public void GetData()
        {
            dJump = DataHolder.jumpSFX;
            dDash = DataHolder.dashSFX;
            dDoubleJump = DataHolder.doubleJumpSFX;
            dShot = DataHolder.shotSFX;
            dHurt = DataHolder.hurtSFX;
            dDie = DataHolder.deathSFX;
        }

        /// <summary>
        /// Uses the audio source component to play a sound, adds a new component
        /// if that component is occupied.
        /// </summary>
        /// <param name="sound"> Which AudioClip in this class instance of 
        /// PlayerSoundHandler to play</param>
        /// <param name="volume"></param>
        public void PlayAudio(AudioClip sound, float volume)
        {
            AudioSource newComp;


            if (sound == null)
            {
                Debug.LogWarning("No valid AudioClip");
                return;

            }


            if (sourceComponent.isPlaying)
            {
                foreach (AudioSource a in sources)
                {
                    if (a.isPlaying)
                        continue;
                    else
                    {
                        newComp = gameObject.AddComponent<AudioSource>();
                        newComp.PlayOneShot(sound, volume);
                        sources.Add(newComp);
                        break;
                    }


                }

            }
            else
            {
                sourceComponent.PlayOneShot(sound, volume);
            }



        }



    }
}