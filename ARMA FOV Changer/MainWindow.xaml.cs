using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ARMA_FOV_Changer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private static Version latest;

        private static string fileParams;

        private static string profilePath;
        private static string[] arrLine;

        private int fovTopLine;
        private int fovLeftLine;

        private static int desiredFov;
        private static string fovstring;

        private static int button;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region Initial Update Check

            // Verion number from assembly
            string AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            MenuItem ver = new MenuItem();
            MenuItem newExistMenuItem = (MenuItem)this.FileMenu.Items[1];
            ver.Header = "v" + AssemblyVersion;
            ver.IsEnabled = false;
            newExistMenuItem.Items.Add(ver);

            try
            {
                int idx = AssemblyVersion.LastIndexOf('0') - 1;
                AssemblyVersion = AssemblyVersion.Substring(0, idx);
            }
            catch { }

            // Check for a new version.
            int updateResult = await CheckForUpdate();
            if (updateResult == -1)
            {
                // Some Error occurred.
                // TODO: Handle this Error.
            }
            else if (updateResult == 1)
            {
                // An update is available, but user has chosen not to update.

            }
            else if (updateResult == 2)
            {
                // An update is available, and the user has chosen to update.
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = true;
                startInfo.FileName = "Updater.exe";
                startInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = fileParams;

                Process.Start(startInfo);
                Environment.Exit(0);
            }

            #endregion

            LoadProfile();
            
        }
    
        private void LoadProfile()
        {
            // Open splash window for game selection
            Splash splashWindow = new Splash();
            splashWindow.ShowDialog();

            // Profile file path from splashWindow
            profilePath = splashWindow.filePath;

            if (splashWindow.filePath == null)
                Environment.Exit(0);

            while (profilePath.Contains(".vars."))
            {
                MessageBox.Show("Incorrect file selected!\nBe sure you are selecting your profile without 'vars' in the file name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                splashWindow = new Splash();
                splashWindow.ShowDialog();
                profilePath = splashWindow.filePath;
            }

            // Get profile name from file path string
            int lastSlashIdx = profilePath.LastIndexOf("\\") + 1;
            int profNameLength = profilePath.LastIndexOf(".") - lastSlashIdx;
            
            int cwSlashIdx = lastSlashIdx > 0 ? profilePath.LastIndexOf("\\", lastSlashIdx - 2) : -1;
            int profNameLengthCW = profilePath.LastIndexOf("\\") - cwSlashIdx;

            string profileName;

            button = splashWindow.buttonPressed;

            //Get profile name from either parent directory or selected file.
            if (button == 0)
                profileName = profilePath.Substring(cwSlashIdx + 1, profNameLengthCW - 1);
            else
                profileName = profilePath.Substring(lastSlashIdx, profNameLength);

            // DayZ Standalone only uses vertical fov
            if (button == 3)
            {
                fovLeftLabel.IsEnabled = false;
                fovLeftLabel_Copy.IsEnabled = false;
                DesiredFovLeftTextBox.IsEnabled = false;
            }

            // Replace %20 (space char) with an actual space
            if (profileName.Contains("%20"))
                profileName = profileName.Replace("%20", " ");

            profileLabel.Content = profileName;

            // Get and set resolution info
            widthTextBox.Text = SystemParameters.PrimaryScreenWidth.ToString();
            heightTextBox.Text = SystemParameters.PrimaryScreenHeight.ToString();

            int nGCD = GetGreatestCommonDivisor((int)SystemParameters.PrimaryScreenHeight, (int)SystemParameters.PrimaryScreenWidth);
            aspectRatioLabel.Content = string.Format("{0} : {1}", (int)SystemParameters.PrimaryScreenWidth / nGCD, (int)SystemParameters.PrimaryScreenHeight / nGCD);

            // Desired fov to radians
            double hfovRad = fovSlider.Value * (Math.PI / 180);
            double hFoV = 2 * Math.Atan(Math.Tan(hfovRad / 2) * (SystemParameters.PrimaryScreenHeight / SystemParameters.PrimaryScreenWidth));
            hFoV = Math.Ceiling(hFoV * 100) / 100;
            DesiredFovTopTextBox.Text = hFoV.ToString();

            double wFoV = hFoV / ((int)SystemParameters.PrimaryScreenHeight / nGCD);
            wFoV = wFoV * ((int)SystemParameters.PrimaryScreenWidth / nGCD);
            wFoV = Math.Floor(wFoV * 100) / 100;

            DesiredFovLeftTextBox.Text = wFoV.ToString();

            string fovTop = null;
            string fovLeft = null;

            // Read entire file into an array of strings
            // Get info we want from desired lines
            arrLine = File.ReadAllLines(profilePath);

            if (button == 3)
                fovstring = "fov";
            else
                fovstring = "fovTop";

            // Find line that controls fov
            for(int i = 0; i < arrLine.Length; i++)
            {
                if (arrLine[i].Contains(fovstring))
                {
                    fovTopLine = i;
                    fovLeftLine = i + 1;

                    break;
                }
            }

            fovTop = arrLine[fovTopLine];
            fovLeft = arrLine[fovLeftLine];

            int eqIdx = fovTop.IndexOf("=") + 1;
            int fovLen = fovTop.IndexOf(";") - eqIdx;
            fovTop = fovTop.Substring(eqIdx, fovLen);

            eqIdx = fovLeft.IndexOf("=") + 1;
            fovLen = fovLeft.IndexOf(";") - eqIdx;
            fovLeft = fovLeft.Substring(eqIdx, fovLen);

            // Update Labels
            CurrentFovTopLabel.Content = fovTop;

            if (button != 3)
                CurrentFovLeftLabel.Content = fovLeft;
        }

        private static int GetGreatestCommonDivisor(int a, int b)
        {
            return b == 0 ? a : GetGreatestCommonDivisor(b, a % b);
        }

        private static async Task<int> CheckForUpdate()
        {
            //Nkosi Note: Always use asynchronous versions of network and IO methods.

            //Check for version updates
            var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 0, 10);
            try
            {
                // Open the text file using a stream reader
                using (Stream stream = await client.GetStreamAsync("http://textuploader.com/d5ivh/raw"))
                {
                    System.Version current = Assembly.GetExecutingAssembly().GetName().Version;
                    StreamReader reader = new StreamReader(stream);
                    latest = System.Version.Parse(reader.ReadLine());

                    List<string> newFiles = new List<string>();

                    while (!reader.EndOfStream)
                    {
                        newFiles.Add(await reader.ReadLineAsync());
                    }

                    for (int i = 0; i < newFiles.Count; i++)
                    {
                        if (i == 0)
                            fileParams += newFiles[i];
                        else
                            fileParams += " " + newFiles[i];
                    }

                    if (latest > current)
                    {
                        MessageBoxResult answer = MessageBox.Show("A new version of Field of Views is available!\n\nCurrent Version     " + current + "\nLatest Version     " + latest + "\n\nUpdate now?", "Field of Views Update", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if (answer == MessageBoxResult.Yes)
                        {
                            //Update is available, and user wants to update. Requires app to close.
                            return 2;
                        }
                        //Update is available, but user chose not to update just yet.
                        return 1;
                    }
                }
                //No update available.
                return 0;
            }
            catch (Exception m)
            {
                //MessageBox.Show("Failed to check for update.\n" + m.Message,"Error", MessageBoxButtons.OK, MessageBoxImage.Error);
                return 0;
            }
        }

        #region Object Click Events

        private void saveButton_Click(object sender, RoutedEventArgs e)
        { 
            arrLine[fovTopLine] = fovstring + "=" + DesiredFovTopTextBox.Text + ";";
            if (button == 3)
                arrLine[fovLeftLine] = "fovLeft=" + DesiredFovLeftTextBox.Text + ";";

            try
            {
                // Write back to file
                File.WriteAllLines(profilePath, arrLine);

                CurrentFovTopLabel.Content = DesiredFovTopTextBox.Text;
                if (button == 3)
                    CurrentFovLeftLabel.Content = DesiredFovLeftTextBox.Text;
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message, "Exception!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            MessageBox.Show("FOV Updated!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void GitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/rex706/ARMA-FOV-Changer/");
        }
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        private void fovSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(heightTextBox != null && widthTextBox != null && heightTextBox.Text.Length > 2 && widthTextBox.Text.Length > 2)
                refreshMath();
            else
                Console.WriteLine("Not enough input!");
        }

        private void refreshMath()
        {
            desiredFov = (int)fovSlider.Value;
            fovLabel.Content = fovSlider.Value.ToString() + "°";

            int nGCD = GetGreatestCommonDivisor(Int32.Parse(heightTextBox.Text), Int32.Parse(widthTextBox.Text));

            int aspect1 = Int32.Parse(widthTextBox.Text) / nGCD;
            int aspect2 = Int32.Parse(heightTextBox.Text) / nGCD;

            string aspectRatio = aspect1 + ":" + aspect2;

            double uiTopLeftX = 0;
            double uiTopLeftY = 0;

            double uiBottomRightX = 1;
            double uiBottomRightY = 1;

            // If chosen game was Cold War Assault, set up ui scaling based on resolution per http://ofp-faguss.com/files/ofp_aspect_ratio.pdf
            if (button == 0 /*|| button == Armed Assault*/)
            {
                uiTopLeftXLabel.IsEnabled = true;
                uiTopLeftYLabel.IsEnabled = true;
                uiBottomRightXLabel.IsEnabled = true;
                uiBottomRightYLabel.IsEnabled = true;

                switch (aspectRatio)
                {
                    case "5:4":
                        uiTopLeftY = 0.03125;
                        uiBottomRightY = 0.96875;
                        break;

                    case "16:10":
                        uiTopLeftX = 0.083333;
                        uiBottomRightX = 0.916667;
                        break;

                    case "15:9":
                        uiTopLeftX = 0.1;
                        uiBottomRightX = 0.9;
                        break;

                    case "16:9":
                        uiTopLeftX = 0.125;
                        uiBottomRightX = 0.875;
                        break;

                    case "21:9":
                        uiTopLeftX = 0.21875;
                        uiBottomRightX = 0.78125;
                        break;

                    case "12:3":
                        uiTopLeftX = 0.333333;
                        uiBottomRightX = 0.666667;
                        break;

                    case "15:4":
                        uiTopLeftX = 0.333333;
                        uiTopLeftY = 0.03125;
                        uiBottomRightX = 0.666667;
                        uiBottomRightY = 0.96875;
                        break;

                    case "48:10":
                        uiTopLeftX = 0.361111;
                        uiBottomRightX = 0.638889;
                        break;

                    case "45:9":
                        uiTopLeftX = 0.366667;
                        uiBottomRightX = 0.633333;
                        break;

                    case "48:9":
                        uiTopLeftX = 0.375;
                        uiBottomRightX = 0.625;
                        break;

                    case "63:9":
                        uiTopLeftX = 0.40625;
                        uiBottomRightX = 0.59375;
                        break;
                }

                uiTopLeftXLabel_Copy.Content = uiTopLeftX.ToString();
                uiTopLeftYLabel_Copy.Content = uiTopLeftY.ToString();
                uiBottomRightXLabel_Copy.Content = uiBottomRightX.ToString();
                uiBottomRightYLabel_Copy.Content = uiBottomRightY.ToString();
            }

            aspectRatioLabel.Content = string.Format("{0} : {1}", aspect1, aspect2);

            // Desired fov to radians
            double hfovRad = fovSlider.Value * (Math.PI / 180);

            double hFoV = 2 * Math.Atan(Math.Tan(hfovRad / 2) * (Double.Parse(heightTextBox.Text) / Double.Parse(widthTextBox.Text)));

            hFoV = Math.Ceiling(hFoV * 100) / 100;

            DesiredFovTopTextBox.Text = hFoV.ToString();

            double wFoV = hFoV / (Double.Parse(heightTextBox.Text) / nGCD);
            wFoV = wFoV * (Double.Parse(widthTextBox.Text) / nGCD);
            wFoV = Math.Floor(wFoV * 100) / 100;

            DesiredFovLeftTextBox.Text = wFoV.ToString();
        }

        private void ProfileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            fovLeftLabel.IsEnabled = true;
            fovLeftLabel_Copy.IsEnabled = true;
            DesiredFovLeftTextBox.IsEnabled = true;

            uiTopLeftXLabel.IsEnabled = false;
            uiTopLeftYLabel.IsEnabled = false;
            uiBottomRightXLabel.IsEnabled = false;
            uiBottomRightYLabel.IsEnabled = false;

            uiTopLeftXLabel_Copy.Content = "";
            uiTopLeftYLabel_Copy.Content = "";
            uiBottomRightXLabel_Copy.Content = "";
            uiBottomRightYLabel_Copy.Content = "";

            LoadProfile();
            refreshMath();
        }

        private void widthTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Regex.IsMatch(widthTextBox.Text, @"^\d+$") && widthTextBox.Text.Length > 2 && Regex.IsMatch(widthTextBox.Text, @"^\d+$") && heightTextBox.Text.Length > 2)
                refreshMath();
        }

        private void heightTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Regex.IsMatch(widthTextBox.Text, @"^\d+$") && heightTextBox.Text.Length > 2 && Regex.IsMatch(widthTextBox.Text, @"^\d+$") && widthTextBox.Text.Length > 2)
                refreshMath();
        }
    }
}