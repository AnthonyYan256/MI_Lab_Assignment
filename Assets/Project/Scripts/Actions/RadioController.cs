using UnityEngine;
using System.Collections.Generic; // Required for using Lists

/// <summary>
/// Manages all logic for the Radio object, including power,
/// song selection, and volume.
/// This script's public methods are called directly by the UI buttons.
/// It is placed on the same object as the InteractionMenu.
/// Conforms to Assignment 3 specifications.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class RadioController : MonoBehaviour
{
    [Header("Audio Setup")]
    [Tooltip("Drag your Audio Clips here. Requires at least 2.")]
    public List<AudioClip> songs; 

    private AudioSource audioSource;
    private int currentSongIndex = 0;
    private bool isPoweredOn = false;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Caches the AudioSource and sets it up.
    /// </summary>
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Configure the AudioSource as per the assignment
        audioSource.playOnAwake = false;
        audioSource.loop = true; // The current song loops
        isPoweredOn = false;
        audioSource.Stop(); // Ensure it's off at the start
        
        // Load the first song
        if (songs.Count > 0)
        {
            audioSource.clip = songs[currentSongIndex];
        }
    }

    /// <summary>
    /// Toggles the radio power on or off.
    /// This is called by the 'Power' UI Button. 
    /// </summary>
    public void TogglePower()
    {
        isPoweredOn = !isPoweredOn;

        if (isPoweredOn)
        {
            // If turning on, play the currently cued song.
            audioSource.Play();
        }
        else
        {
            // If turning off, stop the music.
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Plays the next song in the 'songs' list.
    /// This is called by the 'Change Song' UI Button. 
    /// </summary>
    public void NextSong()
    {
        // Don't do anything if there are no songs 
        if (songs.Count == 0) return;

        // Move to the next song index
        currentSongIndex++;
        
        // If we've passed the end of the list, loop back to the first song
        if (currentSongIndex >= songs.Count)
        {
            currentSongIndex = 0;
        }

        // Set the AudioSource to the new song
        audioSource.clip = songs[currentSongIndex];

        // If the radio is already on, play the new song immediately. 
        // If it's off, this just cues up the next song for when power is toggled.
        if (isPoweredOn)
        {
            audioSource.Play();
        }
    }

    /// <summary>
    /// Sets the volume of the AudioSource.
    /// This is called by the 'Volume' UI Slider's OnValueChanged event.
    /// </summary>
    /// <param name="newVolume">A value between 0.0 and 1.0 from the slider.</param>
    public void SetVolume(float newVolume)
    {
        // The slider's 0-1 value maps directly to the audioSource's volume
        audioSource.volume = newVolume;
    }
}