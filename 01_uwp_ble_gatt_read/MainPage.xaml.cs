using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using System.Windows;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace uwp_ble_gatt_read
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GattDeviceService GattDeviceService { get; set; }

        private GattCharacteristic GattCharacteristic { get; set; }

        private Windows.UI.Core.CoreDispatcher dispatcher;

        public MainPage()
        {
            this.InitializeComponent();

            this.dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
        }

        private async void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            // SensorTagを取得
            var selector = GattDeviceService.GetDeviceSelectorFromUuid(new Guid(SensorTagUuid.UuidIrtService));
            var devices = await DeviceInformation.FindAllAsync(selector);
            
            
            var deviceInformation = devices.FirstOrDefault();
            if (deviceInformation == null)
            {
                //MessageBox.Show("not found");
                return;
            }
            

            this.GattDeviceService = await GattDeviceService.FromIdAsync(deviceInformation.Id);
            //MessageBox.Show($"found {deviceInformation.Id}");

            // センサーの有効化?
            var configCharacteristic = this.GattDeviceService.GetCharacteristics(new Guid(SensorTagUuid.UuidIrtConf)).First();
            var status = await configCharacteristic.WriteValueAsync(new byte[] { 1 }.AsBuffer());
            if (status == GattCommunicationStatus.Unreachable)
            {
                //MessageBox.Show("Initialize failed");
                return;
            }
        }

        private async void ButtonReadValue_Click(object sender, RoutedEventArgs e)
        {
            if (this.GattDeviceService == null)
            {
                //MessageBox.Show("Please click connect button");
                return;
            }

            // 値を読み始める
            if (this.GattCharacteristic == null)
            {
                this.GattCharacteristic = this.GattDeviceService.GetCharacteristics(new Guid(SensorTagUuid.UuidIrtData)).First();
                this.GattCharacteristic.ValueChanged += this.GattCharacteristic_ValueChanged;

                var status = await this.GattCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                if (status == GattCommunicationStatus.Unreachable)
                {
                    //MessageBox.Show("Failed");
                }
            }
        }

        private async void GattCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            // 値を読んで表示する
            //await this.dispatcher.InvokeAsync(() =>
            await this.dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var data = new byte[args.CharacteristicValue.Length];
                DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(data);
                var temp = BitConverter.ToUInt16(data, 2) / 128.0;
                this.TextBlockTemp.Text = $"{temp}℃";
            });
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            // SensorTagを取得
            var selector = GattDeviceService.GetDeviceSelectorFromUuid(new Guid(SensorTagUuid.UuidIrtService));
            var devices = await DeviceInformation.FindAllAsync(selector);

            
            var deviceInformation = devices.FirstOrDefault();
            if (deviceInformation == null)
            {
                //MessageBox.Show("not found");
                return;
            }
            

            this.GattDeviceService = await GattDeviceService.FromIdAsync(deviceInformation.Id);
            //MessageBox.Show($"found {deviceInformation.Id}");
            
            
            // センサーの有効化?
            var configCharacteristic = this.GattDeviceService.GetCharacteristics(new Guid(SensorTagUuid.UuidIrtConf)).First();
            var status = await configCharacteristic.WriteValueAsync(new byte[] { 1 }.AsBuffer());
            if (status == GattCommunicationStatus.Unreachable)
            {
                //MessageBox.Show("Initialize failed");
                return;
            }
        }

        private async void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.GattDeviceService == null)
            {
                //MessageBox.Show("Please click connect button");
                return;
            }

            // 値を読み始める
            if (this.GattCharacteristic == null)
            {
                this.GattCharacteristic = this.GattDeviceService.GetCharacteristics(new Guid(SensorTagUuid.UuidIrtData)).First();
                this.GattCharacteristic.ValueChanged += this.GattCharacteristic_ValueChanged;

                var status = await this.GattCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                if (status == GattCommunicationStatus.Unreachable)
                {
                    //MessageBox.Show("Failed");
                }
            }
        }
    }


}
