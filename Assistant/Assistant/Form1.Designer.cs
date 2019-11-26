namespace Assistant
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbl_date = new System.Windows.Forms.Label();
            this.lbl_time = new System.Windows.Forms.Label();
            this.tb_show_speak = new System.Windows.Forms.TextBox();
            this.comboBox_commands = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_show_info = new System.Windows.Forms.TextBox();
            this.tb_show_answer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_show_return = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_command_save = new System.Windows.Forms.Button();
            this.btn_go = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_computername = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // lbl_date
            // 
            this.lbl_date.AutoSize = true;
            this.lbl_date.Location = new System.Drawing.Point(12, 9);
            this.lbl_date.Name = "lbl_date";
            this.lbl_date.Size = new System.Drawing.Size(44, 13);
            this.lbl_date.TabIndex = 0;
            this.lbl_date.Text = "lbl_date";
            // 
            // lbl_time
            // 
            this.lbl_time.AutoSize = true;
            this.lbl_time.Location = new System.Drawing.Point(164, 9);
            this.lbl_time.Name = "lbl_time";
            this.lbl_time.Size = new System.Drawing.Size(42, 13);
            this.lbl_time.TabIndex = 1;
            this.lbl_time.Text = "lbl_time";
            // 
            // tb_show_speak
            // 
            this.tb_show_speak.Location = new System.Drawing.Point(277, 93);
            this.tb_show_speak.Name = "tb_show_speak";
            this.tb_show_speak.Size = new System.Drawing.Size(254, 20);
            this.tb_show_speak.TabIndex = 2;
            // 
            // comboBox_commands
            // 
            this.comboBox_commands.FormattingEnabled = true;
            this.comboBox_commands.Location = new System.Drawing.Point(12, 93);
            this.comboBox_commands.Name = "comboBox_commands";
            this.comboBox_commands.Size = new System.Drawing.Size(157, 21);
            this.comboBox_commands.TabIndex = 3;
            this.comboBox_commands.SelectedIndexChanged += new System.EventHandler(this.comboBox_commands_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(228, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Befehl: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Info: ";
            // 
            // tb_show_info
            // 
            this.tb_show_info.Location = new System.Drawing.Point(277, 145);
            this.tb_show_info.Multiline = true;
            this.tb_show_info.Name = "tb_show_info";
            this.tb_show_info.Size = new System.Drawing.Size(254, 100);
            this.tb_show_info.TabIndex = 6;
            // 
            // tb_show_answer
            // 
            this.tb_show_answer.Location = new System.Drawing.Point(277, 119);
            this.tb_show_answer.Name = "tb_show_answer";
            this.tb_show_answer.Size = new System.Drawing.Size(254, 20);
            this.tb_show_answer.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Antwort: ";
            // 
            // tb_show_return
            // 
            this.tb_show_return.Location = new System.Drawing.Point(277, 251);
            this.tb_show_return.Name = "tb_show_return";
            this.tb_show_return.Size = new System.Drawing.Size(254, 20);
            this.tb_show_return.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(222, 254);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Return: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "gespeicherte Befehle: ";
            // 
            // btn_command_save
            // 
            this.btn_command_save.Location = new System.Drawing.Point(456, 277);
            this.btn_command_save.Name = "btn_command_save";
            this.btn_command_save.Size = new System.Drawing.Size(75, 23);
            this.btn_command_save.TabIndex = 12;
            this.btn_command_save.Text = "speichern";
            this.btn_command_save.UseVisualStyleBackColor = true;
            this.btn_command_save.Click += new System.EventHandler(this.btn_command_save_Click);
            // 
            // btn_go
            // 
            this.btn_go.Location = new System.Drawing.Point(50, 148);
            this.btn_go.Name = "btn_go";
            this.btn_go.Size = new System.Drawing.Size(75, 63);
            this.btn_go.TabIndex = 13;
            this.btn_go.Text = "START";
            this.btn_go.UseVisualStyleBackColor = true;
            this.btn_go.Click += new System.EventHandler(this.btn_go_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(375, 277);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(75, 23);
            this.btn_delete.TabIndex = 14;
            this.btn_delete.Text = "löschen";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Du gabst mir den Namen: ";
            // 
            // lbl_computername
            // 
            this.lbl_computername.AutoSize = true;
            this.lbl_computername.Location = new System.Drawing.Point(145, 41);
            this.lbl_computername.Name = "lbl_computername";
            this.lbl_computername.Size = new System.Drawing.Size(35, 13);
            this.lbl_computername.TabIndex = 16;
            this.lbl_computername.Text = "label7";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 400);
            this.Controls.Add(this.lbl_computername);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.btn_go);
            this.Controls.Add(this.btn_command_save);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_show_return);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_show_answer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_show_info);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_commands);
            this.Controls.Add(this.tb_show_speak);
            this.Controls.Add(this.lbl_time);
            this.Controls.Add(this.lbl_date);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lbl_date;
        private System.Windows.Forms.Label lbl_time;
        private System.Windows.Forms.TextBox tb_show_speak;
        private System.Windows.Forms.ComboBox comboBox_commands;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_show_info;
        private System.Windows.Forms.TextBox tb_show_answer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_show_return;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_command_save;
        private System.Windows.Forms.Button btn_go;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_computername;
    }
}

