using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace WeatherInformation
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private EditText _editTextCityName;
        private Button _buttonCheckWeather;
        private ImageView _imageViewWeatherIcon;
        private ImageView _imageViewDegree;
        private TextView _textViewCelsuis;
        private TextView _textViewWeatherName;
        private TextView _textViewTemprature;
        private TextView _textViewCityName;
        private string _cityName;
        string cityname;
        private string _apiKey = "60153297ed89fd2f6e9c1cb25317521e";
        private string _apiBase = "https://api.openweathermap.org/data/2.5/weather?q=";
        private string _unit = "metric";
        private ProgressDialogFragment progressDialogFragment;
        private AlertDialog.Builder alert;
       
        private ConstraintLayout _constraintLayout;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            UIReferences();
            UIClicks();
        }
        public override bool OnSupportNavigateUp()
        {
            VisibilityGone();
            _textViewCityName.Visibility = Android.Views.ViewStates.Gone;
            return base.OnSupportNavigateUp();

        }
        void VisibilityGone()
        {
            _imageViewDegree.Visibility = Android.Views.ViewStates.Gone;
            _textViewCelsuis.Visibility = Android.Views.ViewStates.Gone;
            _imageViewWeatherIcon.Visibility = Android.Views.ViewStates.Gone;
            _textViewWeatherName.Visibility = Android.Views.ViewStates.Gone;
            _textViewTemprature.Visibility = Android.Views.ViewStates.Gone;
            _textViewCityName.Visibility = Android.Views.ViewStates.Gone;

        }
        void VisibilityVisible()
        {
            _imageViewDegree.Visibility = Android.Views.ViewStates.Visible;
            _textViewCelsuis.Visibility = Android.Views.ViewStates.Visible;
            _imageViewWeatherIcon.Visibility = Android.Views.ViewStates.Visible;
            _textViewWeatherName.Visibility = Android.Views.ViewStates.Visible;
            _textViewTemprature.Visibility = Android.Views.ViewStates.Visible;
            _textViewCityName.Visibility = Android.Views.ViewStates.Visible;
            _imageViewDegree.Visibility = Android.Views.ViewStates.Visible;
            _textViewCelsuis.Visibility = Android.Views.ViewStates.Visible;
        }
        private void UIClicks()
        {
            _buttonCheckWeather.Click += _buttonCheckWeather_Click;
        }

        private void _buttonCheckWeather_Click(object sender, EventArgs e)
        {
             _cityName = _editTextCityName.Text;
             GetWeatherInfo(_cityName);
        }

        private async void GetWeatherInfo(string cityName)
        {

            if(string.IsNullOrEmpty(cityName))
            {
                VisibilityGone();
                Toast.MakeText(this,"Please enter city....",ToastLength.Short).Show();
                
                return;
            }
            if (!CrossConnectivity.Current.IsConnected)
            {
                VisibilityGone();
                Toast.MakeText(this, "No Internet Connection", ToastLength.Short).Show();
               
                return;
            }

            try
            {
                string url = _apiBase + _cityName + "&appid=" + _apiKey + "&units=" + _unit;
                var handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                string response = await client.GetStringAsync(url);
                //Console.WriteLine(response);
            
             
                if (response != null)
                {
                   

                    ShowPrgoress("Fetching weather details....");
                    _editTextCityName.Text = null;
                    _editTextCityName.Hint = "Enter Your City";
                    var result = JObject.Parse(response);
                    string weatherinformation = result["weather"][0]["description"].ToString();
                    weatherinformation = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(weatherinformation);
                    string icons = result["weather"][0]["icon"].ToString();
                    string temprature = result["main"]["temp"].ToString();
                    cityname = result["name"].ToString();
                    string countrycode = result["sys"]["country"].ToString();
                    VisibilityVisible();
                    _textViewTemprature.Text = temprature;
                    
                    _textViewWeatherName.Text = weatherinformation;
                    _textViewCityName.Text = cityname + ", " + countrycode;
                    if (icons == "11d")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.thunderstrom);
                    }
                    else if (icons == "09d")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.drizzle);
                    }
                    else if (icons == "10n" || icons=="13n" || icons =="09n")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.rain);
                    }
                    else if (icons == "13d")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.snow);

                    }
                    else if (weatherinformation == "Mist" || weatherinformation == "Smoke" || weatherinformation == "Haze" || weatherinformation == "Fog")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.mist);
                    }
                    else if(weatherinformation == "Sand/ Dust Whirls" || weatherinformation== "Dust" || weatherinformation == "Sand")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.sanddustwhrills);
                    }
                    else if(weatherinformation == "Volcanic Ash")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.ashclould);
                    }
                    else if (weatherinformation == "Squalls")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.squalls);
                    }
                    else if (weatherinformation == "Tornado")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.tornado);
                    }
                    else if (icons == "01d" || icons== "01n")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.clearsky);
                    }
                    else if (icons == "02d" || icons == "02n" ||icons == "03d" || icons == "03n" || icons == "04d" || icons == "04n")
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.fewclouds);
                    }
                    else 
                    {
                        _constraintLayout.SetBackgroundResource(Resource.Drawable.weatherbackground);
                    }
                    string imageurl = "http://openweathermap.org/img/wn/" + icons + ".png";

                    WebRequest request = default(WebRequest);
                    request = WebRequest.Create(imageurl);
                    request.Method = "GET";
                    request.Timeout = int.MaxValue;

                    WebResponse webResponse = default(WebResponse);
                    webResponse = await request.GetResponseAsync();
                    MemoryStream memoryStream = new MemoryStream();
                    webResponse.GetResponseStream().CopyTo(memoryStream);

                    byte[] bytes = memoryStream.ToArray();

                    Bitmap bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
                    _imageViewWeatherIcon.SetImageBitmap(bitmap);


                    CloseProgress();



                }
             
            }
            catch(Exception ex)
            {
                View view = LayoutInflater.Inflate(Resource.Layout.customalertdialog, null);
                alert = new AlertDialog.Builder(this);
                alert.SetCancelable(true);
                alert.SetView(view);
                alert.Show();
              
            }
           

        }

        
        void ShowPrgoress(string status)

        {
            progressDialogFragment = new ProgressDialogFragment(status);
            var trans = SupportFragmentManager.BeginTransaction();
            progressDialogFragment.Cancelable = false;
            progressDialogFragment.Show(trans, "progressbar");
            VisibilityGone();

        }
        void CloseProgress()
        {
            if (progressDialogFragment != null)
            {
                progressDialogFragment.Dismiss();
                progressDialogFragment = null;
            }
        }

        private void UIReferences()
        {
              _editTextCityName = FindViewById<EditText>(Resource.Id.editTextCityName);
              _buttonCheckWeather = FindViewById<Button>(Resource.Id.buttonCheckWeather);
              _imageViewWeatherIcon = FindViewById<ImageView>(Resource.Id.imageViewWeatherIcon);
              _textViewWeatherName = FindViewById<TextView>(Resource.Id.textViewWeatherDescription);
              _textViewTemprature = FindViewById<TextView>(Resource.Id.textViewTemprature);
              _textViewCityName = FindViewById<TextView>(Resource.Id.textViewCityName);
              _imageViewDegree = FindViewById<ImageView>(Resource.Id.imageViewDegree);
              _textViewCelsuis = FindViewById<TextView>(Resource.Id.textViewCelcius);
            _constraintLayout = FindViewById<ConstraintLayout>(Resource.Id.constraintLayout);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}