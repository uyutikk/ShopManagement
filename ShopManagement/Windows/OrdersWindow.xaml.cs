using ShopManagement;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopManagement
{
    public partial class OrdersWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.OrdersTableAdapter ordersAdapter;
        private ShopDataSetTableAdapters.CustomersTableAdapter customersAdapter;
        private ShopDataSetTableAdapters.ProductsTableAdapter productsAdapter;

        public OrdersWindow()
        {
            InitializeComponent();
            shopDataSet = new ShopDataSet();
            ordersAdapter = new ShopDataSetTableAdapters.OrdersTableAdapter();
            customersAdapter = new ShopDataSetTableAdapters.CustomersTableAdapter();
            productsAdapter = new ShopDataSetTableAdapters.ProductsTableAdapter();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                ordersAdapter.Fill(shopDataSet.Orders);
                customersAdapter.Fill(shopDataSet.Customers);
                productsAdapter.Fill(shopDataSet.Products);
                OrdersDataGrid.ItemsSource = shopDataSet.Orders.DefaultView;
                StatusTextBlock.Text = "Данные загружены";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrdersWindow addWindow = new AddOrdersWindow(shopDataSet, ordersAdapter, customersAdapter, productsAdapter);
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
                StatusTextBlock.Text = "Заказ добавлен";
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)OrdersDataGrid.SelectedItem;
                EditOrdersWindow editWindow = new EditOrdersWindow(shopDataSet, ordersAdapter, customersAdapter, productsAdapter, selectedRow.Row);
                if (editWindow.ShowDialog() == true)
                {
                    LoadData();
                    StatusTextBlock.Text = "Заказ обновлен";
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить заказ?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)OrdersDataGrid.SelectedItem;
                        selectedRow.Row.Delete();
                        ordersAdapter.Update(shopDataSet.Orders);
                        LoadData();
                        StatusTextBlock.Text = "Заказ удален";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void OrdersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem != null)
            {
                EditButton_Click(sender, e);
            }
        }
    }
}