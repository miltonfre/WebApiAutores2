﻿using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.CustomValidators
{
    public class FirstLetterCapitalize:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var firstChar = value.ToString()[0].ToString();
            if (firstChar != firstChar.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayuscula");
            }
            return ValidationResult.Success; 
        }
    }
}
