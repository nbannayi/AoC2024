using System;
using System.Collections.Generic;

namespace Day22
{
    public class SecretNumber
    {
        private long _number;
        public long Number { get { return _number; } }

        private List<int> _endDigits;        
        private List<int> _endDiffs;        
        private List<(int, int, int, int)> _diffSeqs;
        
        private List<((int, int, int, int), int)> _diffSeqPrices;
        public List<((int, int, int, int), int)> DiffSeqPrices { get { return _diffSeqPrices; } }

        /// <summary>
        /// Represents a monkeymarket secret number.
        /// </summary>
        /// <param name="number">Number to act on.</param>
        public SecretNumber(long number)
        {
            _number = number;
            _endDigits = new List<int>();
            _endDigits.Add(GetEndDigit(_number));
            _endDiffs = new List<int>();
            _diffSeqs = new List<(int, int, int, int)>();
            _diffSeqPrices = new List<((int, int, int, int), int)>();
        }        

        /// <summary>
        /// Evolve a secret number in accordance with monkey market rules. 
        /// </summary>
        /// <returns>Evolved number</returns>
        public void Evolve()
        {            
            long n2 = prune(mix(_number, _number * 64L));
            long n3 = prune(mix(n2, n2 / 32L));
            long n4 = prune(mix(n3, n3 * 2048L));
            _number = n4;

            long mix(long n1, long n2) { return n1 ^ n2; }
            long prune(long n) { return n % 16777216L; }
        }

        /// <summary>
        /// Evolve a secret number a given number of times.
        /// </summary>
        /// <param name="noTimes">No. times to evolve.</param>
        /// <returns>Resultant evolved number.</returns>
        public void Evolve(int noTimes)
        {
            for (int i = 1; i <= noTimes; i++)
            {
                Evolve();
                if (i != noTimes)
                    _endDigits.Add(GetEndDigit(_number));
                else
                {
                    for (int j = 1; j < _endDigits.Count; j++)                    
                        _endDiffs.Add(_endDigits[j] - _endDigits[j - 1]);
                    for (int k = 3; k < _endDiffs.Count; k++)
                        _diffSeqs.Add((_endDiffs[k - 3], _endDiffs[k - 2], _endDiffs[k - 1], _endDiffs[k]));
                    for (int l = 0; l < _diffSeqs.Count; l++)
                        _diffSeqPrices.Add((_diffSeqs[l], _endDigits[l + 4]));
                }
            }
        }

        // Get last digit of a given number.
        private int GetEndDigit(long number)
        {
            var numberString = number.ToString();
            var numberLength = numberString.Length;            
            return int.Parse(numberString[numberLength - 1].ToString());
        }
    }
}