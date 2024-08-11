using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioClip walk;
    public AudioClip Warriorattack;
    public AudioClip WarriorSkill1;
    public AudioClip WarriorSkill2;
    public AudioClip Healerattack;
    public AudioClip HealerSkill1;
    public AudioClip HealerSkill2;
    public AudioClip Wizardattack;
    public AudioClip WizardSkill1;
    public AudioClip WizardSkill2;
    public AudioClip Die;
    public AudioClip Hit;
    public AudioClip Jump;
    public AudioSource audioSource;

    public void WalkSound() 
    {
        audioSource.clip = walk;
        audioSource.volume = 0.9f;
        audioSource.pitch = 2.2f;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void RunSound() 
    {
        audioSource.clip = walk;
        audioSource.volume = 0.9f;
        audioSource.pitch = 3.5f;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void WarriorAttackSound() 
    {
        audioSource.clip = Warriorattack;
        audioSource.volume = 0.5f;
        audioSource.pitch = 1.1f;
        audioSource.Play();
    }

    public void WarriorSkil1Sound() 
    {
        audioSource.clip = WarriorSkill1;
        audioSource.volume = 0.1f;
        audioSource.pitch = 1.8f;
        audioSource.Play();
    }

    public void WarriorSkil2Sound()
    {
        audioSource.clip = WarriorSkill2;
        audioSource.volume = 0.3f;
        audioSource.pitch = 0.8f;
        audioSource.Play();
    }

    public void HealerAttackSound()
    {
        audioSource.clip = Healerattack;
        audioSource.volume = 0.3f;
        audioSource.pitch = 1.0f;
        audioSource.Play();
    }

    public void HealerSkil1Sound()
    {
        audioSource.clip = HealerSkill1;
        audioSource.volume = 0.1f;
        audioSource.pitch = 1.0f;
        audioSource.Play();
    }

    public void HealerSkil2Sound()
    {
        audioSource.clip = HealerSkill2;
        audioSource.volume = 0.3f;
        audioSource.pitch = 1.5f;
        audioSource.Play();
    }

    public void WizardAttackSound()
    {
        audioSource.clip = Wizardattack;
        audioSource.volume = 0.3f;
        audioSource.pitch = 1.0f;
        audioSource.Play();
    }

    public void WizardSkil1Sound()
    {
        audioSource.clip = WizardSkill1;
        audioSource.volume = 0.4f;
        audioSource.pitch = 1.8f;
        audioSource.Play();
    }

    public void WizardSkil2Sound()
    {
        audioSource.clip = WizardSkill2;
        audioSource.volume = 0.3f;
        audioSource.pitch = 1.5f;
        audioSource.Play();
    }

    public void JumpSound()
    {
        audioSource.clip = Jump;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }

    public void DieSound()
    {
        audioSource.clip = Die;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }

    public void HitSound()
    {
        audioSource.clip = Hit;
        audioSource.volume = 0.2f;
        audioSource.pitch = 0.8f;
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}
