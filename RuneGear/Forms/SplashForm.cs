using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RuneGear.Forms
{
    public partial class SplashForm : Form
    {
        private const int WaitTimeBeforeDone = 500;

        public Action OnSplashDone;
        public Action OnSplashIsShown;

        public SplashForm()
        {
            InitializeComponent();
            Shown += SplashForm_Shown;
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            Size = BackgroundImage.Size;
        }

        private void SplashForm_Shown(object sender, EventArgs e)
        {
            OnSplashIsShown?.Invoke();
        }

        public async void SetProgressBar(string action, int progress)
        {
            Invoke(new Action(() =>
            {
                labelProgress.Text = $"{action} {progress}%";
                progressBar.Value = progress;
            }));

            if ("done" == action.ToLower() && progress >= 100)
            {
                // This prevents the splashform from just disappearing
                await Task.Delay(WaitTimeBeforeDone);

                Invoke(new Action(() =>
                {
                    OnSplashDone?.Invoke();
                    Close();
                }));
            }
        }
    }
}
