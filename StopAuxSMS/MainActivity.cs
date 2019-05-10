using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace StopAuxSMS
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            
            if (ContextCompat.CheckSelfPermission(BaseContext, "android.permission.READ_SMS") == Android.Content.PM.Permission.Granted)            
                Toast.MakeText(this, "Permission READ_SMS ok", ToastLength.Short).Show();            
            else       
                ActivityCompat.RequestPermissions(this, new string[] { "android.permission.READ_SMS" }, Const.REQUEST_CODE_ASK_PERMISSIONS);

            if (ContextCompat.CheckSelfPermission(BaseContext, "android.permission.RECEIVE_SMS") == Android.Content.PM.Permission.Granted)
                Toast.MakeText(this, "Permission RECEIVE_SMS ok", ToastLength.Short).Show();
            else
                ActivityCompat.RequestPermissions(this, new string[] { "android.permission.RECEIVE_SMS" }, Const.REQUEST_CODE_ASK_PERMISSIONS);

            CreateNotificationChannel();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }


        public void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = Resources.GetString(Resource.String.channel_name);
            var channelDescription = Resources.GetString(Resource.String.channel_description);
            var channel = new NotificationChannel(Const.CHANNEL_ID, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

    }
}

