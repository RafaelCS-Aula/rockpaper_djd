using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    [RequireComponent(typeof(AudioSource))]
    public class AmdSoundHandler : MonoBehaviour, ISoundHolder,
    IDataUser<AmbientSoundSetData>
    {
        [SerializeField] private AmbientSoundSetData _audioData;
        private AudioSource _bGSource;
        private AudioSource _fXSource;
        private AudioClip _mainBG;
        private AudioClip[] _clips;

        // Start is called before the first frame update
        void Awake()
        {
            GetData();
            _bGSource = gameObject.GetComponent<AudioSource>();
            _bGSource.loop = true;
            _bGSource.playOnAwake = false;
            _bGSource.clip = _mainBG;
            _bGSource.Play();

            _fXSource = gameObject.AddComponent<AudioSource>();
            _fXSource.playOnAwake = false;
            _fXSource.loop = false;

        }

        // Update is called once per frame
        void Update()
        {
            int d = 0;
            // print("Time: " + (int)_bGSource.time);
            // print("Length: " + (int)_bGSource.clip.length);
            if ((int)_bGSource.time == (int)_bGSource.clip.length)
            {
                //print("ass");
                int rnd = Random.Range(0, _clips.Length - 1);
                _fXSource.pitch = 1 + Random.Range(-0.05f, 0.1f);
                _fXSource.PlayOneShot(_clips[rnd], Random.Range(0.2f, 0.4f));


            }
            d++;
            //print(Mathf.Sin(Time.time));
            _bGSource.volume += 0.0005f * Mathf.Sin(Time.time);
        }

        public AmbientSoundSetData DataHolder
        {
            get => _audioData;
            set => value = _audioData;
        }

        public void PlayAudio(AudioClip sound, float volume)
        {
            throw new System.NotImplementedException();
        }

        public void GetData()
        {
            _mainBG = DataHolder.background;
            _clips = DataHolder.audioLayers.ToArray();

        }
    }
}