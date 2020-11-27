using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace X_Forms
{
    public partial class MainPage : ContentPage
    {
        //Property zum Zwischenspeichern der Personenliste (ObservableCollection ist eine generische Liste, welche die GUI
        //automatisch über Veränderungen informiert
        public ObservableCollection<string> NamensListe { get; set; }

        //Konstruktor
        public MainPage()
        {
            //Zugriff auf die Battery-Klasse aus Xamarin.Essentials zum Zugriff auf den Batteriestatus
            Properties.Resources.Culture = new System.Globalization.CultureInfo("de");

            //Initialisierung der UI (Xaml-Datei). Sollte immer erste Aktion des Konstruktors sein
            InitializeComponent();

            //Zuweisung von EventHandlern zu den Completed-Events, damit ein besserer Bedienfluss gegeben ist
            Ent_Vorname.Completed += (s, e) => Ent_Nachname.Focus();
            Ent_Nachname.Completed += Btn_Show_Clicked;

            //Initialisierung der Namesliste
            NamensListe = new ObservableCollection<string>()
            {
                "Anna Nass",
                "Rainer Zufall"
            };

            //Durch Setzen des BindingContextes nehmen Kurzbindungen aus dem XAML-Code automatisch Bezug auf die Properties
            //des im BindingContext gesetzten Objekts
            this.BindingContext = this;

            //Zugriff auf die Battery-Klasse aus Xamarin.Essentials zum Zugriff auf den Batteriestatus (benötigt Permission in OS-Projekten)
            Lbl_Battery.Text = Battery.State.ToString() + " | State: " + Battery.ChargeLevel * 100 + "%";

            //Zuweisung von Sprachressourcen an UI-Elemente als Alternative zur x:Static-Bindung (vgl. Resource.resx und Resource.de.resx)
            //Btn_Localisation.Text = Resource.String_Btn;
            //Lbl_Localisation.Text = Resource.String_Lbl;
        }

        //EventHandler
        private async void Btn_KlickMich_Clicked(object sender, EventArgs e)
        {
            //Neuzuweisung einer UI-Property über die x:Name-Property des Steuerelements
            Lbl_Main.Text = "Danke, dass du auf den Button geklickt hast.";

            //Anzeige einer 'MessageBox' und Abfrage der User-Antwort
            bool erg = await DisplayAlert("Begrüßung", $"Hallo, {Ent_Vorname.Text} {Ent_Nachname.Text}. Geht es dir gut?", "Ja", "Nein");

            if (erg)
                Lbl_Main.Text = "Das ist schön";
            else
                Lbl_Main.Text = "Bald wirds besser";
        }

        private void Btn_Show_Clicked(object sender, EventArgs e)
        {
            if (Ent_Vorname.Text != String.Empty && Ent_Nachname.Text != String.Empty)

                //Erstellen eines neuen Listenelements (aus UI-Properties)
                NamensListe.Add(Ent_Vorname.Text + " " + Ent_Nachname.Text);
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            //Anzeige einer 'MessageBox' und Abfrage der User-Antwort
            bool result = await DisplayAlert("Löschung", "Soll diese Person wirklich gelöscht werden?", "Ja", "Nein");

            if (result)
            {
                //Löschen eines Listeneintrags
                string person = (sender as MenuItem).CommandParameter.ToString();

                NamensListe.Remove(person);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //Aufruf einer neuen Seite innerhalb der aktuellen NavigationPage 
            Navigation.PushAsync(new Navigation.TabbedBsp());
        }
    
        private void Button2_Clicked(object sender, EventArgs e)
        {
            //Aufruf einer neuen Seite innerhalb der aktuellen NavigationPage, welche aber keinen 'Zurück'-Button anzeigt
            Navigation.PushModalAsync(new Layouts.GridLay());
        }

        private void Button3_Clicked(object sender, EventArgs e)
        {
            //Aufruf einer neuen Seite innerhalb der aktuellen NavigationPage 
            Navigation.PushAsync(new Navigation.CarouselBsp());
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            NamensListe.Clear();
        }

        private void Btn_MC_Clicked(object sender, EventArgs e)
        {
            //Mittels des MessagingCenters können zwei voneinander unabhängige Objekte mittels eines Sender/Subscriber-Prinzips
            //miteinander kommunizieren

            //Instanziierung des Emfänger-Objekts (dieses muss zum Zeitpunkt der Nachricht-Sendes bereits existieren)
            Page page = new MC_SubscriberPage();

            //Senden der Nachricht inkl. Sender, Titel und Inhalt
            MessagingCenter.Send(this, "Gesendeter Text", Ent_Vorname.Text);

            //Navigation zum Empfänger-Objekt (vgl. MC_SubscriberPage.xaml)
            Navigation.PushAsync(page);
        }

        private async void Btn_Youtube_Clicked(object sender, EventArgs e)
        {
            //Öffnen der Youtube-App über die Launcher-Klasse von Xamarin.Essentials per URI (Packagename://ÜbergebeneDaten <- Hier das Youtube-Video)
            if (await Launcher.CanOpenAsync("vnd.youtube://"))
                await Launcher.OpenAsync("vnd.youtube://rLKnqR9Oqh8");
        }
    }
}
