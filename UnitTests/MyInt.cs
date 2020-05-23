﻿using System;
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
            int[] newValue = new int[value.Length];
            value.CopyTo(newValue, 0);
            return newValue;
        }

        public MyInt add(MyInt b)
        {
            int[] valueA = getValue();
            int[] valueB = b.getValue();

            int[] result;

            bool isPositiveA = value[0] == 0;
            bool isPositiveB = valueB[0] == 0;            

            if (isPositiveA == isPositiveB)
            {
                result = new int[Math.Max(value.Length, valueB.Length) + 1];
                valueA = value.Skip(1).Reverse().ToArray();
                valueB = valueB.Skip(1).Reverse().ToArray();

                int temp = 0;
                for (int i = 0; i < result.Length; i++)
                {
                    int firstNum = valueA.Length - 1 < i ? 0 : valueA[i];
                    int secondNum = valueB.Length - 1 < i ? 0 : valueB[i];
                    int sum = firstNum + secondNum + temp;
                    result[i] = Math.Abs(sum % 10);
                    temp = sum / 10;
                    if (i == result.Length - 1)
                    {
                        result[result.Length - 1] = isPositiveA ? 0 : 1;
                    }
                }                
            }
            else
            {
                result = new int[Math.Max(value.Length, valueB.Length)];

                int[] maxNum = abs().max(b.abs()).getValue();
                int[] minNum = abs().min(b.abs()).getValue();

                bool resultIsPositive = abs().compareTo(new MyInt(maxNum)) ? valueA[0] == 0 : valueB[0] == 0;

                maxNum = maxNum.Skip(1).ToArray().Reverse().ToArray();
                minNum = minNum.Skip(1).ToArray().Reverse().ToArray();

                int temp = 0;
                for(int i = 0; i < result.Length; i++)
                {
                    int firstNum = maxNum.Length - 1 < i ? 0 : maxNum[i];
                    int secondNum = minNum.Length - 1 < i ? 0 : minNum[i];
                    int diff = firstNum - secondNum - temp;
                    result[i] = Math.Abs((diff + 10) % 10);
                    temp = diff < 0 ? 1 : 0;
                    if(i == result.Length - 1)
                    {
                        result[result.Length - 1] = resultIsPositive ? 0 : 1;
                    }
                }
            }

           
            result = result.Reverse().ToArray();
            
            return new MyInt(result);
        }

        public MyInt substract(MyInt b)
        {
            int[] valueB = b.getValue();
            valueB[0] = valueB[0] == 0 ? 1 : 0;
            return add(new MyInt(valueB));
        }

        public MyInt multiply(MyInt b)
        {
            int[] valueA = getValue();
            int[] valueB = b.getValue();

            int[] result = new int[Math.Max(valueA.Length, valueB.Length) * 2];

            bool isPositiveA = valueA[0] == 0;
            bool isPositiveB = valueB[0] == 0;

            valueA = valueA.Skip(1).Reverse().ToArray();
            valueB = valueB.Skip(1).Reverse().ToArray();

            for(int i = 0; i < valueA.Length; i++)
            {
                for(int j = 0, carry = 0; j < valueB.Length || carry > 0; j ++) {
                    int sum = result[i + j] + valueA[i] * (valueB.Length - 1 < j ? 0 : valueB[j]) + carry;
                    result[i + j] = sum % 10;
                    carry = sum / 10;
                }
            }

            result[result.Length - 1] = isPositiveA == isPositiveB ? 0 : 1;

            return new MyInt(result.Reverse().ToArray());
        }

        public MyInt divide(MyInt b)
        {
            if(b.compareTo(new MyInt(0)))
            {
                return new MyInt(0);
            }

            int[] valueA = getValue();
            int[] valueB = b.getValue();
            

            List<int> result = new List<int>();

            bool isPositiveA = valueA[0] == 0;
            bool isPositiveB = valueB[0] == 0;

            valueA = valueA.Skip(1).ToArray();
            valueB = valueB.Skip(1).ToArray();
            
            int start = 0;
            int end = 1;
            while(end <= valueA.Length)
            {
                List<int> tempAValue = valueA.Skip(start).Take(end - start).ToList(); ;
                tempAValue.Insert(0, 0);

                List<int> tempBValue = valueB.ToList();
                tempBValue.Insert(0, 0);

                MyInt tempA = new MyInt(tempAValue.ToArray());
                MyInt tempB = new MyInt(tempBValue.ToArray());

                if(tempA.max(tempB).compareTo(tempA))
                {
                    
                    MyInt resultDivide = new MyInt(1);
                    while(!tempB.multiply(resultDivide).min(tempA).compareTo(tempA))
                    {
                        resultDivide = resultDivide.add(new MyInt(1));
                    }
                    if(!resultDivide.compareTo(new MyInt(1)))
                    {
                        resultDivide = resultDivide.substract(new MyInt(1));
                    }                   
                    result.AddRange(resultDivide.getValue().Skip(1));
                    MyInt resultMultiply = resultDivide.multiply(tempB);
                    MyInt resultDivideRest = tempA.substract(resultMultiply);
                    start = end - resultDivideRest.getValue().Length + 1;
                    for(int i = 0; i + start < end; i++)
                    {
                        valueA[i + start] = resultDivideRest.getValue()[i + 1]; 
                    }
                }
                end++;
            }

            if (result.Count == 0)
            {
                result.Insert(0, 0);
            }

            result.Insert(0, isPositiveA == isPositiveB ? 0 : 1);
           
            return new MyInt(result.ToArray());
        }

        public MyInt max(MyInt b)
        {
            int[] valueA = getValue();
            int[] valueB = b.getValue();

            bool isPositiveA = valueA[0] == 0;
            bool isPositiveB = valueB[0] == 0;
            if(isPositiveA != isPositiveB)
            {
                return isPositiveA ? this : b;
            }

            int lengthA = valueA.Length;
            int lengthB = valueB.Length;
            if (lengthA != lengthB)
            {
                return lengthA > lengthB ? this : b;
            }

            int numA = 0, numB = 0, i = 1;
            do
            {
                numA = valueA[i];
                numB = valueB[i];
                i++;
            } while (numA == numB && i < lengthA);

            return numA > numB ? this : b;
        }

        public MyInt min(MyInt b)
        {
            return max(b) == this ? b : this;
        }

        public MyInt abs()
        {
            int[] newValue = new int[value.Length];
            value.CopyTo(newValue, 0);
            newValue[0] = 0;
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
