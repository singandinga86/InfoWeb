﻿using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.Domain.Interfaces
{
    public interface INotificationRepository: IGenericRepository<Notification,int>
    {
    }
}