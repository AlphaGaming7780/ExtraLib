using Colossal.UI.Binding;
using Game;
using Game.Objects;
using Game.SceneFlow;
using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace ExtraLib.Systems.UI.ExtraPanels
{
    internal abstract partial class ExtraPanelBase : UISystemBase, IJsonWritable
    {
        public string ID => GetType().FullName;
        public override GameMode gameMode => GameMode.Game;
        public virtual string Icon => "Media/Placeholder.svg";


        protected ExtraPanelsUISystem m_ExtraPanelsUISystem;

        protected virtual bool m_ShowInSelector => true;

        private bool m_Visible = false;

        protected bool m_Dirty;

        public float2 PanelLocation { get; private set; }
        public float2 PanelSize { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();

            PanelLocation = new float2(0.01f, 0.07f);
            m_ExtraPanelsUISystem = World.GetOrCreateSystemManaged<ExtraPanelsUISystem>();
        }

        public void PerformUpdate()
        {
            Update();
            if (Visible())
            {
                Reset();
                OnPreProcess();
                if (m_Dirty)
                {
                    m_Dirty = false;
                    OnProcess();
                }
            }
        }

        protected virtual void OnPreProcess() { }

        protected virtual void Reset() { }

        protected abstract void OnProcess();

        public void Write(IJsonWriter writer)
        {
            writer.TypeBegin(ID);
            //writer.PropertyName("id");
            //writer.Write(ID);
            writer.PropertyName("icon");
            writer.Write( Icon );
            writer.PropertyName("visible");
            writer.Write(Visible());
            writer.PropertyName("showInSelector");
            writer.Write(ShowInSelector());
            writer.PropertyName("panelLocation");
            writer.Write(PanelLocation);
            writer.PropertyName("panelSize");
            writer.Write(PanelSize);
            writer.TypeEnd();
            return;
        }

        public void RequestUpdate()
        {
            m_Dirty = true;
        }

        public void SetVisible( bool visible )
        {
            m_Visible = visible;
            RequestUpdate();
            m_ExtraPanelsUISystem.RequestBindingUpdate();
        }

        public bool Visible()
        {
            return m_Visible;

            // Not needed anymore, since I'm filtering the panel inside the main system.
            //return m_Visible && ((GameManager.instance.gameMode & gameMode) > 0);
        }

        public bool ShowInSelector()
        {
            return m_ShowInSelector;
            // Not needed anymore, since I'm filtering the panel inside the main system.
            //return m_ShowInSelector && ((GameManager.instance.gameMode & gameMode) > 0);
        }

        public void SetPanelLocation(float2 panelLocation)
        {
            PanelLocation = panelLocation;
            m_ExtraPanelsUISystem.RequestBindingUpdate();
        }
    }
}
