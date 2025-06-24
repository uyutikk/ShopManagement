using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;

namespace ShopManagement
{
    public partial class EditCustomersWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.CustomersTableAdapter customersAdapter;
        private DataRow selectedRow;

        public EditCustomersWindow(ShopDataSet dataSet, ShopDataSetTableAdapters.CustomersTableAdapter adapter, DataRow row)
        {
            InitializeComponent();
            shopDataSet = dataSet;
            customersAdapter = adapter;
            selectedRow = row;

            FirstNameTextBox.Text = selectedRow["FirstName"].ToString();
            LastNameTextBox.Text = selectedRow["LastName"].ToString();
            PhoneTextBox.Text = selectedRow["Phone"].ToString();
            EmailTextBox.Text = selectedRow["Email"].ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    selectedRow["FirstName"] = FirstNameTextBox.Text;
                    selectedRow["LastName"] = LastNameTextBox.Text;
                    selectedRow["Phone"] = PhoneTextBox.Text;
                    selectedRow["Email"] = EmailTextBox.Text;
                    customersAdapter.Update(shopDataSet.Customers);
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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