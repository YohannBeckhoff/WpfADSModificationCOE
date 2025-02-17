using System.Text;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwinCAT.Ads;

namespace WpfADSModificationCOE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TwinCAT.Ads.AdsClient tcClient;
        public MainWindow()
        {
            InitializeComponent();
            tcClient = new TwinCAT.Ads.AdsClient();
        }

        private void _ReadClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Read ADS");
            string sAmsNetID = tbAmsNetID.Text;             
            int nPort = Convert.ToInt32(tbPort.Text);
            uint valueToRead = 0;
     
            try
            {
                
                tcClient.Connect(sAmsNetID, nPort);
                uint nGroup = uint.Parse(tbIndexGroup.Text, System.Globalization.NumberStyles.HexNumber); // uint nGroup = 0xF302;
                uint nOffset = uint.Parse(tbIndexOffset.Text, System.Globalization.NumberStyles.HexNumber); //uint nOffset = 0x80000015;
                valueToRead = (uint)tcClient.ReadAny(nGroup, nOffset, typeof(UInt32));
            
                tbValue.Text = valueToRead.ToString();
                tcClient.Disconnect();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void _WriteClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Write ADS");
            string sAmsNetID = tbAmsNetID.Text;
            int nPort = Convert.ToInt32(tbPort.Text);
            UInt16 valueToWrite = Convert.ToUInt16(tbValue.Text);
            //uint resultWrite = 0;

            try
            {
                tcClient.Connect(sAmsNetID, nPort);
                CancellationToken cancel = CancellationToken.None;


                uint nGroup = uint.Parse(tbIndexGroup.Text, System.Globalization.NumberStyles.HexNumber); // uint nGroup = 0xF302;
                uint nOffset = uint.Parse(tbIndexOffset.Text, System.Globalization.NumberStyles.HexNumber); //uint nOffset = 0x80000015;
 
                tcClient.WriteAnyAsync(nGroup, nOffset, valueToWrite,cancel);
        
                  tcClient.Disconnect();   
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}