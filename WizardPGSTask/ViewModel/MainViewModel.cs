using Prism.Commands;
using Prism.Mvvm;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace WizardPGSTask.ViewModel
{
    /// <summary>
    /// Wizard-like application made for PGS Software recruitment task by Barbara Kemska.
    /// In this project Prism framework is used in order to keep view and code separation.
    /// Architecture is similar to MVVM architecture - project doesn't contain Model, but it's easy to add in case of further development.
    /// </summary>
    class MainViewModel : BindableBase
    {
        #region propertiesAndCommands

        /// <summary>
        /// Property Name binded to "Imię" textbox.
        /// </summary>
        private string name;        public string Name
        {
            get { return name; }
            set
            {
                if (Regex.IsMatch(value, @"^[a-ząćęłńóśżźA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                { SetProperty(ref name, value); tabFilled[0] = true;  }
                else
                { SetProperty(ref name, ""); tabFilled[0] = false; } 
                (NextTabCommand as DelegateCommand).RaiseCanExecuteChanged(); }
            }

        /// <summary>
        /// Property Surname binded to "Nazwisko" textbox.
        /// </summary>
        private string surname;
        public string Surname
        {
            get { return surname; }
            set
            {
                if (Regex.IsMatch(value, @"^[a-ząćęłńóśżźA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                { SetProperty(ref surname, value); tabFilled[1] = true; }
                else
                { SetProperty(ref surname, ""); tabFilled[1] = false; } 
                (NextTabCommand as DelegateCommand).RaiseCanExecuteChanged(); }
        }

        /// <summary>
        /// Property City binded to "Miasto" textbox.
        /// </summary>
        private string city="";
        public string City
        {
            get { return city; }
            set
            {
                if (Regex.IsMatch(value, @"^[a-ząćęłńóśżźA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                    SetProperty(ref city, value);
                else SetProperty(ref city, "");
                tabFilled[2] = checkAddress();
                (NextTabCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Property Street binded to "Ulica" textbox.
        /// </summary>
        private string street="";
        public string Street
        {
            get { return street; }
            set
            {
                if (Regex.IsMatch(value, @"^[a-ząćęłńóśżźA-ZĄĆĘŁŃÓŚŹŻ0-9]+$"))
                    SetProperty(ref street, value);
                else SetProperty(ref street, "");
                tabFilled[2] = checkAddress();
                (NextTabCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Property HouseNumber binded to "Budynek" textbox.
        /// </summary>
        private string houseNumber="";
        public string HouseNumber
        {
            get { return houseNumber; }
            set
            {
                if (Regex.IsMatch(value, @"^[a-ząćęłńóśżźA-ZĄĆĘŁŃÓŚŹŻ0-9]+$"))
                    SetProperty(ref houseNumber , value);
                else SetProperty(ref houseNumber, "");
                tabFilled[2] = checkAddress();
                (NextTabCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Property FlatNumber binded to "Mieszkanie" textbox.
        /// </summary>
        private string flatNumber="";
        public string FlatNumber
        {
            get { return flatNumber; }
            set
            {
                if (Regex.IsMatch(value, @"^[a-ząćęłńóśżźA-ZĄĆĘŁŃÓŚŹŻ0-9]+$"))
                    SetProperty(ref flatNumber, value);
                else { SetProperty(ref flatNumber, ""); }
                tabFilled[2] = checkAddress();
                (NextTabCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Property Phone binded to NumerTelefonu textbox.
        /// </summary>
        private string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                if (Regex.IsMatch(value, @"^[0-9+-]+$"))
                {
                    SetProperty(ref phone, value);
                    tabFilled[3] = true;
                }
                else
                {
                    SetProperty(ref phone, "");
                    tabFilled[3] = false;
                } 
                (NextTabCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Property Address binded to "Adres" textbox.
        /// </summary>
        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        /// <summary>
        /// Properties binded to tabs defining if tabs are enabled (true) or disabled(false).
        /// </summary>
        private bool nameTab = true;
        public bool NameTab
        {
            get { return nameTab; }
            set { SetProperty(ref nameTab, value); }
        }

        private bool surnameTab=false;
        public bool SurnameTab
        {
            get { return surnameTab; }
            set { SetProperty(ref surnameTab, value); }
        }

        private bool addressTab = false;
        public bool AddressTab
        {
            get { return addressTab; }
            set { SetProperty(ref addressTab, value); }
        }

        private bool phoneTab = false;
        public bool PhoneTab
        {
            get { return phoneTab; }
            set { SetProperty(ref phoneTab, value); }
        }

        /// <summary>
        /// Buttons "Wstecz"(prev) and "Dalej"(Next) properties: labels and visibilities.
        /// </summary>
        private bool prevButtonVisibility =false;
        public bool PrevButtonVisibility
        {
            get { return prevButtonVisibility; }
            set { SetProperty(ref prevButtonVisibility, value); }
        }

        private string nextButtonLabel="Dalej >>";
        public string NextButtonLabel
        {
            get { return nextButtonLabel; }
            set { SetProperty(ref nextButtonLabel, value); }
        }

        private bool nextButtonVisibility = true;
        public bool NextButtonVisibility
        {
            get { return nextButtonVisibility; }
            set { SetProperty(ref nextButtonVisibility, value); }
        }

        /// <summary>
        /// Index of selected tab. When changed, buttons commands are raising.
        /// </summary>
        private int selectedIndex=0;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value == 3) NextButtonLabel = "Podsumowanie"; else NextButtonLabel = "Dalej >>";
                if (value == 4) completeAddress();
                if (value >= 0 && value <= 4)
                {
                    SetProperty(ref selectedIndex, value);
                    (PrevTabCommand as DelegateCommand).RaiseCanExecuteChanged();
                    (NextTabCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }

        }

        /// <summary>
        /// Array specifying if tab is correctly filled.
        /// </summary>
        private bool[] tabFilled = new bool[4];

        /// <summary>
        /// Button commands.
        /// </summary>
        public ICommand PrevTabCommand { get; set; }
        public ICommand NextTabCommand { get; set; }
        public ICommand CloseTabCommand { get; set; }

        #endregion

        public MainViewModel()
        {
            PrevTabCommand = new DelegateCommand(PrevTabCommandExecute, PrevTabCommandCanExecute);
            NextTabCommand = new DelegateCommand(NextTabCommandExecute, NextTabCommandCanExecute);
            CloseTabCommand = new DelegateCommand(CloseTabCommandExecute);
        }

        /// <summary>
        /// Closing application command.
        /// </summary>
        private void CloseTabCommandExecute()
        {
            Application.Current.Windows[0].Close();
        }

        /// <summary>
        /// Switching tab for next one.
        /// </summary>
        private void NextTabCommandExecute()
        {
            SelectedIndex++;
            updateTabs();
        }

        /// <summary>
        /// Checking if next tab can be activated - if selected tab is last one, return false. 
        /// </summary>
        /// <returns> Bool value specyfing if tab can be switched to next.</returns>
        private bool NextTabCommandCanExecute()
        {
            updateTabs();
            if (selectedIndex == 4) { NextButtonVisibility = false; return false; }
            else {NextButtonVisibility = true; return (tabFilled[selectedIndex]);  }
        }

        /// <summary>
        /// Switching tab to previous.
        /// </summary>
        private void PrevTabCommandExecute()
        {
            SelectedIndex--;
        }

        /// <summary>
        /// Checking if tab can be switched to previous. If active tab is first or last ("Summary"), then return false.
        /// </summary>
        /// <returns>Bool alue specyfing if tab can be switched to previous.</returns>
        private bool PrevTabCommandCanExecute()
        {
            if (selectedIndex == 0 || selectedIndex == 4) { PrevButtonVisibility=false; return false; }
            else { PrevButtonVisibility = true; return true; }
        }

        /// <summary>
        /// Function checks if address is complete - contains cityy, street, house number (flat number is optional).
        /// </summary>
        /// <returns>Bool alue specyfing if address is valid.</returns>
        private bool checkAddress()
        {
            return (City.Length > 0 && Street.Length > 0 && HouseNumber.Length > 0);
        }

        /// <summary>
        /// Specyfing enabled and disabled tabs.
        /// </summary>
        private void updateTabs()
        {
            SurnameTab = tabFilled[1] && tabFilled[0];
            AddressTab = tabFilled[2] && SurnameTab;
            PhoneTab = tabFilled[3] && AddressTab;
            if (SelectedIndex == 4) NameTab = SurnameTab = AddressTab = PhoneTab = false;
        }

        /// <summary>
        /// Completing address in format: "Miasto, ul. nrBudynku/nrMieszkania" - "City, street HouseNumber/FlatNumber".
        /// </summary>
        private void completeAddress()
        {
            Address = string.Concat(City, ", ul. ", Street, " ", HouseNumber, (FlatNumber.Length > 0 ? "/"+FlatNumber : ""));
        }

    }
}
