using UnityEngine;

public class CelebrationState : TowerState
{
    /// <summary>
    /// Up rotation angle
    /// </summary>
    private Quaternion _aimAngle = Quaternion.LookRotation(Vector3.up);

    private bool _rotationFinished = false;

    /// <summary>
    /// Rotates the tower towards _aimAngle, when rotation has finished 
    /// start celebration and keep rotating slowly on the Y axis
    /// </summary>
    void FixedUpdate()
    {
        if (_rotationFinished)
        {
            transform.Rotate(new Vector3(0, 0, 0.2f));
            return;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, _aimAngle, 2);

        if (Quaternion.Angle(transform.rotation, _aimAngle) <= 0.01f)
        {
            _rotationFinished = true;
            PlayCelebration();
        }
    }

    /// <summary>
    /// Create a new firework particle system instance and start the celebration
    /// </summary>
    private void PlayCelebration()
    {
        var celebrationParticle = Resources.Load<ParticleSystem>("Effects/FireworkParticleSystem");

        var celebrationInstance = Instantiate(
            celebrationParticle,
            transform.position,
            transform.rotation
        );

        celebrationInstance.Play();
    }
}