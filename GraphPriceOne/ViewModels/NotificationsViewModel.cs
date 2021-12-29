﻿using GraphPriceOne.Core.Models;
using GraphPriceOne.Models;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.ViewModels
{
    public class NotificationsViewModel : NotificationsModel
    {

        public ObservableCollection<NotificationsModel> ListViewCollection { get; set; }

        public NotificationsViewModel()
        {
            ListViewCollection = new ObservableCollection<NotificationsModel>();
            _ = GetNotificationsAsync();

        }
        public ICommand RemoveItemCommand => new RelayCommand<int>(new Action<int>(async e => await RemoveItem(e)));
        public ICommand BuyNowCommand => new RelayCommand<string>(new Action<string>(async e => await BuyNow(e)));

        private async Task BuyNow(string Url_Product)
        {
            //DOCUMENTATION https://docs.microsoft.com/en-us/windows/uwp/launch-resume/launch-default-app
            var success = await Launcher.LaunchUriAsync(new Uri(Url_Product));

            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }
        }
        private async Task ExceptionDialog(string ex)
        {
            ContentDialog ExcepcionMessage = new ContentDialog()
            {
                Title = "Exception",
                PrimaryButtonText = "Ok",
                Content = ex.ToString()
            };
            await ExcepcionMessage.ShowAsync();
        }
        private async Task RemoveItem(int id_item)
        {
            await App.PriceTrackerService.DeleteNotificationAsync(id_item);
            await GetNotificationsAsync();
        }

        private async Task GetNotificationsAsync()
        {
            try
            {
                string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                var NotificationsList = (List<Notifications>)await App.PriceTrackerService.GetNotificationsAsync();

                ListViewCollection.Clear();

                foreach (var item in NotificationsList)
                {
                    //var Products = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();
                    //var Product = (ProductInfo)Products.Where(u => u.ID_PRODUCT.Equals(item.PRODUCT_ID));

                    List<ProductPhotos> Images = (List<ProductPhotos>)await App.PriceTrackerService.GetImagesAsync();
                    var ProductImages = Images.Where(u => u.ID_PRODUCT.Equals(item.PRODUCT_ID)).ToList();

                    //string mensaje = item.Message?.Replace("\n", "");
                    var message = "📉 Dropped " + item.PRODUCT_ID + " (" + item.PreviousPrice + " to " + item.NewPrice + ")";

                    ImageLocation = "";
                    if (ProductImages != null && ProductImages.Count != 0)
                    {
                        ImageLocation = LocalState + ProductImages.First().PhotoSrc;
                    }

                    ListViewCollection.Add(new NotificationsModel()
                    {
                        PRODUCT_ID = item.PRODUCT_ID,
                        ID_Notification = item.ID_Notification,
                        ProductName = message,
                        ProductDescription = message,
                        NewPrice = item.NewPrice,
                        PreviousPrice = item.PreviousPrice,
                        ProductUrl = "https://www.youtube.com/",
                        ImageLocation = ImageLocation
                    });
                }
            }
            catch (Exception ex)
            {
                await ExceptionDialog(ex.ToString());
            }
        }
    }
}
