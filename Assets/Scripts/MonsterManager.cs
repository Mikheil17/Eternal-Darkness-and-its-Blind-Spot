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
    public float appearDistance = 10f;
    public float shadowDuration = 5f;
    public float timeBetweenShadows = 20f;

    [Header("Audio Settings")]
    public List<AudioSource> audioCues;
    public float timeBetweenSounds = 20f;

    [Header("Environmental Object Activity")]
    public List<GameObject> flickerObjects;
    public float minObjectToggleDelay = 5f;
    public float maxObjectToggleDelay = 15f;

    void Start()
    {
        // Hide ALL shadow objects at start
        if (shadowObjects != null)
        {
            foreach (GameObject shadow in shadowObjects)
            {
                if (shadow != null)
                {
                    shadow.SetActive(false);
                }
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

        StartCoroutine(ShadowRoutine());
        StartCoroutine(AudioRoutine());
        StartCoroutine(ObjectActivityRoutine());
    }

    IEnumerator ShadowRoutine()
    {
        while (true)
        {
            // Wait 20 seconds
            yield return new WaitForSeconds(timeBetweenShadows);

            if (shadowObjects == null || shadowObjects.Count == 0)
                continue;

            // Pick a random shadow
            GameObject randomShadow = shadowObjects[Random.Range(0, shadowObjects.Count)];

            if (randomShadow != null)
            {
                // Position it randomly around the player
                Vector3 randomPos = playerHead.position + Random.onUnitSphere * appearDistance;
                randomPos.y = playerHead.position.y - 0.5f;

                randomShadow.transform.position = randomPos;
                randomShadow.transform.LookAt(playerHead);

                // Show the shadow
                randomShadow.SetActive(true);

                // Wait 5 seconds
                yield return new WaitForSeconds(shadowDuration);

                // Hide the shadow
                randomShadow.SetActive(false);
            }
        }
    }

    IEnumerator AudioRoutine()
    {
        while (true)
        {
            // Wait 20 seconds
            yield return new WaitForSeconds(timeBetweenSounds);

            if (audioCues == null || audioCues.Count == 0)
                continue;

            // Pick a random audio and play it
            AudioSource randomCue = audioCues[Random.Range(0, audioCues.Count)];

            if (randomCue != null)
            {
                randomCue.Play();
            }
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

                if (currentState)
                {
                    yield return new WaitForSeconds(Random.Range(1f, 5f));
                    if (obj != null) obj.SetActive(true);
                }
            }
        }
    }
}