using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace DigitalSignature
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Keys keys { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            keys = new Keys();
        }

        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {                                         
                RSACrypt.getParams(openFileDialog.FileName);                
                textBoxPath.Text = RSACrypt.filePath;                          
            }
        }

        private void buttonPatch_Click(object sender, RoutedEventArgs e)
        {
            if (RSACrypt.HashAndSignBytes(keys.privateKey))
            {
                MessageBox.Show("Signed!");
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void buttonCheck_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var file = File.ReadAllBytes(openFileDialog.FileName);
                if (RSACrypt.VerifySignedHash(file, keys.publicKey))
                {
                    MessageBox.Show("GOOD");
                }
                else
                {
                    MessageBox.Show("FAIL");
                }
            }
        }
    }
}
