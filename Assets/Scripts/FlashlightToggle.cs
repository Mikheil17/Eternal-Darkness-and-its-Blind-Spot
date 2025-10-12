using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightToggle : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlightLight1;                  // The spotlight component
    public Light flashlightLight2;                  // The spotlight component
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
        flashlightLight1.enabled = !flashlightLight1.enabled;
        flashlightLight2.enabled = !flashlightLight2.enabled;

        // Play click sound (if assigned)
        if (clickAudio != null)
            clickAudio.Play();
    }
}
