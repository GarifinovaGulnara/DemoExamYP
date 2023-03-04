using DemoExamYP.DB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddProdPage.xaml
    /// </summary>
    public partial class AddProdPage : Page
    {
        public AddProdPage()
        {
            InitializeComponent();
            TypeProd.ItemsSource = App.db.ProductType.ToList();
            TypeProd.DisplayMemberPath = "Title";
        }

        private void NameProd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NameProd.Text = "";
        }

        private void ArticleProd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ArticleProd.Text = "";
        }

        private void DesProd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DesProd.Text = "";
        }

        private void PersonCount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PersonCount.Text = "";
        }

        private void WorkshopNumber_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WorkshopNumber.Text = "";
        }

        private void CostForAgent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CostForAgent.Text = "";
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListProdPage());
        }

        private void AddProdBtn_Click(object sender, RoutedEventArgs e)
        {
            Product prod = new Product();
            {
                prod.Title = NameProd.Text;
                prod.ArticleNumber = ArticleProd.Text;
                prod.Description = DesProd.Text;
                prod.MinCostForAgent = Convert.ToDecimal(CostForAgent.Text);
                var type = TypeProd.SelectedItem;
                var typeId = ((ProductType)type).ID;
                prod.ProductTypeID = typeId;
                prod.ProductionWorkshopNumber = Convert.ToInt32(WorkshopNumber.Text);
                prod.ProductionPersonCount = Convert.ToInt32(PersonCount.Text);
                prod.Image = openFileDialog.FileName;
            }
            App.db.Product.Add(prod);
            App.db.SaveChanges();
            MessageBox.Show("Продукт добавлен");
        }

        private void ArticleProd_TextChanged(object sender, TextChangedEventArgs e)
        {
            int val;
            if (!Int32.TryParse(ArticleProd.Text, out val))
            {
                MessageBox.Show("Артикул. Введите число!");
                ArticleProd.Text = "";
            }
        }
        OpenFileDialog openFileDialog = new OpenFileDialog();
        private void AddImgBtn_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.Filter = "Image files|*.bmp;*.jpeg|*.png|All files|*.*";
            openFileDialog.FilterIndex = 0;
            if (openFileDialog.ShowDialog() == true)
            {
                File.ReadAllBytes(openFileDialog.FileName);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(openFileDialog.FileName);
                image.EndInit();
                Img.Source = image;
            }

        }
    }
}