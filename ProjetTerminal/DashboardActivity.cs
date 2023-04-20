using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace ProjetTerminal
{
    [Activity(Label = "DashboardActivity")]
    public class DashboardActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.dashboard);

            // Get the user information from the intent extras
            string userId = Intent.GetStringExtra("user_id");
            string email = Intent.GetStringExtra("email");

            // Display the user information on the screen
            TextView textView = FindViewById<TextView>(Resource.Id.user_info);
            textView.Text = $"User ID: {userId}, Email: {email}";
        }
    }
}
