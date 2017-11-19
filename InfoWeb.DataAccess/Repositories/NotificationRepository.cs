using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DataAccess.Repositories
{
    public class NotificationRepository : GenericRepository<Notification, int>, INotificationRepository
    {
        public NotificationRepository(InfoWebDatabaseContext context) :base(context)
        {                
        }

        public IQueryable<Notification> Notifications => context.Notifications;

        public IEnumerable<Notification> GetAll()
        {
            return entitySet.Include(p => p.User)
                            //.Include(p => p.Sender)
                            .ToList();
        }

        public override Notification GetById(int id)
        {
            return entitySet.Where(p => p.Id == id)
                             .Include(p => p.User)
                            // .Include(p => p.Sender)
                             .FirstOrDefault();
        }

        public IEnumerable<Notification> GetNotificationByUserId(int id)
        {

            return entitySet.Where(p => p.UserId == id && !p.Seen)
                             .Include(p => p.User)
                             // .Include(p => p.Sender)
                             .ToList();
        }

        public IEnumerable<Notification> GetRange(int skip, int take)
        {
            return base.GetRange(entitySet.Include(p => p.User), skip, take);
        }

        public void SetVisibilityNotification(int id)
        {
            entitySet.Where(n => n.UserId == id && !n.Seen).ToList().ForEach(item =>
            {
                item.Seen = true;
                entitySet.Update(item);
            });
            
        }
    }
}
