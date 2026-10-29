using Colossal.UI.Binding;
using Game;
using Game.UI.Editor;
using Game.UI.Widgets;
using System;
using Unity.Mathematics;

namespace ExtraLib.Systems.UI.ExtraPanels
{
    public sealed class FileBrowserRequest
    {
        // Localization key or raw text, shown in the panel header.
        public string Title;
        public string StartDirectory;
        // Optional, the user can't go above this folder.
        public string Root;
        public bool PickFiles = true;
        // Extensions with the dot, ex: ".eut". Null shows every file.
        public string[] Extensions;
        public Action<string> OnSelect;
        public Action OnCancel;
    }

    // Shared file/folder picker, wraps the game DirectoryBrowserPanel inside an Extra Panel.
    // Usage : World.GetOrCreateSystemManaged<FileBrowserPanel>().Open(new FileBrowserRequest { ... });
    public partial class FileBrowserPanel : ExtraPanelBase
    {
        public const string kGroup = "elFileBrowser";

        public override GameMode gameMode => GameMode.Game | GameMode.Editor | GameMode.MainMenu;
        public override float2 PanelMinSize => new float2(400, 450);
        protected override bool m_ShowInSelector => false;

        public override string Title => m_Request?.Title ?? (m_Request?.PickFiles == false ? "Editor.SELECT_DIRECTORY" : "Editor.SELECT_FILE");

        private WidgetBindings m_WidgetBindings;
        private DirectoryBrowserPanel m_Browser;
        private FileBrowserRequest m_Request;

        public bool IsOpen => m_Request != null;

        protected override void OnCreate()
        {
            base.OnCreate();
            AddUpdateBinding(m_WidgetBindings = new WidgetBindings(kGroup));
            m_WidgetBindings.AddDefaultBindings();
            m_WidgetBindings.AddBindings<IItemPicker.Bindings>();
        }

        // Only one request at a time, opening a new one cancels the previous one.
        public void Open(FileBrowserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (m_Request != null) Finish(null);

            m_Request = request;
            m_Browser = new DirectoryBrowserPanel(request.StartDirectory, request.Root, watchChanges: false, request.PickFiles, OnBrowserSelect, OnBrowserCancel, request.Extensions);
            m_ExtraPanelsUISystem.RequestBindingUpdate();
            SetVisible(true);
        }

        public void Cancel()
        {
            if (m_Request != null) Finish(null);
        }

        protected override void OnUpdate()
        {
            m_WidgetBindings.children = m_Browser != null ? ((IEditorPanel)m_Browser).children : Array.Empty<IWidget>();
            base.OnUpdate();
        }

        protected override void OnProcess() { }

        public override void OnClose()
        {
            // Closing the panel with its close button is a cancel.
            Cancel();
            base.OnClose();
        }

        private void OnBrowserSelect(string path)
        {
            Finish(path);
        }

        private void OnBrowserCancel()
        {
            Finish(null);
        }

        // path == null means cancel.
        private void Finish(string path)
        {
            FileBrowserRequest request = m_Request;
            m_Request = null;
            m_Browser?.StopRunning();
            m_Browser = null;
            SetVisible(false);
            m_ExtraPanelsUISystem.RequestBindingUpdate();

            if (request == null) return;
            try
            {
                if (path != null) request.OnSelect?.Invoke(path);
                else request.OnCancel?.Invoke();
            }
            catch (Exception e)
            {
                EL.Logger.Error($"FileBrowserPanel : the request callback threw an exception : {e}");
            }
        }
    }
}
