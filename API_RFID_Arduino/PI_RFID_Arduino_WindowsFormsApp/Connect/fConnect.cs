using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace PI_RFID_Arduino_WindowsFormsApp
{
    public partial class fConnect : Form
    {
        #region variable
        string RxString;
        #endregion
        public fConnect()
        {
            InitializeComponent();
            timerCom.Enabled = true;
        }
        #region continuos read of serial port


        private void atualizaListaComs()
        {
            int i =0;
            bool qntDifrente = false;
            if (cbSerial.Items.Count == SerialPort.GetPortNames().Length)
            {
                foreach (var item in SerialPort.GetPortNames())
                {
                    if (cbSerial.Items[i++].Equals(item)==false)
                    {
                        qntDifrente = true;

                    }
                }
            }
            else
            {
                qntDifrente = true;
            }
            if (!qntDifrente)
            {
                return;
            }
            cbSerial.Items.Clear();
            foreach (var serial in SerialPort.GetPortNames())
            {
                cbSerial.Items.Add(serial);
            }
            cbSerial.SelectedIndex=0;
        }

        private void timerCom_Tick(object sender, EventArgs e)
        {
            atualizaListaComs();
        }
        #endregion
        #region button click 


        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.PortName = cbSerial.Items[cbSerial.SelectedIndex].ToString();
                    serialPort1.Open();
                    btnConnect.Text = "Desconectar";
                    cbSerial.Enabled = false;
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Erro na conexão serial"+ erro , "Informação do sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                try
                {
                    serialPort1.Close();
                    cbSerial.Enabled = true;
                    btnConnect.Text = "Conectar";
                }
                catch (Exception error)
                {
                    MessageBox.Show("Erro na conexão serial" + error, "Informação do sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        #endregion
       
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(tbreceive.Text);
            }
        }
        #region form close action 

        
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }
        #endregion
        #region serial communication 
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            RxString = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(trataDadoRecebido));
        }
        private void trataDadoRecebido (object sender, EventArgs e)
        {
            tbreceive.AppendText(RxString);
        }
        #endregion

    }
}
