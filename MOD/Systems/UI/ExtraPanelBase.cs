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

namespace ExtraLib.Systems.UI
{
    internal abstract partial class ExtraPanelBase : UISystemBase, IJsonWritable
    {
        protected bool m_Dirty;

        public override GameMode gameMode => GameMode.Game;

        public virtual string Icon => "Media/Placeholder.svg";

        public bool visible { get; protected set; }

        //public abstract void OnWriteProperties(IJsonWriter writer);

        public void PerformUpdate()
        {
            OnPreUpdate();
            if (m_Dirty)
            {
                m_Dirty = false;
                Reset();
                Update();
                if (Visible())
                {
                    OnProcess();
                }
            }
        }

        protected virtual void OnPreUpdate() { }

        protected virtual void Reset() { }

        protected abstract void OnProcess();

        public void Write(IJsonWriter writer)
        {
            writer.TypeBegin("ExtraPanel");
            writer.PropertyName("id");
            writer.Write(base.GetType().FullName);
            writer.PropertyName("icon");
            writer.Write( Icon );
            writer.PropertyName("visible");
            writer.Write(Visible());
            writer.TypeEnd();
            return;
        }

        private bool Visible()
        {
            return this.visible && ((GameManager.instance.gameMode & gameMode) > 0);
        }
    }
}
