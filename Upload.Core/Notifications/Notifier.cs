﻿using Azure.Core.Interfaces;

namespace Azure.Core.Notifications
{
    public class Notifier : INotifier
    {
        private List<Notification> _notifications;

        public Notifier() => _notifications = new List<Notification>();

        public void Handle(Notification notification) => _notifications.Add(notification);

        public List<Notification> GetNotifications() => _notifications;

        public bool HasNotification() => _notifications.Any();
    }
}