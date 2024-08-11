using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSound : MonoBehaviour
{
    public AudioClip Get;
    public AudioClip Drink;
    public AudioSource audioSource;

    public void GetSound() 
    {
        audioSource.clip = Get;
        audioSource.volume = 0.5f;
        audioSource.pitch = 0.9f;
        audioSource.Play();
    }

    public void DrinkSound()
    {
        audioSource.clip = Drink;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
}
