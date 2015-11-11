using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChromeSessionUSB
{
    public partial class DataInputBox : Form
    {
        public DataInputBox(String title, String text, bool password)
        {
            InitializeComponent();
            Text = title;
            textLabel.Text = text;
            inputTextBox.UseSystemPasswordChar = password;
        }

        public String getInputText()
        {
            return inputTextBox.Text;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
