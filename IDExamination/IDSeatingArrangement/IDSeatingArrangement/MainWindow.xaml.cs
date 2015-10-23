using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Net.Mime;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Diagnostics;

namespace IDSeatingArrangement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Course> course = new List<Course>();
        Dictionary<string, int> Rooms = new Dictionary<string, int>();
        List<string> courses = new List<string>();
        string file1 = "";
        Boolean[] Enabled = { true,false,true,true,false,false};
        string WorkingFolder = "";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void instantiateRooms()
        {
            Rooms.Clear();
            Rooms.Add("F102", 93);
            Rooms.Add("F103", 42);
            Rooms.Add("F104", 42);
            Rooms.Add("F105", 93);
            Rooms.Add("F106", 48);
            Rooms.Add("F107", 53);
            Rooms.Add("F108", 54);
            Rooms.Add("F109", 36);
            Rooms.Add("F201", 28);
            Rooms.Add("F202", 27);
            Rooms.Add("F203", 27);
            Rooms.Add("F204", 27);
            Rooms.Add("G101", 40);
            Rooms.Add("G102", 37);
            Rooms.Add("G103", 37);
            Rooms.Add("G104", 37);
            Rooms.Add("G105", 37);
            Rooms.Add("G106", 37);
            Rooms.Add("G107", 37);
            Rooms.Add("G108", 42);
            Rooms.Add("G201", 40);
            Rooms.Add("G202", 37);
            Rooms.Add("G203", 37);
            Rooms.Add("G204", 37);
            Rooms.Add("G205", 37);
            Rooms.Add("G206", 37);
            Rooms.Add("G207", 37);
            Rooms.Add("G208", 42);
            Rooms.Add("B307", 28);
            Rooms.Add("B308", 28);
            Rooms.Add("B309", 38);
            Rooms.Add("B310", 49);


            Rooms.Add("B211", 48);
            Rooms.Add("F101", 48);

        }
        private void backgroundInitializeTask()
        {
            string file = txtTimingsFile.Text;
            if (!File.Exists(file)) { txtStatus.Text = "Timings file doesnot exist in the following location: " + file; return; }
            StreamReader File1 = new StreamReader(file);
            string line = "", date = "";
            try
            {
                while ((line = File1.ReadLine()) != null)
                {
                    string[] ar = line.Split(',');
                    if (ar[0].Equals("\"CATS "))
                        break;
                    else if (ar[1].Equals(""))
                    {
                        date = ar[0];
                        line = File1.ReadLine();
                        line = File1.ReadLine();
                        ar = line.Split(',');
                    }
                    Course stu = new Course(ar[1], ar[2]);
                    if (!courses.Contains(ar[1] + " " + ar[2]))
                    {
                        stu.T1Date = date; stu.T1Time = ar[0].Replace("--", "-").Replace(" ", string.Empty).Trim();
                        course.Add(stu); courses.Add(ar[1] + " " + ar[2]);
                    }
                    else
                    {
                        int x1 = courses.IndexOf(ar[1] + " " + ar[2]);
                        course[x1].T2Date = date;
                        course[x1].T2Time = ar[0].Replace("--", "-").Replace(" ", string.Empty).Trim();
                        continue;
                    }
                    if (ar[3].Equals("")) { stu.Rooms.Add("N/A"); continue; }
                    int x = line.IndexOf(',');
                    x = line.IndexOf(',', x + 1);
                    x = line.IndexOf(',', x + 1);
                    int y = line.IndexOf(",,,");
                    string[] rooms = line.Substring(x + 1, y - x - 1).Replace("\"", string.Empty).Split(',');
                    foreach (string room in rooms)
                    {
                        stu.Rooms.Add(room.Trim()); if (!Rooms.ContainsKey(room.Trim()))
                        { }
                    }
                }
            }
            catch (Exception e)
            {
            }
            File1.Close();
            txtStatus.Text = "Timings file parsed successfully !!!";
            file = txtStudentsFile.Text;
            if (!File.Exists(file)) { txtStatus.Text = "Students file doesnot exist in the following location: " + file; return; }
            StreamReader File2 = new StreamReader(file);
            line = File2.ReadLine(); line = File2.ReadLine();
            try
            {
                while ((line = File2.ReadLine()) != null)
                {
                    string[] ar = line.Split(',');
                    int x1 = courses.IndexOf(ar[3].Trim() + " " + ar[4].Trim());
                    if (x1 == -1)
                        continue;
                    Course stu = course[x1];
                    stu.IDs.Add(ar[1]);
                }
            }
            catch (Exception e)
            {
            }
            File2.Close();
            txtStatus.Text = "Students file parsed successfully !!!";
            file = txtImageFile.Text;
            if (!File.Exists(file)) { txtStatus.Text = "Image file doesnot exist in the following location: " + file; return; }
            txtStatus.Text = "Image file parsed successfully !!!";
            btnGenerate.IsEnabled = true;
        }
        private void btnInitialize_Click(object sender, RoutedEventArgs e)
        {
            instantiateRooms();
            course.Clear();
            courses.Clear();
            DisableAll();
            backgroundInitializeTask();
            Enabled[1] = true;
            EnableAll();
        }
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WorkingFolder.Equals(""))
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    DialogResult result = fbd.ShowDialog();
                    WorkingFolder = fbd.SelectedPath;
                }
                StreamWriter writer = new StreamWriter(WorkingFolder + "/SeatingArrangement.csv");
                StreamWriter writer1 = new StreamWriter(WorkingFolder + "/StudentsSeating.id");
                file1 = WorkingFolder + "/StudentsSeating.id";
                writer.WriteLine("Course Code,Date,Time,Room No.,Strength,IDs From,IDs To");
                foreach (Course stu in course)
                {
                    try
                    {
                        int x = 0, index = 0;
                        if (stu.Rooms.Count == 1)
                        {
                            try
                            {
                                writer.WriteLine(stu.courseCode + " " + stu.courseNo + ",\"" + stu.T1Date + "," + stu.T2Date + "\"," + stu.T1Time + "," + stu.Rooms[0] + "," + stu.IDs.Count + ",ALL THE STUDENTS");
                                string students = "";
                                foreach(string ids in stu.IDs)
                                { students = students + ids + ":"; }
                                writer1.WriteLine(stu.courseCode + " " + stu.courseNo + "," + stu.T1Date + "," + stu.T2Date + "," + stu.T1Time + "," + stu.Rooms[0] + "," + students);
                            }
                            catch { }
                        }
                        else
                        {
                            foreach (string room in stu.Rooms)
                            {
                                if (index > stu.IDs.Count) break;
                                if (x == 0)
                                {
                                    try
                                    {
                                        writer.WriteLine(stu.courseCode + " " + stu.courseNo + ",\"" + stu.T1Date + "," + stu.T2Date + "\"," + stu.T1Time + "," + room + "," + Rooms[room] + "," + stu.IDs[index] + "," + stu.IDs[index+Rooms[room]]);
                                        index = Rooms[room]; x++;
                                        string students = "";
                                        for (int i = 0; i < index; i++)
                                        { students = students + stu.IDs[i] + ":"; }
                                        writer1.WriteLine(stu.courseCode + " " + stu.courseNo + "," + stu.T1Date + "," + stu.T2Date + "," + stu.T1Time + "," + room + "," + students);
                                        continue;
                                    }
                                    catch {
                                        try
                                        {
                                            writer.WriteLine(",,," + room + "," + (stu.IDs.Count - index) + "," + stu.IDs[index] + "," + stu.IDs[stu.IDs.Count - 1]);
                                            string students = "";
                                            for (int i = index; i < stu.IDs.Count; i++)
                                            { students = students + stu.IDs[i] + ":"; }
                                            writer1.WriteLine(stu.courseCode + " " + stu.courseNo + "," + stu.T1Date + "," + stu.T2Date + "," + stu.T1Time + "," + room + "," + students);
                                            break;
                                        }
                                        catch { }
                                    }
                                }
                                else if (x == stu.Rooms.Count - 1)
                                {
                                    try
                                    {
                                        writer.WriteLine(",,," + room + "," + (stu.IDs.Count - index) + "," + stu.IDs[index] + "," + stu.IDs[stu.IDs.Count - 1]);
                                        string students = "";
                                        for (int i = index; i < stu.IDs.Count; i++)
                                        { students = students + stu.IDs[i] + ":"; }
                                        writer1.WriteLine(stu.courseCode + " " + stu.courseNo + "," + stu.T1Date + "," + stu.T2Date + "," + stu.T1Time + "," + room + "," + students);
                                    }
                                    catch { }
                                }
                                else
                                {
                                    try
                                    {
                                        writer.WriteLine(",,," + room + "," + Rooms[room] + "," + stu.IDs[index] + "," + stu.IDs[index + Rooms[room] - 1]);
                                        string students = "";
                                        for (int i = index; i < index + Rooms[room]; i++)
                                        { students = students + stu.IDs[i] + ":"; }
                                        writer1.WriteLine(stu.courseCode + " " + stu.courseNo + "," + stu.T1Date + "," + stu.T2Date + "," + stu.T1Time + "," + room + "," + students);
                                        index += Rooms[room];
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            writer.WriteLine(",,," + room + "," + (stu.IDs.Count - index) + "," + stu.IDs[index] + "," + stu.IDs[stu.IDs.Count - 1]);
                                            string students = "";
                                            for (int i = index; i < stu.IDs.Count; i++)
                                            { students = students + stu.IDs[i] + ":"; }
                                            writer1.WriteLine(stu.courseCode + " " + stu.courseNo + "," + stu.T1Date + "," + stu.T2Date + "," + stu.T1Time + "," + room + "," + students);
                                            break;
                                        }
                                        catch { }
                                    }
                                }
                                //writer2.WriteLine(stu.courseCode + " " + stu.courseNo+ ",\"" + stu.T1Date + "," + stu.T2Date + "\"," + stu.T1Time + ","  + room + "," + room.Invigilators);
                                x++;
                            }
                        }
                    }
                    catch { }
                }
                writer.Close();
                writer1.Close();
                txtSeatingArrangement.Text = WorkingFolder + "/StudentsSeating.id";
                txtStatus.Text = "Student Seating Arrangement: " + WorkingFolder + "\\SeatingArrangement.csv";// +Environment.NewLine+ "Invigilators Duty: " + fbd.SelectedPath + "\\Invigilators.csv";
                GenerateSeatingPDF(WorkingFolder + "\\SeatingArrangement.csv");
            }
            catch (Exception e1) { txtStatus.Text = "File couldn't be saved. Error:" + e1.Message; }
        }
        private void getStyles(Document document)
        {
            MigraDoc.DocumentObjectModel.Style style = document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
            style = document.Styles["Heading1"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 14;
            style.Font.Bold = true;
            style.Font.Color = MigraDoc.DocumentObjectModel.Colors.DarkBlue;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceAfter = 6;
            style = document.Styles["Heading2"];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;
            style = document.Styles["Heading3"];
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;
            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", MigraDoc.DocumentObjectModel.TabAlignment.Right);
            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", MigraDoc.DocumentObjectModel.TabAlignment.Center);
            // Create a new style called TextBox based on style Normal
            style = document.Styles.AddStyle("TextBox", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Borders.Width = 2.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Shading.Color = MigraDoc.DocumentObjectModel.Colors.SkyBlue;
            // Create a new style called TOC based on style Normal
            style = document.Styles.AddStyle("TOC", "Normal");
            style.ParagraphFormat.AddTabStop("16cm", MigraDoc.DocumentObjectModel.TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = MigraDoc.DocumentObjectModel.Colors.Blue;
        }
        private void GenerateSeatingPDF(string file)
        {
            StreamReader File = new StreamReader(file);
            Document document = new Document();
            document.Info.Title = "Seating Arrangement";
            document.Info.Subject = "Seating Arrangement";
            document.Info.Author = "BPHC-Instruction Division";
            getStyles(document);

            Section section = document.AddSection();
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Portrait;
            section.PageSetup.TopMargin = Unit.FromCentimeter(4.0);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);
            HeaderFooter header = section.Headers.Primary;
            MigraDoc.DocumentObjectModel.Shapes.Image image = header.AddImage(txtImageFile.Text);
            image.Width = Unit.FromCentimeter(15);
            Table table = new Table();
            Column column = table.AddColumn(Unit.FromCentimeter(2.3)); 
            table.AddColumn(Unit.FromCentimeter(2.4)).Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn(Unit.FromCentimeter(2.4)).Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn(Unit.FromCentimeter(1.3)).Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn(Unit.FromCentimeter(2)).Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn(Unit.FromCentimeter(3)).Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn(Unit.FromCentimeter(3)).Format.Alignment = ParagraphAlignment.Center;
            table.Borders.Width = 0.75;
            Row row = table.AddRow();row.HeadingFormat = true;
            row.Shading.Color = MigraDoc.DocumentObjectModel.Colors.PaleGoldenrod;
            Cell cell = row.Cells[0];
            cell.AddParagraph("COURSE CODE");
            cell = row.Cells[1];
            cell.AddParagraph("DATE");
            cell = row.Cells[2];
            cell.AddParagraph("TIME");
            cell = row.Cells[3];
            cell.AddParagraph("ROOM NO");
            cell = row.Cells[4];
            cell.AddParagraph("STRENGTH");
            cell = row.Cells[5];
            cell.AddParagraph("IDs FROM");
            cell = row.Cells[6];
            cell.AddParagraph("IDs TO");
            string line = "";
            File.ReadLine();
            while ((line = File.ReadLine()) != null)
            {
                string[] ar1 = line.Split(',');
                if (line.StartsWith(",,,"))
                {
                    row = table.AddRow();
                    cell = row.Cells[3];
                    cell.AddParagraph(ar1[3]);
                    cell = row.Cells[4];
                    cell.AddParagraph(ar1[4]);
                    cell = row.Cells[5];
                    cell.AddParagraph(ar1[5]);
                    cell = row.Cells[6];
                    cell.AddParagraph(ar1[6]);
                }
                else if (ar1.Length == 7)
                {
                    row = table.AddRow(); 
                    cell = row.Cells[0];
                    cell.AddParagraph(ar1[0]);
                    cell = row.Cells[1];
                    cell.AddParagraph((ar1[1]+", "+ar1[2]).Replace("\"",""));
                    cell = row.Cells[2];
                    cell.AddParagraph(ar1[3]);
                    cell = row.Cells[3];
                    cell.AddParagraph(ar1[4]);
                    cell = row.Cells[4];
                    cell.AddParagraph(ar1[5]);
                    cell = row.Cells[5];
                    cell.AddParagraph(ar1[6]);
                    cell.MergeRight = 1;
                }
                else if (ar1.Length == 8)
                {
                    row = table.AddRow(); 
                    cell = row.Cells[0];
                    cell.AddParagraph(ar1[0]);
                    cell = row.Cells[1];
                    cell.AddParagraph((ar1[1] + ", " + ar1[2]).Replace("\"", ""));
                    cell = row.Cells[2];
                    cell.AddParagraph(ar1[3]);
                    cell = row.Cells[3];
                    cell.AddParagraph(ar1[4]);
                    cell = row.Cells[4];
                    cell.AddParagraph(ar1[5]);
                    cell = row.Cells[5];
                    cell.AddParagraph(ar1[6]);
                    cell = row.Cells[6];
                    cell.AddParagraph(ar1[7]);
                }
            }
            section.Add(table);
            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = document;
            renderer.RenderDocument();
            // Save the document...
            string filename = WorkingFolder + "/SEATING ARRANGEMENT.pdf";
            renderer.PdfDocument.Save(filename);
            Process.Start(filename);
            txtStatus.Text = "Student Seating Arrangement: " + file + Environment.NewLine + "Student Seating Arrangement: " + WorkingFolder + "/SEATING ARRANGEMENT.pdf";

        }
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            if (btn.Tag.ToString().Equals("6"))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult result1 = fbd.ShowDialog();
                txtInvigilationFolder.Text = fbd.SelectedPath;
                return;
            }
                OpenFileDialog fdlg = new OpenFileDialog();
            if (btn.Tag.ToString().Equals("2"))
                fdlg.Title = "Select a .csv file with list of all Exam-Timings:";
            else if (btn.Tag.ToString().Equals("1")) fdlg.Title = "Select a .csv file with list of all Students:";
            else if (btn.Tag.ToString().Equals("4")) fdlg.Title = "Select a .id file with the Seating Arrangement:";
            else if (btn.Tag.ToString().Equals("8")) fdlg.Title = "Select a .id file with the Invigilators Arrangement:";
            else if (btn.Tag.ToString().Equals("7")) fdlg.Title = "Select a .png file tobe used as a header in the Seating Arrangement:";
            else if (btn.Tag.ToString().Equals("5")) fdlg.Title = "Select a .csv file with the Invigilators email addresses:";
            else fdlg.Title = "Select a .csv file with list of all Invigilators:";
            fdlg.Filter = "CSV files (*.csv)|*.csv";
            if (btn.Tag.ToString().Equals("4")) fdlg.Filter = "ID files (*.id)|*.id";
            if (btn.Tag.ToString().Equals("8")) fdlg.Filter = "ID files (*.id)|*.id";
            if (btn.Tag.ToString().Equals("7")) fdlg.Filter = "PNG files (*.png)|*.png";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            DialogResult result = fdlg.ShowDialog();
            if (result==System.Windows.Forms.DialogResult.OK)
            {
                if (btn.Tag.ToString().Equals("1")) { txtStudentsFile.Text = fdlg.FileName; txtStudentsFile1.Text = fdlg.FileName; }
                else if (btn.Tag.ToString().Equals("3")) txtInvigilatorsFile.Text = fdlg.FileName;
                else if (btn.Tag.ToString().Equals("5"))
                {
                    txtInvigilatorEmails.Text = fdlg.FileName; txtInvigilatorsEmails.Text = fdlg.FileName;
                }
                else if (btn.Tag.ToString().Equals("9")) txtInvigilatorEmails.Text = fdlg.FileName;
                else if (btn.Tag.ToString().Equals("4")) txtSeatingArrangement.Text = fdlg.FileName;
                else if (btn.Tag.ToString().Equals("7")) txtImageFile.Text = fdlg.FileName;
                else if (btn.Tag.ToString().Equals("8")) txtInvigilators.Text = fdlg.FileName;
                else txtTimingsFile.Text = fdlg.FileName;
            }
        }
        private void DisableAll()
        {
            groupBox.IsEnabled = false;
            groupBox1.IsEnabled = false;
            groupBox2.IsEnabled = false;
            groupBox3.IsEnabled = false;
            groupBox4.IsEnabled = false;
            groupBox5.IsEnabled = false;
            groupBox6.IsEnabled = false;
        }
        private void EnableAll()
        {
            if(Enabled[0])groupBox.IsEnabled = true;
            if (Enabled[1]) groupBox1.IsEnabled = true;
            if (Enabled[2]) groupBox2.IsEnabled = true;
            groupBox3.IsEnabled = true;
            if (Enabled[3]) groupBox4.IsEnabled = true;
            if (Enabled[4]) groupBox5.IsEnabled = true;
            if (Enabled[5]) groupBox6.IsEnabled = true;
        }
        private async void btnSync_Click(object sender, RoutedEventArgs e)
        {
            DisableAll();
            prg1.Visibility = Visibility.Visible;
            try
            {
                if (!File.Exists(txtSeatingArrangement.Text)) { txtStatus.Text = "Seating Arrangement file doesnot exist in the following location: " + txtSeatingArrangement.Text; return; }
                StreamReader File1 = new StreamReader(txtSeatingArrangement.Text);
                string line = "";

                List<Seating> seating = await App.MobileService.GetTable<Seating>().Take(1000).ToListAsync();
                txtStatus.Text = "Deleting previous records of the Seating Arrangement from the cloud.";
                foreach (Seating stu in seating)
                {
                    await App.MobileService.GetTable<Seating>().DeleteAsync(stu);
                    txtStatus.Text = "Deleting "+stu.Course+" from the cloud...";
                }
                seating = await App.MobileService.GetTable<Seating>().Take(1000).ToListAsync();
                txtStatus.Text = "Deleting previous records of the Seating Arrangement from the cloud.";
                foreach (Seating stu in seating)
                {
                    await App.MobileService.GetTable<Seating>().DeleteAsync(stu);
                    txtStatus.Text = "Deleting " + stu.Course + " from the cloud...";
                }
                seating = await App.MobileService.GetTable<Seating>().Take(1000).ToListAsync();
                txtStatus.Text = "Deleting previous records of the Seating Arrangement from the cloud.";
                foreach (Seating stu in seating)
                {
                    await App.MobileService.GetTable<Seating>().DeleteAsync(stu);
                    txtStatus.Text = "Deleting " + stu.Course + " from the cloud...";
                }
                
                txtStatus.Text = "Adding current records of the Seating Arrangement to the cloud.";
                List<Seating> listSeating = new List<Seating>();
                try
                {
                    while ((line = File1.ReadLine()) != null)
                    {
                        string[] ar = line.Split(',');
                        try { await App.MobileService.GetTable<Seating>().InsertAsync(new Seating(ar[0], ar[1], ar[2], ar[3], ar[4], ar[5], false)); } catch { listSeating.Add(new Seating(ar[0], ar[1], ar[2], ar[3], ar[4], ar[5], false)); }
                        txtStatus.Text = "Adding " + ar[0] + " to cloud...";
                    }
                    while(listSeating.Count!=0)
                    {
                        try
                        {
                            Seating stu = listSeating[0];
                            await App.MobileService.GetTable<Seating>().InsertAsync(stu); txtStatus.Text = "Adding " + stu.Course + " to cloud...";
                            listSeating.Remove(stu);
                        }
                        catch { }
                    }
                }
                catch { txtStatus.Text = "File wasn't uploaded on the cloud. Try Again!"; }
                txtStatus.Text = "Seating Arrangement File uploaded on the cloud.";

            }
            catch { txtStatus.Text = "File wasn't uploaded on the cloud. Try Again!"; }
            prg1.Visibility = Visibility.Collapsed;
            EnableAll();
        }
        private async void btnSync_Click_1(object sender, RoutedEventArgs e)
        {
            DisableAll();
            prg1.Visibility = Visibility.Visible;
            try
            {
                if (!File.Exists(txtStudentsFile.Text)) { txtStatus.Text = "Students file doesnot exist in the following location: " + txtStudentsFile.Text; return; }

                List<Students> students = await App.MobileService.GetTable<Students>().Take(1000).ToListAsync();
                txtStatus.Text = "Deleting previous records of the Students from the cloud.";
                foreach (Students stu in students) { txtStatus.Text = "Deleting " + stu.Name + " from cloud..."; await App.MobileService.GetTable<Students>().DeleteAsync(stu); }
                students = await App.MobileService.GetTable<Students>().Take(1000).ToListAsync();
                txtStatus.Text = "Deleting previous records of the Students from the cloud.";
                foreach (Students stu in students) { txtStatus.Text = "Deleting " + stu.Name + " from cloud..."; await App.MobileService.GetTable<Students>().DeleteAsync(stu); }

                students = await App.MobileService.GetTable<Students>().Take(1000).ToListAsync();
                txtStatus.Text = "Deleting previous records of the Students from the cloud.";
                foreach (Students stu in students) { txtStatus.Text = "Deleting " + stu.Name + " from cloud..."; await App.MobileService.GetTable<Students>().DeleteAsync(stu); }
                students = await App.MobileService.GetTable<Students>().Take(1000).ToListAsync();
                txtStatus.Text = "Deleting previous records of the Students from the cloud.";
                foreach (Students stu in students) { txtStatus.Text = "Deleting " + stu.Name + " from cloud..."; await App.MobileService.GetTable<Students>().DeleteAsync(stu); }

                List<Students> listStudents = new List<Students>();
                StreamReader File1 = new StreamReader(txtStudentsFile.Text);
                string line = File1.ReadLine(); File1.ReadLine();
                try
                {
                    while ((line = File1.ReadLine()) != null)
                    {
                        string[] ar = line.Split(',');
                        Boolean flag = true;
                        foreach (Students stu in listStudents)
                        {
                            if (stu.Campus_ID.Equals(ar[1])) { stu.Course_ID += "," + ar[3] + " " + ar[4].Trim(); flag = false; }
                        }
                        if (flag)
                        {
                            string x = ar[5];
                            if (x.Contains('.')) x = x.Substring(0, ar[5].Length - 2).Trim();
                            if (x[0] < 65 || x[0] > 90) x = x.Substring(1);
                            listStudents.Add(new Students(ar[1], ar[3] + " " + ar[4].Trim(), x));
                        }
                        //await App.MobileService.GetTable<Students>().InsertAsync(new Students(ar[1], ar[3] + " " + ar[4].Trim(), ar[5].Substring(0, ar[5].Length - 2).Trim()));
                    }
                    prg1.IsIndeterminate = false;
                    prg1.Maximum = listStudents.Count;
                    prg1.Minimum = 0;
                    int index = 0;
                    txtStatus.Text = "Adding current records of the Student to the cloud.";
                    List<Students> listStudents1 = new List<Students>();
                    foreach (Students stu in listStudents)
                    {
                        index++;
                        try { await App.MobileService.GetTable<Students>().InsertAsync(stu); prg1.Value = index; txtStatus.Text = "Adding " + stu.Name + " to cloud. (" + index + " of " + listStudents.Count + ")"; } catch { listStudents1.Add(stu); }
                    }
                    txtStatus.Text = "Retrying the failed ones...";
                    while (listStudents1.Count != 0)
                    {
                        try
                        {
                            Students stu = listStudents1[0];
                            await App.MobileService.GetTable<Students>().InsertAsync(stu); prg1.Value = index; txtStatus.Text = "Adding " + stu.Name + " to cloud. (" + index + " of " + listStudents.Count + ")";
                            listStudents1.Remove(stu);
                        }
                        catch { }
                    }
                    prg1.IsIndeterminate = true;
                    txtStatus.Text = "Students File uploaded on the cloud.";
                }
                catch { txtStatus.Text = "File wasn't uploaded on the cloud. Try Again!"; }
            }
            catch { txtStatus.Text = "File wasn't uploaded on the cloud. Try Again!"; }
            prg1.Visibility = Visibility.Collapsed;
            EnableAll();

        }
        private async void btnSync_Click_2(object sender, RoutedEventArgs e)
        {
            DisableAll();
            prg1.Visibility = Visibility.Visible;
            try
            {
                if (!File.Exists(txtInvigilators.Text)) { txtStatus.Text = "Invigilators file doesnot exist in the following location: " + txtInvigilators.Text; return; }
                StreamReader File1 = new StreamReader(txtInvigilators.Text);
                string line = "";

                List<Invigilators> seating = await App.MobileService.GetTable<Invigilators>().Take(1000).ToListAsync();
                txtStatus.Text = "Deleting previous records of the Invigilators from the cloud.";
                foreach (Invigilators stu in seating)
                {
                    await App.MobileService.GetTable<Invigilators>().DeleteAsync(stu);
                    txtStatus.Text = "Deleting " + stu.course + " from the cloud...";
                }
                txtStatus.Text = "Adding current records of the Invigilators to the cloud.";
                List<Invigilators> listSeating = new List<Invigilators>();
                try
                {
                    while ((line = File1.ReadLine()) != null)
                    {
                        string[] ar = line.Split(',');
                        try { await App.MobileService.GetTable<Invigilators>().InsertAsync(new Invigilators(ar[0], ar[1], ar[2], ar[3], ar[4].Replace("--", "-"), ar[5])); } catch { listSeating.Add(new Invigilators(ar[0], ar[1], ar[2], ar[3], ar[4].Replace("--", "-"), ar[5])); }
                        txtStatus.Text = "Adding " + ar[0] + " to cloud...";
                    }
                    while (listSeating.Count != 0)
                    {
                        try
                        {
                            Invigilators stu = listSeating[0];
                            await App.MobileService.GetTable<Invigilators>().InsertAsync(stu); txtStatus.Text = "Adding " + stu.course + " to cloud...";
                            listSeating.Remove(stu);
                        }
                        catch { }
                    }
                    txtStatus.Text = "Invigilators File was uploaded on the cloud.";
                }
                catch { txtStatus.Text = "File wasn't uploaded on the cloud. Try Again!"; }

            }
            catch { txtStatus.Text = "File wasn't uploaded on the cloud. Try Again!"; }
            prg1.Visibility = Visibility.Collapsed;
            EnableAll();
        }
        private async void btnSync_Click_3(object sender, RoutedEventArgs e)
        {
            DisableAll();
            prg1.Visibility = Visibility.Visible;
            try
            {
                if (!File.Exists(txtInvigilatorsEmails.Text)) { txtStatus.Text = "Invigilators Emails file doesnot exist in the following location: " + txtInvigilatorsEmails.Text; return; }
                StreamReader File1 = new StreamReader(txtInvigilatorsEmails.Text);
                string line = "";

                List<Teachers> seating = await App.MobileService.GetTable<Teachers>().Take(1000).ToListAsync();
                txtStatus.Text = "Deleting previous records of the Invigilators from the cloud.";
                foreach (Teachers stu in seating)
                {
                    await App.MobileService.GetTable<Teachers>().DeleteAsync(stu);
                    txtStatus.Text = "Deleting " + stu.name + " from the cloud...";
                }
                txtStatus.Text = "Adding current records of the Invigilators to the cloud.";
                List<Teachers> listSeating = new List<Teachers>();
                try
                {
                    while ((line = File1.ReadLine()) != null)
                    {
                        string[] ar = line.Split(',');
                        try { await App.MobileService.GetTable<Teachers>().InsertAsync(new Teachers(ar[0], ar[1])); } catch { listSeating.Add(new Teachers(ar[0], ar[1])); }
                        txtStatus.Text = "Adding " + ar[0] + " to cloud...";
                    }
                    while (listSeating.Count != 0)
                    {
                        try
                        {
                            Teachers stu = listSeating[0];
                            await App.MobileService.GetTable<Teachers>().InsertAsync(stu); txtStatus.Text = "Adding " + stu.name + " to cloud...";
                            listSeating.Remove(stu);
                        }
                        catch { }
                    }
                    txtStatus.Text = "Invigilators File was uploaded on the cloud.";
                }
                catch { txtStatus.Text = "File wasn't uploaded on the cloud. Try Again!"; }

            }
            catch { txtStatus.Text = "File wasn't uploaded on the cloud. Try Again!"; }
            prg1.Visibility = Visibility.Collapsed;
            EnableAll();
        }

        MailMessage mail;
        SmtpClient smtp;
        private async void backgroundTask()
        {
            prg1.Visibility = Visibility.Visible;
            count = 0;
            prg1.IsIndeterminate = false;

            foreach (string attachmentFilename in Directory.EnumerateFiles(txtInvigilationFolder.Text, "*.pdf"))
            {

                try
                { //this.Invoke((MethodInvoker)delegate {
                    txtStatus.Text = "Sending " + (count + 1) + " of " + attachments + " mails...";
                    prg1.Value = count + 1;
                    //});
                    mail.Body = txtBody.Text.Replace("#name#", Path.GetFileNameWithoutExtension(attachmentFilename));
                    if (attachmentFilename != null)
                    {
                        mail.To.Clear();
                        int index = Names.IndexOf(Path.GetFileNameWithoutExtension(attachmentFilename));
                        if (index == -1)
                            continue;
                        mail.To.Add(Emails[index]);
                        Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                        ContentDisposition disposition = attachment.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                        disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                        disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                        disposition.FileName = Path.GetFileName(attachmentFilename);
                        disposition.Size = new FileInfo(attachmentFilename).Length;
                        disposition.DispositionType = DispositionTypeNames.Attachment;
                        mail.Attachments.Clear();
                        mail.Attachments.Add(attachment);
                    }
                    await smtp.SendMailAsync(mail); count++;
                }
                catch { txtStatus.Text = "Failed sending mail to" + Path.GetFileNameWithoutExtension(attachmentFilename); }
            }
            //this.Invoke((MethodInvoker)delegate {
                txtStatus.Text = "All mails sent!";
                EnableAll();
            prg1.Visibility = Visibility.Collapsed;

            //});
        }

        List<string> Emails = new List<string>(), Names = new List<string>();
        int attachments, Noemails, count = 0;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            backgroundTask();
        }
        private void backgroundInviTask()
        {
            if (WorkingFolder.Equals(""))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult result = fbd.ShowDialog();
                WorkingFolder = fbd.SelectedPath;
            }
            string file = txtInvigilatorsFile.Text;
            StreamReader File1 = new StreamReader(file);
            Directory.CreateDirectory(WorkingFolder + "/Invigilations");
            StreamWriter writer1 = new StreamWriter(WorkingFolder + "/InvigilatorEmails.csv");
            StreamWriter writer = new StreamWriter(WorkingFolder + "/Invigilators2.csv");
            StreamWriter writer2 = new StreamWriter(WorkingFolder + "/Invigilators.id");
            string line = "";
            List<string> lines = new List<string>();
            try
            {
                string s = "", s1 = "";
                while ((line = File1.ReadLine()) != null)
                {
                    if (line.StartsWith("Instructor-in-charge  Prof./ Dr./ Mr./ Ms. "))
                    {
                        s = s1 + s;
                        lines.Add(s); s = ""; s1 = line;
                    }
                    else
                    {
                        s += line + "\n";
                    }
                }
                int x = 0;
                foreach (string st in lines)
                {
                    if (x != 0)
                    {
                        s = ""; s1 = "";
                        string[] arr = st.Split('\n');
                        string Name = arr[0].Substring(44, arr[0].IndexOf(',') - 44);
                        writer1.WriteLine(Name);
                        Document document = new Document();
                        document.Info.Title = Name;
                        document.Info.Subject = "Details of rooms and invigilators for your course(s)";
                        document.Info.Author = "BPHC-Instruction Division";
                        getStyles(document);

                        Section section = document.AddSection();
                        section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
                        Paragraph paragraph = section.AddParagraph("BIRLA INSTITUTE OF TECHNOLOGY AND SCIENCE-PILANI, HYDERABAD CAMPUS", "Heading1");
                        paragraph.Format.Alignment = ParagraphAlignment.Center;
                        paragraph = section.AddParagraph("INSTRUCTION DIVISION", "Heading1");
                        paragraph.Format.Alignment = ParagraphAlignment.Center;
                        paragraph = section.AddParagraph("FIRST SEMESTER 2015-2016", "Heading2");
                        paragraph.Format.Alignment = ParagraphAlignment.Center;
                        section.AddParagraph(arr[0].Replace(",", " "), "Heading2");
                        section.AddParagraph("Details of rooms and invigilators for your course(s):", "Heading2");
                        Table table = new Table();
                        Column column = table.AddColumn(Unit.FromCentimeter(1.3));column.Format.Alignment = ParagraphAlignment.Center;
                        table.AddColumn(Unit.FromCentimeter(2)).Format.Alignment = ParagraphAlignment.Center;
                        table.AddColumn(Unit.FromCentimeter(4)).Format.Alignment = ParagraphAlignment.Center;
                        table.AddColumn(Unit.FromCentimeter(2)).Format.Alignment = ParagraphAlignment.Center;
                        table.AddColumn(Unit.FromCentimeter(2.5)).Format.Alignment = ParagraphAlignment.Center;
                        table.AddColumn(Unit.FromCentimeter(4)).Format.Alignment = ParagraphAlignment.Center;
                        table.AddColumn(Unit.FromCentimeter(6)).Format.Alignment = ParagraphAlignment.Center;
                        table.AddColumn(Unit.FromCentimeter(2)).Format.Alignment = ParagraphAlignment.Center;
                        table.Borders.Width = 0.75;
                        Row row = table.AddRow();
                        row.Shading.Color = MigraDoc.DocumentObjectModel.Colors.PaleGoldenrod;
                        Cell cell = row.Cells[0];
                        cell.AddParagraph("CODE");
                        cell = row.Cells[1];
                        cell.AddParagraph("COURSE NO");
                        cell = row.Cells[2];
                        cell.AddParagraph("COURSE TITLE");
                        cell = row.Cells[3];
                        cell.AddParagraph("DATE");
                        cell = row.Cells[4];
                        cell.AddParagraph("TIME");
                        cell = row.Cells[5];
                        cell.AddParagraph("ROOM");
                        cell = row.Cells[6];
                        cell.AddParagraph("INVIGILATOR");
                        cell = row.Cells[7];
                        cell.AddParagraph("GROUP");

                        writer.WriteLine(arr[0]);
                        List<string> lines1 = new List<string>();

                        for (int i = 6; i < arr.Length; i += 3)
                        {
                            while (arr[i].Equals(",,,,,,,,,,,,,,,,,,,,,")) { i++; }
                            if (arr[i].StartsWith("\"1. In courses with less strength"))
                            {
                                s = s1 + s;
                                if (!s1.Equals(""))
                                    lines1.Add(s);
                                s = ""; s1 = "";
                                break;
                            }
                            if (arr[i].StartsWith(",,,,,,,,,,,,,,,")) s += arr[i] + "\n";
                            else
                            {
                                s = s1 + s;
                                if (!s1.Equals(""))
                                    lines1.Add(s);
                                s = "";
                                if (arr[i].StartsWith("\""))
                                {
                                    s1 = arr[i][1] + "" + arr[i][3] + "" + arr[i][4] + "" + arr[i][5] + arr[i].Substring(7) + "\n";
                                }
                                else
                                    s1 = arr[i] + "\n";
                            }
                        }
                        foreach (string str in lines1)
                        {
                            writer.WriteLine(str);
                            string[] ard = str.Split('\n');
                            int xd = 0;
                            string line1 = "";
                            foreach (string xc in ard)
                            {
                                string[] ds = xc.Split(',');
                                if (ds.Length < 13)
                                { continue; }
                                if (xd == 0)
                                {
                                    row = table.AddRow();
                                    cell = row.Cells[0];
                                    cell.AddParagraph(ds[0]);
                                    cell = row.Cells[1];
                                    cell.AddParagraph(ds[2] + " " + ds[3]);
                                    cell = row.Cells[2];
                                    cell.AddParagraph(ds[5]);
                                    cell = row.Cells[3];
                                    if (ds[7].IndexOf('\"') == -1) { int lh = xc.IndexOf(ds[8]);
                                        string ab = xc.Substring(lh);
                                        string bc = xc.Substring(0, lh);
                                        ds = (bc + "," + ab).Split(',');
                                    }
                                    cell.AddParagraph((ds[7]+","+ds[8]).Replace("\"", ""));
                                    cell = row.Cells[4];
                                    cell.AddParagraph(ds[9]);
                                    line1 += ds[2] + " " + ds[3] + ","+ ds[5] + ","+ (ds[7] + "," + ds[8]).Replace("\"", "") + ","+ ds[9].Replace(" ","") + ",";
                                    if (ds[12].StartsWith("\""))
                                    {
                                        int ld = xc.IndexOf(ds[12]);
                                        string rooms = xc.Substring(ld + 1, xc.IndexOf('\"', ld + 2) - ld - 1);
                                        cell = row.Cells[5];
                                        cell.AddParagraph(rooms);
                                        //line1 += rooms.Replace(',','|')+",";
                                    }
                                    else
                                    {
                                        cell = row.Cells[5];
                                        cell.AddParagraph(ds[12]);
                                        //line1 +=  ds[12] + ",";
                                    }
                                    int lc = ds.Length;
                                    cell = row.Cells[6];
                                    cell.AddParagraph(ds[lc - 7]);
                                    cell = row.Cells[7];
                                    cell.AddParagraph(ds[lc - 3]);
                                    line1 += ds[lc - 7] + ":";//+ ds[lc - 3]+"|";
                                    xd++;
                                }
                                else
                                {
                                    row = table.AddRow();
                                    cell = row.Cells[6];
                                    cell.AddParagraph(ds[15]);
                                    cell = row.Cells[7];
                                    cell.AddParagraph(ds[19]);
                                    line1 += ds[15] + ":";//+ ds[19]+"|";
                                }
                            }
                            writer2.WriteLine(line1);
                            line1 = "";
                        }
                        section.Add(table);
                        section.AddParagraph("1. In courses with less strength (viz. a course in which upto 45 students are registered), the Instructor-In-Charge is the only invigilator.", "Heading3");
                        section.AddParagraph("2. In case of any problems, contact  Prof. P K Sahoo (In-charge, Test Scheduling).", "Heading3");
                        paragraph = section.AddParagraph("ASSOCIATE DEAN", "Heading2");
                        paragraph.Format.Alignment = ParagraphAlignment.Right;
                        paragraph = section.AddParagraph("INSTRUCTION DIVISION", "Heading2");
                        paragraph.Format.Alignment = ParagraphAlignment.Right;
                        MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
                        PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
                        renderer.Document = document;
                        renderer.RenderDocument();
                        // Save the document...
                        string filename = WorkingFolder + "/Invigilations/" + Name + ".pdf";
                        renderer.PdfDocument.Save(filename);
                        writer.WriteLine("-,-,-,-,-,-,-,-,-,--,-,-,-,-,-,-,-,-,-,-,-,-,--,-,-,-,-,-,-,-,-,-,-,-,-,-,-,-,-,-");
                        //writer.WriteLine(st);
                    }
                    x = 1;
                }
            }
            catch (Exception e1)
            {
            }
            File1.Close();
            writer.Close();
            writer1.Close();
            writer2.Close();
            txtInvigilators.Text = WorkingFolder + "/Invigilators.id";
        }

        private void btnInitialize1_Click(object sender, RoutedEventArgs e)
        {
            DisableAll();
            if (!File.Exists(txtInvigilatorEmails.Text)) { txtStatus.Text = "Invigilator Emails file doesnot exist in the following location: " + txtInvigilatorEmails.Text; EnableAll(); return; }
            BrowseNamesEmails(txtInvigilatorEmails.Text);
            if (!Directory.Exists(txtInvigilationFolder.Text)) { txtStatus.Text = "The folder with the attachments is invalid."; EnableAll(); return; }
            attachments = Directory.GetFiles(txtInvigilationFolder.Text, "*.pdf").Length;
            Enabled[4] = true;
            EnableAll();
        }

        private void btnGenerate1_Click(object sender, RoutedEventArgs e)
        {
            DisableAll();
            prg1.IsIndeterminate = true;
            prg1.Visibility = Visibility.Visible;
            //Thread background = new Thread(new ThreadStart(backgroundInviTask));
            //background.Start();
            backgroundInviTask();
            EnableAll();
            txtStatus.Text = "Invigilator Sheets created at " + WorkingFolder + "\\Invigilations\\";
            txtInvigilationFolder.Text = WorkingFolder + "\\Invigilations\\";
            txtInvigilatorEmails.Text = WorkingFolder + "/InvigilatorEmails.csv";
            //groupBox6_count++;
            //if (groupBox6_count == 2) groupBox6.IsEnabled = true;
            prg1.Visibility = Visibility.Collapsed;
        }
        
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            DisableAll();
            mail = new MailMessage();
            mail.From = new MailAddress(txtEmail.Text);
            mail.Subject = txtSubject.Text;
            mail.IsBodyHtml = true;
            smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
            smtp.Credentials = new NetworkCredential(txtEmail.Text, txtPassword.Password); // ***use valid credentials***
            smtp.Port = 587;
            smtp.EnableSsl = true;
            prg1.Minimum = 0;
            prg1.Maximum = attachments;
            txtLoginStatus.Text = "Signed In.";
            btnLogin.IsEnabled = false;
            Enabled[5] = true;
            EnableAll();
            //Thread background = new Thread(new ThreadStart(backgroundTask));
            //backgroundTask();//.Start();
        }

        private void BrowseNamesEmails(string file)
        {
            StreamReader File = new StreamReader(file);
            string line = "";
            while ((line = File.ReadLine()) != null)
            {
                string[] ar1 = line.Split(',');
                Emails.Add(ar1[1]);
                Names.Add(ar1[0]);
            }
            Noemails = Emails.Count;
        }

    }
}
