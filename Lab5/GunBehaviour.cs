using NUnit.Framework.Internal.Builders;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioSource _gunshotSound;
    [SerializeField] private float _fireRate = 5f; // Shots per second

    private float _nextFireTime = 0f;
    private InputAction _fireAction;

    private void Awake()
    {
        if (_animator == null)
            Debug.LogWarning($"{nameof(GunBehaviour)}: Animator reference is missing.", this);
        if (_particleSystem == null)
            Debug.LogWarning($"{nameof(GunBehaviour)}: ParticleSystem reference is missing.", this);
        if (_gunshotSound == null)
            Debug.LogWarning($"{nameof(GunBehaviour)}: AudioSource reference is missing.", this);

        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            _fireAction = playerInput.actions["Fire"];
        }
        else
        {
            Debug.LogWarning($"{nameof(GunBehaviour)}: PlayerInput component is missing.", this);
        }
    }

    private void Update()
    {
        if (_fireAction != null && _fireAction.IsPressed() && Time.time >= _nextFireTime)
        {
            Fire();
            _nextFireTime = Time.time + 1f / _fireRate;
        }
    }

    private void Fire()
    {
        if (_animator != null)
            _animator.SetTrigger("TrShoot");

        if (_particleSystem != null)
            _particleSystem.Play();

        if (_gunshotSound != null)
            _gunshotSound.Play();
    }
}
