using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware;
using Xamarin.Media;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Tesseract;
using Tesseract.Droid;
using TiniyIoC;
using XLabs.Ioc;

namespace Underground_Navigation
{
    [Activity(Label = "cameraView")]
    public class cameraView : Activity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.cameraView);

            //cameraを起動
            var picker = new MediaPicker(this);
            if (!picker.IsCameraAvailable)
                Console.WriteLine("No camera!");
            else
            {
                var intent = picker.GetTakePhotoUI(new StoreCameraMediaOptions
                {
                    Name = "test.jpg",
                    Directory = "MediaPickerSample"
                });
                StartActivityForResult(intent, 1);
            }
            var container = TinyIoCContainer.Current;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            // User canceled
            if (resultCode == Result.Canceled)
                return;

            //Imageの表示
            data.GetMediaFileExtraAsync(this).ContinueWith(t => {
                ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView1);
                imageView.SetImageBitmap(BitmapFactory.DecodeFile(t.Result.Path));
                }, TaskScheduler.FromCurrentSynchronizationContext());


            //テキストファイルのパス
            string langPath = @"MediaPickerSample/tmp";

            //テキストの言語
            string lngStr = "eng";


            ITesseractApi api = new ITesseractApi(Context);
                
        }
    }
}