using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Telephony;
using Android.Views;
using Android.Widget;


namespace StopAuxSMS
{
    [BroadcastReceiver(Enabled = true, Label = "SMS Receiver")]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED", "android.permission.RECEIVE_BOOT_COMPLETED" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class SMSReceiver : BroadcastReceiver
    {

        public static readonly string IntentAction = "android.provider.Telephony.SMS_RECEIVED";

        public override void OnReceive(Context context, Intent intent)
        {
            
            //Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();

            if(intent.HasExtra("pdus"))
            {
                var smsArray = (Java.Lang.Object[])intent.Extras.Get("pdus");

                foreach (var item in smsArray)
                {
                    var sms = SmsMessage.CreateFromPdu((byte[]) item);
                    var address = sms.OriginatingAddress;
                    var message = sms.MessageBody;

                    //Toast.MakeText(context, "Number : " + address + " Message : " + message, ToastLength.Long).Show();

                    int stopnumber = GetStopNumber(message);

                    if(stopnumber > 0)
                        PushNotif(context, stopnumber);
                                        
                }

            }
        }

        public int GetStopNumber(string message)
        {
            try
            {
                int stopnumber = 0;

                string temp = message.ToLower();
                int index = temp.LastIndexOf("stop");

                if (index > 0)
                {
                    temp = temp.Substring(index, temp.Length - index);

                    Regex regex = new Regex(@"\d+");
                    Match match = regex.Match(temp);
                    string stop_number = match.Value;

                    stopnumber = Convert.ToInt32(stop_number);
                }

                return stopnumber;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        public void PushNotif(Context context, int stopnumber)
        {
            // Pass the current button press count value to the next activity:
            var valuesForActivity = new Bundle();
            valuesForActivity.PutInt(Const.SMSNUMBER_KEY, stopnumber);

            // When the user clicks the notification, SecondActivity will start up.
            var resultIntent = new Intent(context, typeof(SMSActivity));

            // Pass some values to SecondActivity:
            resultIntent.PutExtras(valuesForActivity);

            // Construct a back stack for cross-task navigation:
            var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(context);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(SMSActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            var resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);
            
            var smsUri = Android.Net.Uri.Parse($"smsto:{stopnumber}");
            var smsIntent = new Intent(Intent.ActionSendto, smsUri);
            smsIntent.PutExtra("sms_body", "STOP");

            var stackBuilderSMS = Android.Support.V4.App.TaskStackBuilder.Create(context);
            stackBuilderSMS.AddParentStack(Java.Lang.Class.FromType(typeof(SMSActivity)));
            stackBuilderSMS.AddNextIntent(smsIntent);

            // Create the PendingIntent with the back stack:            
            var smsPendingIntent = stackBuilderSMS.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);


            // Build the notification:
            var builder = new NotificationCompat.Builder(context, Const.CHANNEL_ID)
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          //.SetContentIntent(resultPendingIntent) // Start up this activity when the user clicks the intent.
                          .SetContentTitle("Stop aux annonces par SMS") // Set the title
                          //.SetNumber(stopnumber) // Display the count in the Content Info
                          .SetSmallIcon(Resource.Drawable.ic_stat_button_click) // This is the icon to display
                          .AddAction(Resource.Drawable.ic_stat_button_click, "ENVOYER", smsPendingIntent)
                          .SetContentText($"Le bouton suivant enverra STOP par sms au {stopnumber}"); // the message to display.

            // Finally, publish the notification:
            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(Const.NOTIFICATION_ID, builder.Build());
        }


     
    }
}