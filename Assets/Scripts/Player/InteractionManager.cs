using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class InteractionManager : MonoBehaviour {
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject curInteractGameObject;
    private IInteractable curInteractable;
    public TextMeshProUGUI promptText;
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private GameObject debugTransform;

    private Camera cam;
    private bool check;

    void Start() {
        cam = Camera.main;
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }
    void Update() {
        // checking every so often to see if cursor is hovering interactable
        if (Time.time - lastCheckTime > checkRate) {
            lastCheckTime = Time.time;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            // did we hit an interactable layer
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) {
                if (hit.collider.gameObject != curInteractGameObject) {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            } else {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
        if(starterAssetsInputs.interact){
            OnInteractInput();
        }
    }
    void SetPromptText() {
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[E]</b> {0}", curInteractable.GetInteractPrompt());
    }
    // called when we press the "E" button - managed by the Input System
    public void OnInteractInput() {
        // did we press down this frame and are we hovering over an interactable?
        if (curInteractable != null) {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
        starterAssetsInputs.interact = false;
    }
}


public interface IInteractable {
    string GetInteractPrompt();
    void OnInteract();
}