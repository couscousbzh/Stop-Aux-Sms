using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StopAuxSMS
{
    public class Const
    {
        // Unique ID for our notification: 
        public static readonly int NOTIFICATION_ID = 1000;
        public static readonly string CHANNEL_ID = "location_notification";

        public static readonly int REQUEST_CODE_ASK_PERMISSIONS = 123;

        public static readonly string SMSNUMBER_KEY = "SMSNUMBER_KEY";

        



    }
}