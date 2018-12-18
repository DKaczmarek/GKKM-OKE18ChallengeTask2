namespace OKE2018_Challenge___Task_2
{
    partial class Main_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.OutputTextbox = new System.Windows.Forms.TextBox();
            this.Btn_Parse = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Btn_FileOpen = new System.Windows.Forms.Button();
            this.BtnSaveResults = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // InputTextBox
            // 
            this.InputTextBox.Location = new System.Drawing.Point(3, 3);
            this.InputTextBox.Multiline = true;
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(532, 149);
            this.InputTextBox.TabIndex = 0;
            // 
            // OutputTextbox
            // 
            this.OutputTextbox.Enabled = false;
            this.OutputTextbox.Location = new System.Drawing.Point(3, 190);
            this.OutputTextbox.Multiline = true;
            this.OutputTextbox.Name = "OutputTextbox";
            this.OutputTextbox.Size = new System.Drawing.Size(532, 158);
            this.OutputTextbox.TabIndex = 1;
            // 
            // Btn_Parse
            // 
            this.Btn_Parse.Location = new System.Drawing.Point(3, 3);
            this.Btn_Parse.Name = "Btn_Parse";
            this.Btn_Parse.Size = new System.Drawing.Size(75, 20);
            this.Btn_Parse.TabIndex = 2;
            this.Btn_Parse.Text = "Parse";
            this.Btn_Parse.UseVisualStyleBackColor = true;
            this.Btn_Parse.Click += new System.EventHandler(this.Btn_Parse_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.16805F));
            this.tableLayoutPanel1.Controls.Add(this.InputTextBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.OutputTextbox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.82829F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.17172F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(538, 380);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel2.Controls.Add(this.Btn_Parse, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.Btn_FileOpen, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.BtnSaveResults, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 158);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(263, 26);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // Btn_FileOpen
            // 
            this.Btn_FileOpen.Location = new System.Drawing.Point(91, 3);
            this.Btn_FileOpen.Name = "Btn_FileOpen";
            this.Btn_FileOpen.Size = new System.Drawing.Size(75, 20);
            this.Btn_FileOpen.TabIndex = 3;
            this.Btn_FileOpen.Text = "Open File";
            this.Btn_FileOpen.UseVisualStyleBackColor = true;
            this.Btn_FileOpen.Click += new System.EventHandler(this.Btn_FileOpen_Click);
            // 
            // BtnSaveResults
            // 
            this.BtnSaveResults.Location = new System.Drawing.Point(179, 3);
            this.BtnSaveResults.Name = "BtnSaveResults";
            this.BtnSaveResults.Size = new System.Drawing.Size(75, 20);
            this.BtnSaveResults.TabIndex = 4;
            this.BtnSaveResults.Text = "Save results";
            this.BtnSaveResults.UseVisualStyleBackColor = true;
            this.BtnSaveResults.Click += new System.EventHandler(this.BtnSaveResults_Click);
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 405);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Main_Form";
            this.Text = "Parser";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.TextBox OutputTextbox;
        private System.Windows.Forms.Button Btn_Parse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button Btn_FileOpen;
        private System.Windows.Forms.Button BtnSaveResults;
    }
}

