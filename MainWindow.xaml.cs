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
        public MainWindow()
        {
            InitializeComponent();
            dt = new DataTable();
            dt.Columns.Add("Vreme (s)");
            dt.Columns.Add("Potrošnja (l/min)");
            dataGridView1.ItemsSource = dt.DefaultView;

            /*   //chart
               SfChart chart = new SfChart();
               chart.Header = "Chart";
               CategoryAxis primaryAxis = new CategoryAxis();
               primaryAxis.Header = "Name";
               primaryAxis.FontSize = 14;
               chart.PrimaryAxis = primaryAxis;

               NumericalAxis secondaryAxis = new NumericalAxis();
               secondaryAxis.Header = "Height(in cm)";
               secondaryAxis.FontSize = 14;
               chart.SecondaryAxis = secondaryAxis;
               ColumnSeries columnSeries = new ColumnSeries();
               columnSeries.XBindingPath = "Name";
               columnSeries.YBindingPath = "Height";
               columnSeries.ItemsSource = (chart.DataContext as ViewModel).Data;
               columnSeries.Label = "Height";
               chart.Series.Add(columnSeries);*/
        }


        public class Podatak
        {
            public string Vreme { get; set; }
            public string Vrednost { get; set; }
        }
        private object txtFilename;

        public List<Podatak> podaci = new List<Podatak>();


        DataTable dt;
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

                        DataRow dr = dt.NewRow();

                        dr[0] = podaci[i - 2].Vreme.ToString();
                        dr[1] = podaci[i - 2].Vrednost.ToString();

                        dt.Rows.Add(dr);
                        MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(btnUpload, false);



                    }


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
            dt.Clear();
            podaci.Clear();
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
