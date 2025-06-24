using System;
using System.Data;
using System.Windows;

namespace ShopManagement
{
    public partial class AddProductsWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.ProductsTableAdapter productsAdapter;
        private ShopDataSetTableAdapters.CategoriesTableAdapter categoriesAdapter;
        private ShopDataSetTableAdapters.SuppliersTableAdapter suppliersAdapter;

        public AddProductsWindow(ShopDataSet dataSet, ShopDataSetTableAdapters.ProductsTableAdapter prodAdapter,
            ShopDataSetTableAdapters.CategoriesTableAdapter catAdapter, ShopDataSetTableAdapters.SuppliersTableAdapter supAdapter)
        {
            InitializeComponent();
            shopDataSet = dataSet;
            productsAdapter = prodAdapter;
            categoriesAdapter = catAdapter;
            suppliersAdapter = supAdapter;

            categoriesAdapter.Fill(shopDataSet.Categories);
            suppliersAdapter.Fill(shopDataSet.Suppliers);
            CategoryComboBox.ItemsSource = shopDataSet.Categories.DefaultView;
            SupplierComboBox.ItemsSource = shopDataSet.Suppliers.DefaultView;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    DataRow newRow = shopDataSet.Products.NewRow();
                    newRow["ProductName"] = ProductNameTextBox.Text;
                    newRow["CategoryID"] = ((DataRowView)CategoryComboBox.SelectedItem)["CategoryID"];
                    newRow["SupplierID"] = ((DataRowView)SupplierComboBox.SelectedItem)["SupplierID"];
                    newRow["Price"] = decimal.Parse(PriceTextBox.Text);
                    newRow["StockQuantity"] = int.Parse(StockQuantityTextBox.Text);
                    shopDataSet.Products.Rows.Add(newRow);
                    productsAdapter.Update(shopDataSet.Products);
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