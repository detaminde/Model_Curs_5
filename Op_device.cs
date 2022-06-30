using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_Curs_5
{
    class Op_device
    {
        public Op_device(uint a_num, uint b_num)
        {
            A_num = a_num;
            B_num = b_num;
            X_Stor = (byte)0b0000001; // х0=1, потому что инициирован запуск устройства
        }

        // Входящие числа
        public uint A_num { get; set; }
        public uint B_num { get; set; }
        //

        // регистры памяти для операнд
        public uint AM_reg { get; set; }
        public uint BM_reg { get; set; }
        public byte C4_reg { get; set; }
        public uint C_reg { get; set; }
        public ushort D_reg { get; set; }
        //

        // регистры памяти для векторов слов
        public uint Y_vector // переменная представлена в виде вектора вида "000000000000000000",
                             // где каждый разряд - отдельный закодированный Y
                             // зависит от вектора Х и текущего состояния А
        {
            get;
            set;
            
        }
        public ushort A_state // переменная представлена в виде вектора состояний, например ус-во находится в состоянии "а1" - вектор выглядит так - 0b0000000000010
        {
            get;
            set; 
        }
        public byte Q_storState // переменная представлена в виде двоичного числа
                                // // (зависит от A_state и X_Vector)
        {
            get;
            set;
            

        }
        public ushort X_Stor // память логических условий
        {
            get;
            set;
        }
        public byte D_stor // комбинационная схема D, зависит от ЛУ и состояний А
        {
            get;
            set;
        }
        //

        // Дешифровка из памяти состояний
        public void Decode_FromQtoA(byte q_storState)
        {
            A_state = 0;
            A_state = (ushort)(1 << q_storState);
        }

        // Установка векторов Y
        public void set_YVector(ushort a_state, ushort x_stor)
        {

            // y0
            if(
                (((a_state & 0b0001) == 0b0001) & ((x_stor & 0b000000001) == 0)) ||
                (((a_state & 0b0001) == 0b0001) & ((x_stor & 0b000000011) == 0b000000011)) ||
                (((a_state & 0b0001) == 0b0001) & ((x_stor & 0b000000111) == 0b000000101)) ||
                (((a_state & 0b0100) == 0b0100) & ((x_stor & 0b111000000) == 0b101000000)) ||
                (((a_state & 0b0100) == 0b0100) & ((x_stor & 0b111000000) == 0b001000000)) ||
                (((a_state & 0b1000) == 0b1000) & ((x_stor & 0b100000000) == 0b100000000)) ||
                (((a_state & 0b1000) == 0b1000) & ((x_stor & 0b100000000) == 0)) 
                )
            {
                Y_vector |= 0b1;
            }
            // y1
            if(
                (((a_state & 0b0001) == 0b0001) & ((x_stor & 0b000000011) == 0b000000011)) ||
                (((a_state & 0b0001) == 0b0001) & ((x_stor & 0b000000111) == 0b000000101)) ||
                (((a_state & 0b0001) == 0b0001) & ((x_stor & 0b000000111) == 0b000000001))
                )
            {
                Y_vector |= 0b10;
            }
            // y2 y3 y4 y5 y6 y7
            if(
                (((a_state & 0b0001) == 0b0001) & ((x_stor & 0b000000111) == 0b000000001))
                )
            {
                Y_vector |= 0b11111100;
            }
            // y8 y10
            if(
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000001000) == 0b000001000)) ||
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000011000) == 0b000010000)) ||
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000111000) == 0b000100000)) ||
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000111000) == 0))
                )
            {
                Y_vector |= 0b10100000000;
            }
            // y9
            if (
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000001000) == 0b000001000)) ||
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000011000) == 0b000010000)) ||
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000111000) == 0b000100000))
                )
            {
                Y_vector |= 0b1000000000;
            }
            // y11
            if (
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000011000) == 0b000010000))
                )
            {
                Y_vector |= 0b100000000000;
            }
            // y12
            if(
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000111000) == 0b000100000))
                )
            {
                Y_vector |= 0b1000000000000;
            }
            // y13 y14
            if (
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000111000) == 0))
                )
            {
                Y_vector |= 0b110000000000000;
            }
            // y15
            if(
                (((a_state & 0b0100) == 0b0100) & ((x_stor & 0b011000000) == 0b011000000))
                )
            {
                Y_vector |= 0b1000000000000000;
            }
            // y16
            if(
                (((a_state & 0b0100) == 0b0100) & ((x_stor & 0b101000000) == 0b101000000)) ||
                (((a_state & 0b1000) == 0b1000) & ((x_stor & 0b100000000) == 0b100000000))
                )
            {
                Y_vector |= 0b10000000000000000;
            }
        }
        // Операционный автомат
        public ushort OperationAut(uint y_vector)
        {
            // y0
            if ((y_vector & 0b1) == 0b1)
            {
                return 0b0;
            }
            // y1
            if((y_vector & 0b10) == 0b10)
            {
                C_reg = 0;
            }
            // y2
            if ((y_vector & 0b100) == 0b100)
            {
                C4_reg = 16;
            }
            // y3
            if ((y_vector & 0b1000) == 0b1000)
            {
                D_reg = (ushort)(B_num & 0b1000000000000000);
            }
            // y4
            if ((y_vector & 0b10000) == 0b10000)
            {
                BM_reg = B_num & 0b0111111111111111;
            }
            // y5
            if ((y_vector & 0b100000) == 0b100000)
            {
                AM_reg = ((AM_reg & 0b0111111111111111111111111111111) | ((A_num & 0b1000000000000000) << 15));
            }
            // y6
            if ((y_vector & 0b1000000) == 0b1000000)
            {
                AM_reg = (AM_reg & 0b1000000000000000111111111111111);
            }
            // y7
            if ((y_vector & 0b10000000) == 0b10000000)
            {
                AM_reg = (AM_reg & 0b1111111111111111000000000000000) | (A_num & 0b0111111111111111);
            }

            // y11
            if ((y_vector & 0b100000000000) == 0b100000000000)
            {
                C_reg = C_reg + (AM_reg & 0b0111111111111111111111111111111);
            }
            // y12
            if ((y_vector & 0b1000000000000) == 0b1000000000000)
            {
                C_reg = C_reg + ((AM_reg & 0b0111111111111111111111111111111) << 1);
            }
            // y13
            if ((y_vector & 0b10000000000000) == 0b10000000000000)
            {
                C_reg = C_reg - (AM_reg & 0b0111111111111111111111111111111);
            }
            // y14
            if ((y_vector & 0b100000000000000) == 0b100000000000000)
            {
                BM_reg = (BM_reg & 0b0000000000000000) | ((BM_reg & 0b1111111111111100) + 0b100);
            }
            // y8
            if ((y_vector & 0b100000000) == 0b100000000)
            {
                AM_reg = (AM_reg & 0b1000000000000000000000000000000) | ((AM_reg & 0b0111111111111111111111111111111) << 2);
            }
            // y9
            if ((y_vector & 0b1000000000) == 0b1000000000)
            {
                BM_reg = (BM_reg & 0b1111111111111111) >> 2;
            }
            // y10
            if ((y_vector & 0b10000000000) == 0b10000000000)
            {
                C4_reg--;
            }
            // y15
            if ((y_vector & 0b1000000000000000) == 0b1000000000000000)
            {
                C_reg = (C_reg & 0b0000000000000001111111111111111) | (((C_reg & 0b0111111111111111000000000000000) + 0b1000000000000000) << 1);
            }
            // y16
            if ((y_vector & 0b10000000000000000) == 0b10000000000000000)
            {
                C_reg = (C_reg & 0b01111111111111111111111111111111) | 0b10000000000000000000000000000000;
            }
            
            //сделать просмотр условий  
            ushort returnX = 1;
            // x1
            if ((A_num & 0b0111111111111111) == 0)
                returnX |= 0b0000010;
            // x2
            if ((B_num & 0b0111111111111111) == 0)
                returnX |= 0b0000100;
            // x3 
            if ((BM_reg & 0b11) == 0b00)
            {
                returnX |= 0b0001000;
            }
            // x4
            if ((BM_reg & 0b11) == 0b01)
                returnX |= 0b0010000;
            // x5
            if ((BM_reg & 0b11) == 0b10)
                returnX |= 0b0010000;
            // x6
            if (C4_reg == 0)
                returnX |= 0b1000000;
            // x7
            if((C_reg & 0b100000000000000) == 0b100000000000000)
            {
                returnX |= 0b10000000;
            }
            // x8
            if(((A_num & 0b1) ^ ((D_reg >> 15) & 0b1)) == 0b1)
                returnX |= 0b100000000;

            return returnX;
        }
        
        // Установка ЛУ
        public void set_XStates(ushort dxt)
        {
            X_Stor = dxt;
        }
        
        // Установка вектора D
        public void set_DStates(ushort a_state, ushort x_stor)
        {
            // D0 
            if(
                (((a_state & 0b0001) == 0b0001) & ((x_stor & 0b000000111) == 0b000000001)) ||
                (((a_state & 0b0001) == 0b0100) & ((x_stor & 0b000000111) == 0b001000000)) ||
                (((a_state & 0b0100) == 0b0100) & ((x_stor & 0b011000000) == 0b011000000))
                )
            {
                D_stor |= 0b1;
            }
            // D1
            if (
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000001000) == 0b000001000)) ||
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000011000) == 0b000010000)) ||
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000111000) == 0b000100000)) ||
                (((a_state & 0b0010) == 0b0010) & ((x_stor & 0b000111000) == 0)) ||
                (((a_state & 0b0100) == 0b0100) & ((x_stor & 0b011000000) == 0b011000000))
                )
            {
                D_stor |= 0b10;
            }
        }
        
        // Установка состояния в памяти состояний
        public void setInto_StorageState()
        {
            if ((D_stor & 0b01) == 0b01)
            {
                Q_storState |= 0b0001;
            }
            if ((D_stor & 0b10) == 0b10)
            {
                Q_storState |= 0b10;
            }
        }

    }
}
