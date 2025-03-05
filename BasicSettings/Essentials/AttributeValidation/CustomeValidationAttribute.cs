namespace BasicSettings.Essentials.AttributeValidation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomeValidationAttribute : ValidationAttribute
    {
        public bool IsDate { get; set; }
        public bool IsNumber { get; set; }
        public bool IsPinFL { get; set; }
        public bool IsEmail { get; set; }
        public bool IsPhoneNumber { get; set; }
        public bool IsName { get; set; }
        public int MaxLength { get; set; } = int.MaxValue;
        public int MinLength { get; set; } = 0;
        public double RangeMin { get; set; } = double.MinValue;
        public double RangeMax { get; set; } = double.MaxValue;

        public CustomeValidationAttribute(string errorMessage = "Invalid value.") : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            ErrorMessage = GetErrorMessage("Invalid value.");

            var stringValue = value.ToString();


            if (IsDate && !IsValidDate(stringValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            if (IsNumber && !IsValidNumber(stringValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            if (IsPinFL && !IsValidPinFL(stringValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            if (IsEmail && !IsValidEmail(stringValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            if (IsPhoneNumber && !IsValidPhoneNumber(stringValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            if (IsName && !IsValidName(stringValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            if (stringValue.Length > MaxLength)
            {
                return new ValidationResult($"Value exceeds maximum length of {MaxLength}.");
            }

            if (stringValue.Length < MinLength)
            {
                return new ValidationResult($"Value is less than minimum length of {MinLength}.");
            }

            if (double.TryParse(stringValue, out double numericValue))
            {
                if (numericValue < RangeMin || numericValue > RangeMax)
                {
                    return new ValidationResult($"Value is out of range ({RangeMin} - {RangeMax}).");
                }
            }

            return ValidationResult.Success;
        }

        private bool IsValidDate(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return false;
            }

            try
            {
                var dateRegex = new Regex(@"^(19|20)\d{2}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$", RegexOptions.Compiled);
                return dateRegex.IsMatch(date);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsValidNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return false;
            }

            try
            {
                var numberRegex = new Regex(@"^\d+$", RegexOptions.Compiled);
                return numberRegex.IsMatch(number);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsValidPinFL(string pinFL)
        {
            if (string.IsNullOrWhiteSpace(pinFL))
            {
                return false;
            }

            try
            {
                var pinFLRegex = new Regex(@"^\d{14}$", RegexOptions.Compiled);
                return pinFLRegex.IsMatch(pinFL);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
                return emailRegex.IsMatch(email);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }

            try
            {
                var phoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled);
                return phoneRegex.IsMatch(phoneNumber);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            try
            {
                var nameRegex = new Regex(@"^[a-zA-Z\u0400-\u04FF\s]+$", RegexOptions.Compiled);
                return nameRegex.IsMatch(name);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GetErrorMessage(string defaultMessage)
        {
            return string.IsNullOrWhiteSpace(ErrorMessage) ? defaultMessage : ErrorMessage;
        }
    }
}
