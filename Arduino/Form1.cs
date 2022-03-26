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
using System.Threading;
using System.IO;

namespace Arduino
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            serialPort1.PortName = "COM3";
            serialPort1.BaudRate = 9600;
            //serialPort1.Open();
            status.ForeColor = Color.Red;
            ImagemClr();
            CampoDownload();
            ObtemDiretorioAplicacao();
        }
        public void LerSerial()
        {
            if (serialPort1.IsOpen)
            {
                if (serialPort1.BytesToRead > 0)
                {
                    byte[] rxBuff = new byte[serialPort1.BytesToRead];
                    int nRxed = serialPort1.Read(rxBuff, 0, serialPort1.BytesToRead);
                    string act8 = System.Text.Encoding.ASCII.GetString(rxBuff);

                    richTextBox1.Text += $"Temperatura = {act8} Data { DateTime.Now} \n";

                    if (!File.Exists(textBox1.Text + "\\log.txt"))
                    {
                       File.CreateText(textBox1.Text + "\\log.txt").Close();
                    }
                    else
                    {
                        GeradorDeLogs($"Temperatura = {act8} Data { DateTime.Now}");
                    }
                }
            }
        }
        private void checkConectar_CheckedChanged(object sender, EventArgs e)
        {
            if (checkConectar.Checked == true)
            {
                richTextBox1.Text = "Comunicação ativa \n";
                richTextBox1.Text += "-------------------------------\n\n";
                pictureBox2.Visible = false;
                pictureBox1.Visible = true;
                progressBar1.Visible = true;
                status.ForeColor = Color.Green;
                timer1.Start();
            }
            if (checkConectar.Checked == false)
            {
                richTextBox1.Text = "Comunicação desativada \n";
                pictureBox2.Visible = true;
                pictureBox1.Visible = false;
                progressBar1.Visible = false;
                status.ForeColor = Color.Red;
                timer1.Stop();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            LerSerial();
        }
        private void ImagemClr()
        {
            pictureBox1.Visible = false;
        }

        private void ImagemPb()
        {
            pictureBox2.Visible = true;
        }
        private void CampoDownload()
        {
            progressBar1.Visible = false;
        }

        private void ObtemDiretorioAplicacao()
        {
            textBox1.Text = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            textBox1.Enabled = false;
        }

        private void GeradorDeLogs(string log)
        {
            using (StreamWriter writer = new StreamWriter(textBox1.Text + "\\log.txt", true))
            {
                writer.WriteLine(log);
                writer.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string diretorio = textBox1.Text +"\\log.txt";
            try
            {
                System.Diagnostics.Process.Start("notepad", diretorio);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}

