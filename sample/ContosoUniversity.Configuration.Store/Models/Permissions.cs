namespace ContosoUniversity.Configuration.Store.Models
{
    public static class Permissions
    {
        public const string GradesRead = "grades.read";
        public const string GradesEdit = "grades.edit";
    }

    public static class Policies
    {
        public const string ValidateGrades = "ValidateGrades";
    }
}
