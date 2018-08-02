using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

                label2.Text = "Resistor (R), Ohm:";
                label3.Text = "Capacitor (C), nF:";
                label2.Visible = true;
                label3.Visible = true;

                textBox2.Visible = true;
                textBox3.Visible = true;

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
                textBox2.Text = textBox2.Text.Replace('.', ',');
                textBox3.Text = textBox3.Text.Replace('.', ',');
                double resisitor_val = Convert.ToDouble(textBox2.Text);
                double cap_val = Convert.ToDouble(textBox3.Text);
                if (cap_val <= 0 || resisitor_val <= 0)
                    label9.Text = "Wrong input value";
                else
                {
                    double cut_off_frequency, Vout = 0, Vin = 5, Xc;
                    label9.Visible = true;
                    cut_off_frequency = 1000000000 / (6.28 * cap_val * resisitor_val);

                    

                    label10.Text = "Cut-off frequency (LPF):";
                    label10.Visible = true;
                    label9.Visible = true;
                    label9.Text = Convert.ToString(cut_off_frequency) + " Hz";
                    for (int i = 0; i < (cut_off_frequency + cut_off_frequency / 10); i += (int)cut_off_frequency / 100) {
                        Xc = Math.Pow(10,9)/(6.28*cap_val);
                        Vout = Vin * (Xc / Math.Sqrt(Math.Pow(resisitor_val, 2) + Math.Pow(Xc, 2)));
                        chart1.Series["Series1"].Points.AddXY(i, Vout);
                        if (Vout <= 0.718)
                            return;
                    }
                    
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