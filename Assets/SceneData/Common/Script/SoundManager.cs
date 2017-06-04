namespace Common
{
  using UnityEngine;
  using System.Collections;
  using System.Collections.Generic;

  public class SoundManager : SingletonUnityObject<SoundManager>
  {
    AudioSource[] bgmSource = new AudioSource[2];
    List<AudioSource> seSource = new List<AudioSource>();

    float seVol;
    float bgmVol;

    public float SeVol { set { seVol = value; } }
    public float BgmVol { set { bgmVol = value; } }

    bool isFade = false;

    public void Start()
    {
      bgmSource[0] = gameObject.AddComponent<AudioSource>();
      bgmSource[1] = gameObject.AddComponent<AudioSource>();

      for(int i = 0; i < 3; i++ )
      {
        var se = gameObject.AddComponent<AudioSource>();
        seSource.Add(se);
      }

    }

    AudioSource GetSeSource()
    {
      for(int i = 0; i < seSource.Count; i++)
      {
        if(!seSource[i].isPlaying)
        {
          return seSource[i];
        }
      }

      var se = gameObject.AddComponent<AudioSource>();
      seSource.Add(se);
      return se;
    }

    //SE再生
    public void PlaySe(AudioClip _clip)
    {
      var se = GetSeSource();

      se.clip = _clip;
      se.loop = false;
      se.volume = seVol;
      se.Play();
    }

    //BGM再生
    public void PlayBgm(AudioClip _clip,bool _isLoop)
    {
      bgmSource[0].loop = _isLoop;
      bgmSource[0].clip = _clip;
      bgmSource[0].volume = bgmVol;
      bgmSource[0].Play();
    }

    public void PlayBgmFade(AudioClip _clip,bool _isLoop,float _fadeTime)
    {
      AudioSource outSource = null;
      AudioSource inSource = null;

      //なってるソースを見つけてそれをアウトさせる
      if (bgmSource[0].isPlaying)
      {
        outSource = bgmSource[0];
        inSource = bgmSource[1];
      }
      else if (bgmSource[1].isPlaying)
      {
        outSource = bgmSource[1];
        inSource = bgmSource[0];
      }
      else
      {
        //何もなってない場合は普通に再生
        PlayBgm(_clip, _isLoop);
        return;
      }

      inSource.loop = _isLoop;
      inSource.clip = _clip;
      inSource.volume = bgmVol;

      //フェード処理スタート
      StartCoroutine(Fade(outSource, inSource,_fadeTime));

    }

    IEnumerator Fade(AudioSource _outSource, AudioSource _inSource,float _fadeTime)
    {
      isFade = true;

      float timer = 0;
      float stVol = _outSource.volume;
      while(timer >= _fadeTime)
      {
        timer += Time.deltaTime;

        float vol = stVol * (1 - timer / _fadeTime);
        _outSource.volume = vol;
        yield return null;
      }

      _outSource.volume = 0;
      _outSource.Stop();

      timer = 0;
      float tarVol = _inSource.volume;
      _inSource.volume = 0;
      _inSource.Play();
      while(timer >= _fadeTime)
      {
        timer += Time.deltaTime;

        float vol = tarVol * (timer / _fadeTime);

        _outSource.volume = vol;
        yield return null;

      }

      _inSource.volume = tarVol;

      isFade = false;

    }

  }
    
}