using ShopManagement;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopManagement
{
    public partial class CustomersWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.CustomersTableAdapter customersAdapter;

        public CustomersWindow()
        {
            InitializeComponent();
            shopDataSet = new ShopDataSet();
            customersAdapter = new ShopDataSetTableAdapters.CustomersTableAdapter();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                customersAdapter.Fill(shopDataSet.Customers);
                CustomersDataGrid.ItemsSource = shopDataSet.Customers.DefaultView;
                StatusTextBlock.Text = "Данные загружены";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddCustomersWindow addWindow = new AddCustomersWindow(shopDataSet, customersAdapter);
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
                StatusTextBlock.Text = "Клиент добавлен";
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)CustomersDataGrid.SelectedItem;
                EditCustomersWindow editWindow = new EditCustomersWindow(shopDataSet, customersAdapter, selectedRow.Row);
                if (editWindow.ShowDialog() == true)
                {
                    LoadData();
                    StatusTextBlock.Text = "Клиент обновлен";
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить клиента?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)CustomersDataGrid.SelectedItem;
                        selectedRow.Row.Delete();
                        customersAdapter.Update(shopDataSet.Customers);
                        LoadData();
                        StatusTextBlock.Text = "Клиент удален";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void CustomersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem != null)
            {
                EditButton_Click(sender, e);
            }
        }
    }
}