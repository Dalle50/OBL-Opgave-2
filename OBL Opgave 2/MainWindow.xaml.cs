using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OBL_Opgave_2
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            Container.threadList.Add("");
            Container.threadList.Add("");
            Container.threadList.Add("");
            Container.threadList.Add("");
            Thread updatingListBox = new Thread(updateListBox);
            updatingListBox.Start();
            //string insertStatus = Container.threadList[1];
            //string updateStatus = Container.threadList[2];
            //string deleteStatus = Container.threadList[3];

        }
        private void updateListBox()
        {
            int counter = 0;
            while (Thread.CurrentThread.IsAlive)
            {
                    try
                {
                    if (counter % 20 == 0)
                    {
                        Container.threadList[0] = "Venter på handling.";
                        Container.threadList[1] = "Venter på handling.";
                        Container.threadList[2] = "Venter på handling.";
                        Container.threadList[3] = "Venter på handling.";
                        

                    }
                    else
                    {
                        Thread.Sleep(1000);
                        this.Dispatcher.Invoke(() =>
                        {
                            threadStatus.Items.Clear();
                            foreach(string element in Container.threadList)
                            {
                                threadStatus.Items.Add(element);

                            }


                        });

                    }
                    
                }
                catch (ThreadInterruptedException e)
                {
                    Thread.CurrentThread.Interrupt();
                }
                counter++;
            }
        }
        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread t1 = new Thread(() => selectQuery());
            t1.Start();
        }
        private async void selectQuery()
        {
            this.Dispatcher.Invoke(() =>
            {
            myListBox.Items.Clear();

            try
            {
                Container.threadList[0] = "Igangsætter forespørgsel.";
                    SqlConnection conn =
                    new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\teodo\\source\\repos\\OblOpgave1\\OblOpgave1\\OBL-Opgave-2\\OBL Opgave 2\\Database1.mdf;Integrated Security=True;Connect Timeout=30");

                SqlCommand command;
                string sqlQuery;
                SqlDataReader reader;
                string id = IDText.Text;
                string salleryID = SalleryText.Text;
                conn.Open();
                clearAll();
                IDText.Text = id;
                if (id.Length != 0)
                {
                    Container.threadList[0] = "Sender forespørgsel.";
                        sqlQuery = "SELECT Medarbejder.ID,Medarbejder.Fornavn,Medarbejder.Efternavn,Sallery.Navn,Sallery.amount,Dept.amount FROM Medarbejder, Sallery,Dept " +
                            "WHERE Medarbejder.ID = " + id + " AND Dept.PersonID = " + id + " AND Sallery.SalleryID = Medarbejder.SalleryID;";
                    command = new SqlCommand(sqlQuery, conn);
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Container.threadList[0] = "Opdaterer tabel.";
                            while (reader.Read())
                        {
                            myListBox.Items.Add(reader.GetInt32(0) + "  " + reader.GetString(1) + "  " + reader.GetString(2) + "  " + reader.GetString(3) + "  " + reader.GetInt32(4) + "  " + reader.GetInt32(5) + "  ");
                            FirstNameText.Text = reader.GetString(1);
                            LastNameText.Text = reader.GetString(2);
                            Dept.Text = reader.GetInt32(5).ToString();
                            SalleryText.Text = reader.GetInt32(4).ToString();
                        }
                    }
                        else
                        {
                            Container.threadList[0] = "Der var ikke nogen matchene query.";
                        }

                }
                else
                {
                    Container.threadList[0] = "Sender forespørgsel på alle.";
                        sqlQuery = "SELECT Medarbejder.ID,Medarbejder.Fornavn,Medarbejder.Efternavn,Sallery.Navn,Sallery.amount" +
                        ",Dept.amount FROM Medarbejder " +
                        "JOIN Dept ON Medarbejder.ID = Dept.PersonID " +
                        "JOIN Sallery ON Medarbejder.SalleryID = Sallery.SalleryID" +
                        " ORDER BY Dept.amount;";

                    command = new SqlCommand(sqlQuery, conn);
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Container.threadList[0] = "Opdaterer tabel.";
                        while (reader.Read())
                        {
                            myListBox.Items.Add(reader.GetInt32(0) + "  " + reader.GetString(1) + "  " + reader.GetString(2) + "  " + reader.GetString(3) + "  " + reader.GetInt32(4) + "  " + reader.GetInt32(5) + "  ");
                        }
                     }
                    Container.threadList[0] = "Tabel er opdateret.";
                        
                }
                conn.Close();
            }
            catch (Exception ef)
            {
                Container.threadList[0] = "Der skete en fejl under SQL Query. Læs fejlmeddedelse";
                    myListBox.Items.Add("E:" + ef.ToString());   
            }

            });
        }
        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread t1 = new Thread(() => insertQuery());
            t1.Start();

        }
        private void insertQuery()
        {
            this.Dispatcher.Invoke(() =>
            {
            myListBox.Items.Clear();
                Container.threadList[1] = "Laver connection";
                SqlConnection conn =
        new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\teodo\\source\\repos\\OblOpgave1\\OblOpgave1\\OBL-Opgave-2\\OBL Opgave 2\\Database1.mdf;Integrated Security=True;Connect Timeout=30");

                SqlCommand command;
                string sqlQuery1;
                string sqlQuery2;
                SqlDataReader reader;
                string FirstName = FirstNameText.Text;
                string id = IDText.Text;
                string LastName = LastNameText.Text;
                string løn = SalleryText.Text;
                string gæld = Dept.Text;
                sqlQuery1 = "INSERT INTO  Medarbejder (ID, Fornavn, Efternavn, SalleryID)" +
                " VALUES (@ID,@firstname,@lastname, @sallery);";
                sqlQuery2 = "INSERT into Dept(PersonId, amount)" +
                " VALUES (@PersonID,@DEPT);";
                conn.Open();
                
                if(FirstName.Length > 0 && LastName.Length > 0 && id.Length >0 && løn.Length > 0 && gæld.Length >0)
                {
                    try
                    {
                        command = new SqlCommand(sqlQuery1, conn);
                        command.Parameters.AddWithValue("@ID", id);
                        command.Parameters.AddWithValue("@firstname", FirstName);
                        command.Parameters.AddWithValue("@lastname", LastName);
                        command.Parameters.AddWithValue("@sallery", løn);
                        var result1 = command.ExecuteNonQuery();
                        Container.threadList[1] = "Tilføjer til database";
                        if (result1 > 0)
                        {
                            myListBox.Items.Add(FirstName + " " + LastName + " has been added to the medarbejder table.");

                        }
                        if (gæld.Length > 0)
                        {
                            command = new SqlCommand(sqlQuery2, conn);
                            command.Parameters.AddWithValue("@PersonID", id);
                            command.Parameters.AddWithValue("@DEPT", gæld);
                            var result2 = command.ExecuteNonQuery();

                            if (result2 > 0)
                            {
                                myListBox.Items.Add(FirstName + " " + LastName + " has been added to the dept database.");

                            }
                        }


                    }
                    catch (Exception ef)
                    {
                        Container.threadList[1] = "Der er opstået en fejl";
                        myListBox.Items.Add("E:" + ef.ToString());
                    }
                }
                else
                {
                    Container.threadList[1] = "Venter på rigtige informationer";
                    MessageBox.Show("Der mangler at blive udfyldt data i nogle af felterne.");
                }
                
            });
        }

            private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread t1 = new Thread(new ThreadStart(updateDB));
            t1.Start();
        }
        private void updateDB()
        {
            this.Dispatcher.Invoke(() =>
            {
                myListBox.Items.Clear();
                Container.threadList[2] = "Laver connection";
                SqlConnection conn =
        new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\teodo\\source\\repos\\OblOpgave1\\OblOpgave1\\OBL-Opgave-2\\OBL Opgave 2\\Database1.mdf;Integrated Security=True;Connect Timeout=30");

                SqlCommand command;
                string sqlQuery1;
                string sqlQuery2;
                SqlDataReader reader;
                string id = IDText.Text;
                string FirstName = FirstNameText.Text;
                string LastName = LastNameText.Text;
                string løn = SalleryText.Text;
                string gæld = Dept.Text;
                sqlQuery1 = "UPDATE Medarbejder  SET Fornavn=@firstname, Efternavn=@lastname;";

                sqlQuery2 = "UPDATE Dept SET amount=@DEPT;";
                conn.Open();

                if (FirstName.Length == 0 | LastName.Length == 0 | id.Length == 0 || løn.Length == 0 || gæld.Length == 0)
                {
                    MessageBox.Show("Venligst vælg select først, og dernæst opdater det du skal.");
                    Container.threadList[2] = "Venter på rigtige informationer";
                }
                else
                {
                    try
                    {
                        command = new SqlCommand(sqlQuery1, conn);
                        command.Parameters.AddWithValue("@ID", id);
                        command.Parameters.AddWithValue("@firstname", FirstName);
                        command.Parameters.AddWithValue("@lastname", LastName);
                        command.Parameters.AddWithValue("@sallery", løn);
                        var result1 = command.ExecuteNonQuery();
                        
                        if (result1 > 0)
                        {
                            Container.threadList[2] = "Opdaterer  medarbejder database";
                            myListBox.Items.Add(FirstName + " " + LastName + " has been updated in the medarbejder table.");

                        }
                        if (gæld.Length > 0)
                        {

                            command = new SqlCommand(sqlQuery2, conn);
                            command.Parameters.AddWithValue("@PersonID", id);
                            command.Parameters.AddWithValue("@DEPT", gæld);
                            var result2 = command.ExecuteNonQuery();
                            if (result2 > 0)
                            {
                                myListBox.Items.Add(FirstName + " " + LastName + " has been updated in the dept database.");
                                MessageBox.Show("If you want to change the sallery on the worker, you must inser a new worker, with a new id.");
                            }

                        }

                    }
                    catch (Exception ef)
                    {
                        Container.threadList[2] = "Der er opstået en fejl";
                        myListBox.Items.Add("E:" + ef.ToString());
                    }
                }
            });
        }
                    
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread t1 = new Thread(() => deleteQuery());
            t1.Start();
        }
        private void deleteQuery()
        {
            this.Dispatcher.Invoke(() =>
            {
                myListBox.Items.Clear();
                Container.threadList[3] = "Laver connection";
                SqlConnection conn =
        new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\teodo\\source\\repos\\OblOpgave1\\OblOpgave1\\OBL-Opgave-2\\OBL Opgave 2\\Database1.mdf;Integrated Security=True;Connect Timeout=30");

                SqlCommand command;
                SqlCommand command2;
                string sqlQuery1;
                string sqlQuery2;
                SqlDataReader reader;
                string id = IDText.Text;
                string FirstName = FirstNameText.Text;
                string LastName = LastNameText.Text;
                string løn = SalleryText.Text;
                string gæld = Dept.Text;
                sqlQuery1 = "DELETE FROM Medarbejder WHERE ID = @ID;";
                sqlQuery1 = "DELETE FROM DEPT WHERE PersonID = @ID;";

                conn.Open();

                if (FirstName.Length == 0 | LastName.Length == 0 | id.Length == 0 || løn.Length == 0 || gæld.Length == 0)
                {
                    MessageBox.Show("Venligst vælg select først, og dernæst opdater det du skal.");
                    Container.threadList[3] = "Venter på rigtige informationer";
                }
                else
                {
                    try
                    {
                        command = new SqlCommand(sqlQuery1, conn);
                        command.Parameters.AddWithValue("@ID", id);
                        var result1 = command.ExecuteNonQuery();
                        command2 = new SqlCommand(sqlQuery1, conn);
                        command2.Parameters.AddWithValue("@ID", id);
                        var result2 = command.ExecuteNonQuery();

                        if (result1 > 0)
                        {
                            Container.threadList[3] = "Sletter  medarbejder database";
                            myListBox.Items.Add(FirstName + " " + LastName + " has been removed in the medarbejder table.");

                        }
                        if (result2 > 0)
                        {
                            Container.threadList[3] = "Sletter  medarbejder database";
                            myListBox.Items.Add(FirstName + " " + LastName + " has been removed in the dept table.");

                        }
                    }
                    catch (Exception ef)
                    {
                        Container.threadList[3] = "Der er opstået en fejl";
                        myListBox.Items.Add("E:" + ef.ToString());
                    }
                }
            });
        }

        private void ClrBtn_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
        }
        private void clearAll()
        {
            this.Dispatcher.Invoke(() =>
            {

                myListBox.Items.Clear();
                IDText.Clear();
                FirstNameText.Clear();
                LastNameText.Clear();
                Dept.Clear();
                SalleryText.Clear();
            });
        }


    }
    public class Employee
    {
        public int id;
        public string name;
        public Employee(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
    public class SaleryGroup
    {
        public int id;
        public string name;
        public double amount;
        public SaleryGroup(int id, string name, double amount) 
        {
            this.id = id;
            this.name = name;
            this.amount = amount;
        }
    }
    public class Dept
    {
        public int personId;
        public double amount;
        public Dept(int personId, double amount)
        {
            this.personId = personId;
            this.amount = amount;
        }

    }
    static class Container
    {
        public static List<string> threadList = new List<string>();
    }
}
