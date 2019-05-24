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

    private int _spawnIndex = 0;
    private ParticleSystem _particleSystemInstance;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="celebrationState"></param>
    public Idle(CelebrationState celebrationState)
    {
        _celebrationState = celebrationState;

        // Create new firework particle system instances and start the celebration
        _particleSystemInstance = MonoBehaviour.Instantiate(
            _celebrationState.CelebrationEffect,
            _celebrationState.ParticleSystemSpawns[_spawnIndex].transform.position,
            _celebrationState.ParticleSystemSpawns[_spawnIndex].transform.rotation
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
        _spawnIndex = (_spawnIndex + 1) % _celebrationState.ParticleSystemSpawns.Length;

        _particleSystemInstance.transform.position =
            _celebrationState.ParticleSystemSpawns[_spawnIndex].transform.position;

        _particleSystemInstance.Play();
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