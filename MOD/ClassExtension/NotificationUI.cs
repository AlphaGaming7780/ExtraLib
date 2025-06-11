using Game.UI.Menu;
using static Game.UI.Menu.NotificationUISystem;

namespace ExtraLib.ClassExtension
{
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
    }

}