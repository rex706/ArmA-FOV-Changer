using System;
using System.IO;
using System.Windows;

namespace ARMA_FOV_Changer
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {

        private int button;
        private string filename;

        public Splash()
        {
            InitializeComponent();
        }

        public int buttonPressed
        {
            get { return button; }
            set { }
        }

        public string filePath
        {
            get { return filename; }
            set { }
        }

        private void Arma2OAButton_Click(object sender, RoutedEventArgs e)
        {
            button = 1;

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and initial directory
            dlg.DefaultExt = ".ArmA2OAProfile";
            dlg.Filter = "ArmA 2 Profile (*.ArmA2Profile)|*.ArmA2Profile| ArmA 2 OA Profile (*.ArmA2OAProfile)|*.ArmA2OAProfile|All files (*.*)|*.*";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ArmA 2 Other Profiles";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
        
            // Get the selected file name and display in a TextBox 
            if (result == true)
                filename = dlg.FileName;

            Close();
        }

        private void Arma3Button_Click(object sender, RoutedEventArgs e)
        {
            button = 2;

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and initial directory
            dlg.DefaultExt = ".ArmA3Profile";
            dlg.Filter = "ArmA 3 Profile (*.ArmA3Profile)|*.ArmA3Profile|All files (*.*)|*.*";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ArmA 3";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
                filename = dlg.FileName;

            Close();
        }

        private void DayZButton_Click(object sender, RoutedEventArgs e)
        {
            button = 3;

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and initial directory
            dlg.DefaultExt = ".DayZProfile";
            dlg.Filter = "DayZ Profile (*.DayZProfile)|*.DayZProfile|All files (*.*)|*.*";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DayZ";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
                filename = dlg.FileName;

            Close();
        }

        private void ArmaCWQAButton_Click(object sender, RoutedEventArgs e)
        {
            button = 0;

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            string primary = @"C:\Program Files (x86)\Steam\steamapps\common\ARMA Cold War Assault\Users";
            string secondary = @"D:\Program Files (x86)\Steam\steamapps\common\ARMA Cold War Assault\Users";
            string tertiary = @"E:\Program Files (x86)\Steam\steamapps\common\ARMA Cold War Assault\Users";

            // Set filter for file extension and initial directory
            dlg.DefaultExt = ".cfg";
            dlg.Filter = "UserInfo (*.cfg)|*.cfg|All files (*.*)|*.*";

            if (Directory.Exists(primary))
                dlg.InitialDirectory = primary;
            else if(Directory.Exists(secondary))
                dlg.InitialDirectory = secondary;
            else if (Directory.Exists(tertiary))
                dlg.InitialDirectory = tertiary;

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
                filename = dlg.FileName;

            Close();
        }

        private void ArmaAAButton_Click(object sender, RoutedEventArgs e)
        {
            button = 4;

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and initial directory
            dlg.DefaultExt = ".ArmAProfile";
            dlg.Filter = "Arma Profile (*.ArmAProfile)|*.ArmAProfile|All files (*.*)|*.*";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ArmA";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
                filename = dlg.FileName;

            Close();
        }
    }
}
