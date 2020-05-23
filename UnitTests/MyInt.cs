using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIntProject
{
    public class MyInt
    {
        private int[] value;

        public MyInt(long number)
        {
            bool isPositive = number >= 0;
            List<int> items = new List<int>();

            number = Math.Abs(number);

            while(number / 10 != 0)
            {
                int rest = (int)(number % 10);
                items.Insert(0, rest);
                number /= 10;
            }

            items.Insert(0, (int) (number % 10));
            items.Insert(0, isPositive ? 0 : 1);

            value = validate(items.ToArray());
        }

        public MyInt(string number)
        {
            bool isPositive = number[0] != '-';
            List<int> items = number.Where(num => num != '-').Select(num => int.Parse(num.ToString())).ToList();
            items.Insert(0, isPositive ? 0 : 1);

            value = validate(items.ToArray());
        }

        public MyInt(int[] number)
        {
            value = validate(number);            
        }

        private int[] validate(int[] number)
        {
            int[] result = number.Take(1).Concat(number.Skip(1).FirstOrDefault(num => num != 0) != 0 
                    ? number.Skip(1).SkipWhile(num => num == 0) 
                    : number.Skip(1).Take(1)
                ).ToArray();

            if (result[0] == 1 && result.Length == 2 && result[1] == 0)
            {
                result[0] = 0;
            }
            return result;
        }


        public int[] getValue()
        {
            return value;
        }

        public MyInt add(MyInt b)
        {
            int[] valueB = b.getValue();
            int[] result = new int[Math.Max(value.Length, valueB.Length) + 1];
            int temp = 0;
            for(int i = 0; i < result.Length - 1; i++)
            {
                int firstNum = (value.Length - 2 < i ? 0 : value[value.Length - 1 - i]);
                int secondNum = (valueB.Length - 2 < i ? 0 : valueB[valueB.Length - 1 - i]);
                int sum = firstNum + secondNum + temp;
                result[result.Length - 1 - i] = sum % 10;
                temp = sum / 10;
            }
            return new MyInt(result);
        }

        public MyInt substract(MyInt b)
        {
            int[] valueB = b.getValue();
            valueB[0] = valueB[0] == 0 ? 1 : 0;
            return add(new MyInt(valueB));
        }

        /*public MyInt multiply(MyInt b)
        {

        }

        public MyInt divide(MyInt b)
        {

        }*/

        public MyInt max(MyInt b)
        {
            int[] valueB = b.getValue();

            bool isPositiveA = value[0] == 0;
            bool isPositiveB = valueB[0] == 0;
            if(isPositiveA != isPositiveB)
            {
                return isPositiveA ? this : b;
            }

            int lengthA = value.Length;
            int lengthB = valueB.Length;
            if (lengthA != lengthB)
            {
                return lengthA > lengthB ? this : b;
            }

            int numA = 0, numB = 0, i = 1;
            do
            {
                numA = value[i];
                numB = valueB[i];
                i++;
            } while (numA == numB);

            return numA > numB ? this : b;
        }

        public MyInt min(MyInt b)
        {
            return max(b) == this ? b : this;
        }

        public MyInt abs()
        {
            int[] newValue = value;
            value[0] = 0;
            return new MyInt(newValue);
        }

        public bool compareTo(MyInt b)
        {
            return value.SequenceEqual(b.getValue());
        }

        public MyInt gcd(MyInt b)
        {
            MyInt a = abs();
            b = b.abs();
            while(!a.compareTo(b))
            {
                if (a.max(b) == a)
                {
                    a = a.substract(b);
                }
                else
                {
                    b = b.substract(a);
                }
            }

            return a;
        }

        public override string ToString()
        {
            return String.Concat((value[0] == 0 ? "" : "-"), string.Join(string.Empty, value.Skip(1).ToArray()));
        }

        public long longValue()
        {
            return long.Parse(ToString());
        }
    }
}
