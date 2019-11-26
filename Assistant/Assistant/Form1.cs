using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Speech.Recognition;
using System.Speech.Synthesis;
using Tulpep.NotificationWindow;
using System.IO;
using System.Diagnostics;
using System.Data.SQLite;
using System.Threading;
using Microsoft.VisualBasic;

namespace Assistant
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine h = new SpeechRecognitionEngine();

        SpeechSynthesizer s = new SpeechSynthesizer();

        SQLiteConnection connection = new SQLiteConnection("Data Source=test.dat");     // ("Data Source=database.sqlite;Version=3;New=True;Compress=True;")


        string db_command;
        string db_answer;
        string db_info;
        string db_return;


        /*
         vielleicht noch die Befehle ansagen lassen
         */

        // Hauptcommandos
        string[] main_commands = {      "pause",
                                        "schalte dich aus",
                                        "datum",
                                        "zeit",
                                        "beenden",
                                        "vorlesen"
                                        };


        bool tool_close = false;

        string user;
        string computer;
        /*
         * wirre Gedanken ^^
         * 
         * wie intelligent soll er werden?
         * 
        tb_memory soll noch als Gedächnis benutzt werden 
        wurde erstmal genau so wie die Befehlsdatenbank erstellt
        1. was und wie soll er sich was merken
        2. wie und wann soll er es abrufen
        3. wie soll er es verarbeiten und wie vor allem wann und wie soll er es benutzen
        */


        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            db_create();

            // abfrage ob schon der User angelegt wurde
            // wenn ja dann den Usernamen und den Computernamen holen
            // wenn nicht, dann nach Namen fragen und in den Dateien speichern
            if (System.IO.File.Exists("user.dat"))
            {
                user = User.Get_User_Data("user.dat");
                computer = User.Get_User_Data("computer.dat");
            }
            else
            {
                user = Prompt.ShowDialog("Wie soll ich dich nennen?", "Dein Name?");
                computer = Prompt.ShowDialog("Wie willst du mich nennen?", "Siri, Contana, Computer oder sonst ein Name ...");
                User.Set_User_Data("user.dat", user);
                User.Set_User_Data("computer.dat", computer);
            }

            // Ausgabe des Computernamen 
            this.lbl_computername.Text = computer;







        }






        private void Form1_Load(object sender, EventArgs e)
        {         
            
            Choices commands = new Choices();


            // Computernamen in die Befehlliste hinzufügen 
            comboBox_commands.Items.Add(computer);
            // Computernamen in die Commandoliste hinzufügen damit er angesprochen werden kann
            commands.Add(computer);
            // Hauptcommandos hinzufügen
            commands.Add(main_commands);
            // Hauptcommandos in die Befehlliste hinzufügen 
            foreach (string value in main_commands)
            {
                comboBox_commands.Items.Add(value);
            }
            
            // Commandos aus der Datenbank holen und zu den Command / Befehlsliste hinzufügen
            connection.Open();

            SQLiteCommand cmd = new SQLiteCommand("select command from tb_commands;", connection);
            SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                {
                    commands.Add(reader.GetString(reader.GetOrdinal("command")));                    
                    comboBox_commands.Items.Add(reader.GetString(reader.GetOrdinal("command")));
                }
            connection.Close();

            

            GrammarBuilder gbuilder = new GrammarBuilder();
            gbuilder.Append(commands);
                        
            Grammar grammar = new Grammar(gbuilder);

            //DictationGrammar dgrammar = new DictationGrammar();

            h.LoadGrammar(grammar);
            //h.LoadGrammar(dgrammar);
            h.SetInputToDefaultAudioDevice();
            h.SpeechRecognized += recEngine_SpeechRecognized;

            h.RecognizeAsync(RecognizeMode.Multiple);
            s.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            s.SpeakAsync("hallo "+user+" wie kann ich dir heute helfen");
        }

        private void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {            
            
            string mode = e.Result.Text;
            
            Mode_to_run(mode);



        }
        private void Mode_to_run(string mode)
        {
            db_command_select("tb_commands", mode);


            if (db_return != "nix")
            {

                // vielleicht kann man mit ID und p.start das programm in den Vordergrund rufen             

                if (tool_close == true)
                {
                    my_popup(mode, mode + "wird geschlossen");
                    if (File.Exists(mode))
                    {
                        s.SpeakAsync(mode + "wird geschlossen");
                        StreamReader myFile = new StreamReader(mode, System.Text.Encoding.Default);
                        string P_ID = myFile.ReadToEnd();
                        myFile.Close();

                        int _P_ID = Convert.ToInt32(P_ID);

                        Process P = Process.GetProcessById(_P_ID);
                        P.CloseMainWindow();
                        tool_close = false;
                        File.Delete(mode);
                    }
                }
                else
                {

                    string file = @"" + db_return;
                    string extension;

                    extension = Path.GetExtension(db_return);

                    if (extension == ".exe")
                    {

                        if (File.Exists(mode))
                        {
                            StreamReader myFile = new StreamReader(mode, System.Text.Encoding.Default);
                            string P_ID = myFile.ReadToEnd();
                            myFile.Close();

                            int _P_ID = Convert.ToInt32(P_ID);
                            Process P = Process.GetProcessById(_P_ID);
                            //P.Refresh(); //.Start();
                            BringMainWindowToFront(P.ProcessName);

                            s.SpeakAsync("Dieses Tool wird schon ausgeführt! ");
                        }
                        else
                        {
                            s.SpeakAsync(db_answer);
                            Process P = new Process();
                            P.StartInfo.FileName = file;
                            P.Start();
                            int ID = P.Id;

                            StreamWriter myFile = new StreamWriter(mode);
                            myFile.Write(ID);
                            myFile.Close();
                        }

                        db_return = "nix";
                    }
                    else
                    {
                        MessageBox.Show("Bitte nur Programme der Dateitypen .exe benutzen.");
                        s.SpeakAsync("Ich kann keine ausfürbare Datei finden!");
                    }

                }

            }
            else
            {
                s.SpeakAsync(db_answer);
            }


            Main_command(mode, true);
            Command_info();
        }

        /// <summary>
        /// Hier sind alle Haupt Commandos (Systembefehle) mit der auswahl ob es ausgeführt werden soll oder nur angeseigt werden soll
        /// </summary>
        /// <param name="mode">Der Befehl</param>
        /// <param name="run">true zum Befehl ausführen</param>
        private void Main_command(string mode, bool run)
        {
            

            if (mode == computer) {
                // Antwortvielfalt
                Random r = new Random();
                string[] hallo_computer = new string[4] { user+" was kann ich für dich tun", "ja "+ user, "ja", "jup" };
                
                if (run)
                {

                    s.SpeakAsync(hallo_computer[r.Next(4)]);
                    this.WindowState = FormWindowState.Normal;
                }
                db_command = computer;
                db_answer = hallo_computer[r.Next(4)];
                db_info = "Systembefehl, holt die App wieder in den Vordergrund! Selbstgegebener Name: " + computer;
                db_return = "nix";
            }

           if(mode == "pause") {
                if (run)
                {
                    s.SpeakAsync("bis gleich " + user);
                    this.WindowState = FormWindowState.Minimized;
                }
                db_command = "pause";
                db_answer = "bis gleich " +user;
                db_info = "Systembefehl, minimiert die App";
                db_return = "nix";
            }

           if(mode == "schalte dich aus") {
                if (run)
                {
                    s.Speak("ok "+ user + ", dann bis demnächst");                    
                    Application.Exit();
                }
                db_command = "schalte dich aus";
                db_answer = "ok "+ user + ", dann bis demnächst";
                db_info = "Systembefehl, schließt die App.";
                db_return = "nix";
            }
           if(mode == "datum") {
                if (run)
                {
                    s.SpeakAsync(DateTime.Now.ToString("d"));   // Befehl welcher Tag heute ist, anwort zb Montag
                    my_popup(mode, DateTime.Now.ToString("d"));
                }
                db_command = "datum";
                db_answer = DateTime.Now.ToString("d");
                db_info = "Systembefehl, sagt das aktuelle Datum.";
                db_return = "nix";
            }
           if(mode == "zeit") {
                if (run)
                {
                    s.SpeakAsync(DateTime.Now.ToString("HH:mm"));
                    my_popup(mode, DateTime.Now.ToString("HH:mm"));
                }
                db_command = "zeit";
                db_answer = DateTime.Now.ToString("HH:mm");
                db_info = "Systembefehl, sagt die Uhrzeit an.";
                db_return = "nix";
            }
           if(mode == "beenden") {
                if (run)
                {
                    s.SpeakAsync("Welches Programm "+ user + "?");
                    tool_close = true;
                    db_command = "";
                    db_answer = "";
                    db_info = "";
                    db_return = "";
                }
                db_command = "beenden";
                db_answer = "Welches Programm?";
                db_info = "Systembefehl, 'beenden' und danach den Befehl (mit ganz kurzer pause) des offenen Programmes, zum beenden. ";
                db_return = "nix";
            }
            if(mode == "vorlesen"){
                if (run)
                {
                    string zwischenablage = null;
                    if (Clipboard.ContainsText())
                    {
                        zwischenablage = Clipboard.GetText();
                        my_popup(mode, zwischenablage);
                        s.SpeakAsync(zwischenablage);
                    }
                }
                db_command = "vorlesen";
                db_answer = "";
                db_info = "Systembefehl, liest den Text aus der Zwischenablage vor.";
                db_return = "nix";
            }

            






        }

        /// <summary>
        /// zeigt im Fenster ausgewählten Befehl mit Antwort, Info und was ans System zurückgegeben wird wie Pfad eines Programmes
        /// </summary>
        private void Command_info()
        {            
            tb_show_speak.Text = db_command;
            tb_show_answer.Text = db_answer;
            tb_show_info.Text = db_info;
            tb_show_return.Text = db_return;
        }
        

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lbl_time.Text = DateTime.Now.ToLongTimeString();
            lbl_date.Text = DateTime.Now.ToLongDateString();
        }
        private void my_popup(string titel, string context)
        {
            PopupNotifier popup = new PopupNotifier();

            popup.TitleText = "Befehl: " + titel;
            popup.ContentText = context;
            popup.Popup();
        }

        /// <summary>
        /// erstellt die Datenbanktabellen falls sie noch nicht existieren
        /// </summary>
        private void db_create()
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = String.Format("create table if not exists {0} (" +
                                                "  ID integer not null primary key autoincrement," +
                                                "  command varchar(100) not null," +
                                                "  answer varchar(100) not null," +
                                                "  info varchar(250) not null," +
                                                "  return varchar(100) not null)",
                                                "tb_commands");
            command.ExecuteNonQuery();

            command.CommandText = String.Format("create table if not exists {0} (" +
                                                "  ID integer not null primary key autoincrement," +
                                                "  command varchar(100) not null," +
                                                "  answer varchar(100) not null," +
                                                "  info varchar(250) not null," +
                                                "  return varchar(100) not null)",
                                                "tb_memory");
            command.ExecuteNonQuery();

            connection.Close();
        }
        /// <summary>
        /// Speichert die Kommandos in die Datenbank
        /// </summary>
        /// <param name="table">tb_commands oder tb_memory</param>
        /// <param name="_command"></param>
        /// <param name="_answer"></param>
        /// <param name="_info"></param>
        /// <param name="_return"></param>
        private void db_command_insert(string table, string _command, string _answer, string _info, string _return)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = String.Format("insert into "+table+" (command, answer, info, return) values ('{0}','{1}','{2}','{3}')",
                                                _command,
                                                _answer,
                                                _info,
                                                _return);
            command.ExecuteNonQuery();

            

            connection.Close();
        }

        private void db_command_update(string table, string _command, string _answer, string _info, string _return)
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = String.Format("update " + table + " set answer = '{0}', info = '{1}', return = '{2}'  where command = '{3}';",
                                                _answer,
                                                _info,
                                                _return,
                                                _command);
            command.ExecuteNonQuery();



            connection.Close();
        }

        private void db_command_select(string table, string search)
        {
            connection.Open();

            SQLiteCommand cmd = new SQLiteCommand("select * from "+table+" where command ='"+search+"';" , connection);
            SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    db_command = reader.GetString(reader.GetOrdinal("command"));
                    db_answer = reader.GetString(reader.GetOrdinal("answer"));
                    db_info = reader.GetString(reader.GetOrdinal("info"));
                    db_return = reader.GetString(reader.GetOrdinal("return"));
                    //MessageBox.Show(string.Format("{0}, {1}, {2}, {3}", db_command, db_answer, db_info, db_return));
                }
            }
            else
            {
                db_command = search;
                db_answer = "";
                db_info = "";
                db_return = "nix";
            }

            Command_info();

            connection.Close();
        }

        private void comboBox_commands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox_commands.SelectedIndex != -1)
            {
                string curItem = comboBox_commands.SelectedItem.ToString();
                Main_command(curItem, false);
                db_command_select("tb_commands", curItem);
                
                

                this.btn_command_save.Visible = true;
                this.btn_delete.Visible = true;

                foreach (string value in main_commands)
                {
                    if(value == curItem)
                    {
                        this.btn_command_save.Visible = false;
                        this.btn_delete.Visible = false;
                    }
                }
                Main_command(curItem, false);


                Command_info();


                
            }
        }

        private void btn_command_save_Click(object sender, EventArgs e)
        {
            if (tb_show_return.Text == "")
            {
                this.tb_show_return.Text = "nix";
            }
            for (int i = 0; i <= comboBox_commands.Items.Count - 1; i++)
            {


                if (comboBox_commands.Items[i].ToString() == tb_show_speak.Text)
                {


                    s.SpeakAsync(comboBox_commands.Items[i].ToString() + " habe ich schon als Befehl in meiner Datenbank. Wenn ich die Änderungen speichern soll, dann klicke bitte auf ja.");
                    //MessageBox.Show(comboBox_commands.Items[i].ToString() + " habe ich schon als Befehl in meiner Datenbank");

                    string message = '"'+comboBox_commands.Items[i].ToString() + '"' + " habe ich schon als Befehl in meiner Datenbank! /nUpdate ausführen?";
                    string caption = "Befehl updaten?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    result = MessageBox.Show(this, message, caption, buttons);

                    if (result == DialogResult.Yes)
                    {
                        db_command_update("tb_commands", this.tb_show_speak.Text, this.tb_show_answer.Text, this.tb_show_info.Text, this.tb_show_return.Text);
                        s.Speak("Deine Änderungen wurden gespeichert");


                    }
                    else
                    {
                        s.SpeakAsync("OK, es wurde nichts verändert.");
                    }
                    
                    return;
                }
            }

            

            db_command_insert("tb_commands", this.tb_show_speak.Text, this.tb_show_answer.Text, this.tb_show_info.Text, this.tb_show_return.Text);

            s.Speak("gespeichert, ich muss mich jetzt neustarten, damit der neue Befehl verfügbar ist");
            //Thread.Sleep(55000);
            Application.Restart();

        }





        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);
        private enum ShowWindowEnum
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };
        public void BringMainWindowToFront(string processName)
        {
            //get the process
            Process bProcess = Process.GetProcessesByName(processName).FirstOrDefault();
            //check if the process is running
            if (bProcess != null)
            {
                //check if the window is hidden /minimized
                if (bProcess.MainWindowHandle == IntPtr.Zero)
                {
                    //the window is hidden so try to restore it before setting focus.
                    ShowWindow(bProcess.Handle, ShowWindowEnum.Restore);
                    MessageBox.Show("bin da");
                }
                //set user the focus to the window
                SetForegroundWindow(bProcess.MainWindowHandle);
            }
            
        }

        private void btn_go_Click(object sender, EventArgs e)
        {
            if (comboBox_commands.SelectedIndex != -1)
            {
                string mode = comboBox_commands.SelectedItem.ToString();

                Mode_to_run(mode);


            }

            
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (comboBox_commands.SelectedIndex != -1)
            {
                string curItem = comboBox_commands.SelectedItem.ToString();
                                
                this.btn_delete.Visible = true;

                foreach (string value in main_commands)
                {
                    if (value == curItem)
                    {
                        // Systembefehl, kann nicht gelöscht werden
                    }
                    else
                    {
                        if (curItem == tb_show_speak.Text)
                        {


                            s.SpeakAsync(curItem + " aus meiner Datenbank löschen? ");
                            

                            string message = '"' + curItem + '"' + " aus meiner Datenbank löschen? Wenn ich das wirklich machen soll, dann klicke bitte auf ja.";
                            string caption = "Befehl updaten?";
                            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                            DialogResult result;

                            result = MessageBox.Show(this, message, caption, buttons);

                            if (result == DialogResult.Yes)
                            {
                                connection.Open();
                                 
                                SQLiteCommand command = new SQLiteCommand("delete from tb_commands where command ='" + curItem + "';", connection);
                                command.ExecuteNonQuery();

                                Command_info();

                                connection.Close();
                                //s.SpeakAsync("Der Befehl "+ curItem+" wurde aus der Datenbank gelöscht.");
                                s.Speak("Der Befehl " + curItem + " wurde aus der Datenbank gelöscht.");
                                //Thread.Sleep(4500);
                                s.Speak("ich muss mich jetzt neustarten, damit der alte Befehl aus dem System raus ist");
                                //Thread.Sleep(5000);
                                Application.Restart();


                            }
                            else
                            {
                                s.SpeakAsync("OK, es wurde nichts gelöscht.");
                            }

                            return;
                        }
                        else
                        {
                            s.SpeakAsync("Dieser Befehl stimmt nicht mit der Befehlsliste überein. Bitte neu auswählen.");
                        }
                    }
                }
                



            }
        }
    }
    
    public static class Prompt
    {
        /// <summary>
        /// erstellt ein Eingabefenster
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns>den eingegebenen Wert</returns>
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Width = 200, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

    public static class User
    {
        public static string Get_User_Data(string text)
        {
            if (System.IO.File.Exists(text))
            {
                StreamReader datafile = new StreamReader(text, System.Text.Encoding.Default);
                string data = datafile.ReadToEnd();
                datafile.Close();
                return data;
            }
            else
            {
                return "computer";
            }
            
        }

        public static void Set_User_Data(string name, string content)
        {
            if (!System.IO.File.Exists(name))
            {
                StreamWriter datafile = new StreamWriter(name);
                datafile.Write(content);
                datafile.Close();
                
            }
            
        }
    }
}
