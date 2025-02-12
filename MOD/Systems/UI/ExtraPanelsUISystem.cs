using Colossal.UI.Binding;
using Game.Settings;
using Game.UI;
using Game.UI.InGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLib.Systems.UI
{
    internal partial class ExtraPanelsUISystem : UISystemBase
    {
        private GetterValueBinding<bool> m_GVB_ShowExtraPanelsButton;

        private bool m_ExtraPanelsMenuOpened = false;
        private GetterValueBinding<bool> m_GVB_ExtraPanelsMenuOpened;

        private List<ExtraPanelBase> m_Panels;
        private RawValueBinding m_PanelsBinding;


        protected override void OnCreate()
        {
            base.OnCreate();

            m_Panels = new List<ExtraPanelBase>();

            AddBinding(m_GVB_ShowExtraPanelsButton = new GetterValueBinding<bool>("el", "ShowExtraPanelsButton", () => m_Panels.Count > 0));

            AddBinding(m_GVB_ExtraPanelsMenuOpened = new GetterValueBinding<bool>("el", "ExtraPanelsMenuOpened", () => m_ExtraPanelsMenuOpened));
            AddBinding(new TriggerBinding<bool>("el", "ExtraPanelsMenuOpened", new Action<bool>((bool value) => { m_ExtraPanelsMenuOpened = value; m_GVB_ExtraPanelsMenuOpened.Update(); })));
            AddBinding(m_PanelsBinding = new RawValueBinding("el", "ExtraPanels", WritePanels ) );
        }


        protected override void OnUpdate()
        {
            base.OnUpdate();
            UpdatePanels();
            m_PanelsBinding.Update();
        }

        private void UpdatePanels()
        {
            for (int i = 0; i < m_Panels.Count; i++) 
            {
                m_Panels[i].PerformUpdate();
            }
        }

        public void AddExtraPanel(ExtraPanelBase panel)
        {
            this.m_Panels.Add(panel);

            // Update the show extra panel button binding when there is 0 and 1 panel in the list.
            if (m_Panels.Count <= 1) m_GVB_ShowExtraPanelsButton.Update();
        }

        private void WritePanels(IJsonWriter writer)
        {
            writer.ArrayBegin(m_Panels.Count);
            for (int i = 0; i < m_Panels.Count; i++)
            {
                writer.Write(m_Panels[i]);
            }
            writer.ArrayEnd();
        }

    }
}
