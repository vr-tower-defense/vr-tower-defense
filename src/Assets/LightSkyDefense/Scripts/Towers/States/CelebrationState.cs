using System;
using System.Collections;
using UnityEngine;

interface IStep
{
    void FixedUpdate();
}

public class RotateUp : IStep
{
    private readonly CelebrationState _celebrationState;
    private readonly Quaternion _aimRotation;

    public RotateUp(CelebrationState celebrationState)
    {
        _celebrationState = celebrationState;
        _aimRotation = Quaternion.LookRotation(celebrationState.AimDirection);
    }

    public void FixedUpdate()
    {
        if (Quaternion.Angle(_celebrationState.transform.rotation, _aimRotation) > 1)
        {
            _celebrationState.transform.rotation = Quaternion.Slerp(
                _celebrationState.transform.rotation,
                _aimRotation, 
                _celebrationState.RotationSpeed * Time.deltaTime
            );

            return;
        }

        _celebrationState.SetStep(typeof(Idle));
    }
}

public class Idle : IStep
{
    private readonly CelebrationState _celebrationState;
    private int _spawnIndex = 0;

    public Idle(CelebrationState celebrationState)
    {
        _celebrationState = celebrationState;

        // Create new firework particle system instances and start the celebration
        _celebrationState.ParticleSystemInstance = MonoBehaviour.Instantiate(
            _celebrationState.CelebrationEffect,
            _celebrationState.ParticleSystemSpawns[_spawnIndex].transform.position,
            _celebrationState.ParticleSystemSpawns[_spawnIndex].transform.rotation
        );

        _celebrationState.ParticleSystemInstance.Play();

    }

    public void FixedUpdate()
    {
        _celebrationState.transform.Rotate(_celebrationState.RotationAxis * _celebrationState.RotationSpeed);

        if(_celebrationState.ParticleSystemInstance.isStopped)
        {
            SetCelebrationToNextSpawn();
        }
    }

    private void SetCelebrationToNextSpawn()
    {
        _spawnIndex++;
        if (_spawnIndex == _celebrationState.ParticleSystemSpawns.Length)
            _spawnIndex = 0;

        _celebrationState.ParticleSystemInstance.transform.position = 
            _celebrationState.ParticleSystemSpawns[_spawnIndex].transform.position;

        _celebrationState.ParticleSystemInstance.Play();
    }
}

public class CelebrationState : TowerState
{
    public ParticleSystem CelebrationEffect;
    public Transform[] ParticleSystemSpawns;

    public Vector3 AimDirection = Vector3.up;
    public Vector3 RotationAxis = Vector3.up;
    public float RotationSpeed = 0.1f;

    private IStep _currentStep;

    /// <summary>
    /// Up rotation angle
    /// </summary>
    [HideInInspector]
    public ParticleSystem ParticleSystemInstance;

    /// <summary>
    /// Stop fireworks when state is disabled
    /// </summary>
    private void OnDisable()
    {
        Destroy(ParticleSystemInstance);
    }

    private void OnEnable()
    {
        _currentStep = new RotateUp(this);

        ParticleSystemInstance = new ParticleSystem();
    }

    void FixedUpdate()
    {
        _currentStep.FixedUpdate();
    }

    public void SetStep(Type type)
    {
        _currentStep = (IStep) Activator.CreateInstance(type, new object[] { this });
    }
}