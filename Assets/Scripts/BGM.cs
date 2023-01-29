using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{

    [SerializeField] AudioSource _mainBgm;
    [SerializeField] float _maxVol;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BGM");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    public void CallFader(bool start)
    {
        if (start)
        {
           // StopCoroutine(BgmFader(false));
            StartCoroutine(BgmFader(true));
        }
        else
        {
            //StopCoroutine(BgmFader(true));
            StartCoroutine(BgmFader(false));
        }
    }
    public IEnumerator BgmFader(bool start)
    {

        if (start)
        {
            if (!_mainBgm.isPlaying) _mainBgm.Play();

            while (_mainBgm.volume < _maxVol)
            {
                _mainBgm.volume += 0.3f *Time.deltaTime;
                yield return null;
            }

        }
        else
        {
            while (_mainBgm.volume > 0f)
            {
                _mainBgm.volume -= 0.3f * Time.unscaledDeltaTime;
                yield return null;

            }

        }
        yield return 0;
    }
}
