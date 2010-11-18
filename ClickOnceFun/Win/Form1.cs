using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            statusLabel.Visible = false;
            statusBar.Visible = false;
            installButton.Visible = false;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                currentVersion.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();

                ApplicationDeployment.CurrentDeployment.CheckForUpdateCompleted += CheckForUpdateCompleted;
                ApplicationDeployment.CurrentDeployment.CheckForUpdateAsync();

                if (ApplicationDeployment.CurrentDeployment.IsFirstRun)
                {
                    MessageBox.Show("This is the first run, we will update some of your old data files now.");
                    MessageBox.Show("Done!");
                }
            }
        }

        private void CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("ERROR: Could not retrieve new version of the application. Reason: \n" + e.Error.Message + "\nPlease report this error to the system administrator.");
                return;
            }

            if (e.Cancelled)
            {
                MessageBox.Show("The update was cancelled.");
            }

            // Ask the user if they would like to update the application now.
            if (e.UpdateAvailable)
            {
                string sizeText = e.UpdateSizeBytes/1024/1024 + "MB";

                if (!e.IsUpdateRequired)
                {
                    DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?\n\nThe download size is: " + sizeText, "Update Available", MessageBoxButtons.OKCancel);
                    if (DialogResult.OK == dr)
                    {
                        BeginUpdate();
                    }
                }
                else
                {
                    MessageBox.Show("A mandatory update is available for your application. We will install the update now, after which we will save all of your in-progress data and restart your application.");
                    BeginUpdate();
                }
            }
        }

        private void BeginUpdate()
        {
            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            ad.UpdateCompleted += new AsyncCompletedEventHandler(UpdateCompleted);

            // Indicate progress in the application's status bar.
            ad.UpdateProgressChanged += new DeploymentProgressChangedEventHandler(UpdateProgressChanged);
            ad.UpdateAsync();
        }

        void UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            String progressText = String.Format("{0:D}K out of {1:D}K downloaded - {2:D}% complete", e.BytesCompleted / 1024, e.BytesTotal / 1024, e.ProgressPercentage);

            statusLabel.Text = progressText;
            statusLabel.Visible = true;

            statusBar.Maximum = 100;
            statusBar.Minimum = 0;
            statusBar.Value = e.ProgressPercentage;
            statusBar.Visible = true;
        }

        void UpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                statusLabel.Text = "Update cancelled";
                statusBar.Visible = false;
                MessageBox.Show("The update of the application's latest version was cancelled.");
                return;
            }

            if (e.Error != null)
            {
                statusLabel.Text = "Error during update";
                statusBar.Visible = false;
                MessageBox.Show("ERROR: Could not install the latest version of the application. Reason: \n" + e.Error.Message + "\nPlease report this error to the system administrator.");
                return;
            }

            DialogResult dr = MessageBox.Show("The application has been updated. Restart? (If you do not restart now, the new version will not take effect until after you quit and launch the application again.)", "Restart Application", MessageBoxButtons.OKCancel);
            if (DialogResult.OK == dr)
            {
                Application.Restart();
            }
            else
            {
                statusBar.Visible = false;
                statusLabel.Visible = false;
                installButton.Visible = true;
            }
        }
    }
}
