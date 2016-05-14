using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SDVSaveUpdater2
{
    public partial class Form1 : Form
    {
        Fonctions mOperation;

        XDocument docName;
        XDocument docGame;

        private string pathName;
        private string pathGame;

        bool done;
        string[] directories;

        public Form1()
        {
            InitializeComponent();

            directories = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\StardewValley\\Saves");
            int i = 0;

            while (i < directories.GetLength(0))
            {
                comboBox1.Items.AddRange(new object[] { Path.GetFileNameWithoutExtension(directories[i]).Trim(new Char[] { '_', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }) });
                i++;
            }

            done = false;
            mOperation = new Fonctions();

            // Corriger
            buttonReplace.Enabled = false;
            buttonReplace.BackColor = Color.White;
            buttonReplace.Text = "Choisissez votre save";
        }

        private async void buttonReplace_Click(object sender, EventArgs el)
        {
            try
            {

                // Bouton en cours
                buttonReplace.Enabled = false;
                buttonReplace.BackColor = Color.White;

                // Remplacements
                await Task.Run(() =>
                {
                    mOperation.RemplaceDraivin(docName, docGame);
                });

                // Sauvegarde
                docName.Save(pathName);
                docGame.Save(pathGame);

                // Changements tête bouton
                buttonReplace.Enabled = false;
                buttonReplace.BackColor = Color.White;
                buttonReplace.Text ="Save corrigé !";
                labelAnalyse.Text = "Choisissez une autre save";
                labelAnalyse.ForeColor = Color.Black;
                done = true;
            }
            catch
            {
                buttonReplace.Enabled = false;
                buttonReplace.BackColor = Color.White;
                buttonReplace.Text = "Erreur";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            string name = Path.GetFileNameWithoutExtension(directories[index]);
            pathName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\StardewValley\\Saves\\" + name + "\\" + name;
            pathGame = Path.GetDirectoryName(pathName) + "\\SaveGameInfo";
            docName = XDocument.Load(pathName);
            docGame = XDocument.Load(pathGame);

            string result = mOperation.Analyse(docName, docGame);
            labelAnalyse.Text = result;

            switch (result)
            {
                case "Votre save est en anglais":
                    labelAnalyse.ForeColor = Color.Black;
                    buttonReplace.Enabled = false;
                    buttonReplace.BackColor = Color.White;
                    buttonReplace.Text = "Impossible de corriger";
                    break;
                case "Votre save a besoin d'être corrigé":
                    labelAnalyse.ForeColor = Color.Blue;
                    buttonReplace.Enabled = true;
                    buttonReplace.BackColor = SystemColors.Highlight;
                    buttonReplace.Text = "Corriger";
                    break;
                case "Votre save est compatible":
                    labelAnalyse.ForeColor = Color.Green;
                    buttonReplace.Enabled = false;
                    buttonReplace.BackColor = Color.White;
                    buttonReplace.Text = "Vous pouvez jouer !";
                    break;
            }

            
        }
    }
}
