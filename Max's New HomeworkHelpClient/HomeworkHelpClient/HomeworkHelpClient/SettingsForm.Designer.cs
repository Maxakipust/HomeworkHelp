namespace HomeworkHelpClient
{
    partial class SettingsForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.generalPage = new System.Windows.Forms.TabPage();
            this.schoolPages = new System.Windows.Forms.TabPage();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.schoolCombo = new System.Windows.Forms.ComboBox();
            this.classesPage = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.classesAppBtn = new System.Windows.Forms.Button();
            this.ClassesOKbtn = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.schoolPages.SuspendLayout();
            this.classesPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.generalPage);
            this.tabControl1.Controls.Add(this.schoolPages);
            this.tabControl1.Controls.Add(this.classesPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(636, 547);
            this.tabControl1.TabIndex = 0;
            // 
            // generalPage
            // 
            this.generalPage.Location = new System.Drawing.Point(4, 25);
            this.generalPage.Name = "generalPage";
            this.generalPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalPage.Size = new System.Drawing.Size(628, 518);
            this.generalPage.TabIndex = 0;
            this.generalPage.Text = "General";
            this.generalPage.UseVisualStyleBackColor = true;
            // 
            // schoolPages
            // 
            this.schoolPages.Controls.Add(this.ApplyBtn);
            this.schoolPages.Controls.Add(this.OkBtn);
            this.schoolPages.Controls.Add(this.label1);
            this.schoolPages.Controls.Add(this.schoolCombo);
            this.schoolPages.Location = new System.Drawing.Point(4, 25);
            this.schoolPages.Name = "schoolPages";
            this.schoolPages.Padding = new System.Windows.Forms.Padding(3);
            this.schoolPages.Size = new System.Drawing.Size(628, 518);
            this.schoolPages.TabIndex = 1;
            this.schoolPages.Text = "School";
            this.schoolPages.UseVisualStyleBackColor = true;
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Location = new System.Drawing.Point(469, 485);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(75, 27);
            this.ApplyBtn.TabIndex = 3;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(550, 485);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 27);
            this.OkBtn.TabIndex = 2;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(237, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose your School";
            // 
            // schoolCombo
            // 
            this.schoolCombo.FormattingEnabled = true;
            this.schoolCombo.Location = new System.Drawing.Point(231, 242);
            this.schoolCombo.Name = "schoolCombo";
            this.schoolCombo.Size = new System.Drawing.Size(150, 24);
            this.schoolCombo.TabIndex = 0;
            // 
            // classesPage
            // 
            this.classesPage.Controls.Add(this.classesAppBtn);
            this.classesPage.Controls.Add(this.ClassesOKbtn);
            this.classesPage.Controls.Add(this.button1);
            this.classesPage.Location = new System.Drawing.Point(4, 25);
            this.classesPage.Name = "classesPage";
            this.classesPage.Size = new System.Drawing.Size(628, 518);
            this.classesPage.TabIndex = 2;
            this.classesPage.Text = "Classes";
            this.classesPage.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(257, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add Class";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // classesAppBtn
            // 
            this.classesAppBtn.Location = new System.Drawing.Point(465, 487);
            this.classesAppBtn.Name = "classesAppBtn";
            this.classesAppBtn.Size = new System.Drawing.Size(75, 27);
            this.classesAppBtn.TabIndex = 5;
            this.classesAppBtn.Text = "Apply";
            this.classesAppBtn.UseVisualStyleBackColor = true;
            this.classesAppBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // ClassesOKbtn
            // 
            this.ClassesOKbtn.Location = new System.Drawing.Point(546, 487);
            this.ClassesOKbtn.Name = "ClassesOKbtn";
            this.ClassesOKbtn.Size = new System.Drawing.Size(75, 27);
            this.ClassesOKbtn.TabIndex = 4;
            this.ClassesOKbtn.Text = "OK";
            this.ClassesOKbtn.UseVisualStyleBackColor = true;
            this.ClassesOKbtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 551);
            this.Controls.Add(this.tabControl1);
            this.Name = "SettingsForm";
            this.Text = "Homework Help Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.schoolPages.ResumeLayout(false);
            this.schoolPages.PerformLayout();
            this.classesPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage generalPage;
        private System.Windows.Forms.TabPage schoolPages;
        private System.Windows.Forms.TabPage classesPage;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox schoolCombo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button classesAppBtn;
        private System.Windows.Forms.Button ClassesOKbtn;
    }
}