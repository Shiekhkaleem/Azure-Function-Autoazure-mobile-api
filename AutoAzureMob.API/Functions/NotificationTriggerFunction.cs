using System;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutoAzureMob.API.Functions
{
    public class NotificationTriggerFunction
    {
        private readonly ILogger _logger;
        private readonly IConfiguration config;
        private readonly ExecuteContext executecontext;
        private readonly NotificationHandler notificationHandler;

        public NotificationTriggerFunction(ILoggerFactory loggerFactory, ExecuteContext _executecontext = null)
        {
            _logger = loggerFactory.CreateLogger<NotificationTriggerFunction>();
            executecontext = _executecontext;
            config = ConfigurationHelper.GetConfiguration();
            notificationHandler = new NotificationHandler(executecontext, config);
        }

        [Function("NotificationTriggerFunction")]
        public void Run([TimerTrigger("10 * * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"NotificationTriggerFunction Timer trigger function executed at: {DateTime.Now}");

            notificationHandler.SendTriggerNewNotifications();
            _logger.LogInformation($"NotificationTriggerFunction Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
