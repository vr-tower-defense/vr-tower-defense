using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCelebrationBehaviour : MonoBehaviour
{
    private ParticleSystem _fireWorkEffect;
    private Quaternion _aimAngle;
    private bool _effectIsPlaying = false;

    void Start()
    {
        _fireWorkEffect = Resources.Load<ParticleSystem>("Effects/FireworkParticleSystem");

        // Set aim angle upwards
        _aimAngle = Quaternion.LookRotation(Vector3.up);
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _aimAngle, 2);

        if (!_effectIsPlaying && (Quaternion.Angle(transform.rotation, _aimAngle) <= 0.01f))
        {
            _effectIsPlaying = true;
            FireCelebration();
        }
    }

    private void FireCelebration()
    {
        var fireWorkInstance = Instantiate(_fireWorkEffect, transform.position, transform.rotation);
        fireWorkInstance.Play();
    }
}
