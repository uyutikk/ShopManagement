using ShopManagement;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopManagement
{
    public partial class CategoriesWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.CategoriesTableAdapter categoriesAdapter;

        public CategoriesWindow()
        {
            InitializeComponent();
            shopDataSet = new ShopDataSet();
            categoriesAdapter = new ShopDataSetTableAdapters.CategoriesTableAdapter();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                categoriesAdapter.Fill(shopDataSet.Categories);
                CategoriesDataGrid.ItemsSource = shopDataSet.Categories.DefaultView;
                StatusTextBlock.Text = "Данные загружены";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategoriesWindow addWindow = new AddCategoriesWindow(shopDataSet, categoriesAdapter);
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
                StatusTextBlock.Text = "Категория добавлена";
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)CategoriesDataGrid.SelectedItem;
                EditCategoriesWindow editWindow = new EditCategoriesWindow(shopDataSet, categoriesAdapter, selectedRow.Row);
                if (editWindow.ShowDialog() == true)
                {
                    LoadData();
                    StatusTextBlock.Text = "Категория обновлена";
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить категорию?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)CategoriesDataGrid.SelectedItem;
                        selectedRow.Row.Delete();
                        categoriesAdapter.Update(shopDataSet.Categories);
                        LoadData();
                        StatusTextBlock.Text = "Категория удалена";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void CategoriesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem != null)
            {
                EditButton_Click(sender, e);
            }
        }
    }
}