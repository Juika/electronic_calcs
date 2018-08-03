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

namespace TemperatureCalculatings
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Temperature")
            {
                reset_values();
                pictureBox_lowpass_filter.Visible = false;
                pictureBox_temperature.Visible = true;
                pictureBox_dependences.Visible = true;

                label2.Text = "Max power dissipation (P):";
                label3.Text = "Ambient temperature (Ta):";
                label4.Text = "Thermal resistance, Junction-to-Ambient (Rja):";
                label5.Text = "Thermal resistance, Radiator-to_Atmosphere(Rra):";
                label6.Text = "Thermal resistance, Junction-to-Case (Rjc):";
                label7.Text = "Thermal resistance, Case-to-Radiator(avrg 1)(Rcr):";
                

                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;

                label10.Visible = true;
                label10.Text = "Tj = P * Rja + Ta";

                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = false;
                textBox6.Visible = false;
                textBox7.Visible = false;
                textBox8.Visible = false;

                button1.Text = "Calculate";
                button1.Visible = true;

                checkBox1.Visible = true;
                checkBox1.Text = "Radiator ENABLE";
            }
            else if(comboBox1.Text == "Low-pass filter")
            {
                reset_values();
                pictureBox_lowpass_filter.Visible = true;

                label2.Text = "Resistor (R):";
                comboBox2.Visible = true;
                comboBox2.Items.Clear();
                comboBox2.Items.Add("Ohm");
                comboBox2.Items.Add("kOhm");
                comboBox2.Items.Add("MOhm");
                comboBox2.Items.Add("GOhm");
                comboBox2.SelectedItem = "Ohm";

                label3.Text = "Capacitor (C):";
                label4.Text = "Vin (V) =";

                comboBox3.Visible = true;
                comboBox3.Items.Clear();
                comboBox3.Items.Add("pF");
                comboBox3.Items.Add("nF");
                comboBox3.Items.Add("uF");
                comboBox3.Items.Add("mF");
                comboBox3.SelectedItem = "nF";

                label2.Visible = true;//Resistor val
                label3.Visible = true;//Cap val
                label4.Visible = true;//Vin, V

                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                
                button1.Visible = true;
                button1.Text = "Calculate";
                
            }
        }
        private void reset_values() {
            pictureBox_dependences.Visible = false;
            pictureBox_lowpass_filter.Visible = false;
            pictureBox_temperature.Visible = false;

            label2.Visible = false;
            textBox2.Visible = false;
            textBox2.Text = "0";
            label3.Visible = false;
            textBox3.Visible = false;
            textBox3.Text = "0";
            label4.Visible = false;
            textBox4.Visible = false;
            textBox4.Text = "0";
            label5.Visible = false;
            textBox5.Visible = false;
            textBox5.Text = "0";
            label6.Visible = false;
            textBox6.Visible = false;
            textBox6.Text = "0";
            label7.Visible = false;
            textBox7.Visible = false;
            textBox7.Text = "0";
            label8.Visible = false;
            textBox8.Visible = false;
            textBox8.Text = "0";
            label9.Visible = false;
            label10.Visible = false;

            button1.Visible = false;

            checkBox1.Visible = false;

            comboBox2.Visible = false;
            comboBox2.Items.Clear();
            comboBox3.Visible = false;
            comboBox3.Items.Clear();

            chart1.Visible = false;
            chart1.Series["Series1"].Points.Clear();
            chart1.Series["Series2"].Points.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text == "Temperature")
            {
                label9.Visible = true;
                if(checkBox1.CheckState == CheckState.Unchecked)
                {
                    try
                    {
                        double TempJunction;
                        textBox2.Text = textBox2.Text.Replace('.', ',');
                        textBox7.Text = textBox7.Text.Replace('.', ',');
                        textBox4.Text = textBox4.Text.Replace('.', ',');
                        TempJunction = Convert.ToDouble(textBox2.Text) * Convert.ToDouble(textBox4.Text) + Convert.ToDouble(textBox3.Text);
                        label9.Text = "Crystal temperature is " + Convert.ToString(TempJunction)+" C";
                    }
                    catch (FormatException)
                    {
                        label9.Text = "Wrong input value";
                    }
                }
            }
            else if (comboBox1.Text == "Low-pass filter")
            {
                    try
                    {
                    textBox2.Text = textBox2.Text.Replace('.', ',');
                    textBox3.Text = textBox3.Text.Replace('.', ',');
                    textBox4.Text = textBox4.Text.Replace('.', ',');

                    
                    double resisitor_val = Convert.ToDouble(textBox2.Text);
                    double cap_val = Convert.ToDouble(textBox3.Text);
                    double pow_val, pow_val_cap = 1000000000, pow_val_res = 1;//значение обратное например 10^-9

                    if (cap_val <= 0 || resisitor_val <= 0)
                        label9.Text = "Wrong input value or out of range";
                        else
                        {
                            button1.Enabled = false;
                            double cut_off_frequency, Vout = 0, Vin = 0, Xc;
                            label9.Visible = true;
                        chart1.Visible = true;

                        Vin = Convert.ToDouble(textBox4.Text);

                            switch (comboBox3.Text)//capacitor
                            {
                                case "nF":
                                    pow_val_cap *= 1;
                                    break;
                                case "pF":
                                    pow_val_cap *= 1000;
                                    break;
                                case "uF":
                                    pow_val_cap /= 1000;
                                    break;
                                case "mF":
                                    pow_val_cap /= 1000000;
                                    break;
                            }
                            pow_val = pow_val_cap;
                            switch (comboBox2.Text)//resistor
                            {
                                case "Ohm":
                                    pow_val_res /= 1;
                                    break;
                                case "kOhm":
                                    pow_val_res = 1000;
                                    break;
                                case "MOhm":
                                    pow_val_res = 1000000;
                                    break;
                                case "GOhm":
                                    pow_val_res = 1000000000;
                                    break;
                            }
                    

                            cut_off_frequency = pow_val_cap / pow_val_res / resisitor_val  / cap_val / 6.28;
                            label10.Text = "Cut-off frequency (LPF):";
                            label10.Visible = true;
                            label9.Visible = true;
                            label9.Text = Convert.ToString(cut_off_frequency) + " Hz";

                        chart1.Series["Series1"].Points.Clear();
                        chart1.Series["Series2"].Points.Clear();
                        chart1.ChartAreas[0].AxisX.IsLogarithmic = true;
                        
                        
                        double Gain = 0;
                            for (double i = 1; i < (cut_off_frequency + cut_off_frequency / 10) && Gain >= -3; i += cut_off_frequency / 100)
                            {
                                Xc = pow_val_cap / (6.28 * cap_val * i);
                                Vout = Vin * (Xc / Math.Sqrt(Math.Pow(resisitor_val, 2) + Math.Pow(Xc, 2)));
                                chart1.Series["Series1"].Points.AddXY(i, Vout);
                            Gain = 20 * Math.Log(Vout / Vin);
                            chart1.Series["Series2"].Points.AddXY(i, Gain);
                            }
                        button1.Enabled = true;
                        }
                    }
                    catch(FormatException)
                    {
                        label9.Text = "Format exception";
                    }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Temperature")
                if (checkBox1.CheckState == CheckState.Checked)
                {
                label10.Text = "Tj = P * (Rjc + Rcr + Rra) + Ta";
                
                label4.Visible = false;
                textBox4.Visible = false;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;
                textBox7.Visible = true;
                }
                else if(checkBox1.CheckState == CheckState.Unchecked)
                {
                    label10.Text = "Tj = P * Rja + Ta";
                    label4.Visible = true;
                    textBox4.Visible = true;
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;
                    textBox5.Visible = false;
                    textBox6.Visible = false;
                    textBox7.Visible = false;
                }
            
        }
    }
}