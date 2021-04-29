namespace RMays.ChessEncode
{
    partial class Form1
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
            this.txtPlaintext = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBase64 = new System.Windows.Forms.TextBox();
            this.txtBigNumerator = new System.Windows.Forms.TextBox();
            this.txtDecimal = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBigDenominator = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkAutoUpdate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtPlaintext
            // 
            this.txtPlaintext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPlaintext.Location = new System.Drawing.Point(204, 15);
            this.txtPlaintext.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPlaintext.Multiline = true;
            this.txtPlaintext.Name = "txtPlaintext";
            this.txtPlaintext.Size = new System.Drawing.Size(575, 155);
            this.txtPlaintext.TabIndex = 0;
            this.txtPlaintext.TextChanged += new System.EventHandler(this.txtPlaintext_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Plaintext:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 186);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Base64:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 466);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Decimal (0 to 1)";
            // 
            // txtBase64
            // 
            this.txtBase64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBase64.Location = new System.Drawing.Point(204, 182);
            this.txtBase64.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBase64.Multiline = true;
            this.txtBase64.Name = "txtBase64";
            this.txtBase64.Size = new System.Drawing.Size(575, 155);
            this.txtBase64.TabIndex = 8;
            this.txtBase64.TextChanged += new System.EventHandler(this.txtBase64_TextChanged);
            // 
            // txtBigNumerator
            // 
            this.txtBigNumerator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBigNumerator.Location = new System.Drawing.Point(204, 348);
            this.txtBigNumerator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBigNumerator.Multiline = true;
            this.txtBigNumerator.Name = "txtBigNumerator";
            this.txtBigNumerator.Size = new System.Drawing.Size(575, 46);
            this.txtBigNumerator.TabIndex = 10;
            this.txtBigNumerator.TextChanged += new System.EventHandler(this.txtBigNumerator_TextChanged);
            // 
            // txtDecimal
            // 
            this.txtDecimal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDecimal.Location = new System.Drawing.Point(204, 462);
            this.txtDecimal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDecimal.Multiline = true;
            this.txtDecimal.Name = "txtDecimal";
            this.txtDecimal.Size = new System.Drawing.Size(575, 155);
            this.txtDecimal.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 352);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "BigFraction numerator";
            // 
            // txtBigDenominator
            // 
            this.txtBigDenominator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBigDenominator.Location = new System.Drawing.Point(204, 405);
            this.txtBigDenominator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBigDenominator.Multiline = true;
            this.txtBigDenominator.Name = "txtBigDenominator";
            this.txtBigDenominator.Size = new System.Drawing.Size(575, 46);
            this.txtBigDenominator.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 409);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(183, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "BigFraction denominator";
            // 
            // chkAutoUpdate
            // 
            this.chkAutoUpdate.AutoSize = true;
            this.chkAutoUpdate.Location = new System.Drawing.Point(22, 492);
            this.chkAutoUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkAutoUpdate.Name = "chkAutoUpdate";
            this.chkAutoUpdate.Size = new System.Drawing.Size(122, 24);
            this.chkAutoUpdate.TabIndex = 15;
            this.chkAutoUpdate.Text = "AutoUpdate";
            this.chkAutoUpdate.UseVisualStyleBackColor = true;
            this.chkAutoUpdate.CheckedChanged += new System.EventHandler(this.chkAutoUpdate_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 635);
            this.Controls.Add(this.chkAutoUpdate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBigDenominator);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDecimal);
            this.Controls.Add(this.txtBigNumerator);
            this.Controls.Add(this.txtBase64);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPlaintext);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPlaintext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBase64;
        private System.Windows.Forms.TextBox txtBigNumerator;
        private System.Windows.Forms.TextBox txtDecimal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBigDenominator;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkAutoUpdate;
    }
}

