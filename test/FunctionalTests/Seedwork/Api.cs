namespace FunctionalTests.Seedwork
{
    public static class Api
    {
        public static class School
        {
            public static string GetGrades => "api/school/grades";
            public static string EditGrades => "api/school/grades";
            public static string InvalidEndpoint => "api/school/invalid";
            public static string GetSchemes => "api/school/schemes";
            public static string GetCustomPolicy => "api/school/custom-policy";
            public static string GetAbacPolicy => "api/school/abac";
        }
    }
}
