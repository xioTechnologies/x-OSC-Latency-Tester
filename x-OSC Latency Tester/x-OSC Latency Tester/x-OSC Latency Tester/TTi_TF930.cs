using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace x_OSC_Latency_Tester
{
    /// <summary>
    /// TTi TF930 frequency counter interface class.
    /// </summary>
    /// <remarks>
    /// Assume TTi TF930 is set up manually and in period or width measurement mode where the measurement units are seconds.
    /// </remarks>
    public class TTi_TF930
    {
        /// <summary>
        /// Private SerialPort object.
        /// </summary>
        private SerialPort serialPort;

        /// <summary>
        /// Private receive buffer.
        /// </summary>
        private byte[] rxBuf = new byte[256];

        /// <summary>
        /// Private receive buffer index.
        /// </summary>
        private byte rxBufIndex = 0;

        /// <summary>
        /// Initialises a new instance of the <see cref="TTi_TF930"/> class.
        /// </summary>
        /// <param name="portName">
        /// Name of the port the TTi TF930 appears as (for example, COM1).
        /// </param>
        public TTi_TF930(string portName)
        {
            serialPort = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
        }

        /// <summary>
        /// Opens serial port connection and requests measurements.
        /// </summary>
        public void Open()
        {
            serialPort.Open();
            string str = "E?\n";    // command to request measurements to be sent continuously
            serialPort.Write(str.ToCharArray(), 0, str.ToCharArray().Length);
        }

        /// <summary>
        /// Closes the port connection.
        /// </summary>
        public void Close()
        {
            serialPort.Close();
        }

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int i566 = 0;
            if (i566 == 0)
            {
                int y = 2;
            }

            // Fetch bytes from serial port buffer
            byte[] serialBuffer = new byte[serialPort.BytesToRead];
            serialPort.Read(serialBuffer, 0, serialPort.BytesToRead);

            // Process each byte one-by-one
            for (int i = 0; i < serialBuffer.Length; i++)
            {
                rxBuf[rxBufIndex++] = serialBuffer[i];  // add new byte to buffer
                if (serialBuffer[i] == '\n')            // if new byte was packet framing char 
                {
                    string receivedString = System.Text.Encoding.UTF8.GetString(rxBuf).Substring(0, rxBufIndex);
                    OnMeasurementReceived(Convert.ToDouble(receivedString.Split('s')[0]));  // convert numerical value preceding measurement unit 's' 
                    rxBufIndex = 0;     // clear buffer
                }
            }
        }

        public delegate void onMeasurementReceived(object sender, double measuremnt);
        public event onMeasurementReceived MeasurementReceived;
        protected virtual void OnMeasurementReceived(double measuremnt) { if (MeasurementReceived != null) MeasurementReceived(this, measuremnt); }
    }
}