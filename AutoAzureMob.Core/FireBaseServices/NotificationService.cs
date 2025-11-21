using AutoAzureMob.Models.DTO.NotiDTO;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Core.FireBaseServices
{
    public static class NotificationService
    {
        public static async Task<string> SendNotification(string DeviceToken, FirebaseAdmin.Messaging.Notification notify, SendNotifyDTO notifyDTO = null)
        {
            // This registration token comes from the client FCM SDKs.
            var registrationToken = DeviceToken;
            var fcm = FirebaseMessaging.DefaultInstance;
            var message = new FirebaseAdmin.Messaging.Message()
            {
                Notification = notify,
                Data = new Dictionary<string, string>()
                {
                    {"navigateTo","detailNotification"},
                    {"notifyId",notifyDTO.NotifyId},
                    {"total",notifyDTO.Total},
                    {"company",notifyDTO.CompanyId},
                },
                Token = registrationToken,
            };

            // Send a message to the device corresponding to the provided registration token.
            string response = await fcm.SendAsync(message).ConfigureAwait(true);

            // Response is a message ID string.
            return response;
        }

        public static async Task<BatchResponse> SendMultipleNotifications(List<string> DeviceTokens, FirebaseAdmin.Messaging.Notification notify, string screenPath)
        {
            // This registration token comes from the client FCM SDKs.
            var registrationToken = DeviceTokens;

            var fcm = FirebaseMessaging.DefaultInstance;

            var message = new MulticastMessage()
            {
                Notification = notify,
                Data = new Dictionary<string, string>()
                {
                    {"navigateTo",screenPath}
                },
                Tokens = registrationToken,
            };

            // Send a message to the device corresponding to the provided registration token.
            var response = await fcm.SendMulticastAsync(message).ConfigureAwait(true);

            // Response is a message ID string.
            return response;
        }

    }
}
