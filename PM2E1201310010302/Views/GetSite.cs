using PM2E1201610010417.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PM2E1201610010417.Views
{
    public class GetSite : ContentPage
    {
        private ListView listView;

        private Entry idEntry;
        private Entry descEntry;
        private Entry montoEntry;
        private DatePicker fechaEntry;

        private Button mapButton;
        private Button editButton;
        private Button deleteButton;

        Pagos sitio = new Pagos();

        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDbSite.db3");
        public GetSite()
        {
            this.Title = "Sitios";

            var db = new SQLiteConnection(dbPath);

            StackLayout stackLayout = new StackLayout();

            listView = new ListView();
            listView.ItemsSource = db.Table<Pagos>().OrderBy(x => x.Id).ToList();
            listView.ItemSelected += listViewItem;
            stackLayout.Children.Add(listView);

            // ID Entry
            idEntry = new Entry();
            idEntry.Placeholder = "ID";
            idEntry.IsVisible = false;
            stackLayout.Children.Add(idEntry);

            // Descripcion Entry
            descEntry = new Entry();
            descEntry.Keyboard = Keyboard.Text;
            descEntry.Placeholder = "Descripción";
            stackLayout.Children.Add(descEntry);

            // Latitud Entry
            montoEntry = new Entry();
            montoEntry.Placeholder = "Latitud";
            montoEntry.IsEnabled = false;
            stackLayout.Children.Add(montoEntry);

            // Longitud Entry
            fechaEntry = new DatePicker();
            stackLayout.Children.Add(fechaEntry);


            //Edit Btn
            editButton = new Button();
            editButton.Text = "Editar";
            editButton.Clicked += editButton_Clicked;
            stackLayout.Children.Add(editButton);
            
            //Delete Btn
            deleteButton = new Button();
            deleteButton.Text = "Eliminar";
            deleteButton.Clicked += deleteButton_Clicked;
            stackLayout.Children.Add(deleteButton);
            
            Content = stackLayout;
        }        

        private void listViewItem(object sender, SelectedItemChangedEventArgs e)
        {
            sitio = (Pagos)e.SelectedItem;

            idEntry.Text = sitio.Id.ToString();
            descEntry.Text = sitio.desc.ToString();
            montoEntry.Text = sitio.monto.ToString();
            fechaEntry.Date = sitio.fecha;
        }

  

        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbPath);

            if (!string.IsNullOrEmpty(descEntry.Text))
            {
                Pagos pago = new Pagos()
                {
                    Id = Convert.ToInt32(idEntry.Text),
                    desc = descEntry.Text,
                    monto = montoEntry.Text,
                    fecha = fechaEntry.Date
                };

                db.Update(pago);
                await DisplayAlert(null, "Sitio: " + sitio.desc + " Editado!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "OK");
            }
        }
        
        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbPath);
            db.Table<Pagos>().Delete(x => x.Id == sitio.Id);
            await DisplayAlert(null, "Sitio: " + sitio.desc + " Eliminado", "OK");
            await Navigation.PopAsync();
        }
        
        
    }
}