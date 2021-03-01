using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ILR
{
    public struct InputedLetterResult
    {
        public bool isAttackValid;
        public bool isCorrect;

        public InputedLetterResult(bool _isAttackValid, bool _isCorrect)
        {
            isAttackValid = _isAttackValid;
            isCorrect = _isCorrect;
        }
    }
}