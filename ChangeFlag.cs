using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Svg;

namespace ChangeLangPhotoshop
{
    public partial class MainForm : Form
    {
        private const string filePath = @"C:\Program Files\Adobe\Adobe Photoshop 2024\Locales\fr_FR\Support Files\tw10428_Photoshop_fr_FR";

        private Button btnRenameToDat;
        private Button btnRenameToBak;
        private FileSystemWatcher watcher;
        private PictureBox picFlag;
        private Label lblLanguage;

        public MainForm()
        {
            InitializeComponent();
            InitializeButtons();
            InitializeFlags();
            InitializeWatcher();
            UpdateUI();
        }

        private void InitializeButtons()
        {
            btnRenameToDat = new Button();
            btnRenameToBak = new Button();

            btnRenameToDat.Location = new Point(130, 260);
            btnRenameToDat.Size = new Size(250, 50);
            btnRenameToDat.BackColor = Color.FromArgb(0, 123, 255);
            btnRenameToDat.ForeColor = Color.White;
            btnRenameToDat.FlatStyle = FlatStyle.Flat;
            btnRenameToDat.Font = new Font("Roboto", 10, FontStyle.Bold);
            btnRenameToDat.Text = "Utiliser Photoshop en français";
            btnRenameToDat.Click += BtnRenameToDat_Click;

            btnRenameToBak.Location = new Point(130, 260);
            btnRenameToBak.Size = new Size(250, 50);
            btnRenameToBak.BackColor = Color.FromArgb(0, 123, 255);
            btnRenameToBak.ForeColor = Color.White;
            btnRenameToBak.FlatStyle = FlatStyle.Flat;
            btnRenameToBak.Font = new Font("Roboto", 10, FontStyle.Bold);
            btnRenameToBak.Text = "Utiliser Photoshop en anglais";
            btnRenameToBak.Click += BtnRenameToBak_Click;

            this.Controls.Add(btnRenameToDat);
            this.Controls.Add(btnRenameToBak);
        }

        private void InitializeWatcher()
        {
            watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath));
            watcher.Filter = Path.GetFileName(filePath) + ".*";
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.Invoke((MethodInvoker)UpdateUI);
        }

        private void UpdateUI()
        {
            bool isFrench = File.Exists(filePath + ".dat");

            picFlag.Image = isFrench ? GetFranceFlagImage() : GetUKFlagImage();

            btnRenameToDat.Visible = !isFrench;
            btnRenameToBak.Visible = isFrench;

            lblLanguage.Text = isFrench ? "Langue actuelle de Photoshop : Français" : "Langue actuelle de Photoshop : Anglais";
        }

        private void BtnRenameToDat_Click(object sender, EventArgs e)
        {
            try
            {
                File.Move(filePath + ".bak", filePath + ".dat");
                MessageBox.Show("Victoire, Photoshop est en Français!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du renommage : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRenameToBak_Click(object sender, EventArgs e)
        {
            try
            {
                File.Move(filePath + ".dat", filePath + ".bak");
                MessageBox.Show("Victory, Photoshop is in English !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du renommage : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       private void InitializeFlags()
{
    lblLanguage = new Label();
    lblLanguage.Text = "Langue actuelle de Photoshop :";
    lblLanguage.AutoSize = true; // Cette propriété permettra au label de s'ajuster automatiquement en fonction de son contenu.
    lblLanguage.Font = new Font("Roboto", 18, FontStyle.Bold);
    lblLanguage.Location = new Point(30, 200); // Ajustement de la position du label
    this.Controls.Add(lblLanguage);

    picFlag = new PictureBox();
    picFlag.SizeMode = PictureBoxSizeMode.StretchImage;
    picFlag.Size = new Size(100, 100); // Augmentation de la hauteur du PictureBox
    picFlag.Location = new Point(200, 50); // Ajustement de la position du PictureBox
    this.Controls.Add(picFlag);
}





        private Image GetFranceFlagImage()
        {
            string svgCode = @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""900"" height=""600""><rect width=""900"" height=""600"" fill=""#ED2939""/><rect width=""600"" height=""600"" fill=""#fff""/><rect width=""300"" height=""600"" fill=""#002395""/></svg>";
            return ImageFromSvgString(svgCode);
        }

        private Image GetUKFlagImage()
        {
            string svgCode = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 50 30"" width=""1000"" height=""600""><clipPath id=""t""><path d=""M25,15h25v15zv15h-25zh-25v-15zv-15h25z""/></clipPath><path d=""M0,0v30h50v-30z"" fill=""#012169""/><path d=""M0,0 50,30M50,0 0,30"" stroke=""#fff"" stroke-width=""6""/><path d=""M0,0 50,30M50,0 0,30"" clip-path=""url(#t)"" stroke=""#C8102E"" stroke-width=""4""/><path d=""M-1,11h22v-12h8v12h22v8h-22v12h-8v-12h-22z"" fill=""#C8102E"" stroke=""#FFF"" stroke-width=""2""/></svg>";
            return ImageFromSvgString(svgCode);
        }

        private Image ImageFromSvgString(string svg)
        {
            var doc = SvgDocument.Open<SvgDocument>(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(svg)));
            var bitmap = new Bitmap((int)doc.Width.Value, (int)doc.Height.Value);
            using (var g = Graphics.FromImage(bitmap))
            {
                doc.Draw(g);
            }
            return bitmap;
        }
    }
}
