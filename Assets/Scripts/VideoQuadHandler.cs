using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))]
public class VideoQuadHandler : MonoBehaviour
{
    [Header("References (assign in Inspector or let script create)")]
    public VideoPlayer videoPlayer;
    public GameObject quad;
    public MeshRenderer quadRenderer;
    public Transform vrCamera;

    [Header("Post-Video")]
    public GameObject uiToShowAfterVideo;
    public Canvas canvasToDeactivate;

    [Header("Placement (only used if quad is auto-created)")]
    public float distanceFromCamera = 1.0f;
    public Vector2 quadResolution = new Vector2(1920, 1080);


    void Start()
    {

        // Basic validation
        if (vrCamera == null && Camera.main != null)
            vrCamera = Camera.main.transform;

        if (vrCamera == null)
        {
            Debug.LogError("[VideoQuadHandler] VR Camera not assigned and no MainCamera found.");
            enabled = false;
            return;
        }

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Track whether we need to create and position the quad
        bool quadWasManuallyPlaced = (quad != null);

        // Create quad only if needed
        if (quad == null)
        {
            quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = "VideoQuad_Auto";
            DestroyImmediate(quad.GetComponent<Collider>());

            // Only position it if we just created it
            quad.transform.SetParent(vrCamera, false);
            quad.transform.localPosition = new Vector3(0f, 0f, distanceFromCamera);
            quad.transform.localRotation = Quaternion.identity;

            float aspect = quadResolution.x / quadResolution.y;
            quad.transform.localScale = new Vector3(aspect, 1f, 1f);
        }
        // If quad was manually placed, don't touch its transform at all

        if (quadRenderer == null)
            quadRenderer = quad.GetComponent<MeshRenderer>();

        if (quadRenderer == null)
        {
            Debug.LogError("[VideoQuadHandler] Quad renderer missing.");
            enabled = false;
            return;
        }

        if (quadRenderer.sharedMaterial == null)
        {
            Shader unlit = Shader.Find("Unlit/Texture");
            if (unlit != null)
                quadRenderer.sharedMaterial = new Material(unlit);
            else
                quadRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        }

        if (videoPlayer == null)
            videoPlayer = quad.AddComponent<VideoPlayer>();

        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = quadRenderer;
        videoPlayer.targetMaterialProperty = "_MainTex";

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);

        videoPlayer.loopPointReached += OnVideoEnd;

        if (!videoPlayer.isPrepared)
            videoPlayer.Prepare();

        videoPlayer.prepareCompleted += (vp) =>
        {
            if (vp.clip != null || vp.url != null)
                vp.Play();
            else
                Debug.LogWarning("[VideoQuadHandler] VideoPlayer prepared but has no clip/url.");
        };

        if (videoPlayer.isPrepared && !videoPlayer.isPlaying)
            videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Hide the quad
        if (quad != null)
            quad.SetActive(false);

        // Deactivate the canvas (if assigned)
        if (canvasToDeactivate != null)
            canvasToDeactivate.gameObject.SetActive(false);

        // Show post-video UI
        if (uiToShowAfterVideo != null)
            uiToShowAfterVideo.SetActive(true);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoEnd;
    }
}