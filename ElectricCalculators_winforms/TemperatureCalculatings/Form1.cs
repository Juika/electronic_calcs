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
                pictureBox_lowpass_filter.Visible = false;
                pictureBox_temperature.Visible = true;

                label2.Text = "Max power dissipation (P):";
                label3.Text = "Ambient temperature (Ta):";
                label4.Text = "Thermal resistance, Junction-to-Ambient (Rja):";
                label5.Text = "Thermal resistance, Radiator-to_Atmosphere(Rra):";
                label6.Text = "Thermal resistance, Junction-to-Case (Rjc):";
                label7.Text = "Thermal resistance, Case-to-Radiator(average 1)(Rcr):";
                
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                //label5.Visible = true;
                //label6.Visible = true;
                //label7.Visible = true;
                label10.Visible = true;
                label10.Text = "Tj = P * Rja + Ta";

                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                //textBox5.Visible = true;
                //textBox6.Visible = true;
                //textBox7.Visible = true;
                //textBox8.Visible = true;

                button1.Text = "Calculate";
                button1.Visible = true;

                checkBox1.Visible = true;
                checkBox1.Text = "Radiator ENABLE";
            }
            else if(comboBox1.Text == "Low-pass filter")
            {
                pictureBox_temperature.Visible = false;
                pictureBox_lowpass_filter.Visible = true;
            }
        }
        private void reset_values() {

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
                else
                {
                    try
                    {
                        double TempJunction;
                        textBox2.Text = textBox2.Text.Replace('.', ',');
                        textBox3.Text = textBox3.Text.Replace('.', ',');
                        textBox5.Text = textBox5.Text.Replace('.', ',');
                        textBox6.Text = textBox6.Text.Replace('.', ',');
                        textBox7.Text = textBox7.Text.Replace('.', ',');

                        TempJunction = Convert.ToDouble(textBox2.Text) *
                            (Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text) + Convert.ToDouble(textBox7.Text))
                            + Convert.ToDouble(textBox3.Text);
                        label9.Text = Convert.ToString(TempJunction) + " C";
                    }
                    catch (FormatException) {
                        label9.Text = "Wrong input value";
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.CheckState == CheckState.Checked)
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