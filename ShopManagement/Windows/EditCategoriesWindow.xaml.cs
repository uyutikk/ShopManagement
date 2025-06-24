using System;
using System.Data;
using System.Windows;

namespace ShopManagement
{
    public partial class EditCategoriesWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.CategoriesTableAdapter categoriesAdapter;
        private DataRow selectedRow;

        public EditCategoriesWindow(ShopDataSet dataSet, ShopDataSetTableAdapters.CategoriesTableAdapter adapter, DataRow row)
        {
            InitializeComponent();
            shopDataSet = dataSet;
            categoriesAdapter = adapter;
            selectedRow = row;

            CategoryNameTextBox.Text = selectedRow["CategoryName"].ToString();
            DescriptionTextBox.Text = selectedRow["Description"].ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    selectedRow["CategoryName"] = CategoryNameTextBox.Text;
                    selectedRow["Description"] = DescriptionTextBox.Text;
                    categoriesAdapter.Update(shopDataSet.Categories);
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