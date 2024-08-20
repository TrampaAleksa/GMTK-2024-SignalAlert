using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MotorHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private ParticleSystem smokeParticle;
    [SerializeField]
    private float minimum;
    [SerializeField]
    private float maximumSmoke;
    [SerializeField]
    private float maximumFire;
    public void StartMotor(float duration)
    {
        StartParticle(smokeParticle);
        StartFade(smokeParticle, maximumSmoke, duration);
    }
    public void LaunchMotor(float duration, Action onLaunchFinished=null)
    {
        StartParticle(fireParticle);
        StartFade(fireParticle, maximumFire, duration, onLaunchFinished);
    }
    public void StopMotor(float duration)
    {
        StartFade(smokeParticle, minimum, duration, () => StopParticle(smokeParticle));
        StartFade(fireParticle, minimum, duration, () => StopParticle(fireParticle));
    }
    private void StartFade(ParticleSystem particleSystem, float value, float duration, Action onDone=null) => StartCoroutine(FadeFire(particleSystem, value, duration, onDone));
    private IEnumerator FadeFire(ParticleSystem particleSystem, float value, float duration, Action onDone)
    {
        float min = particleSystem.startLifetime;
        float max = value;
        float time = 0f;
        while(time < duration)
        {
            time += Time.deltaTime;
            float percentage= time / duration;
            particleSystem.startLifetime = (max - min) * percentage + min;
            yield return null;
        }
        onDone?.Invoke();
    }
    public void StartParticle(ParticleSystem particleSystem) => particleSystem.Play();
    public void StopParticle(ParticleSystem particleSystem) => particleSystem.Stop();
    public static void StartMotors(List<MotorHandler> motors, float duration)=>motors.ForEach(m => m.StartMotor(duration));
    public static void StopMotors(List<MotorHandler> motors, float duration) => motors.ForEach(m => m.StopMotor(duration));
    public static void StartLaunch(List<MotorHandler> motors, float duration) => motors.ForEach(m => m.LaunchMotor(duration));
}
