using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Windows.Mvvm;
using Prism.Commands;

using Windows.Devices.Bluetooth.Advertisement;

namespace uwp_ble_passive_scan.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private BluetoothLEAdvertisementWatcher watcher;
        private Windows.UI.Core.CoreDispatcher dispatcher;

        public DelegateCommand StartScaningCommand;
        public DelegateCommand StopScaningCommand;

        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public MainPageViewModel()
        {
            this.StartScaningCommand = new DelegateCommand(StartScaning);
            this.StopScaningCommand = new DelegateCommand(StopScaning);

            this.watcher = new BluetoothLEAdvertisementWatcher();
            this.watcher.SignalStrengthFilter.InRangeThresholdInDBm = -80;
            this.watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -85;
            this.watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(2000);
            this.watcher.Received += this.Watcher_Received;

            this.dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
        }

        public void StartScaning()
        {
            Message = "Passive Scan Start";
            this.watcher.Start();
        }

        public void StopScaning()
        {
            Message = "Stopped Scanning";
            this.watcher.Stop();
        }


        private async void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Message = $"{args.Timestamp:HH\\:mm\\:ss}, RSSI: {args.RawSignalStrengthInDBm}, Address: {args.BluetoothAddress.ToString("X")}, Type: {args.AdvertisementType}";
            });
        }
    }
}
