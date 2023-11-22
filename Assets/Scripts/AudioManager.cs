using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource ballAudioSrc;
    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private AudioClip bounceClip;
    [SerializeField] private AudioClip sizzle;
    [SerializeField] private AudioClip ringCollect;
    [SerializeField] private AudioClip cpActivate;
    [SerializeField] private AudioClip mud;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip lose;

    private void Awake()
    {
        AudioManager.Instance = this;
    }

    private void Start()
    {
        bgMusic.mute = false;
    }


    public void Bounce(float volume)
    {
        ballAudioSrc.PlayOneShot(bounceClip,volume);
    }public void Burn()
    {
        ballAudioSrc.PlayOneShot(sizzle,0.3f);
    }

    public void Ring()
    {
        ballAudioSrc.PlayOneShot(ringCollect,0.5f);
    }public void Mud()
    {
        ballAudioSrc.PlayOneShot(mud,0.5f);
    }public void Win()
    {
        ballAudioSrc.PlayOneShot(victory,0.5f);
        bgMusic.mute = true;
    }public void Lose()
    {
        ballAudioSrc.PlayOneShot(lose,0.5f);
        bgMusic.mute = true;
    }

    public void CpActivate(AudioSource src)
    {
        src.PlayOneShot(cpActivate);
    }
}
