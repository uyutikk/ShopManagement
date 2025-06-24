using ShopManagement;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopManagement
{
    public partial class ProductsWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.ProductsTableAdapter productsAdapter;
        private ShopDataSetTableAdapters.CategoriesTableAdapter categoriesAdapter;
        private ShopDataSetTableAdapters.SuppliersTableAdapter suppliersAdapter;

        public ProductsWindow()
        {
            InitializeComponent();
            shopDataSet = new ShopDataSet();
            productsAdapter = new ShopDataSetTableAdapters.ProductsTableAdapter();
            categoriesAdapter = new ShopDataSetTableAdapters.CategoriesTableAdapter();
            suppliersAdapter = new ShopDataSetTableAdapters.SuppliersTableAdapter();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                productsAdapter.Fill(shopDataSet.Products);
                categoriesAdapter.Fill(shopDataSet.Categories);
                suppliersAdapter.Fill(shopDataSet.Suppliers);
                ProductsDataGrid.ItemsSource = shopDataSet.Products.DefaultView;
                StatusTextBlock.Text = "Данные загружены";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductsWindow addWindow = new AddProductsWindow(shopDataSet, productsAdapter, categoriesAdapter, suppliersAdapter);
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
                StatusTextBlock.Text = "Товар добавлен";
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ProductsDataGrid.SelectedItem;
                EditProductsWindow editWindow = new EditProductsWindow(shopDataSet, productsAdapter, categoriesAdapter, suppliersAdapter, selectedRow.Row);
                if (editWindow.ShowDialog() == true)
                {
                    LoadData();
                    StatusTextBlock.Text = "Товар обновлен";
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить товар?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)ProductsDataGrid.SelectedItem;
                        selectedRow.Row.Delete();
                        productsAdapter.Update(shopDataSet.Products);
                        LoadData();
                        StatusTextBlock.Text = "Товар удален";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ProductsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem != null)
            {
                EditButton_Click(sender, e);
            }
        }
    }
}