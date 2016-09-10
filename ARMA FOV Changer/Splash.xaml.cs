using System;
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
            dlg.Filter = "ArmA 2 Profile (*.ArmA2OAProfile)|*.ArmA2OAProfile";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ArmA 2 Other Profiles";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
        
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
            }

            Close();
        }

        private void Arma3Button_Click(object sender, RoutedEventArgs e)
        {
            button = 2;

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and initial directory
            dlg.DefaultExt = ".ArmA3Profile";
            dlg.Filter = "ArmA 3 Profile (*.ArmA3Profile)|*.ArmA3Profile";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ArmA 3";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
            }

            Close();
        }
    }
}
