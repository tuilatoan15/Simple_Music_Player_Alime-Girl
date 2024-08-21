using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Music_Player
{
    public partial class Form1 : Form
    {
        private string settingsFilePath = "Settings.txt";

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            //ToolTip
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnPlay, "Shortcut Key is P");
            toolTip.SetToolTip(btnPause, "Shortcut Key is K");
            toolTip.SetToolTip(btnOpenFile, "Shortcut Key is O");
            toolTip.SetToolTip(btnStop, "Shortcut Key is S");

            LoadSettings();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.K)
            {
                btnPause.PerformClick();
            }
            else if (e.KeyCode == Keys.S)
            {
                btnStop.PerformClick();
            }
            else if (e.KeyCode == Keys.O)
            {
                btnOpenFile.PerformClick();
            }
            else if (e.KeyCode == Keys.P)
            {
                btnPlay.PerformClick();
            }
            else
            {
                //Ko co gi trong nay dau!
            }
        }

        private void btnOpenFile_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Title = "Hãy chọn một File để mở!";
            if (open.ShowDialog() == DialogResult.OK)
            {
                axWindowsMediaPlayer1.URL = open.FileName;
            }
        }

        private void btnPlay_Click_1(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void btnPause_Click_1(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void btnHelp_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn gửi Email cho Chủ sở hữu?", "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                string url = @"https://mail.google.com/mail/u/0/#inbox?compose=CllgCJvlqmtgfjxDCxZJPlvkrQTjxkdmqhmNHkBKbsmCZGvHDFbSMPfwtkSKsWfrFhJKjsWSjnq";
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi mở liên kết: " + ex.Message);
                }
            }

            return;
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 1)
            {
                btnPlay.PerformClick();
            }
        }

        private void LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                string[] settings = File.ReadAllLines(settingsFilePath);
                if (settings.Length > 0)
                {
                    axWindowsMediaPlayer1.URL = settings[0];
                    if (settings.Length > 1 && double.TryParse(settings[1], out double position))
                    {
                        axWindowsMediaPlayer1.Ctlcontrols.currentPosition = position;
                    }
                }
            }
        }

        private void SaveSettings()
        {
            using (StreamWriter writer = new StreamWriter(settingsFilePath))
            {
                writer.WriteLine(axWindowsMediaPlayer1.URL);
                writer.WriteLine(axWindowsMediaPlayer1.Ctlcontrols.currentPosition);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
    }
}