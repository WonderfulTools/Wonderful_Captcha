using Core.Interfaces;

namespace sample.WinformClient
{
    public partial class Form1 : Form
    {
        private readonly IWonderfulCaptchaService _wonderfulCaptchaService;
        public Form1(IWonderfulCaptchaService wonderfulCaptchaService)
        {
            InitializeComponent();
            _wonderfulCaptchaService = wonderfulCaptchaService;
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            var base64 = await _wonderfulCaptchaService.GenerateCaptchaAsync(default);

            byte[] bytes = Convert.FromBase64String(base64);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                pic.Image = Image.FromStream(ms);
            }
        }
    }
}