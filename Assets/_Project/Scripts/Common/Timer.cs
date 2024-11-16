using System;
using UnityEngine;
using System.Collections.Generic;

namespace DungeonCrawler._Project.Scripts.Common
{
    public abstract class Timer
    {
        protected float initialTime;
        public float Time { get; set; }
        public bool IsRunning { get; protected set; }

        // Est à X% du début ou de la fin du timer
        public float Progress => Time / initialTime;

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            initialTime = value;
            IsRunning = false;
        }

        public virtual void Start()
        {
            Time = initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimerStart.Invoke();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop.Invoke();
            }
        }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        public abstract void Tick(float deltaTime);
    }

    public class CountdownTimer : Timer
    {
        private const float EPSILON = 0.01f;
        private HashSet<float> _triggeredTimePoints = new();
        
        public CountdownTimer(float value) : base(value) { }
    
        public bool IsTimeRemaining(float seconds)
        {
            if (!IsRunning || Time <= 0 || _triggeredTimePoints.Contains(seconds))
                return false;
            
            if (Mathf.Abs(Time - seconds) < EPSILON)
            {
                _triggeredTimePoints.Add(seconds);
                return true;
            }
            return false;
        }

        public override void Start()
        {
            base.Start();
            _triggeredTimePoints.Clear(); // Reset l'état quand on redémarre le timer
        }

            
        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0)
            {
                Time -= deltaTime;
            }

            if (IsRunning && Time <= 0)
            {
                Stop();
            }
        }

        public bool IsFinished => Time <= 0;

        public void Reset(float newTime)
        {
            initialTime = newTime;
            _triggeredTimePoints.Clear();
        }
    }

    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning)
            {
                Time += deltaTime;
            }
        }

        public void Reset() => Time = 0;

        public float GetTime() => Time;
    }
}