using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Model_Curs_5
{
    public partial class Form1 : Form
    {
        // два вводимых числа
        int A_num, B_num;
        //  "фронт" для исходных данных
        List<TextBox> A_textoboxes;
        List<TextBox> B_textoboxes;

        // "фронт" для данных во время выполнения
        List<TextBox> A_textBoxes;
        List<TextBox> B_textBoxes;
        List<TextBox> C_textBoxes;
        List<TextBox> C4_textBoxes;
        bool isReseting = false;
        double textA = 0, textB = 0;
        Op_device op_device;
        //
        ManualResetEventSlim limiter = new ManualResetEventSlim(true);
        public Form1()
        {
            InitializeComponent();

            A_num = 0;
            B_num = 0;

            // данные листы хранят только ссылки на объекты, так что памяти они не занимают
            //  "фронт" для исходных данных
            A_textoboxes = new List<TextBox>() {textBox17, textBox18, textBox19, textBox20, textBox21, textBox22, textBox23, textBox24, textBox25,
                textBox26, textBox27, textBox28, textBox29, textBox30, textBox31, textBox32};
            B_textoboxes = new List<TextBox>() {textBox35, textBox36, textBox37, textBox38, textBox39, textBox40, textBox41, textBox42, textBox43,
                textBox44, textBox45, textBox46, textBox47, textBox48, textBox49, textBox50};

            // "фронт" для данных во время выполнения
            A_textBoxes = new List<TextBox>()
           {    textBox233, textBox232, textBox231, textBox230, textBox229, textBox228, textBox227, textBox226, textBox225, textBox224, textBox223, 
                textBox222, textBox221, textBox220, textBox218, textBox219,
                textBox68, textBox69, textBox70, textBox71, textBox72, textBox73, textBox74, textBox75, textBox76,
                textBox77, textBox78, textBox79, textBox80, textBox81, textBox82, textBox83};

            B_textBoxes = new List<TextBox>() { textBox101, textBox102, textBox103, textBox104, textBox105, textBox106, textBox107,
                textBox108, textBox109, textBox110, textBox111, textBox112, textBox113, textBox114, textBox115, textBox116, textBox34 };

            C_textBoxes = new List<TextBox>() { 
                textBox205, textBox204, textBox203, textBox202, textBox201, textBox200, textBox199, textBox198, 
                textBox197, textBox196, textBox195, textBox194, textBox193, textBox192, textBox190, textBox191, textBox206,
                textBox67, textBox100, textBox133, textBox134, textBox135, textBox136, textBox137,
                textBox138, textBox139, textBox140, textBox141, textBox142, textBox143, textBox144, textBox145, };

            C4_textBoxes = new List<TextBox>() { textBox163, textBox164, textBox165, textBox166 };
        }

        // функция, отвечающая за внесение/удаление 0/1 в определенный разряд числа при нажатии на textBox
        //         также эта функция позволяет сразу вывести представление числа в десятичном виде
        // функция для первого операнда
        private void textBox_Click(object sender, EventArgs e)
        {
            if (isReseting)
                return;
            if (((TextBox)sender).Text == "0")
            {
                ((TextBox)sender).Text = "1";
                int digit = 1 << A_textoboxes.IndexOf((TextBox)sender); // реализуется через Object.Equals()
                A_num += digit;
                if (A_textoboxes.IndexOf((TextBox)sender) != 15)
                {
                    textA += Math.Pow(2, -1 * (15 - A_textoboxes.IndexOf((TextBox)sender)));
                }
                else
                    textA *= -1;
            }
            else
            {
                ((TextBox)sender).Text = "0";
                int digit = 1 << A_textoboxes.IndexOf((TextBox)sender);
                A_num -= digit;
                if (A_textoboxes.IndexOf((TextBox)sender) != 15)
                {
                    textA -= Math.Pow(2, -1 * (15 - A_textoboxes.IndexOf((TextBox)sender)));
                }
                else
                    textA *= -1;

            }
            A_tenth_textBox.Text = textA.ToString();
        }

        // функция для второго операнда
        private void textBoxB_Click(object sender, EventArgs e)
        {
            if (isReseting)
                return;
            if (((TextBox)sender).Text == "0")
            {
                ((TextBox)sender).Text = "1";
                int digit = 1 << B_textoboxes.IndexOf((TextBox)sender);
                B_num += digit;
                if (B_textoboxes.IndexOf((TextBox)sender) != 15)
                {
                    textB += Math.Pow(2, -1 * (15 - B_textoboxes.IndexOf((TextBox)sender)));
                }
                else
                    textB *= -1;
            }
            else
            {
                ((TextBox)sender).Text = "0";
                int digit = 1 << B_textoboxes.IndexOf((TextBox)sender);
                B_num -= digit;
                if (B_textoboxes.IndexOf((TextBox)sender) != 15)
                {
                    textB -= Math.Pow(2, -1 * (15 - B_textoboxes.IndexOf((TextBox)sender)));
                }
                else
                    textB *= -1;
            }
            B_tenth_textBox.Text = textB.ToString();
        }

        
        
        // Функция-сброс значений
        async private void Reset_button_Click(object sender, EventArgs e)
                {
                    CheckForIllegalCrossThreadCalls = false;
                    await Task.Run(() =>
                    {
                        
                        isReseting = true;

                        A_num = 0;
                        B_num = 0;
                        textA = 0;
                        textB = 0;
                        for (int i = 0; i < A_textoboxes.Count; i++)
                        {
                            A_textoboxes[i].Text = "0";
                            B_textoboxes[i].Text = "0";

                            
                            
                            B_textBoxes[i].Text = "0";
                            
                        }
                        for(int i = 0; i < C4_textBoxes.Count; i++)
                            C4_textBoxes[i].Text = "0";
                        for (int i = 0; i < C_textBoxes.Count; i++)
                            C_textBoxes[i].Text = "0";
                        for(int i = 0; i < A_textBoxes.Count; i++)
                            A_textBoxes[i].Text = "0";
                        B_tenth_textBox.Text = "0";
                        A_tenth_textBox.Text = "0";

                        isReseting = false;
                    }
                    );
                }
        // Функция-событие, отвечающая за старт выполнения моделирования
        private void Start_button_Click(object sender, EventArgs e)
        {
            Reset_button.Enabled = false;
            if(tabControl1.SelectedIndex == 0) // Микропрограмма
            {
                Execute_MicroProgram(); 
            }
            else if(tabControl1.SelectedIndex == 1) // Взаимодействие УА и ОА
            {
                Execute_UAnOA(); 
            }
            Reset_button.Enabled = true;
        }
        
        // Функция-перевод из десятичной в двоичную систему исчисления для вывода на интерфейс пользователя
        private void FromTenthToBin_front(List<TextBox> textboxes, uint num)
        {
            for (int i = 0; i < textboxes.Count; i++)
            {
                textboxes[i].Text = "0";
            }
            string bin_numStr = Convert.ToString(num, 2);
            if (bin_numStr.Length < textboxes.Count)
            {
                int stringLength = bin_numStr.Length;
                for (int i = 0; i < stringLength; i++)
                {
                    textboxes[i].Text = bin_numStr.Last().ToString();
                    bin_numStr = bin_numStr.Remove(bin_numStr.Length - 1);
                }
            }
            else
                for (int i = 0; i < textboxes.Count; i++)
                {
                    textboxes[i].Text = bin_numStr.Last().ToString();
                    bin_numStr = bin_numStr.Remove(bin_numStr.Length - 1);
                }
        }

        // Функция, инициирующая выполнение моделирования на уровне микропрограммы
        //  выполняется при выборе первой вкладки ("Микропрограмма") в tabControl1 и нажатии кнопки "Запуск"
        async private void Execute_MicroProgram() 
        {
            CheckForIllegalCrossThreadCalls = false;
            await Task.Run(() =>
           {
               for (int i = 0; i < A_textoboxes.Count; i++)
               { 
                   A_textBoxes[i].Text = "0";
                   C_textBoxes[i].Text = "0";
                   B_textBoxes[i].Text = "0";
               }
               for(int i = 0; i < C4_textBoxes.Count; i++)
                   C4_textBoxes[i].Text = "0";
               limiter.Wait();
               // x1
               if ((A_num & 0b0111111111111111) == 0)
               {
                   uint C_zero = 0;
                   A0_radioButton.Checked = true;
                   if (StepMode_radioButton.Checked)
                       limiter.Reset();
                   else
                       Thread.Sleep(1000);
                   return;
               }
               // x2
               if ((B_num & 0b0111111111111111) == 0)
               {
                   uint C_zero = 0;
                   A0_radioButton.Checked = true;
                   if (StepMode_radioButton.Checked)
                       limiter.Reset();
                   else
                       Thread.Sleep(1000);
                   return;
               }
               uint C_num = 0; // y1
               byte C4_num = 16; // y2
               ushort D_reg = (ushort)(B_num & 0b1000000000000000); // y3
               uint BM_num = 0;
               BM_num = (uint)(B_num & 0b0111111111111111); // y4
               // y5
               uint AM_num = 0; 
               AM_num = (((uint)((AM_num & 0b0111111111111111111111111111111) | ((A_num & 0b1000000000000000) << 15))));
               AM_num = (AM_num & 0b1000000000000000111111111111111);// y6
               AM_num = (uint)((AM_num & 0b1111111111111111000000000000000) | (A_num & 0b0111111111111111)); // y7
               
               FromTenthToBin_front(A_textBoxes, (AM_num & 0b1111111111111111111111111111111));
               FromTenthToBin_front(A_textBoxes, (BM_num & 0b1111111111111111));
               limiter.Wait();
               // сделать textbox для D_reg
               do
               {
                   
                   A1_radioButton.Checked = true;
                   if (StepMode_radioButton.Checked)
                       limiter.Reset();
                   else
                       Thread.Sleep(1000);
                   // x3
                   if ((BM_num & 0b11) == 0b00)
                   {
                       // y8 y9 y10
                       AM_num = (AM_num & 0b1000000000000000000000000000000) | ((AM_num & 0b0111111111111111111111111111111) << 2);
                       BM_num = (BM_num & 0b1111111111111111) >> 2;
                       C4_num--;
                   }
                   // x4
                   else if ((BM_num & 0b11) == 0b01)
                   {
                       C_num = C_num + (AM_num & 0b0111111111111111111111111111111); // y11
                       // y8 y9 y10
                       AM_num = (AM_num & 0b1000000000000000000000000000000) | ((AM_num & 0b0111111111111111111111111111111) << 2);
                       BM_num = (BM_num & 0b1111111111111111) >> 2;
                       C4_num--;
                   }
                   // x5
                   else if ((BM_num & 0b11) == 0b10)
                   {
                       C_num = C_num + ((AM_num & 0b0111111111111111111111111111111) << 1); // y12
                       // y8 y9 y10
                       AM_num = (AM_num & 0b1000000000000000000000000000000) | ((AM_num & 0b0111111111111111111111111111111) << 2);
                       BM_num = (BM_num & 0b1111111111111111) >> 2;
                       C4_num--;
                   }
                   else
                   {
                       C_num = C_num - (AM_num & 0b0111111111111111111111111111111); // y13
                       // y8 y9 y10
                       AM_num = (AM_num & 0b1000000000000000000000000000000) | ((AM_num & 0b0111111111111111111111111111111) << 2);
                       BM_num = (BM_num & 0b0000000000000000) | ((BM_num & 0b1111111111111100) + 0b100); // y14
                       C4_num--;
                   }
                   FromTenthToBin_front(A_textBoxes, (AM_num & 0b1111111111111111111111111111111));
                   FromTenthToBin_front(B_textBoxes, (BM_num & 0b1111111111111111));
                   FromTenthToBin_front(C_textBoxes, (C_num & 0b11111111111111111111111111111111));
                   FromTenthToBin_front(C4_textBoxes, (uint)(C4_num & 0b1111));

                   A2_radioButton.Checked = true;
                   limiter.Wait();
               } while(C4_num != 0); // x6

               // x7
               if ((C_num & 0b100000000000000) == 0b100000000000000)
               {
                   C_num = (C_num & 0b0000000000000001111111111111111) | (((C_num & 0b0111111111111111000000000000000) + 0b1000000000000000) << 1);
                   FromTenthToBin_front(C_textBoxes, (C_num & 0b11111111111111111111111111111111));
                   A3_radioButton.Checked = true;
                   if (StepMode_radioButton.Checked)
                       limiter.Reset();
                   else
                       Thread.Sleep(1000);
                   limiter.Wait();
               }
               // x8
               if (((A_num & 0b1) ^ ((D_reg >> 15) & 0b1)) == 0b1)
               {
                   C_num = (C_num & 0b01111111111111111111111111111111) | 0b10000000000000000000000000000000;
                   FromTenthToBin_front(C_textBoxes, (C_num & 0b11111111111111111111111111111111));
               }

               A0_radioButton.Checked = true;
               if (StepMode_radioButton.Checked)
                   limiter.Reset();
               else
                   Thread.Sleep(1000);
               CTenth_textBox.Text = String.Format("{0:0.0####}", FromBinToTenth(C_num));
               return;
           }
           );
        }
        private double FromBinToTenth(uint number)
        {
            string number_str = Convert.ToString(number, 2);
            uint digit = number & 0b1000000000000000;
            number_str = number_str.Remove(0, 1);
            //number_str.Reverse();
            int countStr = number_str.Length;
            double resultNum = 0;
            for(int i = 1; i < countStr; i++)
            {
                //resultNum += Math.Pow(2, -);
                if(number_str.Substring(0, 1) == "1")
                {
                    resultNum += Math.Pow(2, -i);
                }
                number_str = number_str.Remove(0, 1);
            }
            if ((digit & 0b10000000000000000) == 0b10000000000000000)
                resultNum *= -1;
            return resultNum;
        }

        // Функция, инициирующая выполнение моделирования на уровне взаимодействия УА и ОА
        //  выполняется при выборе второй вкладки ("Взаимодействие УА и ОА) в tabControl1 и нажатии кнопки "Запуск"
        async private void Execute_UAnOA()
        {
            CheckForIllegalCrossThreadCalls = false;
            await Task.Run( () =>
            {
                op_device = new Op_device((uint)A_num, (uint)B_num);
                // Вектор Х(t+1)
                ushort DXt = 1;

                // строки для работы с фронтом (см. работу функции toFront(string, List<>)
                string Q_storState_string = "";
                string XStates_string = "";
                string AState_string = "";
                string YVector_string = "";
                string DStates_string = "";
                //

                // Списки для работы с фронтом
                List<CheckBox> Q_checkBoxes = new List<CheckBox>(2) { Q0_checkBox, Q1_checkBox};
                List<CheckBox> X_checkBoxes = new List<CheckBox>(9) { X0_checkBox, X1_checkBox, X2_checkBox, X3_checkBox, X4_checkBox, X5_checkBox, X6_checkBox, X7_checkBox, X8_checkBox };
                List<CheckBox> A_checkBoxes = new List<CheckBox>(4) { A0_checkBox, A1_checkBox, A2_checkBox, A3_checkBox, };
                List<CheckBox> Y_checkBoxes = new List<CheckBox>(16) { Y0_checkBox, Y1_checkBox, Y2Q_checkBox, Y3_checkBox, Y4_checkBox, Y5_checkBox, Y6_checkBox, Y7_checkBox, Y8_checkBox,
                Y9_checkBox, Y10_checkBox, Y11_checkBox, Y12_checkBox, Y13_checkBox, Y14_checkBox, Y15_checkBox };
                List<CheckBox> D_checkBoxes = new List<CheckBox>(2) { D0_checkBox, D1_checkBox };
                //___
                // главный цикл работы ОУ (операционного устройства)
                do 
                {
                    // записать в память состояний
                    limiter.Wait(); // пауза для потактовой работы
                    op_device.Q_storState = 0;
                    op_device.setInto_StorageState();
                    Q_storState_string = Convert.ToString(op_device.Q_storState, 2);
                    toFront(Q_storState_string, Q_checkBoxes);
                    /*if (StepMode_radioButton.Checked)
                        limiter.Reset();*/

                    // записать в память ЛУ
                    /*limiter.Wait();*/
                    op_device.set_XStates(DXt);
                    XStates_string = Convert.ToString(op_device.X_Stor, 2);
                    toFront(XStates_string, X_checkBoxes);
                    /*if (StepMode_radioButton.Checked)
                        limiter.Reset();*/

                    // декодирование состояния
                    /*limiter.Wait();*/
                    op_device.Decode_FromQtoA(op_device.Q_storState);
                    AState_string = Convert.ToString(op_device.A_state, 2);
                    toFront(AState_string, A_checkBoxes);
                    /*if (StepMode_radioButton.Checked)
                        limiter.Reset();*/

                    // вычисление вектора игриков
                    /*limiter.Wait();*/
                    op_device.set_YVector(op_device.A_state, op_device.X_Stor);
                    YVector_string = Convert.ToString(op_device.Y_vector, 2);
                    toFront(YVector_string, Y_checkBoxes);
                    /*if (StepMode_radioButton.Checked)
                        limiter.Reset();*/

                    // вычисление значений триггеров для t+1
                    /*limiter.Wait();*/ 
                    op_device.D_stor = 0;
                    op_device.set_DStates(op_device.A_state, op_device.X_Stor);
                    DStates_string = Convert.ToString(op_device.D_stor, 2);
                    toFront(DStates_string, D_checkBoxes);
                    /*if (StepMode_radioButton.Checked)
                        limiter.Reset();*/

                    // значение Х для t+1
                    
                    DXt = op_device.OperationAut(op_device.Y_vector);
                    //Вывод во фронт
                    //
                    FromTenthToBin_front(A_textBoxes, op_device.AM_reg);
                    FromTenthToBin_front(C_textBoxes, op_device.C_reg & 0b11111111111111111);
                    FromTenthToBin_front(B_textBoxes, op_device.BM_reg & 0b11111111111111111);
                    FromTenthToBin_front(C4_textBoxes, (uint)op_device.C4_reg & 0b1111);
                    textBox177.Text = Convert.ToString(op_device.D_reg >> 15, 2);
                    //
                    op_device.Y_vector = 0;
                    if (StepMode_radioButton.Checked)
                        limiter.Reset();

                } while (((op_device.X_Stor & 0b00000001) == 1)); // пока ЛУ х0 не равно 0 и !OverFlow
                CTenth_textBox.Text = String.Format("{0:0.0####}",FromBinToTenth(op_device.C_reg));
            }
            );
        }

        // Функция, которая отображает при моделировании на уровне взаимодействяи УА и ОА состояние регистров
        private void toFront(string dataString, List<CheckBox> checkBoxes)
        {
            for(int i = 0; i < checkBoxes.Count; i++)
            {
                if(dataString.Length < checkBoxes.Count)
                {
                    dataString = dataString.Insert(0, "0");
                }
                if(dataString.Last() == '1')
                {
                    checkBoxes[i].Checked = true;
                }
                else
                    checkBoxes[i].Checked = false;

                dataString = dataString.Remove(dataString.Length - 1);
            }
        }
        private void Stepmode_radioButton_CheckedChanged(object sender, EventArgs e)
        {

            if (StepMode_radioButton.Checked)
            {
                nextTact_button.Show();
            }
            else
            {
                nextTact_button.Hide();
            }
        }


        private void nextTact_button_Click(object sender, EventArgs e)
        {
            limiter.Set();
        }

        private void Automode_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoMode_radioButton.Checked)
            {
                nextTact_button.Hide();
            }
            else
            {
                nextTact_button.Show();
            }
        }
    }
}
