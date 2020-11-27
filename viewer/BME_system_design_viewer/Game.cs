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
    public partial class Game : UserControl
    {
        int stage = 2; // N-Back Training에서 N의 값

        int[] pastAnswer = new int[3]; // 과거에 제시된 손동작이 저장되는 배열
        int index = 0; // pastAnswer 배열에 저장된 데이터 중 몇 번째 데이터를 맞춰야 하는지
        
        int currentImage = 0; // 현재 컴퓨터가 보여주는 이미지
        int total = 0, correctAnswer = 0, wrongAnswer = 0; // 점수를 저장

        const int gameEnd = 12; // 12개의 그림을 보여주면 게임 끝
        const int firstNBackDelay = 3000; // 3초마다 새로운 그림을 보여줌

        Random rand = new Random();

        public Game()
        {
            InitializeComponent();
        }

        private async void nBackTraining_Load(object sender, EventArgs e)
        {
            initSetup();
            await firstNBack(stage);
            currentImage = nextNBack();
            while (true)
            {
                await processHandSign();
            }
        }

        private void initSetup()
        {
            computerHand.SizeMode = PictureBoxSizeMode.StretchImage;
            userHand.SizeMode = PictureBoxSizeMode.StretchImage;
            stageDisplay.Text = Convert.ToString(stage);
        }

        private void updateIndexDisplay() // 화면에 index 변수를 출력함. 3-Back의 경우 0, 1, 2가 반복됨
        {
            indexDisplay.Text = Convert.ToString(index);
        }

        private async Task firstNBack(int stage) // 처음 N개를 3초에 하나씩 보여주는 함수
        {
            for (; index < stage; index++)
            {
                int imageNum = rand.Next(1, 7);
                computerHand.Image = getImageByNum(imageNum); // 랜덤으로 결정된 이미지를 보여준다.
                pastAnswer[index] = imageNum; // 위에서 보여준 이미지를 정답 배열에 저장함
                updateIndexDisplay();
                await Task.Delay(firstNBackDelay);
            }
            index = 0; // 처음 N개를 보여줬으므로 인덱스를 다시 0으로 만든다.
            userHand.Focus();
            updateIndexDisplay();
        }

        private int nextNBack() // 처음 N개 이후 이미지를 하나씩 랜덤으로 보여주는 함수 
        {
            int imageNum = rand.Next(1, 7);
            computerHand.Image = getImageByNum(imageNum);
            return imageNum;
        }

        private Image getImageByNum(int num) // 1부터 6까지를 받아 해당 손동작의 이미지로 반환하는 함수
        {
            switch (num)
            {
                case 0:
                    return Properties.Resources.hand_undefined;
                case 1:
                    return Properties.Resources.hand_rock;
                case 2:
                    return Properties.Resources.hand_paper;
                case 3:
                    return Properties.Resources.hand_scissors;
                case 4:
                    return Properties.Resources.hand_okay;
                case 5:
                    return Properties.Resources.hand_thumbup;
                case 6:
                    return Properties.Resources.hand_phone;
                default:
                    return Properties.Resources.hand_undefined;
            }
        }

        private async Task processHandSign()
        {
            await runTimer();
            int handSign = checkHandSign(MainForm.handSign, 250);

            userHand.Image = getImageByNum(handSign);

            if (handSign == pastAnswer[index]) correctAnswer++;
            else wrongAnswer++;

            pastAnswer[index++] = currentImage;
            currentImage = nextNBack();
            if (index >= stage) index = 0;

            updateIndexDisplay();
            if (++total == gameEnd)
            {
                MainForm.f.frame.Controls.Clear();
                Scoreboard scoreboard = new Scoreboard();
                scoreboard.correct = correctAnswer; // 변수 전달
                scoreboard.wrong = wrongAnswer; // 변수 전달
                MainForm.f.frame.Controls.Add(scoreboard);
            }
        }

        private async Task runTimer()
        {
            counter.Text = "3";
            await Task.Delay(1000);
            counter.Text = "2";
            await Task.Delay(1000);
            counter.Text = "1";
            await Task.Delay(1000);
            counter.Text = "";
        }

        private int checkHandSign(int[] array, int arraySize)
        {
            int[] action = new int[7];
            int frequency = 0, maxValue = 0;

            for (int i = 0; i < arraySize; i++)
            {
                switch (array[i])
                {
                    case 0:
                        action[0]++;
                        break;
                    case 1:
                        action[1]++;
                        break;
                    case 2:
                        action[2]++;
                        break;
                    case 4:
                        action[3]++;
                        break;
                    case 8:
                        action[4]++;
                        break;
                    case 16:
                        action[5]++;
                        break;
                    case 32:
                        action[6]++;
                        break;
                    default:
                        break;
                }
            }

            for (int j = 0; j < 7; j++)
            {
                if (frequency <= action[j])
                {
                    frequency = action[j];
                    maxValue = j;
                }
            }

            return maxValue;
        }
    }
}