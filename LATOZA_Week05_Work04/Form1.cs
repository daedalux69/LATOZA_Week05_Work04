using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LATOZA_Week05_Work04
{
    public partial class Form1 : Form
    {
        private readonly List<string> savedBarcodes = new List<string>();

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            txtBarcode.TextChanged += txtBarcode_TextChanged;
            txtCost.KeyPress += NumericOnly_KeyPress;
            txtPrice.KeyPress += NumericOnly_KeyPress;
            txtCost.TextChanged += CostPrice_TextChanged;
            txtPrice.TextChanged += CostPrice_TextChanged;
            btnSave.Click += btnSave_Click;
            btnClear.Click += btnClear_Click;
            btnCancel.Click += btnCancel_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtDescription.Multiline = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.AcceptsReturn = true;
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Items.Clear();
            cmbCategory.Items.AddRange(new object[]
            {
                "Beverages", "Snacks", "Canned Goods",
                "Personal Care", "Household", "Others"
            });
            txtMarkup.ReadOnly = true;
            txtMarkup.TabStop = false;
            rdoLocal.Checked = true;
            chkActive.Checked = true;
            lblBarcodeStatus.Text = "";
            AcceptButton = btnSave;
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            string code = txtBarcode.Text.Trim();
            if (code.Length == 0)
            {
                lblBarcodeStatus.Text = "";
            }
            else if (savedBarcodes.Contains(code))
            {
                lblBarcodeStatus.Text = "Barcode already exists.";
                lblBarcodeStatus.ForeColor = Color.Red;
            }
            else
            {
                lblBarcodeStatus.Text = "Barcode is available.";
                lblBarcodeStatus.ForeColor = Color.Green;
            }
        }

        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (char.IsControl(e.KeyChar)) return;
            bool isDigit = char.IsDigit(e.KeyChar);
            bool isFirstDot = e.KeyChar == '.' && !box.Text.Contains(".");
            if (!isDigit && !isFirstDot) e.Handled = true;
        }

        private void CostPrice_TextChanged(object sender, EventArgs e)
        {
            decimal cost;
            decimal price;
            if (decimal.TryParse(txtCost.Text, out cost) &&
                decimal.TryParse(txtPrice.Text, out price) && cost > 0)
            {
                txtMarkup.Text = ((price - cost) / cost * 100m).ToString("N2");
            }
            else
            {
                txtMarkup.Text = "";
            }
        }

        private bool ValidateInputs()
        {
            decimal cost;
            decimal price;
            if (string.IsNullOrWhiteSpace(txtBarcode.Text))
                return Fail("Barcode is required.", txtBarcode);
            if (savedBarcodes.Contains(txtBarcode.Text.Trim()))
                return Fail("This barcode already exists.", txtBarcode);
            if (string.IsNullOrWhiteSpace(txtName.Text))
                return Fail("Name is required.", txtName);
            if (cmbCategory.SelectedIndex == -1)
                return Fail("Please select a category.", cmbCategory);
            if (!decimal.TryParse(txtCost.Text, out cost) || cost <= 0)
                return Fail("Enter a valid cost greater than 0.", txtCost);
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
                return Fail("Enter a valid price greater than 0.", txtPrice);
            if (price < cost)
                return Fail("Price cannot be lower than cost.", txtPrice);
            return true;
        }

        private bool Fail(string message, Control focusTarget)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusTarget.Focus();
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            string origin = rdoLocal.Checked ? "Local" : "Abroad";
            string status = chkActive.Checked ? "Active" : "Inactive";
            savedBarcodes.Add(txtBarcode.Text.Trim());
            MessageBox.Show(
                "Product saved!\n\n" +
                "Barcode: " + txtBarcode.Text + "\n" +
                "Name: " + txtName.Text + "\n" +
                "Category: " + cmbCategory.Text + "\n" +
                "Origin: " + origin + "\n" +
                "Status: " + status + "\n" +
                "Cost: " + txtCost.Text + "\n" +
                "Price: " + txtPrice.Text + "\n" +
                "Markup: " + txtMarkup.Text + "%",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Close the form? Unsaved data will be lost.",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) Close();
        }

        private void ClearForm()
        {
            txtBarcode.Clear();
            txtName.Clear();
            txtDescription.Clear();
            cmbCategory.SelectedIndex = -1;
            rdoLocal.Checked = true;
            chkActive.Checked = true;
            txtCost.Clear();
            txtPrice.Clear();
            txtMarkup.Clear();
            lblBarcodeStatus.Text = "";
            txtBarcode.Focus();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
    }
}