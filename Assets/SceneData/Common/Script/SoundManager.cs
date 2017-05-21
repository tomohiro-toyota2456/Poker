namespace Common
{
    using UnityEngine;
    using System.Collections;

    public class SoundManager : SingletonUnityObject<SoundManager>
    {
        AudioSource[] seAudioSources;
        AudioSource[] bgmAudioSources;
        [SerializeField]
        int seChannelSum;
        [SerializeField]
        int bgmChannelSum;

        public enum AudioType
        {
            BGM,
            SE,
        }

        private void Awake()
        {
            //チャンネル数だけオーディオソースを作成する
            seAudioSources = new AudioSource[seChannelSum];
            for(int i = 0; i < seChannelSum; i++)
            {
               seAudioSources[i] = gameObject.AddComponent<AudioSource>();
            }

            bgmAudioSources = new AudioSource[bgmChannelSum];
            for (int i = 0; i < bgmChannelSum; i++)
            {
                bgmAudioSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        public void PlayBGM(AudioClip _clip,bool _isLoop,int _channel = 0)
        {
            bgmAudioSources[_channel].clip = _clip;
            bgmAudioSources[_channel].volume = 1.0f;
            bgmAudioSources[_channel].loop = _isLoop;
            bgmAudioSources[_channel].Play();
        }

        public void PlayBGMFade(AudioClip _clip,bool _isLoop,float _fadeTime)
        {
            StartCoroutine(Fade(_clip, _isLoop, _fadeTime));
        }


        IEnumerator Fade(AudioClip _clip,bool _isLoop,float _fadeTime)
        {
      yield return null;
        }


        public void PlaySE(AudioClip _clip, int _channel = 0)
        {
            seAudioSources[_channel].clip = _clip;
            seAudioSources[_channel].volume = 1.0f;
            seAudioSources[_channel].loop = false;

            if(_channel == 0)
            {
                seAudioSources[_channel].PlayOneShot(_clip);
            }
            else
            {
                seAudioSources[_channel].Play();
            }
        }

        public void StopBGM(int _channel)
        {
            bgmAudioSources[_channel].Stop();
        }

        public void PauseBGM(int _channel)
        {
            bgmAudioSources[_channel].Pause();
        }

        public void UnPauseBGM(int _channel)
        {
            bgmAudioSources[_channel].UnPause();
        }
    }

}