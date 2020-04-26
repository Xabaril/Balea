namespace FunctionalTests.Seedwork
{
    public static class Api
    {
        public static class School
        {
            public static string GetGrades => "api/school/grades";
            public static string EditGrades => "api/school/grades";
            public static string InvalidEndpoint => "api/school/invalid";
        }
    }
}
