namespace DeveloperStore.Sales.Application;

public static class Constants
{
    public static class Filter
    {
        public const string PAGE_NUMBER_KEY = "_page";
        public const string PAGE_SIZE_KEY = "_size";
    }

    public static class Validations
    {
        public const string REQUIRED = "{PropertyName} is required!";
        public const string SHOULD_NOT_BE_EMPTY = "{PropertyName} should not be empty!";
        public const string SHOULD_BE_GREATER_THAN_ZERO = "The {PropertyName} must be greater than 0.";
        public const string SHOULD_BE_GREATER_THAN_OR_EQUAL_TO_ZERO = "The {PropertyName} must be greater than or equal to 0.";
        public const string SHOULD_BE_LESS_THAN_TWENTY = "The {PropertyName} must be less than or equal to 20.";
        public const string DATE_SHOULD_BE_LESS_OR_EQUAL_TO = "The {PropertyName} cannot be in the future.";
    }
}
