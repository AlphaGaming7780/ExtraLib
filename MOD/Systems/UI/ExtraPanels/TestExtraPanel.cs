﻿#if DEBUG
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLib.Systems.UI.ExtraPanels
{
    internal partial class TestExtraPanel : ExtraPanelBase
    {
        public override GameMode gameMode => GameMode.Game | GameMode.Editor;

        protected override bool m_CanFullScreen => true;

        protected override void OnCreate()
        {
            base.OnCreate();
            EL.Logger.Info("TestExtraPanel OnCreate");
        }

        protected override void OnProcess()
        {
            
        }
    }
}
#endif