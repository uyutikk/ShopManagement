using System;
using System.Data;
using System.Windows;

namespace ShopManagement
{
    public partial class AddCategoriesWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.CategoriesTableAdapter categoriesAdapter;

        public AddCategoriesWindow(ShopDataSet dataSet, ShopDataSetTableAdapters.CategoriesTableAdapter adapter)
        {
            InitializeComponent();
            shopDataSet = dataSet;
            categoriesAdapter = adapter;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    DataRow newRow = shopDataSet.Categories.NewRow();
                    newRow["CategoryName"] = CategoryNameTextBox.Text;
                    newRow["Description"] = DescriptionTextBox.Text;
                    shopDataSet.Categories.Rows.Add(newRow);
                    categoriesAdapter.Update(shopDataSet.Categories);
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
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                MessageBox.Show("Название категории не может быть пустым", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}