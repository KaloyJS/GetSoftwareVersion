using GetSoftwareVersion.Enum;
using System;
using System.Windows;
using System.Windows.Interop;

namespace GetSoftwareVersion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IntPtr windowHandle;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(System.EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Adds the windows message processing hook and registers USB device add/removal notification.
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            if (source != null)
            {
                windowHandle = source.Handle;
                source.AddHook(HwndHandler);
               UsbNotification.RegisterUsbDeviceNotification(windowHandle);
            }
        }

        public event EventHandler<DeviceChange> DeviceChanged;
       
        /// <summary>
        /// Method that receives window messages.
        /// https://stackoverflow.com/questions/16245706/check-for-device-change-add-remove-events
        /// </summary>
        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == UsbNotification.WmDevicechange)
            {

                switch ((int)wparam)
                {
                    case UsbNotification.DbtDeviceremovecomplete:
                        this.DeviceChanged?.Invoke(this, DeviceChange.Remove);
                        //OnThresholdReached();
                        break;
                    case UsbNotification.DbtDevicearrival:
                        //this.DeviceChanged?.Invoke(this, DeviceChange.Arrive);
                        DeviceInfo device = new DeviceInfo();
                        MessageBox.Show(device.deviceType);
                        // OnThresholdReached();
                        break;
                }
            }

            handled = false;
            return IntPtr.Zero;
        }
      

        



    }
}
