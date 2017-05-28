using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;

namespace PhoneApp
{
    [Activity(Label = "Phone App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var TranslateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var CallButton = FindViewById<Button>(Resource.Id.CallButton);
            var MessageView = FindViewById<TextView>(Resource.Id.ResultTextView);

            CallButton.Enabled = false;
            MessageView.Enabled = false;
            var ServiceClient = new SALLab05.ServiceClient();
            string StudentEmail = "email at domain dot net";
            string Password = "PassWord";
            string myDevice = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            var Result = await ServiceClient.ValidateAsync(StudentEmail, Password, myDevice);

            MessageView.Text = $"{ Result.Status}\n{ Result.Fullname}\n{ Result.Token}";

            //MessageView.Text = this.Validate().Result;
           // MessageView.Text = "asdfbib \napshdfaosdnfon\n apsodfna \t 123";

            var TranslatedNumber = string.Empty;
            TranslateButton.Click += (object sender, System.EventArgs e) =>
            {
                var Translator = new PhoneTranslator();
                TranslatedNumber = Translator.ToNumber(PhoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(TranslatedNumber))
                {
                    // No hay número a llamar
                    CallButton.Text = "Llamar";
                    CallButton.Enabled = false;
                }
                else
                {
                    // Hay un posible número telefónico a llamar
                    CallButton.Text = $"Llamar al { TranslatedNumber}";
                    CallButton.Enabled = true;
                }
            };

            CallButton.Click += (object sender, System.EventArgs e) =>
            {
                // Intentar marcar el número telefónico
                var CallDialog = new AlertDialog.Builder(this);
                CallDialog.SetMessage($"Llamar al número {TranslatedNumber}?");
                CallDialog.SetNeutralButton("Llamar", delegate
                {
                    // Crear un intento para marcar el número telefónico
                    var CallIntent = new Android.Content.Intent(Android.Content.Intent.ActionCall);
                    CallIntent.SetData(Android.Net.Uri.Parse($"tel: { TranslatedNumber}"));
                    StartActivity(CallIntent);
                });
                CallDialog.SetNegativeButton("Cancelar", delegate { });
                // Mostrar el cuadro de diálogo al usuario y esperar una respuesta.
                CallDialog.Show();
            };
        }
        private async Task<string> Validate()
        {
            var ServiceClient = new SALLab05.ServiceClient();
            string StudentEmail = "sestada@praxya.es";
            string Password = "N0v3mb3rPraxya";
            string myDevice = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            var Result = await ServiceClient.ValidateAsync(StudentEmail, Password, myDevice);
           
            var txtResult = $"{ Result.Status}\n{ Result.Fullname}\n{ Result.Token}";
            return txtResult;
        }
    }
}
