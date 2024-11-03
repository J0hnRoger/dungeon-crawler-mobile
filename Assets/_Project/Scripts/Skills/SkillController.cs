using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Events.Inputs;
using DungeonCrawler._Project.Scripts.Skills.Components;
using DungeonCrawler.Skills;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonCrawler._Project.Scripts.Skills
{
    public class SkillController
    {
        private readonly SkillModel _model;

        private readonly SkillView _view;

        // Services
        private readonly Queue<SkillCommand> _skillQueue = new();
        private readonly CountdownTimer _timer = new CountdownTimer(0);

        // Events
        private EventBinding<TapEvent> _tapEventBinding;

        public SkillController(SkillModel model, SkillView view)
        {
            _model = model;
            _view = view;

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

            if (!_timer.IsRunning && _skillQueue.TryDequeue(out SkillCommand command))
            {
                command.Execute();
                _timer.Reset(command.Cooldown);
                _timer.Start();
            }
        }

        private void ConnectModel()
        {
            _model._skills.AnyValueChanged += UpdateButtons;
        }

        private void UpdateButtons(IList<Skill> skills) => _view.UpdateButtonSprites(skills);

        private void ConnectView()
        {
            for (int i = 0; i < _view.buttons.Length; i++)
            {
                // _view.buttons[i].RegisterListener(OnAbilityButtonPressed);
            }

            _view.UpdateButtonSprites(_model._skills);
        }

        private void OnScreenClicked(TapEvent tapEvent)
        {
            if (_timer.Progress < 0.25f || !_timer.IsRunning)
            {
                var hitPoint = GetEnemyHitZone(tapEvent.ScreenPosition);
                var currentSkill = _model._skills.First()
                    .CreateCommand(_timer.Progress, hitPoint);

                _skillQueue.Enqueue(currentSkill);
            }

            EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary>
        /// Retourne la position de l'impact
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private HitInfo GetEnemyHitZone(Vector2 screenPosition)
        {
            if (Camera.main == null)
                throw new Exception("[Skill System] Pas de camera sur la scene - impossible de calculer le hitpoint");

            // Raycast depuis la caméra
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            // Ignorer le raycast sur l'UI
            int layerMask = ~LayerMask.GetMask("UI");
            RaycastHit hit; 
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1f); // Visualiser le ray dans la scène
            Debug.Log($"Ray Origin: {ray.origin}, Direction: {ray.direction}");           
            if (Physics.Raycast(ray, out hit, 1000, layerMask)){
                // Vérifier si on a touché une zone de hit
                var hitZoneComponent = hit.collider.GetComponent<HitZone>();
                return new HitInfo()
                {
                    HitPosition = hit.point,
                    HitZone = hitZoneComponent.BodyPart,
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

            public SkillController Build(SkillView view)
            {
                if (view == null)
                    throw new Exception("[SkillSystem] SkillView is not defined");
                return new SkillController(_model, view);
            }
        }
    }
}