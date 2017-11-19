using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoWeb.Domain.Interfaces
{
    public interface INotificationRepository: IGenericRepository<Notification,int>
    {
        IQueryable<Notification> Notifications { get; }

        IEnumerable<Notification> GetNotificationByUserId(int id);
        void SetVisibilityNotification(int id);
    }
}
