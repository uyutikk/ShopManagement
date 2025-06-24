using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;

namespace ShopManagement
{
    public partial class AddSuppliersWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.SuppliersTableAdapter suppliersAdapter;

        public AddSuppliersWindow(ShopDataSet dataSet, ShopDataSetTableAdapters.SuppliersTableAdapter adapter)
        {
            InitializeComponent();
            shopDataSet = dataSet;
            suppliersAdapter = adapter;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    DataRow newRow = shopDataSet.Suppliers.NewRow();
                    newRow["SupplierName"] = SupplierNameTextBox.Text;
                    newRow["Phone"] = PhoneTextBox.Text;
                    newRow["Email"] = EmailTextBox.Text;
                    shopDataSet.Suppliers.Rows.Add(newRow);
                    suppliersAdapter.Update(shopDataSet.Suppliers);
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
            if (string.IsNullOrWhiteSpace(SupplierNameTextBox.Text))
            {
                MessageBox.Show("Название поставщика не может быть пустым", "Ошибка валидации",
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