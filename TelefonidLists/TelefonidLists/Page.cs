using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;
using static CountryLists.Page;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;

namespace CountryLists
{
    public partial class Page : ContentPage
    {
        public class Country
        {
            public string Nimi { get; set; }
            public string Capital { get; set; }
            public string People { get; set; }
            public string Picture { get; set; }
        }
        Label lbl;
        ListView lv;
        List<Country> countryd;
        
        public Page()
        {
            //Preferences.Set("SavedList", "");
            LoadPhones();
            lbl = new Label
            {
                Text = "Euroopa riikide loetelu",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            };
            lv = new ListView
            {
                SeparatorColor = Color.Red,
                Header = "Euroopa riikid",
                Footer = DateTime.Now.ToString("g"),
                HasUnevenRows = true,
                /*ItemTemplate = new DataTemplate (() =>
                {
                    Label nimetus = new Label { FontSize = 20 };
                    nimetus.SetBinding(Label.TextProperty, "Nimi");

                    Label hind = new Label();
                    hind.SetBinding(Label.TextProperty, "Hind");

                    Image img = new Image();
                    img.SetBinding(Image.SourceProperty, "Picture");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            Children = { nimetus, img, hind }
                        }
                    };
                }),*/
                ItemTemplate = new DataTemplate(() =>
                {
                    ImageCell imgC = new ImageCell { TextColor = Color.Red, DetailColor = Color.Green };
                    imgC.SetBinding(ImageCell.TextProperty, "Nimi");
                    Binding comp = new Binding { Path = "Capital", StringFormat = "Tore riik, pealinnaga {0}" };
                    imgC.SetBinding(ImageCell.DetailProperty, comp);
                    imgC.SetBinding(ImageCell.ImageSourceProperty, "Picture");
                    return imgC;
                }),
                ItemsSource = countryd,
            };
            Button add = new Button
            {
                Text = "Lisa riik",
            };
            Button remove = new Button
            {
                Text = "Kustuta riik",
            };
            add.Clicked += async (e, v) => {
                List<Country> countryd = Prefs.SavedList;
                //countryd.Add(new Country { Nimi = "Riik", Capital = "Capital", People = "0", Picture = "link" });
                MyPopupPage popup = new MyPopupPage();
                await PopupNavigation.Instance.PushAsync(popup);
                if (await popup.PopupClosedTask != null)
                {
                    countryd.Add(popup.PopupClosedTask.Result);
                    SaveCountries(countryd);
                }
            };
            remove.Clicked += async (e, v) =>
            {
                List<Country> countryd = Prefs.SavedList;
                string action = await DisplayActionSheet("Kustuta riik", "Cancel", null, countryd.Select(device => device.Nimi).ToArray());
                List<Country> removable = countryd.Where(i => i.Nimi == action).ToList();
                if (!countryd.Remove(removable[0]))
                    throw new Exception();
                SaveCountries(countryd);
            };
            lv.ItemTapped += Lv_ItemTapped;
            Content = new StackLayout { Children = { lbl, lv, add, remove} };
        }

        private async void Lv_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Country sel = e.Item as Country;
            if (sel != null)
                await DisplayAlert("Valitud riik: ", $"{sel.Capital} {sel.Nimi}", "OK");
        }

        private void LoadPhones()
        {
            if (Prefs.SavedList.Count <= 0)
            {
                countryd = new List<Country>
                {
                    new Country{ Nimi = "Eesti", Capital = "Tallinn", People = "600", Picture = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8f/Flag_of_Estonia.svg/1200px-Flag_of_Estonia.svg.png"},
                    new Country{ Nimi = "Läti", Capital = "Riga", People = "2000", Picture = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fe/Flag_of_Latvia_with_border.svg/1280px-Flag_of_Latvia_with_border.svg.png"},
                    new Country{ Nimi = "Leedu", Capital = "Vilnus", People = "1500", Picture = "https://www.adaur.ee/wp-content/2018/02/Leedu-lipp.png"},
                    new Country{ Nimi = "Soome", Capital = "Helsinki", People = "300", Picture = "https://www.eures.ee/sites/eures.ee/files/2018-09/640px-Flag_of_Finland.svg_.png"},
                };
            }
            else { 
                countryd = new List<Country>(Prefs.SavedList);
            }
            SaveCountries(countryd);
        }
        private void SaveCountries(List<Country>  countryd)
        {
            var savedList = new List<Country>();
            foreach (Country country in countryd)
            {
                savedList.Add(country);
            }
            Prefs.SavedList = countryd;
            if (lv != null)
                lv.ItemsSource = countryd;
        }   
    }
    static class Prefs
    {
        public static List<Country> SavedList
        {
            get
            {
                var savedList = Deserialize<List<Country>>(Preferences.Get(nameof(SavedList), ""));
                return savedList ?? new List<Country>();
            }
            set
            {
                var serializedList = Serialize(value);
                Preferences.Set(nameof(SavedList), serializedList);
            }
        }

        static T Deserialize<T>(string serializedObject) => JsonConvert.DeserializeObject<T>(serializedObject, new Newtonsoft.Json.JsonSerializerSettings
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
        });

        static string Serialize<T>(T objectToSerialize) => JsonConvert.SerializeObject(objectToSerialize);
    }
}