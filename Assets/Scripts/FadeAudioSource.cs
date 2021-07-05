using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public static class FadeAudioSource
{

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        if(duration < 0.001f)
        {
            duration = 0.001f;
        }
        currentVol = audioSource.volume;
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioSource.volume = Mathf.Log10(newVol) * 20;
            yield return null;
        }
        yield break;
    }
}