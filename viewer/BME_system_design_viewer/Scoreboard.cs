﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BME_system_design_viewer
{
    public partial class Scoreboard : UserControl
    {
        public int correct = 0; // 변수의 값을 이전 유저 컨트롤에서 받기 위해 public으로 지정.
        public int wrong = 0;
        public Scoreboard()
        {
            InitializeComponent();
        }

        private void gameResult_Load(object sender, EventArgs e)
        {
            correctDisplay.Text = Convert.ToString(correct);
            wrongDisplay.Text = Convert.ToString(wrong);
        }
    }
}