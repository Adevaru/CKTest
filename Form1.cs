namespace CTKDriverTest
{
    using System;
    using System.Windows.Forms;
    using CTKLib;

    public partial class Form1 : Form
    {
        private const int HiPro = 0;

        private const int Microcard = 1;

        private const int NOAHLink = 2;

        private const int CAA = 3;

        public Form1()
        {
            InitializeComponent();

            // Common handlers for exceptions
            AppDomain.CurrentDomain.UnhandledException +=
                this.CurrentDomain_UnhandledException;

            Application.ThreadException += this.Application_ThreadException;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const string ConfigName = "Test1";

            var comManager = new CCommunicationManagerClass();
            comManager.CreateConfiguration(ConfigName);
            var communicationInterface = comManager.GetCommunicationInterface(ComboBoxProgrammer.Text);

            var protocol = communicationInterface.CreateProtocol("I2C");
            var configuration = comManager.GetConfiguration(ConfigName);
            configuration.CommunicationInterface = communicationInterface.Name;
            configuration.Protocol = protocol.Name;
            configuration.InterfaceSettings = this.GetInterfaceSettings();
            configuration.ProtocolSettings = "address=1;";
            protocol.Init(configuration.InterfaceSettings, configuration.ProtocolSettings);
            MessageBox.Show(ComboBoxProgrammer.Text + " successfully initialised");

            protocol.Close();
            comManager.RemoveConfiguration(ConfigName);
            MessageBox.Show(ComboBoxProgrammer.Text + " successfully uninitialised");
        }

        private string GetInterfaceSettings()
        {
            switch (this.ComboBoxProgrammer.SelectedIndex)
            {
                case HiPro:
                    return "port=default;ear=left;iovoltage=1.35V;hiprodriverversion=225;";
                case Microcard:
                    return "port=default;ear=left;iovoltage=1.25V;batteryvoltage=1.25V;";
                case NOAHLink:
                    return "ear=left;iovoltage=1V;batteryvoltage=1.35V;";
                case CAA:
                    return "iovoltage=Vsense;index=0;address=96;speed=400;multi-master=off;";
                default:
                    return string.Empty;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ComboBoxProgrammer.SelectedIndex = 0;
        }

        private void CurrentDomain_UnhandledException(
            object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Application_ThreadException(
                object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
    }
}
