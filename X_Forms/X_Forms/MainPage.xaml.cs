using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace X_Forms
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> NamensListe { get; set; }

        public MainPage()
        {
            InitializeComponent();

            Ent_Vorname.Completed += (s, e) => Ent_Nachname.Focus();
            Ent_Nachname.Completed += Btn_Show_Clicked;

            NamensListe = new ObservableCollection<string>()
            {
                "Anna Nass",
                "Rainer Zufall"
            };

            this.BindingContext = this;
        }

        private async void Btn_KlickMich_Clicked(object sender, EventArgs e)
        {
            Lbl_Main.Text = "Danke, dass du auf den Button geklickt hast.";

            bool erg = await DisplayAlert("Begrüßung", $"Hallo, {Ent_Vorname.Text} {Ent_Nachname.Text}. Geht es dir gut?", "Ja", "Nein");

            if (erg)
                Lbl_Main.Text = "Das ist schön";
            else
                Lbl_Main.Text = "Bald wirds besser";
        }

        private void Btn_Show_Clicked(object sender, EventArgs e)
        {
            if (Ent_Vorname.Text != String.Empty && Ent_Nachname.Text != String.Empty)
                NamensListe.Add(Ent_Vorname.Text + " " + Ent_Nachname.Text);
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            string person = (sender as MenuItem).CommandParameter.ToString();

            NamensListe.Remove(person);
        }
    }
}
