using ShopManagement;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopManagement
{
    public partial class SuppliersWindow : Window
    {
        private ShopDataSet shopDataSet;
        private ShopDataSetTableAdapters.SuppliersTableAdapter suppliersAdapter;

        public SuppliersWindow()
        {
            InitializeComponent();
            shopDataSet = new ShopDataSet();
            suppliersAdapter = new ShopDataSetTableAdapters.SuppliersTableAdapter();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                suppliersAdapter.Fill(shopDataSet.Suppliers);
                SuppliersDataGrid.ItemsSource = shopDataSet.Suppliers.DefaultView;
                StatusTextBlock.Text = "Данные загружены";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddSuppliersWindow addWindow = new AddSuppliersWindow(shopDataSet, suppliersAdapter);
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
                StatusTextBlock.Text = "Поставщик добавлен";
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)SuppliersDataGrid.SelectedItem;
                EditSuppliersWindow editWindow = new EditSuppliersWindow(shopDataSet, suppliersAdapter, selectedRow.Row);
                if (editWindow.ShowDialog() == true)
                {
                    LoadData();
                    StatusTextBlock.Text = "Поставщик обновлен";
                }
            }
            else
            {
                MessageBox.Show("Выберите поставщика для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить поставщика?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)SuppliersDataGrid.SelectedItem;
                        selectedRow.Row.Delete();
                        suppliersAdapter.Update(shopDataSet.Suppliers);
                        LoadData();
                        StatusTextBlock.Text = "Поставщик удален";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SuppliersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItem != null)
            {
                EditButton_Click(sender, e);
            }
        }
    }
}