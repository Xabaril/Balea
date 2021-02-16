namespace FunctionalTests
{
    public static class Policies
    {
        public const string ViewGrades = "view.grades";
        public const string EditGrades = "edit.grades";
    }

    public static class Roles
    {
        public const string Custodian = "custodian";
        public const string Teacher = "teacher";
        public const string Student = "student";
    }

    public static class Subs
    {
        public const string Teacher = "1";
        public const string Client = "m2m";
        public const string SubstituteOne = "2";
        public const string SubstituteTwo = "3";
    }


    public static class AbacPolicies
    {
        public const string Validate = "policy substitute begin\r\n    rule A (PERMIT) begin\r\n  Subject.Role CONTAINS \"Teacher\" AND Subject.Sub = \"1\" AND Resource.Controller = \"School\" \r\n  end\r\nend";
    }
    public static class ConnectionStrings
    {
        public const string Default = nameof(Default);
    }
}
