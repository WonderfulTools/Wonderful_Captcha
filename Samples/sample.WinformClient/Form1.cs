using Core.Interfaces;

namespace sample.WinformClient
{
    public partial class Form1 : Form
    {
        private readonly ICaptchaBuilder _captchaPerBuilderService;
        public Form1(ICaptchaBuilder captchaPerBuilderService)
        {
            InitializeComponent();
            _captchaPerBuilderService = captchaPerBuilderService;
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            //var base64 = _captchaPerBuilderService.

            //byte[] bytes = Convert.FromBase64String(base64);

            //using (MemoryStream ms = new(bytes))
            //{
            //    pic.Image = Image.FromStream(ms);
            //}
        }
    }
}