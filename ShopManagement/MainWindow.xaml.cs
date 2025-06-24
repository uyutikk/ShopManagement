using System.Windows;

namespace ShopManagement
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CategoriesMenu_Click(object sender, RoutedEventArgs e)
        {
            CategoriesWindow window = new CategoriesWindow();
            window.Show();
        }

        private void SuppliersMenu_Click(object sender, RoutedEventArgs e)
        {
            SuppliersWindow window = new SuppliersWindow();
            window.Show();
        }

        private void ProductsMenu_Click(object sender, RoutedEventArgs e)
        {
            ProductsWindow window = new ProductsWindow();
            window.Show();
        }

        private void CustomersMenu_Click(object sender, RoutedEventArgs e)
        {
            CustomersWindow window = new CustomersWindow();
            window.Show();
        }

        private void OrdersMenu_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow window = new OrdersWindow();
            window.Show();
        }
    }
}