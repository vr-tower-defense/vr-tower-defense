using System;
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

    public Idle(CelebrationState celebrationState)
    {
        // Create new firework particle system instances and start the celebration
        for (int i = 0; i < celebrationState.ParticleSystemSpawns.Length; i++)
        {
            celebrationState.ParticleSystemInstances[i] = MonoBehaviour.Instantiate(
                celebrationState.CelebrationEffect,
                celebrationState.ParticleSystemSpawns[i].transform.position,
                celebrationState.ParticleSystemSpawns[i].transform.rotation
            );

            celebrationState.ParticleSystemInstances[i].Play();
        }

        _celebrationState = celebrationState;
    }

    public void FixedUpdate()
    {
        _celebrationState.transform.Rotate(_celebrationState.RotationAxis * _celebrationState.RotationSpeed);
    }
}

public class CelebrationState : TowerState
{
    public ParticleSystem CelebrationEffect;
    public Transform[] ParticleSystemSpawns;

    public Vector3 AimDirection = Vector3.up;
    public Vector3 RotationAxis = Vector3.up;
    public float RotationSpeed = 0.2f;

    private IStep _currentStep;

    /// <summary>
    /// Up rotation angle
    /// </summary>
    [HideInInspector]
    public ParticleSystem[] ParticleSystemInstances;

    /// <summary>
    /// Stop fireworks when state is disabled
    /// </summary>
    private void OnDisable()
    {
        foreach(var instance in ParticleSystemInstances)
        {
            Destroy(instance);
        }
    }

    private void OnEnable()
    {
        _currentStep = new RotateUp(this);

        ParticleSystemInstances = new ParticleSystem[ParticleSystemSpawns.Length];
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