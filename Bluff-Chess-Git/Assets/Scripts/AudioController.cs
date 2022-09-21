using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioClip move1Sound;
    [SerializeField] AudioClip move2Sound;
    [SerializeField] AudioClip wasBluffSound;
    [SerializeField] AudioClip wasNotBluffSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip bombSound;
    [SerializeField] AudioClip keyboardSound;
    
    [SerializeField] AudioClip roundOverSound;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip sacrificeSound;
    [SerializeField] AudioClip oneSecTick;
    [SerializeField] AudioClip randomizeSound;
    [SerializeField] AudioClip claimKingSound;
    [SerializeField] AudioClip turnOnYouSound;
    [SerializeField] AudioClip passSound;
    [SerializeField] AudioClip bluffClaimedSound;



    AudioSource audioSource;


    Settings settings;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        settings = FindObjectOfType<Settings>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void PlaySound(string soundName)
    {
        if(!settings.audioOpen)return;

        if (soundName == "moveSound")
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) audioSource.PlayOneShot(move1Sound);
            else audioSource.PlayOneShot(move2Sound);
        }
        else if (soundName == "attackSound") audioSource.PlayOneShot(attackSound);
        else if (soundName == "wasNotBluffSound") audioSource.PlayOneShot(wasNotBluffSound);
        else if (soundName == "wasBluffSound") audioSource.PlayOneShot(wasBluffSound);
        else if (soundName == "BombSound") audioSource.PlayOneShot(bombSound);
        else if (soundName == "keyboardSound") audioSource.PlayOneShot(keyboardSound);

        else if (soundName == "roundOverSound") audioSource.PlayOneShot(roundOverSound);
        else if (soundName == "gameOverSound") audioSource.PlayOneShot(gameOverSound);
        else if (soundName == "turnOnYouSound") audioSource.PlayOneShot(turnOnYouSound);
        else if (soundName == "tickSound") audioSource.PlayOneShot(oneSecTick);
        else if (soundName == "claimKingSound") audioSource.PlayOneShot(claimKingSound);
        else if (soundName == "randomizeSound") audioSource.PlayOneShot(randomizeSound);
        else if (soundName == "sacrificeSound") audioSource.PlayOneShot(sacrificeSound);
        else if (soundName == "passSound") audioSource.PlayOneShot(passSound);
        else if (soundName == "bluffClaimedSound") audioSource.PlayOneShot(bluffClaimedSound);
    }
}
