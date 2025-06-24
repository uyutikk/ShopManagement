using System;
using System.Data;
using System.Windows;

namespace ShopManagement
{
    public partial class EditProductsWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.ProductsTableAdapter productsAdapter;
        private ShopDataSetTableAdapters.CategoriesTableAdapter categoriesAdapter;
        private ShopDataSetTableAdapters.SuppliersTableAdapter suppliersAdapter;
        private DataRow selectedRow;

        public EditProductsWindow(ShopDataSet dataSet, ShopDataSetTableAdapters.ProductsTableAdapter prodAdapter,
            ShopDataSetTableAdapters.CategoriesTableAdapter catAdapter, ShopDataSetTableAdapters.SuppliersTableAdapter supAdapter, DataRow row)
        {
            InitializeComponent();
            shopDataSet = dataSet;
            productsAdapter = prodAdapter;
            categoriesAdapter = catAdapter;
            suppliersAdapter = supAdapter;
            selectedRow = row;

            categoriesAdapter.Fill(shopDataSet.Categories);
            suppliersAdapter.Fill(shopDataSet.Suppliers);
            CategoryComboBox.ItemsSource = shopDataSet.Categories.DefaultView;
            SupplierComboBox.ItemsSource = shopDataSet.Suppliers.DefaultView;

            ProductNameTextBox.Text = selectedRow["ProductName"].ToString();
            CategoryComboBox.SelectedValue = selectedRow["CategoryID"];
            SupplierComboBox.SelectedValue = selectedRow["SupplierID"];
            PriceTextBox.Text = selectedRow["Price"].ToString();
            StockQuantityTextBox.Text = selectedRow["StockQuantity"].ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    selectedRow["ProductName"] = ProductNameTextBox.Text;
                    selectedRow["CategoryID"] = ((DataRowView)CategoryComboBox.SelectedItem)["CategoryID"];
                    selectedRow["SupplierID"] = ((DataRowView)SupplierComboBox.SelectedItem)["SupplierID"];
                    selectedRow["Price"] = decimal.Parse(PriceTextBox.Text);
                    selectedRow["StockQuantity"] = int.Parse(StockQuantityTextBox.Text);
                    productsAdapter.Update(shopDataSet.Products);
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
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text))
            {
                MessageBox.Show("Название товара не может быть пустым", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(StockQuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Количество должно быть неотрицательным целым числом", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (CategoryComboBox.SelectedItem == null || SupplierComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию и поставщика", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}