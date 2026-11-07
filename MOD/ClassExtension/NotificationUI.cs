using Colossal.PSI.Common;
using ExtraLib.Helpers;
using Game.UI.Localization;
using Game.UI.Menu;
using System;
using System.Threading.Tasks;
using static Game.UI.Menu.NotificationUISystem;

namespace ExtraLib.ClassExtension
{
    /// <summary>
    /// NotificationUISystem stores its notifications in a plain Dictionary/List read by the main thread every
    /// frame: the non "OnMainThread" methods below must only be called from the main thread. From a background
    /// thread/Task, use the "OnMainThread" variants.
    /// </summary>
    public static class NotificationUIExtension
    {
        public static void AddOrUpdateNotification(this NotificationUISystem notificationUISystem, ref NotificationInfo notificationInfo)
        {
            notificationInfo = notificationUISystem.AddOrUpdateNotification(
                notificationInfo.id,
                notificationInfo.title,
                notificationInfo.text,
                notificationInfo.thumbnail,
                notificationInfo.progressState,
                notificationInfo.progress,
                notificationInfo.onClicked
            );
        }

        public static void RemoveNotification(this NotificationUISystem notificationUISystem, NotificationInfo notificationInfo, float delay = 0)
        {
            notificationUISystem.RemoveNotification(
                notificationInfo.id,
                delay,
                notificationInfo.title,
                notificationInfo.text,
                notificationInfo.thumbnail,
                notificationInfo.progressState,
                notificationInfo.progress,
                notificationInfo.onClicked
            );
        }

        public static void Update(this NotificationInfo notificationInfo)
        {
            EL.m_NotificationUISystem.AddOrUpdateNotification(ref notificationInfo);
        }

        /// <summary>
        /// Thread-safe NotificationUISystem.AddOrUpdateNotification. Runs inline when already on the main thread.
        /// </summary>
        public static Task<NotificationInfo> AddOrUpdateNotificationOnMainThread(this NotificationUISystem notificationUISystem, string identifier, LocalizedString? title = null, LocalizedString? text = null, string thumbnail = null, ProgressState? progressState = null, int? progress = null, Action onClicked = null)
        {
            return MainThreadHelper.RunOnMainThread(() => notificationUISystem.AddOrUpdateNotification(identifier, title, text, thumbnail, progressState, progress, onClicked));
        }

        /// <summary>
        /// Thread-safe NotificationUISystem.RemoveNotification. Runs inline when already on the main thread.
        /// </summary>
        public static Task RemoveNotificationOnMainThread(this NotificationUISystem notificationUISystem, string identifier, float delay = 0f, LocalizedString? title = null, LocalizedString? text = null, string thumbnail = null, ProgressState? progressState = null, int? progress = null, Action onClicked = null)
        {
            return MainThreadHelper.RunOnMainThread(() => notificationUISystem.RemoveNotification(identifier, delay, title, text, thumbnail, progressState, progress, onClicked));
        }

        /// <summary>
        /// Thread-safe <see cref="Update(NotificationInfo)"/>. The fields are read when this is called, not when
        /// it runs. Dispatched calls run in no guaranteed order: wait on the Task (or guard with a flag checked on
        /// the main thread) before removing the notification, or a late update could show it again.
        /// </summary>
        public static Task UpdateOnMainThread(this NotificationInfo notificationInfo)
        {
            return EL.m_NotificationUISystem.AddOrUpdateNotificationOnMainThread(
                notificationInfo.id,
                notificationInfo.title,
                notificationInfo.text,
                notificationInfo.thumbnail,
                notificationInfo.progressState,
                notificationInfo.progress,
                notificationInfo.onClicked
            );
        }

        /// <summary>
        /// Thread-safe <see cref="RemoveNotification(NotificationUISystem, NotificationInfo, float)"/>. The fields
        /// are read when this is called, not when it runs.
        /// </summary>
        public static Task RemoveOnMainThread(this NotificationInfo notificationInfo, float delay = 0f)
        {
            return EL.m_NotificationUISystem.RemoveNotificationOnMainThread(
                notificationInfo.id,
                delay,
                notificationInfo.title,
                notificationInfo.text,
                notificationInfo.thumbnail,
                notificationInfo.progressState,
                notificationInfo.progress,
                notificationInfo.onClicked
            );
        }
    }

}
