using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour {
    [Header("Stats")]
    public int Ammo;

    [Header("State")]
    private bool shootingState;
    public static GunController instance;
    private StarterAssetsInputs starterAssetsInputs;


    private void Awake() {
        instance = this;
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update() {
        if (Ammo <= 0) {
            starterAssetsInputs.shoot = false;
        } 
        if (starterAssetsInputs.reload) {
            Reload();
        }
    }

    void Reload() {
        this.Ammo = 6;
        starterAssetsInputs.reload = false;
    }
}
