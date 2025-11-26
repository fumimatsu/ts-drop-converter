using System.Drawing;
using System.Windows.Forms;

namespace TsToMp4Dropper;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.logTextBox = new System.Windows.Forms.TextBox();
        this.SuspendLayout();
        // 
        // logTextBox
        // 
        this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
        this.logTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.logTextBox.Location = new System.Drawing.Point(0, 0);
        this.logTextBox.Multiline = true;
        this.logTextBox.Name = "logTextBox";
        this.logTextBox.ReadOnly = true;
        this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.logTextBox.Size = new System.Drawing.Size(800, 450);
        this.logTextBox.TabIndex = 0;
        this.logTextBox.WordWrap = false;
        // 
        // Form1
        // 
        this.AllowDrop = true;
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Controls.Add(this.logTextBox);
        this.MinimumSize = new System.Drawing.Size(500, 300);
        this.Name = "Form1";
        this.Text = "TS → MP4 Dropper";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private TextBox logTextBox;
}
