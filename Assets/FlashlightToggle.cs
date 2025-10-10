using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightToggle : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlightLight;                  // The spotlight component
    public InputActionReference toggleAction;      // Input action for the button
    public AudioSource clickAudio;                 // Audio source for the click sound

    private void OnEnable()
    {
        if (toggleAction != null)
            toggleAction.action.performed += ToggleLight;
    }

    private void OnDisable()
    {
        if (toggleAction != null)
            toggleAction.action.performed -= ToggleLight;
    }

    private void ToggleLight(InputAction.CallbackContext ctx)
    {
        // Toggle the light on/off
        flashlightLight.enabled = !flashlightLight.enabled;

        // Play click sound (if assigned)
        if (clickAudio != null)
            clickAudio.Play();
    }
}
