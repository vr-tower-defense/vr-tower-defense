using System;
using System.Collections;
using UnityEngine;

interface IStep
{
    void FixedUpdate();

    void OnDisable();
}

public class RotateUp : IStep
{
    private readonly CelebrationState _celebrationState;
    private readonly Quaternion _aimRotation;

    /// <summary>
    /// 
    /// </summary>
    public void FixedUpdate()
    {
        if (Quaternion.Angle(_celebrationState.transform.rotation, _aimRotation) > 1)
        {
            _celebrationState.transform.rotation = Quaternion.RotateTowards(
                _celebrationState.transform.rotation,
                _aimRotation,
                _celebrationState.RotationSpeed * Time.deltaTime
            );

            return;
        }

        _celebrationState.SetStep(typeof(Idle));
    }

    public void OnDisable()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="celebrationState"></param>
    public RotateUp(CelebrationState celebrationState)
    {
        _celebrationState = celebrationState;
        _aimRotation = Quaternion.LookRotation(celebrationState.AimDirection);
    }
}

public class Idle : IStep
{
    private readonly CelebrationState _celebrationState;

    private readonly ParticleSystem _particleSystemInstance;

    private int _spawnIndex = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="celebrationState"></param>
    public Idle(CelebrationState celebrationState)
    {
        _celebrationState = celebrationState;

        // Get initial spawn reference
        var spawn = _celebrationState.Tower.ProjectileSpawns[_spawnIndex].transform;

        // Create new firework particle system instances and start the celebration
        _particleSystemInstance = MonoBehaviour.Instantiate(
            _celebrationState.CelebrationEffect,
            spawn.position,
            spawn.rotation
        );

        _particleSystemInstance.Play();

    }

    /// <summary>
    /// Destory particle system instance
    /// </summary>
    public void OnDisable()
    {
        MonoBehaviour.Destroy(_particleSystemInstance);
    }

    public void FixedUpdate()
    {
        _celebrationState.transform.Rotate(
            _celebrationState.RotationAxis,
            _celebrationState.RotationSpeed * Time.deltaTime
        );

        if (_particleSystemInstance.isStopped)
        {
            SetCelebrationToNextSpawn();
        }
    }

    private void SetCelebrationToNextSpawn()
    {
        _spawnIndex = (_spawnIndex + 1) % _celebrationState.Tower.ProjectileSpawns.Length;

        // Get new spawn reference
        var spawn = _celebrationState.Tower.ProjectileSpawns[_spawnIndex].transform;

        _particleSystemInstance.transform.position = spawn.position;
        _particleSystemInstance.Play();
    }
}

public class CelebrationState : TowerState
{
    [Header("Appearance")]
    public ParticleSystem CelebrationEffect;

    [Header("Rotation")]
    public Vector3 AimDirection = Vector3.up;
    public Vector3 RotationAxis = Vector3.up;

    [Tooltip("Speed at which the tower rotates in degrees")]
    [Range(0, 360)]
    public float RotationSpeed = 30;

    private IStep _currentStep;

    private void OnEnable()
    {
        _currentStep = new RotateUp(this);
    }

    private void OnDisable()
    {
        _currentStep.OnDisable();
    }

    void FixedUpdate()
    {
        _currentStep.FixedUpdate();
    }

    public void SetStep(Type type)
    {
        _currentStep = (IStep)Activator.CreateInstance(type, new object[] { this });
    }
}