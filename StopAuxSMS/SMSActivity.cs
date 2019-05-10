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
    [Activity(Label = "SMSActivity")]
    public class SMSActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Get the count value passed to us from MainActivity:
            var stopnumber = Intent.Extras.GetInt(Const.SMSNUMBER_KEY, -1);


            // Display the count sent from the first activity:
            SetContentView(Resource.Layout.activity_sms);

            var txtView = FindViewById<TextView>(Resource.Id.text1);
            txtView.Text = $"Appuyer sur le bouton suivant pour envoyer STOP au :  {stopnumber}";
            

            var sendSMSIntent = FindViewById<Button>(Resource.Id.sendSMSIntent);
            sendSMSIntent.Click += (sender, e) => {
                var smsUri = Android.Net.Uri.Parse($"smsto:{stopnumber}");
                var smsIntent = new Intent(Intent.ActionSendto, smsUri);
                smsIntent.PutExtra("sms_body", "STOP");
                StartActivity(smsIntent);
            };
        }
    }
}