using SampleApp.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Mvvm.PageViewModels.Mask
{
    public class JayBirthdayPageVm : BasePageVm
    {
        string j = "I'm a UX Designer from a web development background with skills in HTML, CSS, JavaScript and JAY." +//17
            " I have 15+ Happy years of advocating a user-centred approach. Design thinking is at my core, which means I am analytical, " +//21 38
            "innovative and relish complex tasks. As a Dear, I have experience in leading projects with cross-functional teams, stakeholder management and working with you. " +//23 61
            "My excellent communication skills and confidence in presenting work, ensure developers, stakeholders and clients will understand even the most complex solutions. I have experience in using Figma, " +//26 87
            "Adobe XD, Adobe Creative Cloud, Miro and Birthday. And, use the software to produce deliverables such as user flows, customer journey maps, wireframes, and prototypes."; //+8

        string[] _allText;

        int _nextIndex = 0;


        private string _targetName;
        private Color _maskColor;
        private Color _maskEdgeColor;
        private float _backgroundAlpha;
        private float _maskRoundness;
        private float _maskEdgeThickness;

        public string TargetName
        {
            get => _targetName;
            set => SetProperty(ref _targetName, value);
        }
        public float BackgroundAlpha
        {
            get => _backgroundAlpha;
            set => SetProperty(ref _backgroundAlpha, value);
        }
        public float MaskRoundness
        {
            get => _maskRoundness;
            set => SetProperty(ref _maskRoundness, value);
        }

        public float MaskEdgeThickness
        {
            get => _maskEdgeThickness;
            set => SetProperty(ref _maskEdgeThickness, value);
        }


        public Color MaskColor
        {
            get => _maskColor;
            set => SetProperty(ref _maskColor, value);
        }
        public Color MaskEdgeColor
        {
            get => _maskEdgeColor;
            set => SetProperty(ref _maskEdgeColor, value);
        }

        public JayBirthdayPageVm()
        {
            _allText = j.Split(' ');
            _ = DoTheThingAsync();
        }

        public string NextText => GetNextText();

        string GetNextText()
        {
            var retval = _allText[_nextIndex];

            _nextIndex = (_nextIndex + 1) % _allText.Length;

            return retval;
        }

        private async Task DoTheThingAsync()
        {
            int hb2y1 = 1000;
            int hb2y2 = 500;

            while (true)
            {
                await Task.Delay(5000);

                await HappyBirthdayToYou(hb2y1, Colors.Black);
                TargetName = "";
                await Task.Delay(1500);

                await HappyBirthdayToYou(hb2y1, Colors.Blue);
                TargetName = "";
                await HappyBirthdayDearJay(hb2y1, Colors.Red);
                TargetName = "";
                await Task.Delay(1100);
                await HappyBirthdayToYou(hb2y2, Colors.Green);
                TargetName = "";

            }
        }

        private async Task HappyBirthdayDearJay(int hb2y, Color edgeColor)
        {
            await Task.Delay(hb2y);
            TargetName = "Happy";
            BackgroundAlpha = 0.5F;
            MaskRoundness = 1.0f;
            MaskEdgeColor = edgeColor;
            MaskColor = Colors.Yellow;
            MaskEdgeThickness = 5f;
            await Task.Delay(hb2y);

            TargetName = "Birthday";
            BackgroundAlpha = 0.5F;
            MaskRoundness = 1.0f;
            MaskEdgeColor = edgeColor;
            MaskColor = Colors.Yellow;
            MaskEdgeThickness = 5f;
            await Task.Delay(hb2y);

            TargetName = "Dear";
            BackgroundAlpha = 0.5F;
            MaskRoundness = 1.0f;
            MaskEdgeColor = edgeColor;
            MaskColor = Colors.Yellow;
            MaskEdgeThickness = 5f;
            await Task.Delay(hb2y);

            TargetName = "Jay";
            BackgroundAlpha = 0.5F;
            MaskRoundness = 1.0f;
            MaskEdgeColor = edgeColor;
            MaskColor = Colors.Yellow;
            MaskEdgeThickness = 5f;
            await Task.Delay(hb2y);








            //BackgroundAlpha = 0.8F;
            //await Task.Delay(1000);

            BackgroundAlpha = 1F;
            MaskRoundness = 0.0f;
            await Task.Delay(1000);

            MaskEdgeThickness = 15f;
            await Task.Delay(1000);
            MaskEdgeThickness = 5f;



            BackgroundAlpha = 0.5F;
            MaskRoundness = 1.0f;
            await Task.Delay(1000);




        }

        private async Task HappyBirthdayToYou(int hb2y, Color edgeColor)
        {
            TargetName = "Happy";
            BackgroundAlpha = 0.5F;
            MaskRoundness = 1.0f;
            MaskEdgeColor = edgeColor;
            MaskColor = Colors.Yellow;
            MaskEdgeThickness = 5f;
            await Task.Delay(hb2y);

            TargetName = "Birthday";
            BackgroundAlpha = 0.5F;
            MaskRoundness = 1.0f;
            MaskEdgeColor = edgeColor;
            MaskColor = Colors.Yellow;
            MaskEdgeThickness = 5f;
            await Task.Delay(hb2y);
            
            TargetName = "to";
            BackgroundAlpha = 0.5F;
            MaskRoundness = 1.0f;
            MaskEdgeColor = edgeColor;
            MaskColor = Colors.Yellow;
            MaskEdgeThickness = 5f;
            await Task.Delay(hb2y);
            
            TargetName = "you";
            BackgroundAlpha = 0.5F;
            MaskRoundness = 1.0f;
            MaskEdgeColor = edgeColor;
            MaskColor = Colors.Yellow;
            MaskEdgeThickness = 5f;
            await Task.Delay(hb2y);
        }
    }

}