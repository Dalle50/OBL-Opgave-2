using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
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
            
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection conn =
                    new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\danie\\source\\repos\\DatabaseSamtidighed\\DatabaseSamtidighed\\Database1.mdf;Integrated Security=True;Connect Timeout=30");

                SqlCommand command;
                string sqlQuery;
                SqlDataReader reader;
                string id = IDText.Text;
                conn.Open();
                clearAll();
                IDText.Text = id;
                if (id.Length != 0)
                {

                    sqlQuery = "SELECT Medarbejder.ID,Medarbejder.Fornavn,Medarbejder.Efternavn,Sallery.Navn,Sallery.amount,Dept.amount FROM Medarbejder, Sallery,Dept " +
                        "WHERE Medarbejder.ID = " + id + " AND Dept.PersonID = " + id + " AND Sallery.SalleryID = " + id + ";";
                    command = new SqlCommand(sqlQuery, conn);
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            myListBox.Items.Add(reader.GetInt32(0) + "  " + reader.GetString(1) + "  " + reader.GetString(2) + "  " + reader.GetString(3) + "  " + reader.GetInt32(4) + "  " + reader.GetInt32(5) + "  ");
                            FirstNameText.Text = reader.GetString(1);
                            LastNameText.Text = reader.GetString(2);
                            Dept.Text = reader.GetInt32(5).ToString();
                            SalleryText.Text = reader.GetInt32(4).ToString();
                        }
                    }

                }
                else
                {
                    sqlQuery = "SELECT Medarbejder.ID,Medarbejder.Fornavn,Medarbejder.Efternavn,Sallery.Navn,Sallery.amount" +
                        ",Dept.amount FROM Medarbejder " +
                        "JOIN Dept ON Medarbejder.ID = Dept.PersonID " +
                        "JOIN Sallery ON Medarbejder.SalleryID = Sallery.SalleryID" +
                        " ORDER BY Dept.amount;";

                    command = new SqlCommand(sqlQuery, conn);
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            myListBox.Items.Add(reader.GetInt32(0) + "  " + reader.GetString(1) + "  " + reader.GetString(2) + "  " + reader.GetString(3) + "  " + reader.GetInt32(4) + "  " + reader.GetInt32(5) + "  ");
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception ef)
            {
                myListBox.Items.Add("E:" + ef.ToString());
            }
        }
        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
            SqlConnection conn =
    new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\danie\\source\\repos\\DatabaseSamtidighed\\DatabaseSamtidighed\\Database1.mdf;Integrated Security=True;Connect Timeout=30");

            SqlCommand command;
            string sqlQuery;
            SqlDataReader reader;
            string id = IDText.Text;
            string FirstName = FirstNameText.Text;
            string LastName = LastNameText.Text;
            string gæld = Dept.Text;
            sqlQuery = "select * from Dept;";
            conn.Open();
            try
            {
                command = new SqlCommand(sqlQuery, conn);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    myListBox.Items.Add("Der er tækker");

                }

            }
            catch (Exception ef)
            {
                myListBox.Items.Add("E:" + ef.ToString());
                myListBox.Items.Add(IDText.Text);
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            myListBox.Items.Clear();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            myListBox.Items.Clear();
        }

        private void ClrBtn_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
        }
        private void clearAll()
        {
            myListBox.Items.Clear();
            IDText.Clear();
            FirstNameText.Clear();
            LastNameText.Clear();
            Dept.Clear();
            SalleryText.Clear();
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
}
