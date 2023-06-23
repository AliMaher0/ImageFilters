using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZGraphTools;

namespace ImageFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);

            }
        }
        int Start;
        int End;
        double Time;
        private void btnZGraph_Click(object sender, EventArgs e)
        {   
            int Wmax = Convert.ToInt32(textBox2.Text);
            int Trim = 1;
            if (textBox3.Text != "")
            {
                Trim = Convert.ToInt32(textBox3.Text);
            }
            //Make up some data points from the N, N log(N) functions

            double[] x_values = new double[Wmax];
            double[] y_counting_sort = new double[Wmax];
            double[] y2 = new double[Wmax];

            for (int i = 1; i < (Wmax-1); i += 2)
            {

                x_values[i] = i;//window size 3-5-7-Wmax
                Start = System.Environment.TickCount;

                if (comboBox1.Text == "alpha trim filter")
                {
                    ImageOperations.alpha(ImageMatrix, i, pictureBox2, "countingSort", Trim);
                    Console.WriteLine("alpha");
                }
                else
                { 
                    ImageOperations.adaptive(ImageMatrix, 3, i, pictureBox2, "countingSort");
                    Console.WriteLine("adptive");
                }
                End = System.Environment.TickCount;
                Time = End - Start;
                Time /= 1000;
                y_counting_sort[i] = Time;//time for each window counting sort

                //--------------------------------------------------------------------//

                Start = System.Environment.TickCount;
                if (comboBox1.Text == "alpha trim filter")
                {
                    ImageOperations.alpha(ImageMatrix, i, pictureBox2, "KthElement", Trim);
                    Console.WriteLine("alpha k");
                }
                else
                { 
                    ImageOperations.adaptive(ImageMatrix, 3, i, pictureBox2, "Quick_Sort");
                    Console.WriteLine("adptive q");
                }
                End = System.Environment.TickCount;
                Time = End - Start;
                Time /= 1000;
                y2[i] = Time;//time for each window kth element

                x_values[i + 1] = x_values[i];
                y_counting_sort[i + 1] = y_counting_sort[i];
                y2[i + 1] = y2[i];
            }
           
            //Create a graph and add two curves to it
            ZGraphForm ZGF = new ZGraphForm("Sample Graph", "N", "f(N)");
            ZGF.add_curve("f(N) = counting_sort", x_values, y_counting_sort, Color.Red);
            ZGF.add_curve("f(N) =Y2 ", x_values, y2, Color.Blue);
            ZGF.Show();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            int Trim = 1;
            int windowSize=3;
            if (textBox1.Text != "") 
            {
                windowSize = Convert.ToInt32(textBox1.Text);
            }
            if (textBox3.Text != "")
            {
              Trim = Convert.ToInt32(textBox3.Text);//trim value
            }
            
            if (checkBox1.Checked==true)
                ImageOperations.alpha(ImageMatrix, windowSize, pictureBox2, "countingSort",Trim);
            if(checkBox2.Checked==true)
                ImageOperations.alpha(ImageMatrix, windowSize, pictureBox2, "kthelement",Trim);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                int maxSize = Convert.ToInt32(textBox1.Text);
                if (checkBox1.Checked == true)
                    ImageOperations.adaptive(ImageMatrix, 3, maxSize, pictureBox2, "countingSort");
                else if (checkBox3.Checked == true)
                    ImageOperations.adaptive(ImageMatrix, 3, maxSize, pictureBox2, "Quick_Sort");
            }
            else
            {
                int maxSize = 3;
                if (checkBox1.Checked == true)
                    ImageOperations.adaptive(ImageMatrix, 3, maxSize, pictureBox2, "countingSort");
                else if (checkBox3.Checked == true)
                    ImageOperations.adaptive(ImageMatrix, 3, maxSize, pictureBox2, "Quick_Sort");
            }
          

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)//counting
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)//Kth
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)//Quick
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}