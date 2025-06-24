using System;
using System.Data;
using System.Windows;

namespace ShopManagement
{
    public partial class EditOrdersWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.OrdersTableAdapter ordersAdapter;
        private ShopDataSetTableAdapters.CustomersTableAdapter customersAdapter;
        private ShopDataSetTableAdapters.ProductsTableAdapter productsAdapter;
        private DataRow selectedRow;
        private static readonly DateTime MinOrderDate = new DateTime(2025, 6, 26);

        public EditOrdersWindow(ShopDataSet dataSet, ShopDataSetTableAdapters.OrdersTableAdapter ordAdapter,
            ShopDataSetTableAdapters.CustomersTableAdapter custAdapter, ShopDataSetTableAdapters.ProductsTableAdapter prodAdapter, DataRow row)
        {
            InitializeComponent();
            shopDataSet = dataSet;
            ordersAdapter = ordAdapter;
            customersAdapter = custAdapter;
            productsAdapter = prodAdapter;
            selectedRow = row;

            customersAdapter.Fill(shopDataSet.Customers);
            productsAdapter.Fill(shopDataSet.Products);
            CustomerComboBox.ItemsSource = shopDataSet.Customers.DefaultView;
            ProductComboBox.ItemsSource = shopDataSet.Products.DefaultView;

            CustomerComboBox.SelectedValue = selectedRow["CustomerID"];
            ProductComboBox.SelectedValue = selectedRow["ProductID"];
            OrderDatePicker.SelectedDate = (DateTime)selectedRow["OrderDate"] >= MinOrderDate ? (DateTime)selectedRow["OrderDate"] : MinOrderDate;
            OrderDatePicker.DisplayDateStart = MinOrderDate;
            QuantityTextBox.Text = selectedRow["Quantity"].ToString();
            TotalAmountTextBox.Text = selectedRow["TotalAmount"].ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    selectedRow["CustomerID"] = ((DataRowView)CustomerComboBox.SelectedItem)["CustomerID"];
                    selectedRow["ProductID"] = ((DataRowView)ProductComboBox.SelectedItem)["ProductID"];
                    selectedRow["OrderDate"] = OrderDatePicker.SelectedDate ?? MinOrderDate;
                    selectedRow["Quantity"] = int.Parse(QuantityTextBox.Text);
                    selectedRow["TotalAmount"] = decimal.Parse(TotalAmountTextBox.Text);
                    ordersAdapter.Update(shopDataSet.Orders);
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