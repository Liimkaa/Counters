using System.Collections.ObjectModel;
using System.Text.Json;

namespace counter
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<CounterItem> Counters { get; set; } = new ObservableCollection<CounterItem>();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            string savedData = Preferences.Get("MojeLiczniki", "");
            if (!string.IsNullOrEmpty(savedData))
            {
                var list = JsonSerializer.Deserialize<List<CounterItem>>(savedData);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        Counters.Add(item);
                    }
                }
            }
        }

        private void OnPlusClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (CounterItem)button.CommandParameter;
            item.Value++;
            Save();
        }

        private void OnMinusClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (CounterItem)button.CommandParameter;
            item.Value--;
            Save();
        }

        private async void OnAddCounterClicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Nowy licznik", "Wpisz nazwę:");

            if (!string.IsNullOrEmpty(name))
            {
                string Svalue = await DisplayPromptAsync("Nowy licznik", "Wpisz wartość początkową:", initialValue: "0", keyboard: Keyboard.Numeric);

                int value = 0;
                int.TryParse(Svalue, out value);

                CounterItem newItem = new CounterItem();
                newItem.Name = name;
                newItem.Value = value;

                Counters.Add(newItem);
                Save();
            }
        }

        private void Save()
        {
            string json = JsonSerializer.Serialize(Counters);
            Preferences.Set("MojeLiczniki", json);
        }
    }
}