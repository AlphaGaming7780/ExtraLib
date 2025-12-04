using Colossal.UI.Binding;
using Game;
using Game.UI;
using Unity.Mathematics;

namespace ExtraLib.Systems.UI.ExtraPanels
{
    public abstract partial class ExtraPanelBase : UISystemBase, IJsonWritable
    {
        public string ID => GetType().FullName;
        public override GameMode gameMode => GameMode.Game;
        public virtual string Icon => "Media/Placeholder.svg";


        protected ExtraPanelsUISystem m_ExtraPanelsUISystem;

        protected virtual bool m_ShowInSelector => true;
        protected virtual bool m_CanFullScreen => false;

        private bool m_Visible = false;

        protected bool m_Dirty;

        public float2 PanelLocation { get; private set; }
        public float2 PanelSize { get; private set; }
        public bool IsExpanded { get; private set; } = true;
        public bool IsFullScreen { get; private set; } = false;

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
            writer.PropertyName("icon");
            writer.Write( Icon );
            writer.PropertyName("visible");
            writer.Write(m_Visible);
            writer.PropertyName("isExpanded");
            writer.Write(IsExpanded);
            writer.PropertyName("canFullScreen");
            writer.Write(m_CanFullScreen);
            writer.PropertyName("isFullScreen");
            writer.Write(IsFullScreen);
            writer.PropertyName("showInSelector");
            writer.Write(m_ShowInSelector);
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
            if (visible) RequestUpdate();
            m_ExtraPanelsUISystem.RequestBindingUpdate();
        }

        public bool Visible()
        {
            return m_Visible && IsExpanded;

            // Not needed anymore, since I'm filtering the panel inside the main system.
            //return m_Visible && ((GameManager.instance.gameMode & gameMode) > 0);
        }

        public void SetExpanded(bool expanded)
        {
            IsExpanded = expanded;
            if (expanded) RequestUpdate();
            m_ExtraPanelsUISystem.RequestBindingUpdate();
        }
        public void SetFullScreen(bool fullScreen)
        {
            if (!m_CanFullScreen) return;
            IsFullScreen = fullScreen;
            m_ExtraPanelsUISystem.RequestBindingUpdate();
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
            //m_ExtraPanelsUISystem.RequestBindingUpdate();
        }
    }
}
