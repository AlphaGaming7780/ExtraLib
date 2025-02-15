using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game;
using Game.SceneFlow;
using Game.Settings;
using Game.UI;
using Game.UI.InGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace ExtraLib.Systems.UI.ExtraPanels
{
    internal partial class ExtraPanelsUISystem : UISystemBase
    {
        // Compatible gameMode;
        public override GameMode gameMode => GameMode.Game | GameMode.Editor;

        private GetterValueBinding<bool> m_ShowExtraPanelsButtonBinding;

        private bool m_ExtraPanelsMenuOpened = false;
        private GetterValueBinding<bool> m_ExtraPanelsMenuOpenedBinding;
        private GetterValueBinding<bool> m_ShouldOpenExtraPanelsBinding;

        private List<ExtraPanelBase> m_Panels;
        private List<ExtraPanelBase> m_ValidPanels;
        private bool m_DirtyPanelBinding = false;
        private RawValueBinding m_PanelsBinding;

        public void AddExtraPanel<T>() where T : ExtraPanelBase
        {
            AddExtraPanel(World.GetOrCreateSystemManaged<T>());
        }

        public void AddExtraPanel(ExtraPanelBase panel)
        {
            this.m_Panels.Add(panel);
            if (IsValidPanel(panel))
            {
                m_ValidPanels.Add(panel);
            }
            RequestBindingUpdate();
        }

        public void RequestBindingUpdate()
        {
            m_DirtyPanelBinding = true;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_Panels = new List<ExtraPanelBase>();
            m_ValidPanels = new List<ExtraPanelBase>();

            AddBinding(m_ShowExtraPanelsButtonBinding = new GetterValueBinding<bool>("el", "ShowExtraPanelsButton", ShouldShowExtraPanelsButton));

            AddBinding(m_ExtraPanelsMenuOpenedBinding = new GetterValueBinding<bool>("el", "ExtraPanelsMenuOpened", () => m_ExtraPanelsMenuOpened ));
            AddBinding(m_ShouldOpenExtraPanelsBinding = new GetterValueBinding<bool>("el", "ShouldOpenExtraPanels", ShouldOpenExtraPanels));
            AddBinding(new TriggerBinding<bool>("el", "ExtraPanelsMenuOpened", new Action<bool>((bool value) => { m_ExtraPanelsMenuOpened = value; m_ExtraPanelsMenuOpenedBinding.Update(); })));
            AddBinding(m_PanelsBinding = new RawValueBinding("el", "ExtraPanels", WritePanels ) );
            AddBinding(new TriggerBinding<string>("el", "OpenExtraPanel", OpenExtraPanel));
            AddBinding(new TriggerBinding<string>("el", "CloseExtraPanel", CloseExtraPanel));

            AddBinding(new TriggerBinding<string>("el", "CollapseExtraPanel", CollapseExtraPanel));
            AddBinding(new TriggerBinding<string>("el", "ExpandExtraPanel", ExpandExtraPanel));

            AddBinding(new TriggerBinding<string, float2>("el", "LocationChanged", UpdatePanelLocation));
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            // Updating the list of valid panels for the new game mode.
            UpdateValidPanelsList();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            UpdatePanels();
            if (m_DirtyPanelBinding)
            {
                m_DirtyPanelBinding = false;
                m_PanelsBinding.Update();
                m_ShouldOpenExtraPanelsBinding.Update();
                m_ShowExtraPanelsButtonBinding.Update();
            }
        }

        private void UpdatePanels()
        {
            for (int i = 0; i < m_ValidPanels.Count; i++) 
            {
                m_ValidPanels[i].PerformUpdate();
            }
        }

        private void UpdateValidPanelsList()
        {
            m_ValidPanels.Clear();
            for (int i = 0; i < m_Panels.Count; i++)
            { 
                ExtraPanelBase extraPanelBase = m_Panels[i];
                if(IsValidPanel(extraPanelBase))
                {
                    m_ValidPanels.Add( extraPanelBase );
                }
            }
            RequestBindingUpdate();
        }

        private bool IsValidPanel(ExtraPanelBase extraPanelBase)
        {
            return  (extraPanelBase.gameMode & gameMode) > 0 &&
                    (extraPanelBase.gameMode & GameManager.instance.gameMode) > 0;
        }

        private bool ShouldShowExtraPanelsButton()
        {
            for (int i = 0; i < m_ValidPanels.Count; i++)
            {
                if (m_ValidPanels[i].ShowInSelector()) return true;
            }
            return false;
        }

        private bool ShouldOpenExtraPanels()
        {
            for (int i = 0; i < m_ValidPanels.Count; i++)
            {
                ExtraPanelBase extraPanelBase = m_ValidPanels[i];
                if (extraPanelBase.ShowInSelector() && extraPanelBase.Visible()) return true;
            }
            return false;
        }

        private void WritePanels(IJsonWriter writer)
        {
            // To Do check if this setup using the m_ValidPanels isn't going to create useValue error on the UI side because nbr of hook sin't the same.
            // Edit, normally it shouldn't because their UI are called before rendering.
            writer.ArrayBegin(m_ValidPanels.Count);
            for (int i = 0; i < m_ValidPanels.Count; i++)
            {
                writer.Write(m_ValidPanels[i]);
            }
            writer.ArrayEnd();
        }

        public bool TryToFindPanelByID(string id, out ExtraPanelBase extraPanelBase)
        {
            extraPanelBase = FindPanelByID(id);
            return extraPanelBase != default;
        }

        public ExtraPanelBase FindPanelByID(string id)
        {
            return m_ValidPanels.Find((ExtraPanelBase) => { return ExtraPanelBase.ID == id; });
        }

        private void OpenExtraPanel(string id)
        {

            if(!TryToFindPanelByID(id, out ExtraPanelBase extraPanelBase))
            {
                EL.Logger.Warn($"Try to open an Extra Panel with id : {id}, but this id doesn't exist in the valide panels.");
                return;
            }

            extraPanelBase.SetVisible(true);
            
        }

        private void CloseExtraPanel(string id)
        {

            if (!TryToFindPanelByID(id, out ExtraPanelBase extraPanelBase))
            {
                EL.Logger.Warn($"Try to clsoe an Extra Panel with id : {id}, but this id doesn't exist in the valide panels.");
                return;
            }

            extraPanelBase.SetVisible(false);
        }

        private void ExpandExtraPanel(string id)
        {
            if (!TryToFindPanelByID(id, out ExtraPanelBase extraPanelBase))
            {
                EL.Logger.Warn($"Try to clsoe an Extra Panel with id : {id}, but this id doesn't exist in the valide panels.");
                return;
            }

            extraPanelBase.SetExpanded(true);
        }

        private void CollapseExtraPanel(string id)
        {
            if (!TryToFindPanelByID(id, out ExtraPanelBase extraPanelBase))
            {
                EL.Logger.Warn($"Try to clsoe an Extra Panel with id : {id}, but this id doesn't exist in the valide panels.");
                return;
            }

            extraPanelBase.SetExpanded(false);
        }

        private void UpdatePanelLocation(string id, float2 newLocation)
        {

            if (!TryToFindPanelByID(id, out ExtraPanelBase extraPanelBase))
            {
                EL.Logger.Warn($"Try to update the location of an Extra Panel with id : {id}, but this id doesn't exist in the valide panels.");
                return;
            }

            extraPanelBase.SetPanelLocation(newLocation);

            EL.Logger.Info(newLocation);

        }

    }
}
