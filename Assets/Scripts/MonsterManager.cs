using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform playerHead;

    [Header("Shadow Objects")]
    public List<GameObject> shadowObjects;

    [Header("Appearance Settings")]
    public float shadowDuration = 8f;        // How long shadows stay visible
    public float timeBetweenShadows = 10f;   // Time between shadow appearances

    [Header("Audio Settings")]
    public List<AudioSource> audioCues;
    public float timeBetweenSounds = 20f;

    [Header("Environmental Object Activity")]
    public List<GameObject> flickerObjects;
    public float minObjectToggleDelay = 5f;
    public float maxObjectToggleDelay = 15f;

    [Header("Intro Grace Period")]
    public float introGracePeriod = 30f;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    void Start()
    {
        // Validation
        if (shadowObjects == null || shadowObjects.Count == 0)
        {
            Debug.LogError("[MonsterManager] No shadow objects assigned!");
            enabled = false;
            return;
        }

        // Hide all shadow objects at start (they're already positioned in your scene)
        foreach (GameObject shadow in shadowObjects)
        {
            if (shadow != null)
            {
                shadow.SetActive(false);
                if (enableDebugLogs)
                    Debug.Log($"[MonsterManager] Shadow '{shadow.name}' hidden at start");
            }
        }

        // Stop all audio sources at start
        if (audioCues != null)
        {
            foreach (AudioSource audio in audioCues)
            {
                if (audio != null)
                {
                    audio.Stop();
                    audio.playOnAwake = false;
                }
            }
        }

        if (enableDebugLogs)
            Debug.Log($"[MonsterManager] Starting intro grace period: {introGracePeriod}s");

        StartCoroutine(BeginAfterIntro());
    }

    IEnumerator BeginAfterIntro()
    {
        yield return new WaitForSeconds(introGracePeriod);

        if (enableDebugLogs)
            Debug.Log("[MonsterManager] Grace period ended, starting shadow spawns!");

        StartCoroutine(ShadowRoutine());
        StartCoroutine(AudioRoutine());
        StartCoroutine(ObjectActivityRoutine());
    }

    IEnumerator ShadowRoutine()
    {
        bool shadowActive = false;

        while (true)
        {
            // Random delay between shadows
            float delay = Random.Range(timeBetweenShadows * 0.6f, timeBetweenShadows * 1.0f);

            if (enableDebugLogs)
                Debug.Log($"[MonsterManager] Waiting {delay:F1}s before next shadow...");

            yield return new WaitForSeconds(delay);

            if (shadowActive)
            {
                if (enableDebugLogs)
                    Debug.Log("[MonsterManager] Shadow already active, skipping...");
                continue;
            }

            if (shadowObjects == null || shadowObjects.Count == 0)
            {
                Debug.LogWarning("[MonsterManager] No shadow objects available!");
                continue;
            }

            shadowActive = true;

            // Pick a random shadow from your list
            GameObject randomShadow = shadowObjects[Random.Range(0, shadowObjects.Count)];
            if (randomShadow != null)
            {
                // Just activate it at its current position
                randomShadow.SetActive(true);

                if (enableDebugLogs)
                {
                    Debug.Log($"[MonsterManager] ✓ Shadow '{randomShadow.name}' activated");
                    if (playerHead != null)
                        Debug.Log($"[MonsterManager]   Distance from player: {Vector3.Distance(playerHead.position, randomShadow.transform.position):F2}m");
                }

                yield return new WaitForSeconds(shadowDuration);
                randomShadow.SetActive(false);

                if (enableDebugLogs)
                    Debug.Log($"[MonsterManager] Shadow '{randomShadow.name}' deactivated after {shadowDuration}s");
            }
            else
            {
                Debug.LogWarning("[MonsterManager] Selected shadow object is null!");
            }

            shadowActive = false;
        }
    }

    IEnumerator AudioRoutine()
    {
        bool soundPlaying = false;

        while (true)
        {
            float delay = Random.Range(timeBetweenSounds * 0.8f, timeBetweenSounds * 1.3f);
            yield return new WaitForSeconds(delay);

            if (soundPlaying || audioCues == null || audioCues.Count == 0)
                continue;

            soundPlaying = true;

            AudioSource randomCue = audioCues[Random.Range(0, audioCues.Count)];
            if (randomCue != null && !randomCue.isPlaying)
            {
                randomCue.Play();

                if (enableDebugLogs)
                    Debug.Log($"[MonsterManager] Playing audio cue: {randomCue.name}");

                yield return new WaitForSeconds(randomCue.clip.length + 1f);
            }

            soundPlaying = false;
        }
    }

    IEnumerator ObjectActivityRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minObjectToggleDelay, maxObjectToggleDelay));

            if (flickerObjects == null || flickerObjects.Count == 0)
                continue;

            GameObject obj = flickerObjects[Random.Range(0, flickerObjects.Count)];
            if (obj != null)
            {
                bool currentState = obj.activeSelf;
                obj.SetActive(!currentState);

                if (enableDebugLogs)
                    Debug.Log($"[MonsterManager] Object '{obj.name}' toggled to: {!currentState}");

                if (currentState)
                {
                    yield return new WaitForSeconds(Random.Range(1f, 3f));
                    if (obj != null) obj.SetActive(true);
                }
            }
        }
    }
}