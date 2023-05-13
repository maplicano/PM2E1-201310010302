using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public class AddSite : ContentPage
    {
        byte[] cam_image;

        private Entry descEntry;
        private Entry montoEntry;
        private DatePicker fechaEntry;

        private Image imagePreview;

        private Button cam_imgButton;
        private Button saveButton;

        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDbSite.db3");

        public AddSite()
        {
            this.Title = "Añadir Sitio";
            StackLayout stackLayout = new StackLayout();

            //Camera Btn
            cam_imgButton = new Button();
            cam_imgButton.Text = "Tomar Foto";
            cam_imgButton.Clicked += cam_imgButton_Clicked;
            stackLayout.Children.Add(cam_imgButton);

            //Image Preview
            imagePreview = new Image();
            stackLayout.Children.Add(imagePreview);


            // monto Entry
            montoEntry = new Entry();
            montoEntry.Placeholder = "Monto";
            montoEntry.IsEnabled = true;
            stackLayout.Children.Add(montoEntry);

            // fecha Entry
            fechaEntry = new DatePicker();
            fechaEntry.IsEnabled = true;
            stackLayout.Children.Add(fechaEntry);

            // Descripcion Entry
            descEntry = new Entry();
            descEntry.Keyboard = Keyboard.Text;
            descEntry.Placeholder = "Descripción";
            stackLayout.Children.Add(descEntry);

            //Save Btn
            saveButton = new Button();
            saveButton.Text = "Guardar";
            saveButton.Clicked += saveButton_Clicked;
            stackLayout.Children.Add(saveButton);

            Content = stackLayout;
          
        }

        private async void cam_imgButton_Clicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.CapturePhotoAsync();
            var stream = await result.OpenReadAsync();
            imagePreview.Source = ImageSource.FromStream(() => stream);

            using (MemoryStream memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                cam_image = memory.ToArray();
            }
        }


        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Pagos>();

            var maxPK = db.Table<Pagos>().OrderByDescending(c => c.Id).FirstOrDefault();

            if (!string.IsNullOrEmpty(descEntry.Text))
            {
                Pagos sitio = new Pagos()
                {
                    Id = (maxPK == null ? 1 : maxPK.Id + 1),
                    desc = descEntry.Text,
                   monto = montoEntry.Text,
                    fecha = DateTime.UtcNow.Date,
                    save_image = cam_image
                };

                db.Insert(sitio);
                await DisplayAlert(null, "Sitio: " + sitio.desc + " Guardado!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "OK");
            }
        }
    }
}