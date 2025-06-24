using System;
using System.Data;
using System.Windows;

namespace ShopManagement
{
    public partial class AddOrdersWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.OrdersTableAdapter ordersAdapter;
        private ShopDataSetTableAdapters.CustomersTableAdapter customersAdapter;
        private ShopDataSetTableAdapters.ProductsTableAdapter productsAdapter;
        private static readonly DateTime MinOrderDate = new DateTime(2025, 6, 26);

        public AddOrdersWindow(ShopDataSet dataSet, ShopDataSetTableAdapters.OrdersTableAdapter ordAdapter,
            ShopDataSetTableAdapters.CustomersTableAdapter custAdapter, ShopDataSetTableAdapters.ProductsTableAdapter prodAdapter)
        {
            InitializeComponent();
            shopDataSet = dataSet;
            ordersAdapter = ordAdapter;
            customersAdapter = custAdapter;
            productsAdapter = prodAdapter;

            customersAdapter.Fill(shopDataSet.Customers);
            productsAdapter.Fill(shopDataSet.Products);
            CustomerComboBox.ItemsSource = shopDataSet.Customers.DefaultView;
            ProductComboBox.ItemsSource = shopDataSet.Products.DefaultView;
            OrderDatePicker.DisplayDateStart = MinOrderDate;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    DataRow newRow = shopDataSet.Orders.NewRow();
                    newRow["CustomerID"] = ((DataRowView)CustomerComboBox.SelectedItem)["CustomerID"];
                    newRow["ProductID"] = ((DataRowView)ProductComboBox.SelectedItem)["ProductID"];
                    newRow["OrderDate"] = OrderDatePicker.SelectedDate ?? MinOrderDate;
                    newRow["Quantity"] = int.Parse(QuantityTextBox.Text);
                    newRow["TotalAmount"] = decimal.Parse(TotalAmountTextBox.Text);
                    shopDataSet.Orders.Rows.Add(newRow);
                    ordersAdapter.Update(shopDataSet.Orders);
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
            if (CustomerComboBox.SelectedItem == null || ProductComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента и товар", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (OrderDatePicker.SelectedDate == null || OrderDatePicker.SelectedDate < MinOrderDate)
            {
                MessageBox.Show("Дата заказа не может быть раньше 26 июня 2025 года", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Количество должно быть положительным целым числом", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(TotalAmountTextBox.Text, out decimal total) || total <= 0)
            {
                MessageBox.Show("Сумма должна быть положительным числом", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}