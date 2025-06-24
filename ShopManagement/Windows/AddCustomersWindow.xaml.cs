using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;

namespace ShopManagement
{
    public partial class AddCustomersWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.CustomersTableAdapter customersAdapter;

        public AddCustomersWindow(ShopDataSet dataSet, ShopDataSetTableAdapters.CustomersTableAdapter adapter)
        {
            InitializeComponent();
            shopDataSet = dataSet;
            customersAdapter = adapter;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    DataRow newRow = shopDataSet.Customers.NewRow();
                    newRow["FirstName"] = FirstNameTextBox.Text;
                    newRow["LastName"] = LastNameTextBox.Text;
                    newRow["Phone"] = PhoneTextBox.Text;
                    newRow["Email"] = EmailTextBox.Text;
                    shopDataSet.Customers.Rows.Add(newRow);
                    customersAdapter.Update(shopDataSet.Customers);
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) || string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Имя и фамилия не могут быть пустыми", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(PhoneTextBox.Text, @"^\+?\d{10,15}$"))
            {
                MessageBox.Show("Некорректный формат телефона", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.IsNullOrEmpty(EmailTextBox.Text) &&
                !Regex.IsMatch(EmailTextBox.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                MessageBox.Show("Некорректный формат email", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}