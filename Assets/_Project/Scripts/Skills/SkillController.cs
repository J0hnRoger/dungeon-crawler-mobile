using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Events.Inputs;
using DungeonCrawler._Project.Scripts.Skills.Components;
using DungeonCrawler.Skills;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Skills
{
    public class SkillController
    {
        private readonly SkillModel _model;
        private readonly Camera _raycastCamera;
        private readonly SkillView _view;

        // Services
        private readonly Queue<SkillCommand> _skillQueue = new();
        private readonly CountdownTimer _timer = new CountdownTimer(0);

        // Events
        private EventBinding<TapEvent> _tapEventBinding;
        private EventBinding<DirectHitEvent> _directHitEventBinding;

        public SkillController(SkillModel model, SkillView view, Camera raycastCamera)
        {
            _model = model;
            _view = view;

            // For raycast input
            _raycastCamera = raycastCamera;

            ConnectModel();
            ConnectView();
        }

        public void OnEnable()
        {
            // TODO rendre dynamique  
            _tapEventBinding = new EventBinding<TapEvent>(OnScreenClicked);
            EventBus<TapEvent>.Register(_tapEventBinding);
        }

        public void OnDisable()
        {
            EventBus<TapEvent>.Deregister(_tapEventBinding);
        }

        public void Update(float deltaTime)
        {
            _timer.Tick(deltaTime);
            _view.UpdateRadial(_timer.Progress);
            
            if (_timer.IsTimeRemaining(1f))
               _view.StartCooldownAnimation(); 
            
            if (!_timer.IsRunning && _skillQueue.TryDequeue(out SkillCommand command))
            {
                command.Execute(_timer.Progress);
                _timer.Reset(command.Cooldown);
                _timer.Start();
            }
        }

        private void ConnectModel()
        {
        }

        private void ConnectView()
        {
            _directHitEventBinding = new EventBinding<DirectHitEvent>(OnDirectHit);
            EventBus<DirectHitEvent>.Register(_directHitEventBinding);
        }

        private void OnDirectHit(DirectHitEvent directHitEvent)
        {
            _view.LaunchDirectHitAnimation();
        }

        private void OnScreenClicked(TapEvent tapEvent)
        {
            // Buffer pour éviter de lancer le skill si on est pas encore au début du cooldown
            // défini à 25% de la fin du cooldown
            if ((_timer.Progress < 0.25f && _timer.Progress > 0) || !_timer.IsRunning)
            {
                var hitPoint = GetEnemyHitZone(tapEvent.ScreenPosition);
                // Défini à combien de pourcent de la fin parfaite du timer le joueur a cliqué 
                float timingPressed = (!_timer.IsRunning) ? float.MaxValue : _timer.Progress; 
                var currentSkill = _model._skills.First()
                    .CreateCommand(hitPoint, timingPressed);

                _skillQueue.Enqueue(currentSkill);
            }
        }

        /// <summary>
        /// Retourne la position de l'impact
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private HitInfo GetEnemyHitZone(Vector2 screenPosition)
        {
            if (_raycastCamera == null)
                throw new Exception("[Skill System] Pas de camera sur la scene - impossible de calculer le hitpoint");

            // Raycast depuis la caméra
            Ray ray = _raycastCamera.ScreenPointToRay(screenPosition);
            // Ignorer le raycast sur l'UI
            int layerMask = ~LayerMask.GetMask("UI");
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1f); // Visualiser le ray dans la scène

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                // Vérifier si on a touché une zone de hit
                var hitZoneComponent = hit.collider.GetComponent<HitZone>();
                return new HitInfo()
                {
                    HitPosition = hit.point,
                    HitZone = hitZoneComponent?.BodyPart ?? "nothing",
                    DamageCoefficient = hitZoneComponent.DamageCoefficient
                };
            }

            // Si on ne touche rien, on retourne le point d'impact à cette profondeur
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 100));
            return new HitInfo() { HitPosition = worldPosition, HitZone = "none", DamageCoefficient = 1 };
        }

        public class Builder
        {
            private readonly SkillModel _model = new();

            public Builder WithSkills(SkillData[] datas)
            {
                foreach (SkillData skillData in datas)
                    _model.Add(new Skill(skillData));
                return this;
            }

            public SkillController Build(SkillView view, Camera raycastCamera)
            {
                if (view == null)
                    throw new Exception("[SkillSystem] SkillView is not defined");
                return new SkillController(_model, view, raycastCamera);
            }
        }
    }
}