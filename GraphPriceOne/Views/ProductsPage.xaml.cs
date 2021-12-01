﻿using System;
using GraphPriceOne.Core.Models;
using GraphPriceOne.Services;
using GraphPriceOne.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

namespace GraphPriceOne.Views
{
    public sealed partial class ProductsPage : Page
    {
        public ProductsViewModel ViewModel { get; } = new ProductsViewModel();

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on ProductsPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        private ProductDetailsViewModel selectors;

        public ProductsPage()
        {
            InitializeComponent();

            DataContext = new MainViewModel(ListProducts);

            selectors = new ProductDetailsViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
        private void productView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int itemsSelected = ListProducts.SelectedItems.Count;
            int AllItems = ListProducts.Items.Count;
            if (ListProducts.SelectionMode == ListViewSelectionMode.Multiple || ListProducts.SelectionMode == ListViewSelectionMode.Extended)
            {
                if (itemsSelected == AllItems)
                {
                    CheckBox1.IsChecked = true;
                    CheckBox1Icon.Glyph = "\ue73a";
                }
                else if (itemsSelected == 0)
                {
                    CheckBox1.IsChecked = false;
                    CheckBox1Icon.Glyph = "\ue739";
                }
                else
                {
                    CheckBox1.IsChecked = false;
                    CheckBox1Icon.Glyph = "\uf16e";
                }
            }
            if (ListProducts.SelectionMode == ListViewSelectionMode.Single && ListProducts.SelectedItem != null)
            {
                ProductInfo obj = (ProductInfo)ListProducts.SelectedItem;
                //selectors.PRODUCTSELECT = obj;
                NavigationService.Navigate(typeof(ProductDetailsPage));
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ListProducts.SelectionMode == ListViewSelectionMode.Multiple || ListProducts.SelectionMode == ListViewSelectionMode.Extended)
            {
                CheckBox1.IsChecked = true;
                CheckBox1Icon.Glyph = "\ue73a";
                ListProducts.SelectAll();
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ListProducts.SelectionMode == ListViewSelectionMode.Multiple || ListProducts.SelectionMode == ListViewSelectionMode.Extended)
            {
                CheckBox1.IsChecked = false;
                CheckBox1Icon.Glyph = "\ue739";
                ListProducts.DeselectRange(new ItemIndexRange(0, (uint)ListProducts.Items.Count));
            }
        }
        private void ListViewStores_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            new ProductsViewModel();
        }
    }
}
