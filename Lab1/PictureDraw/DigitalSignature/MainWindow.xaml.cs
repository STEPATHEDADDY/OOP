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
        private RSACrypt instance;
        public RSAParameters privateKey;
        public RSAParameters publicKey;
        public byte[] bytesPublic;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                privateKey = RSAalg.ExportParameters(true);
                publicKey = RSAalg.ExportParameters(false);
                bytesPublic = RSAalg.ExportCspBlob(false);
                instance = new RSACrypt(openFileDialog.FileName, File.ReadAllBytes(openFileDialog.FileName));                
                textBoxPath.Text = instance.filePath;                          
            }
        }

        private void buttonPatch_Click(object sender, RoutedEventArgs e)
        {
            if (instance.HashAndSignBytes(privateKey, publicKey, bytesPublic))
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
                if (RSACrypt.VerifySignedHash(file))
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
