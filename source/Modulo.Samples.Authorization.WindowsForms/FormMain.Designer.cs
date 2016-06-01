namespace Modulo.Samples.Authorization.WindowsForms
{
    partial class FormMain
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxClientId = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.callbackUrlTextBox = new System.Windows.Forms.TextBox();
            this.textBoxClientSecret = new System.Windows.Forms.TextBox();
            this.textBoxMyLogin = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.31533F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.68467F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1037, 577);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxClientId, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.callbackUrlTextBox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.textBoxClientSecret, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxMyLogin, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.loginButton, 0, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(297, 571);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // textBoxClientId
            // 
            this.textBoxClientId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxClientId.Location = new System.Drawing.Point(3, 3);
            this.textBoxClientId.Name = "textBoxClientId";
            this.textBoxClientId.Size = new System.Drawing.Size(291, 20);
            this.textBoxClientId.TabIndex = 0;
            this.textBoxClientId.Text = "3a998f2dc04a46d9a85312164b048e55";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(3, 123);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 1;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // callbackUrlTextBox
            // 
            this.callbackUrlTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.callbackUrlTextBox.Location = new System.Drawing.Point(3, 63);
            this.callbackUrlTextBox.Name = "callbackUrlTextBox";
            this.callbackUrlTextBox.Size = new System.Drawing.Size(291, 20);
            this.callbackUrlTextBox.TabIndex = 0;
            this.callbackUrlTextBox.Text = "http://localhost/app";
            // 
            // textBoxClientSecret
            // 
            this.textBoxClientSecret.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxClientSecret.Location = new System.Drawing.Point(3, 33);
            this.textBoxClientSecret.Name = "textBoxClientSecret";
            this.textBoxClientSecret.Size = new System.Drawing.Size(291, 20);
            this.textBoxClientSecret.TabIndex = 2;
            this.textBoxClientSecret.Text = "f877dfe7752c44318d2c5dd4a74eb501";
            // 
            // textBoxMyLogin
            // 
            this.textBoxMyLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMyLogin.Location = new System.Drawing.Point(3, 93);
            this.textBoxMyLogin.Name = "textBoxMyLogin";
            this.textBoxMyLogin.Size = new System.Drawing.Size(291, 20);
            this.textBoxMyLogin.TabIndex = 3;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 577);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBoxClientId;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox callbackUrlTextBox;
        private System.Windows.Forms.TextBox textBoxClientSecret;
        private System.Windows.Forms.TextBox textBoxMyLogin;
    }
}

