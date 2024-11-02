using System;
using System.Collections.Generic;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Common;
using DungeonCrawler._Project.Scripts.Events.Inputs;
using DungeonCrawler.Skills;
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
            _tapEventBinding = new EventBinding<TapEvent>((_) => OnAbilityButtonPressed(0));
            EventBus<TapEvent>.Register(_tapEventBinding);
        }

        public void OnDisable()
        {
            EventBus<TapEvent>.Deregister(_tapEventBinding);
        }
        
        public void Update(float deltaTime){
            _timer.Tick(deltaTime);
            _view.UpdateRadial(_timer.Progress);

            if (!_timer.IsRunning && _skillQueue.TryDequeue(out SkillCommand command))
            {
                command.Execute();
                _timer.Reset(command.cooldown);
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
                _view.buttons[i].RegisterListener(OnAbilityButtonPressed);
            }
            _view.UpdateButtonSprites(_model._skills);
        }

        private void OnAbilityButtonPressed(int index)
        {
            if (_timer.Progress < 0.25f || !_timer.IsRunning){

               if(_model._skills[index] != null){
                _skillQueue.Enqueue(_model._skills[index].CreateCommand());
               } 
            }
            EventSystem.current.SetSelectedGameObject(null);
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