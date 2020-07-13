using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DanserMenu.v2
{
    public partial class Settings : Form
    {
        JToken value;
        bool boolval;

        public Settings()
        {
            InitializeComponent();
        }

        public void Update_Textbox(JObject settingsJson, TextBox textboxText, params string[] key)
        {
            switch (key.Length)
            {
                case 2:
                    value = settingsJson[key[0]][key[1]].ToString();
                    break;
                case 3:
                    value = settingsJson[key[0]][key[1]][key[2]].ToString();
                    break;
                case 4:
                    value = settingsJson[key[0]][key[1]][key[2]][key[3]].ToString();
                    break;
            }
            textboxText.Text = (string)value;
        }

        public void Update_Checkbox(JObject settingsJson, CheckBox checkboxTick, params string[] key)
        {
            switch (key.Length)
            {
                case 2:
                    if (settingsJson[key[0]][key[1]].ToString() == "True")
                    {
                        checkboxTick.Checked = true;
                    }
                    break;
                case 3:
                    if (settingsJson[key[0]][key[1]][key[2]].ToString() == "True")
                    {
                        checkboxTick.Checked = true;
                    }
                    break;
                case 4:
                    if (settingsJson[key[0]][key[1]][key[2]][key[3]].ToString() == "True")
                    {
                        checkboxTick.Checked = true;
                    }
                    break;
            }

        }

        public bool Check_Checkbox(CheckBox checkbox)
        {
            
            if (checkbox.Checked == true)
            {
                boolval = true;
            }
            else
            {
                boolval = false;
            }

            return boolval;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            var curDir = Directory.GetCurrentDirectory();
            JObject settingsJson = JObject.Parse(File.ReadAllText($@"{curDir}\settings.json"));

            //General
            Update_Textbox(settingsJson, textBox1, "General", "OsuSongsDir");

            //Graphics
            Update_Textbox(settingsJson, textBox8, "Graphics", "Width");
            Update_Textbox(settingsJson, textBox9, "Graphics", "Height");
            Update_Textbox(settingsJson, textBox10, "Graphics", "WindowWidth");
            Update_Textbox(settingsJson, textBox11, "Graphics", "WindowHeight");
            Update_Checkbox(settingsJson, checkBox1, "Graphics", "Fullscreen");
            Update_Checkbox(settingsJson, checkBox2, "Graphics", "VSync");
            Update_Textbox(settingsJson, textBox12, "Graphics", "FPSCap");
            Update_Textbox(settingsJson, textBox13, "Graphics", "MSAA");

            //Audio
            Update_Textbox(settingsJson, textBox14, "Audio", "GeneralVolume");
            Update_Textbox(settingsJson, textBox15, "Audio", "MusicVolume");
            Update_Textbox(settingsJson, textBox16, "Audio", "SampleVolume");
            Update_Textbox(settingsJson, textBox17, "Audio", "Offset");
            Update_Checkbox(settingsJson, checkBox3, "Audio", "IgnoreBeatmapSamples");
            Update_Checkbox(settingsJson, checkBox4, "Audio", "IgnoreBeatmapSampleVolume");

            //Beat
            Update_Textbox(settingsJson, textBox18, "Beat", "BeatScale");

            //Cursor
            Update_Checkbox(settingsJson, checkBox5, "Cursor", "Colors", "EnableRainbow");
            Update_Textbox(settingsJson, textBox3, "Cursor", "Colors", "RainbowSpeed");
            Update_Textbox(settingsJson, textBox2, "Cursor", "Colors", "BaseColor", "Hue");
            Update_Textbox(settingsJson, textBox19, "Cursor", "Colors", "BaseColor", "Saturation");
            Update_Textbox(settingsJson, textBox20, "Cursor", "Colors", "BaseColor", "Value");
            Update_Checkbox(settingsJson, checkBox6, "Cursor", "Colors", "EnableCustomHueOffset");
            Update_Textbox(settingsJson, textBox21, "Cursor", "Colors", "HueOffset");
            Update_Checkbox(settingsJson, checkBox7, "Cursor", "Colors", "FlashToTheBeat");
            Update_Textbox(settingsJson, textBox22, "Cursor", "Colors", "FlashAmplitude");
            Update_Checkbox(settingsJson, checkBox8, "Cursor", "EnableCustomTagColorOffset");
            Update_Textbox(settingsJson, textBox23, "Cursor", "TagColorOffset");
            Update_Checkbox(settingsJson, checkBox9, "Cursor", "EnableTrailGlow");
            Update_Checkbox(settingsJson, checkBox10, "Cursor", "EnableCustomTrailGlowOffset");
            Update_Textbox(settingsJson, textBox24, "Cursor", "TrailGlowOffset");
            Update_Checkbox(settingsJson, checkBox11, "Cursor", "ScaleToCS");
            Update_Textbox(settingsJson, textBox25, "Cursor", "CursorSize");
            Update_Checkbox(settingsJson, checkBox12, "Cursor", "ScaleToTheBeat");
            Update_Checkbox(settingsJson, checkBox13, "Cursor", "ShowCursorsOnBreaks");
            Update_Checkbox(settingsJson, checkBox14, "Cursor", "BounceOnEdges");
            Update_Textbox(settingsJson, textBox26, "Cursor", "TrailEndScale");
            Update_Textbox(settingsJson, textBox27, "Cursor", "TrailDensity");
            Update_Textbox(settingsJson, textBox28, "Cursor", "TrailMaxLength");
            Update_Textbox(settingsJson, textBox29, "Cursor", "TrailRemoveSpeed");
            Update_Textbox(settingsJson, textBox30, "Cursor", "GlowEndScale");
            Update_Textbox(settingsJson, textBox31, "Cursor", "InnerLengthMult");
            Update_Checkbox(settingsJson, checkBox15, "Cursor", "AdditiveBlending");

            //Objects
            Update_Textbox(settingsJson, textBox5, "Objects", "MandalaTexturesTrigger");
            Update_Textbox(settingsJson, textBox7, "Objects", "MandalaTexturesAlpha");
            Update_Checkbox(settingsJson, checkBox16, "Objects", "ForceSliderBallTexture");
            Update_Checkbox(settingsJson, checkBox17, "Objects", "DrawApproachCircles");
            Update_Checkbox(settingsJson, checkBox18, "Objects", "DrawComboNumbers");
            Update_Checkbox(settingsJson, checkBox19, "Objects", "DrawReverseArrows");
            Update_Checkbox(settingsJson, checkBox20, "Objects", "DrawSliderFollowCircle");
            Update_Checkbox(settingsJson, checkBox21, "Objects", "LoadSpinners");
            Update_Checkbox(settingsJson, checkBox23, "Objects", "Colors", "EnableRainbow");
            Update_Textbox(settingsJson, textBox4, "Objects", "Colors", "RainbowSpeed");
            Update_Textbox(settingsJson, textBox34, "Objects", "Colors", "BaseColor", "Hue");
            Update_Textbox(settingsJson, textBox35, "Objects", "Colors", "BaseColor", "Saturation");
            Update_Textbox(settingsJson, textBox36, "Objects", "Colors", "BaseColor", "Value");
            Update_Checkbox(settingsJson, checkBox24, "Objects", "Colors", "EnableCustomHueOffset");
            Update_Textbox(settingsJson, textBox37, "Objects", "Colors", "HueOffset");
            Update_Checkbox(settingsJson, checkBox25, "Objects", "Colors", "FlashToTheBeat");
            Update_Textbox(settingsJson, textBox38, "Objects", "Colors", "FlashAmplitude");
            Update_Textbox(settingsJson, textBox32, "Objects", "ObjectsSize");
            Update_Textbox(settingsJson, textBox33, "Objects", "CSMult");
            Update_Checkbox(settingsJson, checkBox22, "Objects", "ScaleToTheBeat");
            Update_Textbox(settingsJson, textBox40, "Objects", "SliderLOD");
            Update_Textbox(settingsJson, textBox41, "Objects", "SliderPathLOD");
            Update_Checkbox(settingsJson, checkBox28, "Objects", "SliderSnakeIn");
            Update_Textbox(settingsJson, textBox42, "Objects", "SliderSnakeInMult");
            Update_Checkbox(settingsJson, checkBox29, "Objects", "SliderSnakeOut");
            Update_Checkbox(settingsJson, checkBox30, "Objects", "SliderMerge");
            Update_Checkbox(settingsJson, checkBox31, "Objects", "SliderDynamicLoad");
            Update_Checkbox(settingsJson, checkBox32, "Objects", "SliderDynamicUnload");
            Update_Checkbox(settingsJson, checkBox33, "Objects", "DrawFollowPoints");
            Update_Checkbox(settingsJson, checkBox34, "Objects", "WhiteFollowPoints");
            Update_Textbox(settingsJson, textBox44, "Objects", "FollowPointColorOffset");
            Update_Checkbox(settingsJson, checkBox35, "Objects", "EnableCustomSliderBorderColor");
            Update_Checkbox(settingsJson, checkBox36, "Objects", "CustomSliderBorderColor", "EnableRainbow");
            Update_Textbox(settingsJson, textBox6, "Objects", "CustomSliderBorderColor", "RainbowSpeed");
            Update_Textbox(settingsJson, textBox45, "Objects", "CustomSliderBorderColor", "BaseColor", "Hue");
            Update_Textbox(settingsJson, textBox46, "Objects", "CustomSliderBorderColor", "BaseColor", "Saturation");
            Update_Textbox(settingsJson, textBox47, "Objects", "CustomSliderBorderColor", "BaseColor", "Value");
            Update_Checkbox(settingsJson, checkBox37, "Objects", "CustomSliderBorderColor", "EnableCustomHueOffset");
            Update_Textbox(settingsJson, textBox48, "Objects", "CustomSliderBorderColor", "HueOffset");
            Update_Checkbox(settingsJson, checkBox38, "Objects", "CustomSliderBorderColor", "FlashToTheBeat");
            Update_Textbox(settingsJson, textBox49, "Objects", "CustomSliderBorderColor", "FlashAmplitude");
            Update_Checkbox(settingsJson, checkBox26, "Objects", "EnableCustomSliderBorderGradientOffset");
            Update_Textbox(settingsJson, textBox39, "Objects", "SliderBorderGradientOffset");
            Update_Checkbox(settingsJson, checkBox27, "Objects", "StackEnabled");

            //Playfield
            Update_Checkbox(settingsJson, checkBox45, "Playfield", "ShowWarning");
            Update_Textbox(settingsJson, textBox43, "Playfield", "LeadInTime");
            Update_Textbox(settingsJson, textBox50, "Playfield", "LeadInHold");
            Update_Textbox(settingsJson, textBox51, "Playfield", "FadeOutTime");
            Update_Textbox(settingsJson, textBox52, "Playfield", "BackgroundInDim");
            Update_Textbox(settingsJson, textBox53, "Playfield", "BackgroundDim");
            Update_Textbox(settingsJson, textBox54, "Playfield", "BackgroundDimBreaks");
            Update_Checkbox(settingsJson, checkBox39, "Playfield", "BlurEnable");
            Update_Textbox(settingsJson, textBox55, "Playfield", "BackgroundInBlur");
            Update_Textbox(settingsJson, textBox56, "Playfield", "BackgroundBlur");
            Update_Textbox(settingsJson, textBox57, "Playfield", "BackgroundBlurBreaks");
            Update_Textbox(settingsJson, textBox58, "Playfield", "SpectrumInDim");
            Update_Textbox(settingsJson, textBox59, "Playfield", "SpectrumDim");
            Update_Textbox(settingsJson, textBox60, "Playfield", "SpectrumDimBreaks");
            Update_Checkbox(settingsJson, checkBox46, "Playfield", "StoryboardEnabled");
            Update_Textbox(settingsJson, textBox70, "Playfield", "Scale");
            Update_Checkbox(settingsJson, checkBox40, "Playfield", "FlashToTheBeat");
            Update_Checkbox(settingsJson, checkBox41, "Playfield", "UnblurToTheBeat");
            Update_Textbox(settingsJson, textBox62, "Playfield", "UnblurFill");
            Update_Textbox(settingsJson, textBox63, "Playfield", "KiaiFactor");
            Update_Textbox(settingsJson, textBox64, "Playfield", "BaseRotation");
            Update_Checkbox(settingsJson, checkBox42, "Playfield", "RotationEnabled");
            Update_Textbox(settingsJson, textBox65, "Playfield", "RotationSpeed");
            Update_Checkbox(settingsJson, checkBox43, "Playfield", "BloomEnabled");
            Update_Checkbox(settingsJson, checkBox44, "Playfield", "BloomToTheBeat");
            Update_Textbox(settingsJson, textBox66, "Playfield", "BloomBeatAddition");
            Update_Textbox(settingsJson, textBox67, "Playfield", "Bloom", "Threshold");
            Update_Textbox(settingsJson, textBox68, "Playfield", "Bloom", "Blur");
            Update_Textbox(settingsJson, textBox69, "Playfield", "Bloom", "Power");

            //Dance
            Update_Checkbox(settingsJson, checkBox47, "Dance", "SliderDance");
            Update_Checkbox(settingsJson, checkBox48, "Dance", "TAGSliderDance");
            Update_Checkbox(settingsJson, checkBox49, "Dance", "SliderDance2B");
            Update_Textbox(settingsJson, textBox61, "Dance", "Bezier", "Aggressiveness");
            Update_Textbox(settingsJson, textBox71, "Dance", "Bezier", "SliderAggressiveness");
            Update_Checkbox(settingsJson, checkBox50, "Dance", "Flower", "UseNewStyle");
            Update_Textbox(settingsJson, textBox72, "Dance", "Flower", "AngleOffset");
            Update_Textbox(settingsJson, textBox73, "Dance", "Flower", "DistanceMult");
            Update_Textbox(settingsJson, textBox74, "Dance", "Flower", "StreamTrigger");
            Update_Textbox(settingsJson, textBox75, "Dance", "Flower", "StreamAngleOffset");
            Update_Textbox(settingsJson, textBox76, "Dance", "Flower", "LongJump");
            Update_Textbox(settingsJson, textBox77, "Dance", "Flower", "LongJumpMult");
            Update_Checkbox(settingsJson, checkBox51, "Dance", "Flower", "LongJumpOnEqualPos");
            Update_Checkbox(settingsJson, checkBox53, "Dance", "Flower", "RadiusMultiplier");
            Update_Textbox(settingsJson, textBox78, "Dance", "HalfCircle", "RadiusMultiplier");
            Update_Textbox(settingsJson, textBox79, "Dance", "HalfCircle", "StreamTrigger");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string json = File.ReadAllText("settings.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            //General
            jsonObj["General"]["OsuSongsDir"] = textBox1.Text;

            //Graphics
            jsonObj["Graphics"]["Width"] = Convert.ToInt64(textBox8.Text);
            jsonObj["Graphics"]["Height"] = Convert.ToInt64(textBox9.Text);
            jsonObj["Graphics"]["WindowWidth"] = Convert.ToInt64(textBox10.Text);
            jsonObj["Graphics"]["WindowHeight"] = Convert.ToInt64(textBox11.Text);
            jsonObj["Graphics"]["Fullscreen"] = Check_Checkbox(checkBox1);
            jsonObj["Graphics"]["VSync"] = Check_Checkbox(checkBox2);
            jsonObj["Graphics"]["FPSCap"] = Convert.ToInt64(textBox12.Text);
            jsonObj["Graphics"]["MSAA"] = Convert.ToInt32(textBox13.Text);

            //Audio
            jsonObj["Audio"]["GeneralVolume"] = Convert.ToDouble(textBox14.Text);
            jsonObj["Audio"]["MusicVolume"] = Convert.ToDouble(textBox15.Text);
            jsonObj["Audio"]["SampleVolume"] = Convert.ToDouble(textBox16.Text);
            jsonObj["Audio"]["Offset"] = Convert.ToInt64(textBox17.Text);
            jsonObj["Audio"]["IgnoreBeatmapSamples"] = Check_Checkbox(checkBox3);
            jsonObj["Audio"]["IgnoreBeatmapSampleVolume"] = Check_Checkbox(checkBox4);

            //Beat
            jsonObj["Beat"]["BeatScale"] = Convert.ToDouble(textBox18.Text);

            //Cursor
            jsonObj["Cursor"]["Colors"]["EnableRainbow"] = Check_Checkbox(checkBox5);
            jsonObj["Cursor"]["Colors"]["RainbowSpeed"] = Convert.ToDouble(textBox3.Text);
            jsonObj["Cursor"]["Colors"]["BaseColor"]["Hue"] = Convert.ToDouble(textBox2.Text);
            jsonObj["Cursor"]["Colors"]["BaseColor"]["Saturation"] = Convert.ToDouble(textBox19.Text);
            jsonObj["Cursor"]["Colors"]["BaseColor"]["Value"] = Convert.ToDouble(textBox20.Text);
            jsonObj["Cursor"]["Colors"]["EnableCustomHueOffset"] = Check_Checkbox(checkBox6);
            jsonObj["Cursor"]["Colors"]["HueOffset"] = Convert.ToDouble(textBox21.Text);
            jsonObj["Cursor"]["Colors"]["FlashToTheBeat"] = Check_Checkbox(checkBox7);
            jsonObj["Cursor"]["Colors"]["FlashAmplitude"] = Convert.ToDouble(textBox22.Text);
            jsonObj["Cursor"]["EnableCustomTagColorOffset"] = Check_Checkbox(checkBox8);
            jsonObj["Cursor"]["TagColorOffset"] = Convert.ToDouble(textBox23.Text);
            jsonObj["Cursor"]["EnableTrailGlow"] = Check_Checkbox(checkBox29);
            jsonObj["Cursor"]["EnableCustomTrailGlowOffset"] = Check_Checkbox(checkBox30);
            jsonObj["Cursor"]["TrailGlowOffset"] = Convert.ToDouble(textBox24.Text);
            jsonObj["Cursor"]["ScaleToCS"] = Check_Checkbox(checkBox11);
            jsonObj["Cursor"]["CursorSize"] = Convert.ToDouble(textBox25.Text);
            jsonObj["Cursor"]["ScaleToTheBeat"] = Check_Checkbox(checkBox12);
            jsonObj["Cursor"]["ShowCursorsOnBreaks"] = Check_Checkbox(checkBox13);
            jsonObj["Cursor"]["BounceOnEdges"] = Check_Checkbox(checkBox14);
            jsonObj["Cursor"]["TrailEndScale"] = Convert.ToDouble(textBox26.Text);
            jsonObj["Cursor"]["TrailDensity"] = Convert.ToDouble(textBox27.Text);
            jsonObj["Cursor"]["TrailMaxLength"] = Convert.ToInt64(textBox28.Text);
            jsonObj["Cursor"]["TrailRemoveSpeed"] = Convert.ToDouble(textBox29.Text);
            jsonObj["Cursor"]["GlowEndScale"] = Convert.ToDouble(textBox30.Text);
            jsonObj["Cursor"]["InnerLengthMult"] = Convert.ToDouble(textBox31.Text);
            jsonObj["Cursor"]["AdditiveBlending"] = Check_Checkbox(checkBox15);

            //Objects
            jsonObj["Objects"]["MandalaTexturesTrigger"] = Convert.ToInt16(textBox5.Text);
            jsonObj["Objects"]["MandalaTexturesAlpha"] = Convert.ToDouble(textBox7.Text);
            jsonObj["Objects"]["ForceSliderBallTexture"] = Check_Checkbox(checkBox16);
            jsonObj["Objects"]["DrawApproachCircles"] = Check_Checkbox(checkBox17);
            jsonObj["Objects"]["DrawComboNumbers"] = Check_Checkbox(checkBox18);
            jsonObj["Objects"]["DrawReverseArrows"] = Check_Checkbox(checkBox19);
            jsonObj["Objects"]["DrawSliderFollowCircle"] = Check_Checkbox(checkBox20);
            jsonObj["Objects"]["LoadSpinners"] = Check_Checkbox(checkBox21);
            jsonObj["Objects"]["Colors"]["EnableRainbow"] = Check_Checkbox(checkBox23);
            jsonObj["Objects"]["Colors"]["RainbowSpeed"] = Convert.ToDouble(textBox4.Text);
            jsonObj["Objects"]["Colors"]["BaseColor"]["Hue"] = Convert.ToDouble(textBox34.Text);
            jsonObj["Objects"]["Colors"]["BaseColor"]["Saturation"] = Convert.ToDouble(textBox35.Text);
            jsonObj["Objects"]["Colors"]["BaseColor"]["Value"] = Convert.ToDouble(textBox36.Text);
            jsonObj["Objects"]["Colors"]["EnableCustomHueOffset"] = Check_Checkbox(checkBox24);
            jsonObj["Objects"]["Colors"]["HueOffset"] = Convert.ToDouble(textBox37.Text);
            jsonObj["Objects"]["Colors"]["FlashToTheBeat"] = Check_Checkbox(checkBox25);
            jsonObj["Objects"]["Colors"]["FlashAmplitude"] = Convert.ToDouble(textBox38.Text);
            jsonObj["Objects"]["ObjectsSize"] = Convert.ToDouble(textBox32.Text);
            jsonObj["Objects"]["CSMult"] = Convert.ToDouble(textBox33.Text);
            jsonObj["Objects"]["ScaleToTheBeat"] = Check_Checkbox(checkBox22);
            jsonObj["Objects"]["SliderLOD"] = Convert.ToInt64(textBox40.Text);
            jsonObj["Objects"]["SliderPathLOD"] = Convert.ToInt64(textBox41.Text);
            jsonObj["Objects"]["SliderSnakeIn"] = Check_Checkbox(checkBox28);
            jsonObj["Objects"]["SliderSnakeInMult"] = Convert.ToDouble(textBox42.Text);
            jsonObj["Objects"]["SliderSnakeOut"] = Check_Checkbox(checkBox29);
            jsonObj["Objects"]["SliderMerge"] = Check_Checkbox(checkBox30);
            jsonObj["Objects"]["SliderDynamicLoad"] = Check_Checkbox(checkBox31);
            jsonObj["Objects"]["SliderDynamicUnload"] = Check_Checkbox(checkBox32);
            jsonObj["Objects"]["DrawFollowPoints"] = Check_Checkbox(checkBox33);
            jsonObj["Objects"]["WhiteFollowPoints"] = Check_Checkbox(checkBox34);
            jsonObj["Objects"]["FollowPointColorOffset"] = Convert.ToDouble(textBox44.Text);
            jsonObj["Objects"]["EnableCustomSliderBorderColor"] = Check_Checkbox(checkBox35);
            jsonObj["Objects"]["CustomSliderBorderColor"]["EnableRainbow"] = Check_Checkbox(checkBox36);
            jsonObj["Objects"]["CustomSliderBorderColor"]["RainbowSpeed"] = Convert.ToDouble(textBox6.Text);
            jsonObj["Objects"]["CustomSliderBorderColor"]["BaseColor"]["Hue"] = Convert.ToDouble(textBox45.Text);
            jsonObj["Objects"]["CustomSliderBorderColor"]["BaseColor"]["Saturation"] = Convert.ToDouble(textBox46.Text);
            jsonObj["Objects"]["CustomSliderBorderColor"]["BaseColor"]["Value"] = Convert.ToDouble(textBox47.Text);
            jsonObj["Objects"]["CustomSliderBorderColor"]["EnableCustomHueOffset"] = Check_Checkbox(checkBox37);
            jsonObj["Objects"]["CustomSliderBorderColor"]["HueOffset"] = Convert.ToDouble(textBox48.Text);
            jsonObj["Objects"]["CustomSliderBorderColor"]["FlashToTheBeat"] = Check_Checkbox(checkBox38);
            jsonObj["Objects"]["CustomSliderBorderColor"]["FlashAmplitude"] = Convert.ToDouble(textBox49.Text);
            jsonObj["Objects"]["EnableCustomSliderBorderGradientOffset"] = Check_Checkbox(checkBox26);
            jsonObj["Objects"]["SliderBorderGradientOffset"] = Convert.ToDouble(textBox39.Text);
            jsonObj["Objects"]["StackEnabled"] = Check_Checkbox(checkBox27);

            //Playfield
            jsonObj["Playfield"]["ShowWarning"] = Check_Checkbox(checkBox45);
            jsonObj["Playfield"]["LeadInTime"] = Convert.ToDouble(textBox43.Text);
            jsonObj["Playfield"]["LeadInHold"] = Convert.ToDouble(textBox50.Text);
            jsonObj["Playfield"]["FadeOutTime"] = Convert.ToDouble(textBox51.Text);
            jsonObj["Playfield"]["BackgroundInDim"] = Convert.ToDouble(textBox52.Text);
            jsonObj["Playfield"]["BackgroundDim"] = Convert.ToDouble(textBox53.Text);
            jsonObj["Playfield"]["BackgroundDimBreaks"] = Convert.ToDouble(textBox54.Text);
            jsonObj["Playfield"]["BlurEnable"] = Check_Checkbox(checkBox39);
            jsonObj["Playfield"]["BackgroundInBlur"] = Convert.ToDouble(textBox55.Text);
            jsonObj["Playfield"]["BackgroundBlur"] = Convert.ToDouble(textBox56.Text);
            jsonObj["Playfield"]["BackgroundBlurBreaks"] = Convert.ToDouble(textBox57.Text);
            jsonObj["Playfield"]["SpectrumInDim"] = Convert.ToDouble(textBox58.Text);
            jsonObj["Playfield"]["SpectrumDim"] = Convert.ToDouble(textBox59.Text);
            jsonObj["Playfield"]["SpectrumDimBreaks"] = Convert.ToDouble(textBox60.Text);
            jsonObj["Playfield"]["StoryboardEnabled"] = Check_Checkbox(checkBox46);
            jsonObj["Playfield"]["Scale"] = Convert.ToDouble(textBox70.Text);
            jsonObj["Playfield"]["FlashToTheBeat"] = Check_Checkbox(checkBox40);
            jsonObj["Playfield"]["UnblurToTheBeat"] = Check_Checkbox(checkBox41);
            jsonObj["Playfield"]["UnblurFill"] = Convert.ToDouble(textBox62.Text);
            jsonObj["Playfield"]["KiaiFactor"] = Convert.ToDouble(textBox63.Text);
            jsonObj["Playfield"]["BaseRotation"] = Convert.ToDouble(textBox64.Text);
            jsonObj["Playfield"]["RotationEnabled"] = Check_Checkbox(checkBox42);
            jsonObj["Playfield"]["RotationSpeed"] = Convert.ToDouble(textBox65.Text);
            jsonObj["Playfield"]["BloomEnabled"] = Check_Checkbox(checkBox43);
            jsonObj["Playfield"]["BloomToTheBeat"] = Check_Checkbox(checkBox44);
            jsonObj["Playfield"]["BloomBeatAddition"] = Convert.ToDouble(textBox66.Text);
            jsonObj["Playfield"]["Bloom"]["Threshold"] = Convert.ToDouble(textBox67.Text);
            jsonObj["Playfield"]["Bloom"]["Blur"] = Convert.ToDouble(textBox68.Text);
            jsonObj["Playfield"]["Bloom"]["Power"] = Convert.ToDouble(textBox69.Text);

            //Dance
            jsonObj["Dance"]["SliderDance"] = Check_Checkbox(checkBox47);
            jsonObj["Dance"]["TAGSliderDance"] = Check_Checkbox(checkBox48);
            jsonObj["Dance"]["SliderDance2B"] = Check_Checkbox(checkBox49);
            jsonObj["Dance"]["Bezier"]["Aggressiveness"] = Convert.ToDouble(textBox61.Text);
            jsonObj["Dance"]["Bezier"]["SliderAggressiveness"] = Convert.ToDouble(textBox71.Text);
            jsonObj["Dance"]["Flower"]["RadiusMultiplier"] = Check_Checkbox(checkBox50);
            jsonObj["Dance"]["Flower"]["AngleOffset"] = Convert.ToDouble(textBox72.Text);
            jsonObj["Dance"]["Flower"]["DistanceMult"] = Convert.ToDouble(textBox73.Text);
            jsonObj["Dance"]["Flower"]["StreamTrigger"] = Convert.ToInt64(textBox74.Text);
            jsonObj["Dance"]["Flower"]["StreamAngleOffset"] = Convert.ToDouble(textBox75.Text);
            jsonObj["Dance"]["Flower"]["LongJump"] = Convert.ToInt64(textBox76.Text);
            jsonObj["Dance"]["Flower"]["LongJumpMult"] = Convert.ToDouble(textBox77.Text);
            jsonObj["Dance"]["Flower"]["LongJumpOnEqualPos"] = Check_Checkbox(checkBox51);
            jsonObj["Dance"]["Flower"]["RadiusMultiplier"] = Check_Checkbox(checkBox53);
            jsonObj["Dance"]["HalfCircle"]["RadiusMultiplier"] = Convert.ToDouble(textBox78.Text);
            jsonObj["Dance"]["HalfCircle"]["StreamTrigger"] = Convert.ToInt64(textBox79.Text);

            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText("settings.json", output);

            MessageBox.Show("Success!");
        }

        private void checkBox52_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox52.Checked == true)
            {
                textBox12.Text = "0";
                textBox12.ReadOnly = true;
            }
            else
            {
                textBox12.Text = "1000";
                textBox12.ReadOnly = false;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
