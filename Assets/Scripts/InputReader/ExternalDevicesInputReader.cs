using System;
using Core.Services.Updater;
using UnityEngine;
using UnityEngine.EventSystems;
using InputReader;

    public class ExternalDevicesInputReader : IEntityInputSource, IDisposable
    {
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        
        public float VerticalDirection => Input.GetAxisRaw("Vertical");
        
        public  bool Jump { get; private set; }
        public  bool Attack { get; private set; }

        public ExternalDevicesInputReader()
        {
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public void ResetOneTimeActions()
        {
            Jump = false;
            Attack = false;
        }

        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;
        
        private void OnUpdate()
        {
            if (Input.GetButtonDown("Jump"))
                Jump = true;
    
            if (!IsPointerOverUi() && Input.GetButtonDown("Fire1"))
                Attack = true;
        }

        private bool IsPointerOverUi() => EventSystem.current.IsPointerOverGameObject();
        
    }



