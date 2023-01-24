using Microsoft.Win32;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SumAppMaster
{
    public partial class MainWindow : Window
    {
        private object txtFilename;

        public List<Podatak> podaci = new List<Podatak>();


        DataTable dt;

        public List<double> time= new List<double>();
        public List<double> value = new List<double>();




        public MainWindow()
        {
            InitializeComponent();
            dt = new DataTable();
            dt.Columns.Add("Vreme (s)");
            dt.Columns.Add("Potrošnja (l/min)");
            dataGridView1.ItemsSource = dt.DefaultView;

          
            
        }


        public class Podatak
        {
            public string Vreme { get; set; }
            public string Vrednost { get; set; }
        }
       
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");

            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));

        }




        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != "")
                {
                    lblFileName.Content = "Ruta: " + openFileDialog.FileName;
                    SLDocument doc = new SLDocument(openFileDialog.FileName, "Sheet1");

                    for (int i = 2; ; i++)
                    {
                        if (doc.GetCellValueAsString(i, 1) == "")
                        {
                            break;
                        }
                        podaci.Add(new Podatak()
                        {
                            Vreme = doc.GetCellValueAsString(i, 1),
                            Vrednost = doc.GetCellValueAsString(i, 2),
                        });

                        time.Add(double.Parse(podaci[i - 2].Vreme));
                        value.Add(double.Parse(podaci[i - 2].Vrednost));

/*
                        DataRow dr = dt.NewRow();

                        dr[0] = podaci[i - 2].Vreme.ToString();
                        dr[1] = podaci[i - 2].Vrednost.ToString();

                        dt.Rows.Add(dr);*/
                        MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(btnUpload, false);



                    }

                    dataGridView1.ItemsSource= podaci;

                    double[] t= new double[time.Count];
                    double[] v = new double[value.Count];

                    for (int i = 0; i < time.Count; i++)
                    {
                        t[i]=time[i];
                        v[i]=value[i];

                    }


                    plogGraph.Plot.AddScatter(t, v);

                    plogGraph.Refresh();

                }
                else
                {
                    lblFileName.Content = "Greska";

                }
            }
        }

        private void btnPotvrda_Click(object sender, RoutedEventArgs e)
        {
            if (txtTime1.Text == "" || txtTime2.Text == "")
            {
                MessageBox.Show("Popunite oba polja!");
            }

            if (txtTime1.Text != "" && txtTime2.Text != "")
            {

                double Start = double.Parse(txtTime1.Text);
                double End = double.Parse(txtTime2.Text);

                if (Start > End)
                {
                    MessageBox.Show("Početna vrednost mora biti manja od krajnje!");

                }


                double q = 0;
                for (int i = 1; i < podaci.Count; i++)
                {
                    if (Start <= double.Parse(podaci[i].Vreme) && End >= double.Parse(podaci[i].Vreme))
                    {
                        double t = double.Parse(podaci[i].Vreme) - double.Parse(podaci[i - 1].Vreme);
                        double v = double.Parse(podaci[i].Vrednost) + double.Parse(podaci[i - 1].Vrednost);
                        q += (t * v) / 2;
                        q = Math.Round(q, 2);

                    }
                }
                lblResault.Content = q.ToString();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            dataGridView1.DataContext="";
            
            podaci.Clear();
            dataGridView1.ItemsSource = "";
            /* 
             double[] t = new double[time.Count];
             double[] v = new double[value.Count];
             plogGraph.Plot.AddScatter(t,v);
             plogGraph.Refresh();*/


            lblFileName.Content = "";
            lblResault.Content = "";
            txtTime1.Text = "";
            txtTime2.Text = "";
        }

        private void txtTime1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");

            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtTime2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");

            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void btnTable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnChart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
