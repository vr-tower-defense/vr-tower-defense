using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCelebrationBehaviour : MonoBehaviour
{
    // Default aim upwards
    private Quaternion _aimAngle = Quaternion.LookRotation(Vector3.up);
    private bool _rotationFinished = false;

    void FixedUpdate()
    {
        if (_rotationFinished)
        {
            transform.Rotate(new Vector3(0, 0, 0.2f));
            return;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, _aimAngle, 2);

        if(Quaternion.Angle(transform.rotation, _aimAngle) <= 0.01f)
        {
            _rotationFinished = true;
            PlayCelebration();
        }
    }

    private void PlayCelebration()
    {
        var celebrationParticle = Resources.Load<ParticleSystem>("Effects/FireworkParticleSystem");

        var celebrationInstance = Instantiate(celebrationParticle, transform.position, transform.rotation);
        celebrationInstance.Play();
    }
}
