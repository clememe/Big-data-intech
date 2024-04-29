using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Decompileur
{
    public partial class MainWindow : Window
    {
        private string csvStringPath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                // User selected a file
                string selectedFilePath = openFileDialog.FileName;
                csvStringPath = selectedFilePath;
                CSVPath.Text = selectedFilePath;
            }
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {

            if (!File.Exists(csvStringPath))
            {
                MessageBox.Show("The selected file doesn't exist.");
                return;
            }

            int chunkSize = Convert.ToInt32(LinesNumber.Text);

            using (var reader = new StreamReader(csvStringPath))
            {
                string line;
                int fileNumber = 0;
                int lineCount = 0;

                string compiledFolderPath = Path.Combine(Path.GetDirectoryName(csvStringPath), "Compiled");
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvStringPath);
                string fullPath = Path.Combine(compiledFolderPath, fileNameWithoutExtension);
                string fileDestinationPath = Path.Combine(fullPath, fileNameWithoutExtension + "_");

                if (!Directory.Exists(compiledFolderPath))
                {
                    Directory.CreateDirectory(compiledFolderPath);
                }

                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                string header = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    var outputFile = new StreamWriter(fileDestinationPath + fileNumber + ".csv");

                    outputFile.WriteLine(header);

                    lineCount = 0;

                    outputFile.WriteLine(line);
                    lineCount++;
                    while ((line = reader.ReadLine()) != null && lineCount < chunkSize)
                    {
                        outputFile.WriteLine(line);
                        lineCount++;
                    }

                    outputFile.Close();

                    fileNumber++;
                }
            }

            MessageBox.Show("Done.");
        }
    }
}
