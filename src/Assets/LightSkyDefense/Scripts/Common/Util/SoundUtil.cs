using UnityEngine;

public static class SoundUtil 
{
    public static void PlayClipAtPointWithRandomPitch(AudioClip clip, Vector3 pos, float minPitch, float maxPitch)
    {
        if (clip == null)
            return;

        var pitch = Random.value * (minPitch - maxPitch) + maxPitch;

        var tempGO = new GameObject("TempAudio");
        tempGO.transform.position = pos;
        var aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.pitch = pitch;
        aSource.Play(); // start the sound
        Object.Destroy(tempGO, clip.length); // destroy object after clip duration
    }

    public static void PlayOneShotWithRandomPitch(this AudioSource source, AudioClip clip, float minPitch, float maxPitch)
    {
        if (source == null || clip == null)
            return;

        var pitch = Random.value * (minPitch - maxPitch) + maxPitch;
        source.pitch = pitch;
        source.PlayOneShot(clip);
    }

    public static void PlayWithRandomPitch(this AudioSource source, float minPitch, float maxPitch)
    {
        if (source == null)
            return;

        var pitch = Random.value * (minPitch - maxPitch) + maxPitch;
        source.pitch = pitch;
        source.Play();
    }
}
