﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Fietsen.Lib.Entities;

namespace Bikaficionado.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<FietsWinkel> winkels;
        FietsWinkel huidigeFietsWinkel;
        Fiets huidigeFiets;

        public MainWindow()
        {
            InitializeComponent();
            winkels = new List<FietsWinkel>
            {
                new FietsWinkel("Het velootje")
            };
            huidigeFietsWinkel = winkels[0];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KoppelLstFietsen();
            cmbWinkel.ItemsSource = winkels;
            cmbWinkel.SelectedIndex = 0;
                       
            for (int i = 0; i < 5; i++)
            {
                cmbAantalWielen.Items.Add(i);
            }
            ClearPanel(grdInput);
        }



        private void lstFietsen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFietsen.SelectedItem != null)
            {
                huidigeFiets = (Fiets)lstFietsen.SelectedItem;
                txtMerk.Text = huidigeFiets.Merk;
                dtpAankoopDatum.SelectedDate = huidigeFiets.AankoopDatum;
                chkElektrisch.IsChecked = huidigeFiets.IsElektrisch;
                cmbAantalWielen.SelectedItem = huidigeFiets.AantalWielen;
                txtSnelheid.Text = huidigeFiets.Snelheid.ToString();
            }
            else
            {
                ClearPanel(grdInput);
                huidigeFiets = null;
            }
        }

        private void btnVoegWinkelToe_Click(object sender, RoutedEventArgs e)
        {
            string fietsWinkelNaam = txtWinkelNaam.Text;
            try
            {
                FietsWinkel nieuweFietsWinkel = new FietsWinkel(fietsWinkelNaam, false);
                winkels.Add(nieuweFietsWinkel);

                cmbWinkel.Items.Refresh();
                cmbWinkel.SelectedItem = nieuweFietsWinkel;

                txtWinkelNaam.Clear();
                ToonMelding("De winkel is aangemaakt", true);
                txtWinkelNaam.Clear();
            }
            catch (Exception ex)
            {

                ToonMelding(ex.Message);
            }
        }

        private void cmbWinkel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            huidigeFietsWinkel = (FietsWinkel)cmbWinkel.SelectedItem;
            KoppelLstFietsen();
            lstFietsen.SelectedItem = null;
        }

        private void KoppelLstFietsen()
        {
            lstFietsen.ItemsSource = huidigeFietsWinkel.Fietsen;
            lstFietsen.Items.Refresh();
        }

        private void btnSlaOp_Click(object sender, RoutedEventArgs e)
        {
            string merk = txtMerk.Text;
            int wielen = (int)cmbAantalWielen.SelectedItem;
            bool? aangevinkt = chkElektrisch.IsChecked;
            DateTime? aangekocht = dtpAankoopDatum.SelectedDate;
            float snelheid = 0;
            try
            {
                snelheid = float.Parse(txtSnelheid.Text);
                tbkFeedBack.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {
                ToonMelding("De input in de snelheid is ongeldig");
            }
            try
            {
                Guid? id = (huidigeFiets == null) ?
                    null :
                    (Guid?)huidigeFiets.Id;

                Fiets fiets = new Fiets(merk, snelheid, wielen, (bool)aangevinkt, aangekocht, id);
                if(huidigeFietsWinkel.SlaOp(fiets))
                {
                    tbkFeedBack.Visibility = Visibility.Hidden;
                    KoppelLstFietsen();
                    lstFietsen.SelectedIndex = -1;
                    ClearPanel(grdInput);
                }
                else
                {
                    ToonMelding("De fiets bestaat reeds");
                }
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
            }
        }

        private void btnNieuw_Click(object sender, RoutedEventArgs e)
        {
            lstFietsen.SelectedItem = null;
            txtMerk.Focus();
        }

        private void btnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            if (lstFietsen.SelectedItem != null)
            {
                huidigeFietsWinkel.Verwijder((Fiets)lstFietsen.SelectedItem);

            }
        }
    }
}
