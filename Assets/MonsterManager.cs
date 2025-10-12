using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowWatcher : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform playerHead;

    [Header("Appearance Settings")]
    public float appearDistance = 10f;
    public float appearTime = 3f;
    public float disappearTime = 1f;
    public float minWaitBetweenAppearances = 20f;
    public float maxWaitBetweenAppearances = 60f;

    [Header("Audio Settings")]
    public List<AudioSource> audioCues; // sounds that play when shadow appears
    public float minAudioDelay = 0.5f;
    public float maxAudioDelay = 2f;
    public bool randomizeAudioOrder = true;

    [Header("Environmental Object Activity")]
    public List<GameObject> flickerObjects;
    public float minObjectToggleDelay = 5f;
    public float maxObjectToggleDelay = 15f;

    private Renderer shadowRenderer;

    void Start()
    {
        shadowRenderer = GetComponentInChildren<Renderer>();
        if (shadowRenderer != null)
            shadowRenderer.enabled = false;

        StartCoroutine(ShadowRoutine());
        StartCoroutine(ObjectActivityRoutine());
    }

    IEnumerator ShadowRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitBetweenAppearances, maxWaitBetweenAppearances));

            Vector3 randomPos = playerHead.position + Random.onUnitSphere * appearDistance;
            randomPos.y = playerHead.position.y - 0.5f; // keep roughly on ground

            transform.position = randomPos;
            transform.LookAt(playerHead);

            if (shadowRenderer != null)
                shadowRenderer.enabled = true;

            // Play random or sequenced audio cues
            if (audioCues != null && audioCues.Count > 0)
            {
                List<AudioSource> cuesToPlay = new List<AudioSource>(audioCues);
                if (randomizeAudioOrder)
                    cuesToPlay.Sort((a, b) => Random.Range(-1, 2));

                StartCoroutine(PlayAudioCues(cuesToPlay));
            }

            yield return new WaitForSeconds(appearTime);

            // fade out
            if (shadowRenderer != null)
            {
                float t = 0f;
                Color color = shadowRenderer.material.color;
                while (t < disappearTime)
                {
                    color.a = Mathf.Lerp(1f, 0f, t / disappearTime);
                    shadowRenderer.material.color = color;
                    t += Time.deltaTime;
                    yield return null;
                }
                shadowRenderer.enabled = false;
            }
        }
    }

    IEnumerator PlayAudioCues(List<AudioSource> cues)
    {
        foreach (var audio in cues)
        {
            yield return new WaitForSeconds(Random.Range(minAudioDelay, maxAudioDelay));
            if (audio != null)
                audio.Play();
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
                // Optional: add a random re-enable after a few seconds
                if (!currentState)
                {
                    yield return new WaitForSeconds(Random.Range(1f, 5f));
                    if (obj != null) obj.SetActive(true);
                }
            }
        }
    }
}
