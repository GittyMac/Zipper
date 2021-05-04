
namespace Zipper
{
    partial class DebugForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
            this.label1 = new System.Windows.Forms.Label();
            this.MessageBoxButton = new System.Windows.Forms.Button();
            this.TitleText = new System.Windows.Forms.TextBox();
            this.DescriptionText = new System.Windows.Forms.TextBox();
            this.Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "Debug";
            // 
            // MessageBoxButton
            // 
            this.MessageBoxButton.BackColor = System.Drawing.SystemColors.Highlight;
            this.MessageBoxButton.FlatAppearance.BorderSize = 0;
            this.MessageBoxButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MessageBoxButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.MessageBoxButton.Location = new System.Drawing.Point(13, 116);
            this.MessageBoxButton.Name = "MessageBoxButton";
            this.MessageBoxButton.Size = new System.Drawing.Size(125, 35);
            this.MessageBoxButton.TabIndex = 1;
            this.MessageBoxButton.Text = "Test MessageBox";
            this.MessageBoxButton.UseVisualStyleBackColor = false;
            this.MessageBoxButton.Click += new System.EventHandler(this.MessageBoxButton_Click);
            // 
            // TitleText
            // 
            this.TitleText.BackColor = System.Drawing.Color.Gainsboro;
            this.TitleText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TitleText.Location = new System.Drawing.Point(13, 62);
            this.TitleText.Name = "TitleText";
            this.TitleText.Size = new System.Drawing.Size(125, 16);
            this.TitleText.TabIndex = 2;
            this.TitleText.Text = "Title";
            // 
            // DescriptionText
            // 
            this.DescriptionText.BackColor = System.Drawing.Color.Gainsboro;
            this.DescriptionText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DescriptionText.Location = new System.Drawing.Point(13, 91);
            this.DescriptionText.Name = "DescriptionText";
            this.DescriptionText.Size = new System.Drawing.Size(125, 16);
            this.DescriptionText.TabIndex = 3;
            this.DescriptionText.Text = "Description";
            // 
            // Close
            // 
            this.Close.BackColor = System.Drawing.SystemColors.Highlight;
            this.Close.FlatAppearance.BorderSize = 0;
            this.Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Close.Location = new System.Drawing.Point(237, 173);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(72, 35);
            this.Close.TabIndex = 4;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = false;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(321, 220);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.DescriptionText);
            this.Controls.Add(this.TitleText);
            this.Controls.Add(this.MessageBoxButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DebugForm";
            this.Text = "Zipper - Debug";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button MessageBoxButton;
        private System.Windows.Forms.TextBox TitleText;
        private System.Windows.Forms.TextBox DescriptionText;
        private System.Windows.Forms.Button Close;
    }
}