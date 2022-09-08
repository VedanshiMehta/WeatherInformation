
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherInformation
{
    public class ProgressDialogFragment : DialogFragment
    {
        private View view;
        private TextView _textViewStatus;
        // private ProgressBar _progressBar;
        public string progressstatus;
        public ProgressDialogFragment(string progressstatus)
        {
            this.progressstatus = progressstatus;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            view = inflater.Inflate(Resource.Layout.customprogressbar, container, false);
            _textViewStatus = view.FindViewById<TextView>(Resource.Id.textViewWait);
            _textViewStatus.Text = progressstatus;
           // _progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
            return view;
        }
    }
}