using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalGun : MonoBehaviour
{
    // Inputs
    public InputAction equipGun;
    public InputAction shootA;
    public InputAction shootB;

    [Header("References")]
    public Animator animator;
    public AudioSource AudioSource;
    [Tooltip("Referencia al script del jugador para conocer su estado.")]
    public PlayerController playerController;

    Camera cameraMain;

    // Shooting
    [Header("Shooting Settings")]
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private SpriteRenderer spriteRenderer;

    [SerializeField] GameObject projectilePrefabA;
    [SerializeField] GameObject projectilePrefabB;
    [SerializeField] Transform shootPoint;

    public bool weaponEquipped = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraMain = Camera.main;

        // Intentar buscar el PlayerController si no se asignó en el inspector
        if (playerController == null)
        {
            playerController = GetComponentInParent<PlayerController>();
        }
    }

    void Update()
    {
        // 1. Verificación de muerte: Si el jugador está muerto, forzamos el estado desactivado
        if (playerController != null && playerController.state == PlayerController.PlayerState.Dead)
        {
            DisableGunTemporarily();
            return; // Salimos del Update para que no se procesen inputs ni rotación
        }

        HandleActions();
        gunState();
        FlipSprite();

        if (weaponEquipped)
        {
            RotateTowardsMouse();
        }
    }

    private void HandleActions()
    {
        if (weaponEquipped)
        {
            if (Time.time >= nextFireTime)
            {
                if (shootA.triggered)
                {
                    ShootPortal(projectilePrefabA);
                    UpdateNextFireTime();
                }
                else if (shootB.triggered)
                {
                    ShootPortal(projectilePrefabB);
                    UpdateNextFireTime();
                }
            }
        }

        if (equipGun.triggered)
        {
            weaponEquipped = !weaponEquipped;
        }
    }

    private void UpdateNextFireTime()
    {
        nextFireTime = Time.time + fireRate;
    }

    private void ShootPortal(GameObject projectilePrefab)
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;

        animator.SetTrigger("isShooting");
        AudioSource.Play();
    }

    // Apaga el sprite si el jugador muere
    private void DisableGunTemporarily()
    {
        spriteRenderer.enabled = false;
        // Opcional: podrías resetear weaponEquipped = false; si quieres que al revivir tenga que equiparla de nuevo
    }

    private void OnEnable()
    {
        equipGun.Enable();
        shootA.Enable();
        shootB.Enable();
    }

    private void OnDisable()
    {
        equipGun.Disable();
        shootA.Disable();
        shootB.Disable();
    }

    private void gunState()
    {
        spriteRenderer.enabled = weaponEquipped;
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePosition = cameraMain.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FlipSprite()
    {
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 90f && zRotation < 270f)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }
}