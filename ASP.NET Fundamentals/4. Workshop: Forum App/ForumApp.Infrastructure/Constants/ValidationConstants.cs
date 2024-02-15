namespace ForumApp.Infrastructure.Constants
{
    public static class ValidationConstants
    {
        public const int PostTitleMinLength = 10;
        public const int PostTitleMaxLength = 50;

        public const int PostContentMinLength = 30;
        public const int PostContentMaxLength = 1500;

        public const string RequireErrorMessage = "Field {0} is required!";
        public const string IncorrectLengthErrorMessage = "Field {0} must be between {2} and {1} characters long!";
    }
}
