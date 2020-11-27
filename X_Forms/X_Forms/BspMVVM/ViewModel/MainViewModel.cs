﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using X_Forms.BspMVVM.Model;
using X_Forms.BspMVVM.Service;
using Xamarin.Forms;

namespace X_Forms.BspMVVM.ViewModel
{
    //Das ViewModel dient als Verbindungsklasse zwischen einem View und den Model- und Controllerklassen.
    //Mittels des INotifyPropertyChanged-Interfaces kann das View über Property-Veränderungen informiert werden.
    public class MainViewModel : INotifyPropertyChanged
    {
        //Datenbank-Service (sollte als Daten-Objekt eigendlich nicht direkt im ViewModel liegen. 
        //Mögliche Implementierungen: als Statische Klasse oder evtl in der App.xaml)
        public DbService Datenbank { get; set; }

        //Properties zum Anbinden an das View (.NET-Properties benötigen bei Veränderung einen Eventwurf, um das View 
        //über die Veränderung zu informieren
        public string NeuerName { get; set; }
        public int NeuesAlter { get; set; }
        public bool IsRefreshing { get; set; }

        //Die PersonenListe verweist auf die im Model gespeicherte Liste
        public ObservableCollection<Person> PersonenListe
        {
            get { return Model.Person.PersonenListe; }
            set { Model.Person.PersonenListe = value; }
        }

        //Command-Properties werden benötigt, um Eventwürfe auf EventHandler umzuleiten
        public Command HinzufuegenCmd { get; set; }
        public Command LoeschenCmd { get; set; }
        public Command RefreshCmd { get; set; }

        //zugehöriges View (zum Zugriff auf Page-Methoden wie z.B. DisplayAlert)
        public Page ContextView { get; set; }

        //Konstruktor
        public MainViewModel()
        {
            //Property-Initialisierungen
            Datenbank = new DbService();
            PersonenListe = new ObservableCollection<Person>(Datenbank.GetPeople());

            //Initialisierung der Command-Objekte mit Übergabe des auszuführenden EventHandlers
            HinzufuegenCmd = new Command(AddPerson);
            LoeschenCmd = new Command(DeletePerson);
            RefreshCmd = new Command(RefreshList);
        }

        //Durch das Interface gefordertes Event
        public event PropertyChangedEventHandler PropertyChanged;

        //EventHandler-Methoden
        private void RefreshList()
        {
            //Methode zur Aktualisierung des ListViews
            PersonenListe = new ObservableCollection<Person>(Datenbank.GetPeople());
            IsRefreshing = false;

            //Informieren das Views über Veränderung in den Properties
            InformView(nameof(IsRefreshing));
            InformView(nameof(PersonenListe));
        }

        private void AddPerson()
        {
            //Erstellen und Hinzufügen einer neuen Person
            Person person = new Person()
            {
                Name = NeuerName,
                Alter = NeuesAlter
            };
            PersonenListe.Add(person);
            Datenbank.AddPerson(person);

            //Leeren der UI-Elemente
            NeuerName = string.Empty;
            NeuesAlter = 0;

            //Informieren das Views über Veränderung in den Properties
            InformView(nameof(NeuerName));
            InformView(nameof(NeuesAlter));
        }

        private async void DeletePerson(object person)
        {
            //Aufruf einer 'MessageBox' und Abfrage der User-Antwort über ContextPage-Property
            bool result = await ContextPage.DisplayAlert("Löschen", "Soll diese Person wirklich gelöscht werden?", "Ja", "Nein");

            if (result)
            {
                Datenbank.DeletePerson(person as Person);
                PersonenListe.Remove(person as Person);
            }
        }

        //Methode, welche das View über Veränderungen informiert
        private void InformView(string prop)
        {
            //Aufruf des PropertyChanged-Events um das View über die Veränderung zu informieren
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
