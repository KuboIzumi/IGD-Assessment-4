using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private AudioSource audioSource; // the AudioSource component to play audio from
    public AudioClip startSound = null; // the AudioClip to play

    public TextMeshProUGUI beginText; // the text object to show at the start of the scene

    public float timer = 0f; // the timer for spawning powerups

    private void Awake()
    {
        Time.timeScale = 1f; // make sure the timescale is correct
        timer = Time.time;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(startSound); // play the starting sound
    }

    private void Update()
    {
        
    }


    /*
     * Pause the game for 3 seconds, then hide the beginText object
     */
    private IEnumerator StartGame()
    {
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + 3.0f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;

        beginText.gameObject.SetActive(false);
    }
}
