using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSoundManager : MonoBehaviour
{
    public static PSoundManager instance { get; private set; }

    [Header("[ BGM ]")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("[ SFX ]")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    private int channelIndex;

    public enum sfx { Select, Swipe, Error, buy}

    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Start()
    {
        PlayBgm(true);
    }

    private void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("bgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();

        bgmPlayer.playOnAwake = true;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else bgmPlayer.Stop();
    }

    public void PlaySfx(sfx sfx)
    {
        // �� ä���� ã�� ����ϴ� �ڵ�
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx ];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
