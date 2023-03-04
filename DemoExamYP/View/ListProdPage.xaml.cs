using DemoExamYP.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoExamYP.View
{
    /// <summary>
    /// Логика взаимодействия для ListProdPage.xaml
    /// </summary>
    public partial class ListProdPage : Page
    {
        public ListProdPage()
        {
            InitializeComponent();
            UpImg();
            ListProd.ItemsSource = App.db.Product.ToList();
            ElForSort();
            FilterCB.ItemsSource = App.db.ProductType.ToList();
            FilterCB.DisplayMemberPath = "Title";
        }
        private void UpImg()
        {
            foreach(var item in App.db.Product)
            {
                if (item.Image == null || item.Image == "" || item.Image == "null")
                    item.Image = @"\prodacts\picture.png";
            }
        }
        private void ElForSort()
        {
            SortCB.Items.Add("Сбросить сортировку");
            SortCB.Items.Add("Наименование от А до Я");
            SortCB.Items.Add("Наименование от Я до А");
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(SortCB.SelectedItem)
            {
                case "Сбросить сортировку":
                    ListProd.ItemsSource = null;
                    ListProd.ItemsSource = App.db.Product.ToList();
                    break;
                case "Наименование от А до Я":
                    ListProd.ItemsSource = null;
                    ListProd.ItemsSource = App.db.Product.OrderBy(x => x.Title).ToList();
                    break;
                case "Наименование от Я до А":
                    ListProd.ItemsSource = null;
                    ListProd.ItemsSource = App.db.Product.OrderByDescending(x => x.Title).ToList();
                    break;
                default:
                    break;
            }
        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var type = FilterCB.SelectedItem;
            var typeId = ((ProductType)type).ID;
            ListProd.ItemsSource = null;
            ListProd.ItemsSource = App.db.Product.Where(x => x.ProductTypeID == typeId).ToList();
        }

        private void AddProdBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProdPage());
        }

        private void SearchTB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SearchTB.Text = "";
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListProd.ItemsSource = App.db.Product.Where(x => x.Title.ToLower().Contains(SearchTB.Text.ToLower())).ToList();
        }

        private void ListProd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var res = MessageBox.Show("Удалить продукт?", "", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                var prod = ListProd.SelectedItem as Product;
                App.db.ProductCostHistory.RemoveRange(App.db.ProductCostHistory.Where(x => x.ProductID == prod.ID)).ToList();
                App.db.ProductMaterial.RemoveRange(App.db.ProductMaterial.Where(x => x.ProductID == prod.ID)).ToList();
                App.db.ProductSale.RemoveRange(App.db.ProductSale.Where(x => x.ProductID == prod.ID)).ToList();
                App.db.Product.Remove(prod);
                App.db.SaveChanges();
                MessageBox.Show("Продукт удален");
                ListProd.ItemsSource = App.db.Product.ToList();
            }
        }
    }
}
