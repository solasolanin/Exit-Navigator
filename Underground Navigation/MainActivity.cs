using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Runtime;
using Android.Locations;
using System;
using System.Linq;
using Android.Content;

namespace Underground_Navigation
{
    [Activity(Label = "Underground_Navigation", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener

    {
        private Location currentLocation;
        private LocationManager locationManager;
        private string locationProvider;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            //Main.axmlにセット
            SetContentView (Resource.Layout.Main);
            InitializeLocationManager();

            //ボタンが押されたらcameraViewを起動
            var button1 = FindViewById<Button>(Resource.Id.cameraMode);
            button1.Click += (_, __) =>
            {
                var intent = new Intent(this, typeof(cameraView));
                StartActivity(intent);
            };
        }


        private void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
            var criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
                //FineならGPSで測位
            };

            //criteriaForLocationServiceの利用
            var acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

            //ロケーションプロバイダの使用確認
            if (acceptableLocationProviders.Any())
            {
                //acceptableLocationProvidersに何かクエリが入っていたら
                // 最初のクエリを取り出す
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
                Toast.MakeText(this, "Empty", ToastLength.Long).Show();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            // アクティビティがレジュームされたら
            // 位置情報を更新してくれるように依頼する

            locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
            Toast.MakeText(this, locationProvider.ToUpper() + "位置情報更新待ち",ToastLength.Long).Show();
        }

        protected override void OnPause()
        {
            base.OnPause();

            // アクティビティが停止されたら
            // 位置情報更新しないように依頼

            locationManager.RemoveUpdates(this);
        }


        //ILocationListener

        //位置情報が更新された時の処理
        public void OnLocationChanged(Location location)
        {
            currentLocation = location;
            if (currentLocation == null)
            {
                Toast.MakeText(this,"位置を特定できません", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, currentLocation.Latitude +"\n" + currentLocation.Longitude, ToastLength.Long).Show();
                locationManager.RemoveUpdates(this);
            }
        }

        //Location Providerが無効
        public void OnProviderDisabled(string provider)
        {

        }

        //Location Providerが有効
        public void OnProviderEnabled(string provider)
        {
            
        }

        //Location Providerのステータスが変更
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            Toast.MakeText(this, provider + "\n" + status, ToastLength.Long).Show();
        }
    }

    
    

}

