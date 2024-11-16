using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;
using Unity.Cinemachine;

namespace DungeonCrawler._Project.Scripts.Combat
{
    public class ShakeScreenEffect : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _virtualCamera;

        [Header("Shake Parameters")]
        [SerializeField] private float _shakeDuration = .3f;
        [SerializeField] private float _shakeAmplitude = 6f;
        [SerializeField] private float _shakeFrequency = 2.0f;
        
        private CinemachineImpulseSource _impulseSource;
        private float _shakeTimer;
        
        private void Start()
        {
            if (_virtualCamera == null)
            {
                Debug.LogError($"[{nameof(ShakeScreenEffect)}] Missing reference to CinemachineCamera");
                return;
            }

            _impulseSource = _virtualCamera.GetComponent<CinemachineImpulseSource>();
            if (_impulseSource == null)
            {
                Debug.LogError($"[{nameof(ShakeScreenEffect)}] Missing reference to CinemachineImpulseSource");
                return;
            }
            
            _impulseSource.ImpulseDefinition.ImpulseDuration = _shakeDuration;
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void OnCleanHit(DirectHitEvent evt)
        {
            ShakeCamera();
        }

        private void ShakeCamera()
        {
            if (_impulseSource != null)
            {
                Debug.LogWarning("Impulse generated");
                _impulseSource.GenerateImpulseWithForce(_shakeAmplitude);
            }
        }
    }
} 