using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioClip Hit;
    public AudioClip Attack;
    public AudioClip Die;
    public AudioSource audioSource;

    public void HitSound() 
    {
        audioSource.clip = Hit;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }
    public void AttackSound()
    {
        audioSource.clip = Attack;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }
    public void DieSound()
    {
        audioSource.clip = Die;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }
}
