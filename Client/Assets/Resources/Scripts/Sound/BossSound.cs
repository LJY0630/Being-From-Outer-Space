using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour
{
    public AudioClip BossLaser;
    public AudioClip BossPortal;
    public AudioClip BossPatern2;
    public AudioClip BossHit;
    public AudioClip BossDead;
    public AudioSource audioSource;
    public AudioSource laserSource;

    public void Loop(bool active) 
    {
        laserSource.loop = active;
    }

    public void BossPatern1Sound() 
    {
        laserSource.clip = BossLaser;
        laserSource.volume = 0.5f;
        laserSource.Play();
    }

    public void BossPatern2Sound() 
    {
        audioSource.clip = BossPatern2;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    public void BossPatern3Sound() 
    {
        audioSource.clip = BossPortal;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }

    public void HitSound() 
    {
        audioSource.clip = BossHit;
        audioSource.volume = 0.3f;
        audioSource.Play();
    }

    public void DeadSound()
    {
        audioSource.clip = BossDead;
        audioSource.volume = 0.3f;
        audioSource.Play();
    }
}
